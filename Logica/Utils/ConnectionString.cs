namespace Logica.Utils
{
    public sealed class CommandsConnectionString
    {
        public CommandsConnectionString(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public sealed class QueriesConnectionString
    {
        public QueriesConnectionString(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
