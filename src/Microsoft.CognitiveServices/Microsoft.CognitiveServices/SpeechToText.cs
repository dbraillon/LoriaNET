using Microsoft.CognitiveServices.SpeechRecognition;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.CognitiveServices
{
    public class SpeechToText
    {
        public string ApiKey { get; set; }
        public CultureInfo Culture { get; set; }
        public MicrophoneRecognitionClient RecognitionClient { get; set; }

        public event Action<Result> OnSpeechRecognized;

        public SpeechToText(string apiKey, CultureInfo culture, string luisAppId, string luisSubscriptionId)
        {
            ApiKey = apiKey;
            Culture = culture;
            RecognitionClient = SpeechRecognitionServiceFactory.CreateMicrophoneClientWithIntent(Culture.Name, ApiKey, luisAppId, luisSubscriptionId);
            RecognitionClient.AuthenticationUri = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";

            // Event handlers for speech recognition results
            RecognitionClient.OnMicrophoneStatus += RecognitionClient_OnMicrophoneStatus;
            RecognitionClient.OnPartialResponseReceived += RecognitionClient_OnPartialResponseReceived;
            RecognitionClient.OnResponseReceived += RecognitionClient_OnResponseReceived;
            RecognitionClient.OnConversationError += RecognitionClient_OnConversationError;
            RecognitionClient.OnIntent += RecognitionClient_OnIntent;
        }

        private void RecognitionClient_OnIntent(object sender, SpeechIntentEventArgs e)
        {
            Console.WriteLine(e.Intent.Body);
            var result = JsonConvert.DeserializeObject<Result>(e.Intent.Body);
            if (result != null)
            {
                OnSpeechRecognized?.Invoke(result);
            }
        }

        public void Start()
        {
            RecognitionClient.StartMicAndRecognition();
        }

        private void RecognitionClient_OnMicrophoneStatus(object sender, MicrophoneEventArgs e)
        {
            //Console.WriteLine(e.Recording);
        }

        private void RecognitionClient_OnPartialResponseReceived(object sender, PartialSpeechResponseEventArgs e)
        {
            //Console.WriteLine(e.PartialResult);
        }

        private void RecognitionClient_OnResponseReceived(object sender, SpeechResponseEventArgs e)
        {
            //if (e.PhraseResponse.Results.Any())
            //{
            //    OnSpeechRecognized?.Invoke(e.PhraseResponse.Results[0].LexicalForm);
            //    //Console.WriteLine(e.PhraseResponse.Results[0].DisplayText);
            //}
        }

        private void RecognitionClient_OnConversationError(object sender, SpeechErrorEventArgs e)
        {
            Console.WriteLine(e);
        }
    }

    public class Result
    {
        public string Query { get; set; }
        public Guess[] Intents { get; set; }
        public Keyword[] Entities { get; set; }
    }

    public class Guess
    {
        public string Intent { get; set; }
        public decimal Score { get; set; }
    }

    public class Keyword
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public Dictionary<string, string> Resolution { get; set; }
    }
}
