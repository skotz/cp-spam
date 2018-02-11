using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace spam
{
    class AzureMachineLearningClassifier : IClassifier
    {
        private string apikey;
        private string endpoint;

        private BackgroundWorker worker;
        private Stopwatch timer;

        public event EventHandler<List<ClassificationResult>> OnMessagesClassified;

        public AzureMachineLearningClassifier(string azureApiKey, string azureEndpoint)
        {
            apikey = azureApiKey;
            endpoint = azureEndpoint;

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

            using (var client = new HttpClient())
            {
                string[,] values = new string[messages.Count, 1];
                for (int i = 0; i < messages.Count; i++)
                {
                    values[i, 0] = messages[i];
                }

                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, StringTable>()
                    {
                        {
                            "message",
                            new StringTable()
                            {
                                ColumnNames = new string[] { "message" },
                                Values = values
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apikey);
                client.BaseAddress = new Uri(endpoint);

                HttpResponseMessage response = client.PostAsJsonAsync("", scoreRequest).Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    dynamic json = JsonConvert.DeserializeObject(result);

                    for (int i = 0; i < messages.Count; i++)
                    {
                        classifications.Add(new ClassificationResult(messages[i], json.Results.classification.value.Values[i][0].ToString(), timer.Elapsed));
                    }

                    return classifications;
                }
                else
                {
                    File.AppendAllText("error.log", "Azure ML Request Failed: " + response.StatusCode + "\r\n");
                }
            }

            return new List<ClassificationResult>();
        }
    }

    public class AzureServiceRequest
    {
        public List<string> Messages { get; set; }

        public AzureServiceRequest(List<string> messages)
        {
            Messages = messages;
        }
    }

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
}
