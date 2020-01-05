using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public class ContactWitnessDialogueController :IObserver
    {
        long UserID = 1194224344144269312;
        public List<DirectMessage> directMessages;

        public long _witnessID { get; set; }
        //TODO: Add Dialogue
        public ContactWitnessDialogueController(long id)
        {
            directMessages = new List<DirectMessage>();
            _witnessID = id;
        }

        public void Update(IObservable observable)
        {
            MessageScanner scanner = observable as MessageScanner;
            if (scanner.CurrentState == State.Outdated || directMessages.Count == 0)
            {
                Console.WriteLine($"Na de update lijst is equal to {scanner._messages.Count}");
                Console.WriteLine("Update reached");
                directMessages = scanner._messages;
                scanner.CurrentState = State.Updated;
                foreach (var item in directMessages)
                {
                    Console.WriteLine(item._senderID);
                }
            }
        }

        // voor later
        //public void UpdateFromDialogue(string data)
        //{
        //    DirectMessage message = new DirectMessage(UserID, data, DateTime.Now);
        //    directMessages.Add(message);
        //}

        







    }
}
