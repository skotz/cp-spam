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
    class AzureWS
    {
        private string apikey;
        private string endpoint = "https://ussouthcentral.services.azureml.net/workspaces/ede0b3ce4eb042b4952ebec217073d6f/services/85efe934beec479ab1323a47d3a17cfd/execute?api-version=2.0&details=true";

        BackgroundWorker worker;
        Stopwatch timer;

        public EventHandler<ClassificationResult> OnMessageClassified;

        public AzureWS(string azureApiKey)
        {
            apikey = azureApiKey;

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnMessageClassified?.Invoke(this, e.Result as ClassificationResult);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            AzureServiceRequest request = e.Argument as AzureServiceRequest;
            e.Result = ClassifyMessage(request.Message);
        }

        public void ClassifyMesageAsync(string message)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync(new AzureServiceRequest(message));
            }
        }

        public ClassificationResult ClassifyMessage(string message)
        {
            try
            {
                timer = Stopwatch.StartNew();

                using (var client = new HttpClient())
                {
                    var scoreRequest = new
                    {
                        Inputs = new Dictionary<string, StringTable>()
                        {
                            {
                                "message",
                                new StringTable()
                                {
                                    ColumnNames = new string[] { "Col2" },
                                    Values = new string[,] {  { message },  }
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

                        return new ClassificationResult(json.Results.classification.value.Values[0][0].ToString(), timer.Elapsed);
                    }
                    else
                    {
                        File.AppendAllText("error.log", "Azure ML Request Failed: " + response.StatusCode + "\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", "Azure ML Request Failed: " + ex.Message + "\r\n");
            }

            return null;
        }
    }

    public class AzureServiceRequest
    {
        public string Message { get; set; }

        public AzureServiceRequest(string message)
        {
            Message = message;
        }
    }

    public class ClassificationResult
    {
        public string Classification { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public ClassificationResult(string classification, TimeSpan time)
        {
            Classification = classification;
            ElapsedTime = time;
        }
    }

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
}
