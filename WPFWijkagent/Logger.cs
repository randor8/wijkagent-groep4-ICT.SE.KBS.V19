﻿namespace WijkagentWPF
{
    public class Logger
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static Logger _logger = null;
        private Logger()
        { }

        public static Logger Log
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Logger();
                }
                return _logger;
            }
        }

        /// <summary>
        /// error messages
        /// </summary>
        private readonly string _DBErrorMessage = "Er kan geen verbinding worden gemaakt met de database. Controleer alstublieft of u een werkende internet verbing heeft.";

        private readonly string _APIErrorMessage = "Er kan geen verbinding worden gemaakt met Twitter. Controleer alstublieft of u een werkende internet verbing heeft.";

        /// <summary>
        /// sends the apropriate error from the application objects to the main window
        /// </summary>
        /// <param name="sender"></param>
        public void ErrorEventHandler(object sender)
        {
            if (sender is DBContext)
            {
                ErrorToScreenEvent?.Invoke(sender, _DBErrorMessage);
            }
            else if (sender is Scraper)
            {
                ErrorToScreenEvent?.Invoke(sender, _APIErrorMessage);
            }
        }


        /// <summary>
        /// delegate for the error event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="message">message to display</param>
        public delegate void ErrorToScreenHandler(object sender, string message);

        /// <summary>
        /// event to call when error ocurs
        /// </summary>
        public event ErrorToScreenHandler ErrorToScreenEvent;
    }
}
