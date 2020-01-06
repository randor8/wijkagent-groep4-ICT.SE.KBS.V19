using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public class ContactWitnessDialogueController :IObserver
    {
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
                directMessages = scanner._messages;
                scanner.CurrentState = State.Updated;
            }
        }
    }
}
