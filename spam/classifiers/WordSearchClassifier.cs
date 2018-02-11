using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace spam
{
    class WordSearchClassifier : IClassifier
    {
        private Dictionary<string, int> bigrams;
        private double hamCutover;

        private BackgroundWorker worker;
        private Stopwatch timer;

        public event EventHandler<List<ClassificationResult>> OnMessagesClassified;

        public WordSearchClassifier()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnMessagesClassified?.Invoke(this, e.Result as List<ClassificationResult>);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            AzureServiceRequest request = e.Argument as AzureServiceRequest;
            e.Result = ClassifyMessages(request.Messages);
        }

        public void ClassifyMessageAsync(string message)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync(new AzureServiceRequest(new List<string>() { message }));
            }
        }

        public void ClassifyMessagesAsync(List<string> messages)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync(new AzureServiceRequest(messages));
            }
        }

        public ClassificationResult ClassifyMessage(string message)
        {
            return ClassifyMessages(new List<string>() { message })[0];
        }

        public List<ClassificationResult> ClassifyMessages(List<string> messages)
        {
            List<ClassificationResult> classifications = new List<ClassificationResult>();
            timer = Stopwatch.StartNew();

            for (int i = 0; i < messages.Count; i++)
            {
                string prediction = RunModel(messages[i]);
                classifications.Add(new ClassificationResult(messages[i], prediction, timer.Elapsed));
            }

            timer.Stop();
            classifications.ForEach(x => x.ElapsedTime = timer.Elapsed);

            return classifications;
        }

        public void Train(string trainingFile)
        {
            bigrams = new Dictionary<string, int>();
            hamCutover = 0;
            string[] trainingData = File.ReadAllLines(trainingFile);
            List<LabeledMessage> messages = new List<LabeledMessage>();

            // Read all the messages
            foreach (string line in trainingData)
            {
                if (line.StartsWith("Ham") || line.StartsWith("Spam"))
                {
                    string[] data = line.Split(new char[] { ',' }, 2);
                    messages.Add(new LabeledMessage(data[0], data[1]));
                }
            }

            // For each distinct bigram across the training set, keep a running total of the number of times it appears in Ham messages as opposed to Spam
            foreach (LabeledMessage message in messages)
            {
                string[] words = ParseBigrams(CleanMessage(message.Message));

                foreach (string word in words)
                {
                    if (bigrams.ContainsKey(word))
                    {
                        bigrams[word] += message.RealClassification == "Ham" ? 1 : -1;
                    }
                    else
                    {
                        bigrams.Add(word, message.RealClassification == "Ham" ? 1 : -1);
                    }
                }
            }

            // For fun
            // GetBuckets(messages, 20);

            // Calculate the average cutover point where the summation of bigram occurrences switches from a Ham prediction to Spam
            hamCutover = messages.Select(x => GetScore(x.Message)).Average();
        }

        private string RunModel(string message)
        {
            return GetScore(message) > hamCutover ? "Ham" : "Spam";
        }

        private double GetScore(string message)
        {
            double score = 0;
            string[] messageBigrams = ParseBigrams(CleanMessage(message));

            foreach (string word in messageBigrams)
            {
                if (bigrams.ContainsKey(word))
                {
                    score += bigrams[word];
                }
            }

            return score / messageBigrams.Length;
        }

        private Dictionary<string, int[]> GetBuckets(List<LabeledMessage> messages, int numBuckets)
        {
            List<double> allScores = messages.Select(x => GetScore(x.Message)).ToList();
            double min = allScores.Min();
            double max = allScores.Max();
            double bucketSize = (max - min) / numBuckets;

            int[] hamHistogram = new int[numBuckets];
            int[] spamHistogram = new int[numBuckets];

            foreach (LabeledMessage message in messages)
            {
                double score = GetScore(message.Message);
                int bucketIndex = (int)Math.Min(Math.Floor((score - min) / bucketSize), numBuckets - 1);

                if (message.RealClassification == "Ham")
                {
                    hamHistogram[bucketIndex]++;
                }
                else
                {
                    spamHistogram[bucketIndex]++;
                }
            }

            Dictionary<string, int[]> classBuckets = new Dictionary<string, int[]>();
            classBuckets.Add("Ham", hamHistogram);
            classBuckets.Add("Spam", spamHistogram);

            using (StreamWriter w = new StreamWriter("hist.csv"))
            {
                w.WriteLine("Bucket,Ham,Spam");
                for (int i = 0; i < numBuckets; i++)
                {
                    double bucket = min + bucketSize * i;
                    w.WriteLine(bucket + "," + hamHistogram[i] + "," + spamHistogram[i]);
                }
            }

            return classBuckets;
        }

        private string[] ParseBigrams(string[] words)
        {
            string[] bigrams = new string[words.Length - 1];

            for (int i = 0; i < words.Length - 1; i++)
            {
                bigrams[i] = words[i] + " " + words[i + 1];
            }

            return bigrams;
        }

        private string[] CleanMessage(string message)
        {
            return Regex.Replace(message.ToLower(), "[^a-z]", " ").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
