using System;
using System.Linq;

namespace LoriaNET
{
    public class Command
    {
        public string Raw { get; set; }

        public string Header { get; set; }
        public string Module { get; set; }
        public string Body { get; set; }

        public bool IsActionRelated => string.Equals(Header, Actions.Keyword, StringComparison.InvariantCultureIgnoreCase);
        public bool IsCallbackRelated => string.Equals(Header, Callbacks.Keyword, StringComparison.InvariantCultureIgnoreCase);

        public Command(string str)
        {
            var separator = ' ';
            var splitted = str.Split(separator);

            Header = splitted.ElementAtOrDefault(0);
            Module = splitted.ElementAtOrDefault(1);
            Body = string.Join(separator.ToString(), splitted.Skip(2));
            Raw = str;
        }

        public ActionCommand AsActionCommand() => new ActionCommand(Body);
        public CallbackCommand AsCallbackCommand() => new CallbackCommand(Body);

        public override string ToString() => Raw;
    }
}
