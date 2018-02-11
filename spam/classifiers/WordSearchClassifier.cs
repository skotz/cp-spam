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
            string[] messages = File.ReadAllLines(trainingFile);

            // For each distinct bigram across the training set, keep a running total of the number of times it appears in Ham messages as opposed to Spam
            foreach (string message in messages)
            {
                if (message.StartsWith("Ham") || message.StartsWith("Spam"))
                {
                    string[] data = message.Split(new char[] { ',' }, 2);
                    string classification = data[0];
                    string[] words = ParseBigrams(CleanMessage(data[1]));
                    
                    foreach (string word in words)
                    {
                        if (bigrams.ContainsKey(word))
                        {
                            bigrams[word] += classification == "Ham" ? 1 : -1;
                        }
                        else
                        {
                            bigrams.Add(word, classification == "Ham" ? 1 : -1);
                        }
                    }
                }
            }

            // Calculate the average cutover point where the summation of bigram occurrences switches from a Ham prediction to Spam
            hamCutover = messages.Select(x => GetScore(x)).Average();
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
