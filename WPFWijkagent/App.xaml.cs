using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using WijkagentWPF.Session;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Dictionary<string, ASession> _sessions = new Dictionary<string, ASession>();
        public static readonly string ApplicationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wijkagent");
        public static readonly string SessionFile = "./Session.cfg";

        /// <summary>
        /// Creates an SSH tunnel for the database connection.
        /// </summary>
        public static void StartSSH()
        {
            var client = new SshClient(ConfigurationManager.AppSettings.Get("server_ssh_host"), ConfigurationManager.AppSettings.Get("server_ssh_user"), ConfigurationManager.AppSettings.Get("server_ssh_password"));
            var port = new ForwardedPortLocal("127.0.0.1", 1433, "127.0.0.1", 1433);

            client.Connect();
            client.AddForwardedPort(port);
            port.Start();
        }

        /// <summary>
        /// Stores a session object combined with its key.
        /// </summary>
        /// <param name="session">The ASession to be stored.</param>
        public static void RegisterSession(ASession session) => _sessions.Add(session.Key, session);

        /// <summary>
        /// Reads the session file and runs found ASessions to load values.
        /// </summary>
        public static void LoadSession()
        {
            string file = Path.Combine(ApplicationFolder, SessionFile);
            if (!File.Exists(file)) return;

            using var reader = File.OpenText(file);
            string line;

            while ((line = reader.ReadLine()) != null) // checking if we hit the eof, if not; use
            {
                var parts = line.Split(':', 2); // Splits the line, max 2 parts
                if (_sessions.ContainsKey(parts[0])) _sessions[parts[0]].Load(parts[1]);
            }

            reader.Close();
        }

        /// <summary>
        /// Runs all stored ASessions and writes away.
        /// </summary>
        public static void SaveSession()
        {
            Directory.CreateDirectory(ApplicationFolder); // Making sure the folder exists
            using var writer = File.CreateText(Path.Combine(ApplicationFolder, SessionFile));

            foreach (string key in _sessions.Keys)
                writer.WriteLine($"{key}:{_sessions[key].Save()}");

            writer.Close();
        }
    }
}
