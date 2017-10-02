using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace LoriaNET
{
    /// <summary>
    /// The IFTTT module provides intents and entities to interact with IFTTT webhook module.
    /// </summary>
    public sealed class IftttModule : Module, IListener, ICallback
    {
        public override string Name => "IFTTT module";

        public string MakerKey { get; set; }

        public WebServer WebServer { get; set; }
        public bool Paused { get; set; }

        public IftttModule(Loria loria) 
            : base(loria)
        {
        }

        public override void Configure()
        {
            MakerKey = Loria.Data.ConfigurationFile.Get("ifttt::MakerKey");
            WebServer = new WebServer(HandleRequest, "http://*:80/");
            Activate();
        }

        public void Callback(string message)
        {
            using (var httpClient = new HttpClient())
            {
                var serialized = JsonConvert.SerializeObject(new { value1 = message });

                var content = new StringContent(
                    serialized,
                    Encoding.UTF8,
                    "application/json"
                );

                var result = httpClient.PostAsync($"https://maker.ifttt.com/trigger/loria_callback/with/key/{MakerKey}", content).GetAwaiter().GetResult();
                if (result.IsSuccessStatusCode)
                {
                    // TODO
                }
                else
                {
                    // TODO
                }
            }
        }

        public void Start() => WebServer.Run();
        public void Stop() => WebServer.Stop();
        public void Pause() => Paused = true;
        public void Resume() => Paused = false;

        public string HandleRequest(HttpListenerRequest request)
        {
            if (!Paused)
            {
                var contentType = request.ContentType;

                switch (contentType)
                {
                    case "application/json": return HandleJsonRequest(request);
                    default: return "501";
                }
            }

            return "503";
        }

        public string HandleJsonRequest(HttpListenerRequest request)
        {
            if (request.HttpMethod != "POST") return "405";

            var streamReader = new StreamReader(request.InputStream);
            var body = streamReader.ReadToEnd();
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

            var direction = json.ContainsKey("direction") ? json["direction"] : string.Empty;
            var module = json.ContainsKey("module") ? json["module"] : string.Empty;
            var intent = json.ContainsKey("intent") ? json["intent"] : string.Empty;
            var other = json.Where(data =>
                data.Key != "direction" &&
                data.Key != "module" &&
                data.Key != "intent"
            );

            if (string.Equals(direction.ToString(), Callbacks.Keyword, System.StringComparison.InvariantCultureIgnoreCase))
            {
                Loria.Hub.PropagateCallback(json.ContainsKey(string.Empty) ? json[string.Empty].ToString() : string.Empty);
            }
            else
            {
                var command = string.Join(" ",
                    module,
                    intent,
                    string.Join(" ",
                        other.Select(o => $"-{o.Key} {o.Value}")
                    )
                );
                Loria.Hub.PropagateCommand(new Command(command.Trim()));
            }
            
            return "OK";
        }
    }
}
