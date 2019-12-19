using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentWPF
{
    public class Logger
    {

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

        private readonly string DBErrorMessage = "Er kan geen verbinding worden gemaakt met de database. Controleer alstublieft of u een werkende internet verbining heeft.";
        private readonly string APIErrorMessage = "Er kan geen verbinding worden gemaakt met Twitter. Controleer alstublieft of u een werkende internet verbining heeft.";

        public void ErrorEventHandler(object sender)
        {
            if (sender is DBContext)
            {
                ErrorToScreenEvent?.Invoke(sender, DBErrorMessage);

            } else if(sender is Scraper)
            {
                ErrorToScreenEvent?.Invoke(sender, APIErrorMessage);
            }
        }

        public delegate void ErrorToSCreenHandler(object sender, string message);

        public event ErrorToSCreenHandler ErrorToScreenEvent;
    }
}
