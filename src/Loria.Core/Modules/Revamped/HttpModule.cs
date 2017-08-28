using System;
using System.Net;
using System.Text;
using System.Threading;

namespace LoriaNET.Web
{
    /// <summary>
    /// The http module provides intents and entities for forwarding commands to Loria.
    /// </summary>
    internal sealed class HttpModule : IModule, IListener
    {
        public string Name => "Http module";

        /// <summary>
        /// Loria's configuration.
        /// </summary>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// The web server.
        /// </summary>
        public WebServer WebServer { get; set; }
        
        /// <summary>
        /// Create the http module.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        public HttpModule(Configuration configuration)
        {
            Configuration = configuration;
            WebServer = new WebServer(HandleRequest, "http://localhost:8080/");
        }

        /// <summary>
        /// Configure the http module.
        /// </summary>
        public void Configure() => Activate();

        /// <summary>
        /// Check if http module is enabled.
        /// </summary>
        /// <returns>State of http module.</returns>
        public bool IsEnabled() => WebServer.IsListening;

        /// <summary>
        /// Activate the http module.
        /// </summary>
        public void Activate() => WebServer.Run();

        /// <summary>
        /// Deactivate the http module.
        /// </summary>
        public void Deactivate() => WebServer.Stop();

        /// <summary>
        /// Start http listener.
        /// </summary>
        public void Start() => Activate();

        /// <summary>
        /// Stop http listener.
        /// </summary>
        public void Stop() => Deactivate();

        /// <summary>
        /// Pause http listener.
        /// </summary>
        public void Pause()
        {
        }

        /// <summary>
        /// Resume http listener.
        /// </summary>
        public void Resume()
        {
        }

        /// <summary>
        /// Callback to handle a http request.
        /// </summary>
        /// <param name="request">The http request.</param>
        /// <returns>The http response.</returns>
        public string HandleRequest(HttpListenerRequest request)
        {
            // Get URL and remove first slash (/)
            var url = request.RawUrl.Substring(1);

            // Split URL with slash (/) and join with space ( ) to get the command or message
            var commandOrCallback = string.Join(" ", url.Split('/'));

            // Propagate the command or message
            Configuration.Hub.Propagate(commandOrCallback);

            return "OK";
        }
    }

    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public bool IsListening => _listener.IsListening;

        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                //Console.WriteLine("Webserver running...");
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
