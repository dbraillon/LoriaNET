using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace LoriaNET
{
    //    /// <summary>
    //    /// The http module provides a listener to forward commands to Loria.
    //    /// </summary>
    //    internal sealed class HttpModule : Module, IListener, IAction
    //    {
    //        public const string GetIntent = "get";
    //        public const string PostIntent = "post";

    //        public const string UrlEntity = "url";

    //        public override string Name => "Http module";

    //        public string Description => "Accept and send http request with Loria!";

    //        public string Command => "http";

    //        public string[] SupportedIntents => new string[]
    //        {
    //            GetIntent, PostIntent
    //        };

    //        public string[] SupportedEntities => new string[]
    //        {
    //            UrlEntity
    //        };

    //        public string[] Samples => new string[]
    //        {
    //            "http get http://loria.io"
    //        };

    //        /// <summary>
    //        /// The web server.
    //        /// </summary>
    //        public WebServer WebServer { get; set; }

    //        /// <summary>
    //        /// Flag to know if listener is paused.
    //        /// </summary>
    //        public bool Paused { get; set; }

    //        public HttpModule(Configuration configuration)
    //            : base(configuration)
    //        {
    //        }

    //        public override void Configure()
    //        {
    //            WebServer = new WebServer(HandleRequest, "http://*:80/");
    //            Activate();
    //        }

    //        public void Start() => WebServer.Run();
    //        public void Stop() => WebServer.Stop();
    //        public void Pause() => Paused = true;
    //        public void Resume() => Paused = false;

    //        /// <summary>
    //        /// Callback to handle a http request.
    //        /// </summary>
    //        /// <param name="request">The http request.</param>
    //        /// <returns>The http response.</returns>
    //        public string HandleRequest(HttpListenerRequest request)
    //        {
    //            if (!Paused)
    //            {
    //                var contentType = request.ContentType;

    //                switch (contentType)
    //                {
    //                    case "application/json": return HandleJsonRequest(request);
    //                    default: return "501";
    //                }
    //            }

    //            return "503";
    //        }

    //        private string HandleJsonRequest(HttpListenerRequest request)
    //        {
    //            if (request.HttpMethod != "POST") return "405";

    //            var streamReader = new StreamReader(request.InputStream);
    //            var body = streamReader.ReadToEnd();
    //            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

    //            Configuration.Hub.PropagateCallback($"Incoming http request: {string.Join(", ", json.Select(node => $"{node.Key}: {node.Value}"))}");

    //            return "OK";
    //        }

    //        public void Perform(Command command)
    //        {
    //            switch (command.Intent)
    //            {
    //                case GetIntent:
    //                    break;

    //                case PostIntent:
    //                    PostRequest(command);
    //                    break;
    //            }
    //        }

    //        public void PostRequest(Command command)
    //        {
    //            using (var httpClient = new HttpClient())
    //            {
    //                // Get url from entity
    //                var urlEntity = command.GetEntity(UrlEntity);

    //                // Turn entities to form content
    //                var requestContent = new FormUrlEncodedContent(
    //                    command.Entities.Where(e => e.Name != UrlEntity).Select(e =>
    //                        new KeyValuePair<string, string>(e.Name.Substring(1), e.Value)
    //                    )
    //                );

    //                // Perform the request
    //                var response = httpClient.PostAsync(urlEntity.Value, requestContent).GetAwaiter().GetResult();
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    Configuration.Hub.PropagateCallback("Request posted.");
    //                }
    //                else
    //                {
    //                    Configuration.Hub.PropagateCallback($"Something goes wrong, here the detail: {response.ReasonPhrase}");
    //                }
    //            }
    //        }
    //    }

    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public bool IsListening => _listener.IsListening;

        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported) throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0) throw new ArgumentException(nameof(prefixes));

            foreach (string s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            _responderMethod = method ?? throw new ArgumentException(nameof(method));
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method)
        {
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = _responderMethod(ctx.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
