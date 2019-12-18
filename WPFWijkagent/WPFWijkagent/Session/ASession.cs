namespace WijkagentWPF.Session
{
    public abstract class ASession
    {
        public const char Separator = '|';

        public string Key { get; private set; }

        public ASession(string key) => Key = key;

        /// <summary>
        /// Parses the file content and handles the data.
        /// </summary>
        /// <param name="input">string to be parsed</param>
        public abstract void Load(string input);
        
        /// <summary>
        /// Smashes some sort of an object into a string.
        /// </summary>
        /// <returns>value to store</returns>
        public abstract string Save();
    }
}
