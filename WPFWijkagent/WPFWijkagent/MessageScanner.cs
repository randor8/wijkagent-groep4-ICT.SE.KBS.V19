using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WijkagentModels;

namespace WijkagentWPF
{
    public enum State { Updated, Outdated }
    public class MessageScanner : IObservable
    {
        private Timer _timer;
        public Scraper Scraper = new Scraper(new Offence(new DateTime().ToLocalTime(), "", new Location(1.1, 1.1, 1), OffenceCategories.Null, 1));
         
        public State CurrentState { get; set; } = State.Updated;
        public long Id { get; set; }

        public List<DirectMessage> Messages = new List<DirectMessage>();
        private List<IObserver> _observers = new List<IObserver>();

        public MessageScanner(long id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets All Messages with the sender from Twitter
        /// </summary>
        /// <returns>List directMessage Objects</returns>
        public List<DirectMessage> GetConversation() // correct
        {
            List<DirectMessage> messages = new List<DirectMessage>();
            var results = Scraper.GetLatestDirectMessages()
                .Where(x => x.SenderId == Id || x.RecipientId == Id)
                .Select(x => new { x.SenderId, x.Id, x.Text, x.CreatedAt });
            foreach (var item in results)
            {
                messages.Add(new DirectMessage(item.SenderId,item.Id, item.Text, item.CreatedAt));
            }
            messages.Reverse();
            return messages;
        }

        /// <summary>
        /// Compares direct Message Lists to the current list in the MessageScanner, to Inform Observers of Changes
        /// </summary>
        /// <param name="UpdatedConversation"></param>
        public void CompareConversation(List<DirectMessage> UpdatedConversation )
        {
            int currentCount = Messages.Count;
            int newCount = UpdatedConversation.Count;
            if(Messages.Count == 0 || (newCount > currentCount && UpdatedConversation[newCount-1].Content != Messages[currentCount-1].Content))
            {
                Messages = UpdatedConversation;
                CurrentState = State.Outdated;
                Notify();
            }
        }
        /// <summary>
        /// Starts a Timer Tos start scanning Twitter
        /// </summary>
        /// <param name="interval"> the amount of microseconds per before the timer starts scanning</param>
        public void StartScanning(int interval)
        {
            _timer = new Timer(interval);
            _timer.AutoReset = true;
            _timer.Elapsed += ScanConversation;
            _timer.Start();
        }

        /// <summary>
        /// Stops the Twitter Scanning Timer
        /// </summary>
        public void StopScanning()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        /// <summary>
        /// Event connected to the Timer which gets and compares Twitter conversations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanConversation(object sender, ElapsedEventArgs e)
        {
            List<DirectMessage> messages = new List<DirectMessage>();
            messages = GetConversation();
            CompareConversation(messages);
        }

        /// <summary>
        /// Notifies all attached observers of a Update 
        /// </summary>
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        /// <summary>
        /// Attaches a observer to the scanner, so it can be notified
        /// </summary>
        /// <param name="observer"></param>
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }
    }
}
