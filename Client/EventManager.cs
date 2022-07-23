using System;
namespace Client
{
    public class EventManager
    {
        private static EventManager _eventManager;
        private event EventHandler<SuperEventArgs> _signup;
        private event EventHandler<SuperEventArgs> _signin;
        private event EventHandler<SuperEventArgs> _challenge;
        private event EventHandler<SuperEventArgs> _info;
        private event EventHandler<SuperEventArgs> _move;
        private event EventHandler<SuperEventArgs> _result;
        private event EventHandler<SuperEventArgs> _invite;
        private event EventHandler<SuperEventArgs> _list;
        private event EventHandler<SuperEventArgs> _signout;

        public static EventManager eventManager
        {
            get
            {
                if (_eventManager == null)
                {
                    _eventManager = new EventManager();
                }
                return _eventManager;
            }
        }

        public event EventHandler<SuperEventArgs> SignUp
        {
            add
            {
                _signup += value;
            }
            remove
            {
                _signup -= value;
            }
        }
        public event EventHandler<SuperEventArgs> SignIn
        {
            add {
                _signin += value;
            }
            remove {
                _signin -= value;
            }
        }

        public event EventHandler<SuperEventArgs> SignOut
        {
            add
            {
                _signout += value;
            }
            remove
            {
                _signout -= value;
            }
        }

        public event EventHandler<SuperEventArgs> Challenge {
            add {
                _challenge += value;
            }
            remove {
                _challenge -= value;
            }
        }

        public event EventHandler<SuperEventArgs> Info
        {
            add
            {
                _info += value;
            }
            remove
            {
                _info -= value;
            }
        }

        public event EventHandler<SuperEventArgs> Result {
            add {
                _result += value;
            }
            remove {
                _result -= value;
            }
        }

        public event EventHandler<SuperEventArgs> Move {
            add {
                _move += value;
            }
            remove {
                _move -= value;
            }
        }

        public event EventHandler<SuperEventArgs> Invite {
            add {
                _invite += value;
            }
            remove {
                _invite -= value;
            }
        }

        public event EventHandler<SuperEventArgs> List {
            add {
                _list += value;
            }
            remove {
                _list -= value;
            }
        }
        public void notifySignUp(int result)
        {
            if (_signup != null)
                _signup(this, new SuperEventArgs(result));
        }

        //@funtion notifyLogin: notifyy the login result to the event object when receiving a message
        //@param result: result
        public void notifySignIn(int result) {
            if (_signin != null)
                _signin(this, new SuperEventArgs(result));
        }

        public void notifySignout(int result)
        {
            if (_signout != null)
                _signout(this, new SuperEventArgs(result));
        }

        //@funtion notifyChallenge: notifyy the respond of other player to the event objecct when receiving a message
        //@param code: opcode of the meassage
        //@param name: name of the other player
        public void notifyChallenge(int code, string name) {
            if (_challenge != null)
                _challenge(this, new SuperEventArgs(code, name));
        }

        //@funtion notifyChallenge: notify the respond of other player to the event objecct when receiving a message
        //@param code: opcode of the meassage
        //@param name: name of the other player
        public void notifyInfo(int code, string info)
        {
            if (_info != null)
                _info(this, new SuperEventArgs(code, info));
        }

        //@funtion notifyMove: notifyy the move of opponent to the event objecct when receiving a message
        //@param move: string containing position of the move
        public void notifyMove(string move) {
            if (_move != null)
            {
                _move(this, new SuperEventArgs(move));
            }           
        }

        //@funtion notifyResult: notifyy the result of the game to the event object
        //@param name: name of the winner 
        public void notifyResult(string name) {
            if (_result != null)
                _result(this, new SuperEventArgs(name));
        }

        //@funtion notifyInvite: notifyy the challenger received
        //@param name: name of player challenging you
        public void notifyInvite(string name) {
            if (_invite != null)
                _invite(this, new SuperEventArgs(name));
        }

        //@funtion notifyList: notifyy the list player to event object
        //@param listname: string containing the list
        public void notifyList(string listname) {
            if (_list != null)
                _list(this, new SuperEventArgs(listname));
        }
    }

    public class SuperEventArgs : EventArgs
    {
        private int returnCode;
        private string returnText;

        public SuperEventArgs(int returnCode) {
            this.ReturnCode = returnCode;
        }

        public SuperEventArgs(string returnName) {
            this.ReturnText = returnName;
        }

        public SuperEventArgs(int returnCode, string returnName) {
            this.returnCode = returnCode;
            this.returnText = returnName;
        }

        public int ReturnCode {
            get {
                return returnCode;
            }

            set {
                returnCode = value;
            }
        }

        public string ReturnText {
            get {
                return returnText;
            }

            set {
                returnText = value;
            }
        }
    }
}
