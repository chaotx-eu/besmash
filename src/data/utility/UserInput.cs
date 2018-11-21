namespace BesmashGame.Config {
    using GSMXtended;
    using GameStateManagement;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System.Runtime.Serialization;

    [DataContract]
    public class UserInput {
        // [DataMember]
        // public string ID {get; set;}

        [DataMember]
        public List<Keys> TriggerKeys {get; set;}

        [DataMember]
        public List<Buttons> TriggerButtons {get; set;}

        public UserInput(string id) {
            TriggerKeys = new List<Keys>();
            TriggerButtons = new List<Buttons>();
        }

        public UserInput(UserInput other) {
            TriggerKeys = new List<Keys>(other.TriggerKeys);
            TriggerButtons = new List<Buttons>(other.TriggerButtons);
        }

        /// Helper for checking if any of the trigger
        /// keys or buttons is currently pressed
        public bool isTriggered(InputState inputState) {
            PlayerIndex playerIndex;

            foreach(Keys key in TriggerKeys) {
                if(inputState.IsNewKeyPress(key, null, out playerIndex))
                    return true;
            }

            foreach(Buttons button in TriggerButtons) {
                if(inputState.IsNewButtonPress(button, null, out playerIndex))
                    return true;
            }

            return false;
        }

        public override bool Equals(object obj) {
            if(obj == null || obj.GetType() != this.GetType())
                return false;

            UserInput other = (UserInput)obj;
            if(TriggerKeys.Count != other.TriggerKeys.Count
            || TriggerButtons.Count != other.TriggerButtons.Count)
                return false;

            foreach(Keys key in TriggerKeys)
                if(!other.TriggerKeys.Contains(key))
                    return false;

            foreach(Buttons button in TriggerButtons)
                if(!other.TriggerButtons.Contains(button))
                    return false;

            return true;
        }
    }
}