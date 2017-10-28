using Loria.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Loria.Modules.Ifttt
{
    public class IftttModule : Module, IListener, ICallback
    {
        public override string Name => "IFTTT module";
        public override string Description => "It allows me to interact with IFTTT solution";

        public string Command => "ifttt";

        public string MakerKey { get; set; }
        public WebServer WebServer { get; set; }
        public bool Paused { get; set; }

        public IftttModule(Engine engine)
            : base(engine)
        {
        }

        public override void Configure()
        {
            MakerKey = Engine.Data.File.Get("ifttt::MakerKey");
            WebServer = new WebServer(HandleRequest, "http://*:80/");

            Activate();
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

            var command = new Command($"{direction} {module} {intent} {string.Join(" ", other.Select(o => $"-{o.Key} {o.Value}"))}");
            Engine.Propagator.Propagate(command);

            return "OK";
        }

        public void Perform(Command command)
        {
            using (var httpClient = new HttpClient())
            {
                var serialized = JsonConvert.SerializeObject(new { value1 = command.AsCallbackCommand().Message });

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
    }
}
