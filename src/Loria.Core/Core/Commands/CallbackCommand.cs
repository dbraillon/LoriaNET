namespace LoriaNET
{
    public class CallbackCommand
    {
        public string Message { get; set; }

        public CallbackCommand(string str)
        {
            Message = str;
        }

        public override string ToString() => Message;
    }
}
