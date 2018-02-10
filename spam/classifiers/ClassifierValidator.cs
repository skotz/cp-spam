using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spam
{
    class ClassifierValidator
    {
        private IClassifier _classifier;

        private List<LabeledMessage> labeledMessages;
        private List<ClassificationResult> result;

        private BackgroundWorker worker;

        public event EventHandler<ClassifierValidationResult> OnClassifierScored;

        public ClassifierValidator(IClassifier classifier)
        {
            _classifier = classifier;
            
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnClassifierScored?.Invoke(this, GetScoredModel(labeledMessages, result));
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            result = _classifier.ClassifyMessages(labeledMessages.Select(x => x.Message).ToList());
        }
        
        public void ScoreClassifierAsync(string file)
        {
            if (!worker.IsBusy)
            {
                labeledMessages = GetLabeledMessages(file);
                worker.RunWorkerAsync();
            }
        }

        public ClassifierValidationResult ScoreClassifier(string file)
        {
            List<LabeledMessage> labeledMessages = GetLabeledMessages(file);

            List<ClassificationResult> results = _classifier.ClassifyMessages(labeledMessages.Select(x => x.Message).ToList());

            return GetScoredModel(labeledMessages, results);
        }

        private static ClassifierValidationResult GetScoredModel(List<LabeledMessage> labeledMessages, List<ClassificationResult> results)
        {
            for (int i = 0; i < labeledMessages.Count; i++)
            {
                labeledMessages[i].ModelClassification = results.Where(x => x.Message == labeledMessages[i].Message).First().Classification;
            }

            return new ClassifierValidationResult(labeledMessages, results[0].ElapsedTime);
        }

        private static List<LabeledMessage> GetLabeledMessages(string file)
        {
            List<LabeledMessage> labeledMessages = new List<LabeledMessage>();
            string[] messages = File.ReadAllLines(file);

            foreach (string message in messages)
            {
                if (message.StartsWith("Ham") || message.StartsWith("Spam"))
                {
                    string[] data = message.Split(new char[] { ',' }, 2);
                    labeledMessages.Add(new LabeledMessage(data[0], data[1]));
                }
            }

            return labeledMessages;
        }
    }

    class LabeledMessage
    {
        public string Message { get; set; }

        public string ModelClassification { get; set; }

        public string RealClassification { get; set; }

        public LabeledMessage(string realClassification, string message)
        {
            Message = message;
            RealClassification = realClassification;
        }
    }

    class ClassifierValidationResult
    {
        public List<LabeledMessage> LabeledMessages { get; private set; }

        public decimal Accuracy { get; private set; }

        public int Correct { get; private set; }

        public int Total { get; private set; }

        public int Wrong { get; private set; }

        public TimeSpan ElapsedTime { get; private set; }

        public ClassifierValidationResult(List<LabeledMessage> messages, TimeSpan elapsedTime)
        {
            LabeledMessages = messages;
            Correct = messages.Count(x => x.ModelClassification == x.RealClassification);
            Total = messages.Count;
            Wrong = Total - Correct;
            Accuracy = (decimal)Correct / Total;
            ElapsedTime = elapsedTime;
        }
    }
}
