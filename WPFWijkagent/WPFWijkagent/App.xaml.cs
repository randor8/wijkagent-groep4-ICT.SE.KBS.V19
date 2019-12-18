using System.Collections.Generic;
using System.IO;
using System.Windows;
using WijkagentWPF.Session;
using Location = Microsoft.Maps.MapControl.WPF.Location;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Dictionary<string, ASession> _sessions = new Dictionary<string, ASession>();
        public const string SessionFile = "./Session.cfg";

        public static void RegisterSession(ASession session) => _sessions.Add(session.Key, session);

        /// <summary>
        /// This function loads the configuration file and reads it contents.
        /// It loops over all lines and checks if the keys are recognised, the values of these keys are stored in attributes.
        /// </summary>
        public static void LoadSession()
        {
            if (!File.Exists(SessionFile)) return;

            using var reader = File.OpenText(SessionFile);
            string line;

            while ((line = reader.ReadLine()) != null) // checking if we hit the eof, if not; use
            {
                var parts = line.Split(':', 2); // Splits the line, max 2 parts
                if (_sessions.ContainsKey(parts[0])) _sessions[parts[0]].Load(parts[1]);
            }

            reader.Close();
        }

        /// <summary>
        /// This function stores some attributes of the application to a config file.
        /// These values can later be used when the application is loaded.
        /// </summary>
        public static void SaveSession()
        {
            using var writer = File.CreateText(SessionFile);

            foreach (string key in _sessions.Keys)
                writer.WriteLine($"{key}:{_sessions[key].Save()}");

            writer.Close();
        }
    }
}
