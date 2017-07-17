using System;
using System.Globalization;
using System.Media;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.CognitiveServices
{
    public class TextToSpeech
    {
        public async Task RequestAsync(string message, CultureInfo culture)
        {
            var appId = Guid.NewGuid().ToString("N");
            var instanceAppId = Guid.NewGuid().ToString("N");
            var token = await GetAccessTokenAsync();

            var voice = Voice.GetFromCulture(culture);
            if (voice == null) throw new ApplicationException("Unsupported culture to output language.");

            var client = new HttpClient();

            var httpMessage = new HttpRequestMessage();
            httpMessage.Method = HttpMethod.Post;
            httpMessage.Headers.Authorization = GetAuthorizationHeader(token);
            httpMessage.Headers.Add("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm");
            httpMessage.Headers.Add("X-Search-AppId", appId);
            httpMessage.Headers.Add("X-Search-ClientID", instanceAppId);
            httpMessage.Headers.Add("User-Agent", "Bing.TextToSpeech");
            httpMessage.Content = new StringContent($"<speak version='1.0' xml:lang='{culture}'><voice xml:lang='{culture}' xml:gender='{voice.Gender}' name='{voice.ServiceName}'>{message}</voice></speak>");
            //message.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
            //{
            //    { "VoiceType", "Female" },
            //    { "VoiceName", "Microsoft Server Speech Text to Speech Voice (fr-FR, Julie, Apollo)" },
            //    { "Locale", "fr-FR" },
            //    { "Text", Console.ReadLine() }
            //});
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ssml+xml");
            httpMessage.RequestUri = new Uri("https://speech.platform.bing.com/synthesize");

            var response = await client.SendAsync(httpMessage);
            var stream = await response.Content.ReadAsStreamAsync();

            var player = new SoundPlayer(stream);
            player.LoadAsync();
            player.Play();
            player.Play();
        }

        static async Task<string> GetAccessTokenAsync()
        {
            var client = new HttpClient();

            var message = new HttpRequestMessage();
            message.Headers.Add("Ocp-Apim-Subscription-Key", "610b8e0ad3544d99b74e015441648e6e");
            message.Method = new HttpMethod("POST");
            message.RequestUri = new Uri("https://api.cognitive.microsoft.com/sts/v1.0/issueToken");

            var response = await client.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }
        
        static string EncodeAccessToken(string token)
        {
            return token; // Convert.ToBase64String(Encoding.ASCII.GetBytes(token));
        }

        static AuthenticationHeaderValue GetAuthorizationHeader(string token)
        {
            return new AuthenticationHeaderValue("Bearer", EncodeAccessToken(token));
        }


    }
}
