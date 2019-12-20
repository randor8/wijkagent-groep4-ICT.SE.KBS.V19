using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentWPF
{
    public class Logger
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static Logger logger = null;
        private Logger()
        {

        }

        public static Logger Log
        {
            get
            {
                if (logger == null)
                {
                    logger = new Logger();
                }
                return logger;
            }
        }

        /// <summary>
        /// error messages
        /// </summary>
        private readonly string DBErrorMessage = "Er kan geen verbinding worden gemaakt met de database. Controleer alstublieft of u een werkende internet verbining heeft.";

        private readonly string APIErrorMessage = "Er kan geen verbinding worden gemaakt met Twitter. Controleer alstublieft of u een werkende internet verbining heeft.";

        /// <summary>
        /// sends the apropriate error from the application objects to the main window
        /// </summary>
        /// <param name="sender"></param>
        public void ErrorEventHandler(object sender)
        {
            if (sender is DBContext)
            {
                ErrorToScreenEvent?.Invoke(sender, DBErrorMessage);

            } else if(sender is Scraper)
            {
                ErrorToScreenEvent?.Invoke(sender, _APIErrorMessage);
            }
        }


        /// <summary>
        /// delegate for the error event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="message">message to display</param>
        public delegate void ErrorToSCreenHandler(object sender, string message);

        /// <summary>
        /// event to call when error ocurs
        /// </summary>
        public event ErrorToSCreenHandler ErrorToScreenEvent;
    }
}
