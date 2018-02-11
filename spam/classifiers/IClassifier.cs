using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spam
{
    interface IClassifier
    {
        event EventHandler<List<ClassificationResult>> OnMessagesClassified;

        void ClassifyMessageAsync(string message);

        void ClassifyMessagesAsync(List<string> messages);

        ClassificationResult ClassifyMessage(string message);

        List<ClassificationResult> ClassifyMessages(List<string> messages);
    }

    public class ClassificationResult
    {
        public string Message { get; set; }

        public string Classification { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public ClassificationResult(string message, string classification, TimeSpan time)
        {
            Message = message;
            Classification = classification;
            ElapsedTime = time;
        }
    }
}
