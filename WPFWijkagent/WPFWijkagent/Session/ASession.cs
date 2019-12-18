namespace WijkagentWPF.Session
{
    public abstract class ASession
    {
        public const char Separator = '|';
        public string Key { get; private set; }

        public ASession(string key) => Key = key;

        public abstract void Load(string input);

        public abstract string Save();
    }
}
