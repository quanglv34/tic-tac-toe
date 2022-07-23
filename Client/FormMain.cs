using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FormMain : Form
    {
        private static FormMain _app;
        public string playerName;
        public string opponentName;
        private bool isFree;
        /*
         * 
         */
        public static FormMain App
        {
            get
            {
                if (_app == null)
                {
                    _app = new FormMain();
                }
                return _app;
            }
        }
        /*
         * 
         */
        public FormMain() {
            InitializeComponent();
            EventManager.eventManager.Challenge += EventManager_Challenge;
            EventManager.eventManager.Invite += EventManager_Invite;
            EventManager.eventManager.List += EventManager_List;
            EventManager.eventManager.SignOut += EventManager_SignOut;
            EventManager.eventManager.Info += EventManager_Info;
            this.isFree = false;
            this.Shown += FormMain_Shown;
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);
            this.FormClosed += new FormClosedEventHandler(FormMain_FormClosed);
        }
        /*
         * 
         */
        public void setPlayerName(string name)
        {
            this.userNameInfo.Text = name;
            this.toolStripStatusLabel1.Text = "Welcome player " + name + "!";
        }

        public System.Windows.Forms.SaveFileDialog getSaveFileDialog()
        {
            return this.saveFileDialog1;
        }
        /*
         * 
         */
        public string getPlayerName()
        {
            return this.userNameInfo.Text;
        }
        /*
         * 
         */
        public void FormMain_Shown(Object sender, EventArgs e)
        {
            FormManager.openForm(Constants.FORM_ACCOUNT);
            this.isFree = true;
        }
        /*
         * 
         */
        public void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Confirm exit?", "Warning", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
        }
        /*
         * 
         */
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SocketManager.socketManager.closeSocket();
            Application.Exit();
        }
        /*
         * 
         */
        private void exitButton_Click(object sender, EventArgs e) {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /*
         * 
         */
        private void signOutButton_Click(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Do you confirm to log out?", "Question", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.challengedPlayerName.Clear();
                this.listPlayer.Clear();
                signOutButton.Enabled = false;
                SocketManager.socketManager.sendData(new Message(Constants.OPCODE_SIGN_OUT));
            }
        }
        /*
         * 
         */
        private void infoButton_Click(object sender, EventArgs e)
        {
            infoButton.Enabled = false;
            SocketManager.socketManager.sendData(new Message(Constants.OPCODE_INFO));
        }
        /*
         * 
         */
        private void reloadButton_Click(object sender, EventArgs e) {
            reloadButton.Enabled = false;
            SocketManager.socketManager.sendData(new Message(Constants.OPCODE_LIST));
        }

        private void challengeBtn_Click(object sender, EventArgs e)
        {
            challengeBtn.Enabled = false;
            string challengedUsername = challengedPlayerName.Text;
            opponentName = challengedUsername;
            Message sentMessage = new Message(Constants.OPCODE_CHALLENGE, (ushort) challengedUsername.Length, challengedUsername);
            SocketManager.socketManager.sendData(sentMessage);
        }
        /*
         * 
         */
        private void EventManager_Challenge(object sender, SuperEventArgs e) {
            FormMain.App.BeginInvoke((MethodInvoker)(() =>
            {
                challengeBtn.Enabled = true;
                if (e.ReturnCode == Constants.OPCODE_CHALLENGE_ACCEPT)
                {
                    if (String.Compare(e.ReturnText, "") == 0)
                    {
                        MessageBox.Show("Let the game begin!");
                        FormManager.openForm(Constants.FORM_PLAY, e);
                    }
                    else
                    {
                        opponentName = e.ReturnText;
                        MessageBox.Show("Challenge accepted!");
                        FormManager.openForm(Constants.FORM_PLAY, e);
                    }

                }
                else
                {
                    if (e.ReturnCode == Constants.OPCODE_CHALLENGE_REFUSE)
                    {
                        MessageBox.Show("Your challenge is refused!");
                    }
                    else if (e.ReturnCode == Constants.OPCODE_CHALLENGE_INVALID_RANK)
                    {
                        MessageBox.Show("Rank difference can't be more than 10 !");
                    }
                    else if (e.ReturnCode == Constants.OPCODE_CHALLENGE_BUSY)
                    {
                        MessageBox.Show("The player is playing!");
                    }
                    else if (e.ReturnCode == Constants.OPCODE_CHALLENGE_NOT_FOUND)
                    {
                        MessageBox.Show("Sorry, we can't find that player!");
                    }
                }
            }));
        }
        /*
         * 
         */
        private void EventManager_Info(object sender, SuperEventArgs e)
        {
            if (e.ReturnCode == Constants.OPCODE_INFO_FOUND)
            {
                FormMain.App.BeginInvoke((MethodInvoker)(() =>
                {
                    string[] words = e.ReturnText.Split(' ');
                    userRankInfo.Text = words[1];
                    userScoreInfo.Text = words[0];
                    infoButton.Enabled = true;
                }));
            } else
            {
                if (e.ReturnCode == Constants.OPCODE_INFO_NOT_FOUND)
                {
                    MessageBox.Show("Sorry, we can't find that player info!");
                }
            }
        }
        /*
         * 
         */
        private void EventManager_Invite(object sender, SuperEventArgs e) {
            this.opponentName = e.ReturnText;
            if(!isFree)
            {
                Message sentMessage = new Message(Constants.OPCODE_CHALLENGE_REFUSE, (ushort)opponentName.Length, opponentName);
                SocketManager.socketManager.sendData(sentMessage);
                return;
            }
            string msg = opponentName.Substring(0,opponentName.Length-1) + " sent a challenged. Accept?";
            DialogResult dialogResult = MessageBox.Show(msg, "Challenge incoming!", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);   
            if (dialogResult == DialogResult.Yes)
            {
                Message sentMessage = new Message(Constants.OPCODE_CHALLENGE_ACCEPT, (ushort) opponentName.Length, opponentName);
                SocketManager.socketManager.sendData(sentMessage);
            }
            else if (dialogResult == DialogResult.No)
            {
                Message sentMessage = new Message(Constants.OPCODE_CHALLENGE_REFUSE, (ushort) opponentName.Length, opponentName);
                SocketManager.socketManager.sendData(sentMessage);
            }                
        }
        /*
         * 
         */
        private void EventManager_List(object sender, SuperEventArgs e) {
            FormMain.App.BeginInvoke((MethodInvoker)(() =>
            {
                string listname = e.ReturnText;

                listPlayer.Items.Clear();

                string[] list = listname.Split(' ');

                if(list.Length > 1)
                {
                    this.playerListStatus.Hide();

                    for (int i = 0; i< list.Length; i++)
                    {
                        listPlayer.Items.Add(list[i]);
                    }

                } else
                {
                    this.playerListStatus.Show();
                }

                reloadButton.Enabled = true;
            }));
        }
        /*
         * 
         */
        private void EventManager_SignOut(object sender, SuperEventArgs e)
        {
            FormMain.App.BeginInvoke((MethodInvoker) (() =>
            {
                signOutButton.Enabled = true;
                if (e.ReturnCode == Constants.OPCODE_SIGN_OUT_SUCCESS)
                {
                        FormManager.openForm(Constants.FORM_ACCOUNT, e);
                }
                else if(e.ReturnCode == Constants.OPCODE_SIGN_OUT_NOT_LOGGED_IN)
                {
                    MessageBox.Show("Sign out failed!");
                }
            }));
        }
        /*
         * 
         */
        public void changeStatus(string status)
        {
            this.toolStripStatusLabel1.Text = status;
        }

    }
}
