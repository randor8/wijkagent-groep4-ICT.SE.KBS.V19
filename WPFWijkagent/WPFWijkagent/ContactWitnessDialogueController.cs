using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public class ContactWitnessDialogueController :IObserver
    {
        public List<DirectMessage> DirectMessages;
        public ContactWitnessDialogueController()
        {
            DirectMessages = new List<DirectMessage>();
        }

        public void Update(IObservable observable)
        {
            MessageScanner scanner = observable as MessageScanner;
            if (scanner.CurrentState == State.Outdated || DirectMessages.Count == 0 || scanner.Messages.Count > DirectMessages.Count)
            {
                DirectMessages = scanner.Messages;
                scanner.CurrentState = State.Updated;
            }
        }
    }
}
