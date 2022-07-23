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
    public partial class FormStart : Form
    {
        public StartViewManager startView;
        SocketManager client;
        EventManager eventManager;
        private string yourName;
        private string otherName;

        public FormStart() {
            InitializeComponent();

            client = new SocketManager();
            eventManager = new EventManager();
            startView = new StartViewManager(userNameTextBox, client, eventManager);
            startView.initPanelChallenge(panelChallenge);
            yourName = userNameTextBox.Text;

            eventManager.Signin += EventManager_Signin;
            eventManager.Signout += EventManager_Signout;
            eventManager.Respone += EventManager_Respone;
            eventManager.Invite += EventManager_Invite;
            eventManager.List += EventManager_List;

            namePlayerLabel.Visible = false;
            signoutButton.Visible = false;
            resultButton.Visible = false;
            panelChallenge.Visible = false;
            buttonReload.Enabled = false;
            listLabel.Visible = false;
                   
        }


        //@funtion exitButton_Click: exit the program
        private void exitButton_Click(object sender, EventArgs e) {
            try
            {
                client.closeSocket();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void signupButton_Click(object sender, EventArgs e)
        {
            FormRegister formRegister = new FormRegister(client);
            formRegister.ShowDialog();
        }

        //@funtion loginButton_Click: send the login message to server
        private void loginButton_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(userNameTextBox.Text))
            {
                MessageBox.Show("Please enter name!!!");
            }
            else if (String.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show("Please enter password!!!");
            }
            else
            {
                userNameTextBox.ReadOnly = true;
                passwordTextBox.ReadOnly = true;
                signinButton.Enabled = false;
                signupButton.Enabled = false;
                yourName = userNameTextBox.Text;
                if (client.connectServer())
                {
                    string payload = yourName + Cons.SPACE + passwordTextBox.Text;
                    Message mess = new Message(Cons.SIGNIN, payload.Length.ToString(Cons.SAMPLE_0000), payload);
                    client.sendData(mess.convertToString());
                    client.ListenThread(eventManager);
                }
                else
                {
                    MessageBox.Show("Connected failed!");
                    userNameTextBox.ReadOnly = false;
                    signinButton.Enabled = true;
                    signupButton.Enabled = true;
                }
            }
        }

        //@funtion logoutButton_Click: send the logout message to server
        private void logoutButton_Click(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Do you want to log out?", "Question", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                signoutButton.Enabled = false;
                Message mess = new Message(Cons.SIGNOUT, Cons.SAMPLE_0000, "");
                client.sendData(mess.convertToString());
                client.ListenThread(eventManager);
            }
        }

        private void resultButton_Click(object sender, EventArgs e)
        {
            Message mess = new Message(Cons.RESULT, Cons.SAMPLE_0000, "");
            client.sendData(mess.convertToString());
            client.ListenThread(eventManager);
            resultButton.Enabled = false;
        }

        //@funtion buttonReload_Click: send the message that want to reload list free player to server
        private void buttonReload_Click(object sender, EventArgs e) {
            Message mess = new Message(Cons.LIST, Cons.SAMPLE_0000, "");
            client.sendData(mess.convertToString());
            client.ListenThread(eventManager);
            buttonReload.Enabled = false;
        }

        //@funtion EventManager_Login: show the login result to player
        private void EventManager_Signin(object sender, SuperEventArgs e) {
            this.Invoke((MethodInvoker)(() =>
            {
                if (e.ReturnCode == 21)
                {
                    MessageBox.Show("Login successful!");

                    startView.showPanelChallenge(panelChallenge);
                    signinButton.Visible = false;
                    signupButton.Visible = false;
                    signoutButton.Visible = true;
                    signoutButton.Enabled = true;
                    resultButton.Visible = true;
                    resultButton.Enabled = true;
                    buttonReload.Enabled = true;
                    namePlayerLabel.Visible = true;
                    userNameLabel.Visible = false;
                    passwordLabel.Visible = false;
                    passwordTextBox.Visible = false;
                    listLabel.Visible = true;
                    
                    startView.userName = userNameTextBox.Text;

                    client.ListenThread(eventManager);
                }
                else
                {
                    if (e.ReturnCode == 22)
                    {
                        MessageBox.Show("Already login!");
                    }
                    else if (e.ReturnCode == 23)
                    {
                        MessageBox.Show("Incorrect password!");
                    }
                    else if (e.ReturnCode == 24)
                    {
                        MessageBox.Show("Not found player");
                    }
                    else
                    {
                        MessageBox.Show("Login fail");
                    }
                    userNameTextBox.ReadOnly = false;
                    passwordTextBox.ReadOnly = false;
                    signinButton.Enabled = true;
                    signupButton.Enabled = true;
                    client.closeSocket();
                }
                
            }));
        }

        //@funtion EventManager_Respone: show to respond of other players after sending a challenger
        private void EventManager_Respone(object sender, SuperEventArgs e) {
            this.Invoke((MethodInvoker)(() =>
            {
                if (e.ReturnCode == 51)
                {
                    if(String.Compare(e.ReturnText, "") == 0)
                    {
                        MessageBox.Show("Game started!");

                        FormPlay formPlay = new FormPlay(otherName, yourName, client, eventManager, 2);
                        formPlay.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Challenge accepted!");
                        otherName = e.ReturnText;
                        startView.clearNamePlayer();
                        startView.ButtonEnter.Enabled = true;

                        FormPlay formPlay = new FormPlay(yourName, otherName, client, eventManager, 1);
                        formPlay.ShowDialog();
                        
                    }                    
                }
                else if(e.ReturnCode == 61)
                {
                    string[] words = e.ReturnText.Split(' ');
                    MessageBox.Show("Score: " + words[0] + "\n" + "Rank: " + words[1], "Result");
                    resultButton.Enabled = true;
                }
                else
                {
                    if (e.ReturnCode == 52)
                    {
                        MessageBox.Show("Challenge refuse!");
                    }
                    else if (e.ReturnCode == 53)
                    {
                        MessageBox.Show("Can't play with player has too high or too low rank!");
                    }
                    else if (e.ReturnCode == 54)
                    {
                        MessageBox.Show("Player is playing!");
                    }
                    else if (e.ReturnCode == 55)
                    {
                        MessageBox.Show("Not found player!");
                    }
                    startView.ButtonEnter.Enabled = true;
                    startView.clearNamePlayer();
                    client.ListenThread(eventManager);
                }
            }));
        }

        //@funtion EventManager_Invite: show the invitation from other player
        private void EventManager_Invite(object sender, SuperEventArgs e) {
            this.Invoke((MethodInvoker)(() =>
            {
                otherName = e.ReturnText;
                DialogResult dialogResult = MessageBox.Show(otherName + " want to challenge you. Do you accept?", "Invite", MessageBoxButtons.YesNo);
                
                if (dialogResult == DialogResult.Yes)
                {
                    Message mess = new Message(Cons.ACCEPT, otherName.Length.ToString(Cons.SAMPLE_0000), otherName);
                    client.sendData(mess.convertToString());
                    client.ListenThread(eventManager);
                }
                else if (dialogResult == DialogResult.No)
                {
                    Message mess = new Message(Cons.REFUSE, otherName.Length.ToString(Cons.SAMPLE_0000), otherName);
                    client.sendData(mess.convertToString());
                    client.ListenThread(eventManager);
                }                
            }));
        }

        //@funtion EventManager_List: show the list free player 
        private void EventManager_List(object sender, SuperEventArgs e) {
            string listname = e.ReturnText;

            listPlayer.Items.Clear();

            while (String.Compare(listname, "") != 0) 
            {
                int length = listname.Length;
                int i;
                string name;
                for (i = 0; i < length; i++)
                {
                    if (listname[i] == Cons.SPACE[0]) break;
                }

                if (i == length)
                {
                    listPlayer.Items.Add(listname);
                    break;
                }

                name = listname.Substring(0, i);

                listname = listname.Remove(0, i + 1);

                listPlayer.Items.Add(name);
            }                       
            client.ListenThread(eventManager);

            buttonReload.Enabled = true;
        }

        private void EventManager_Signout(object sender, SuperEventArgs e)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                if (e.ReturnCode == 31)
                {
                    MessageBox.Show("Log out successful");
                    signinButton.Visible = true;
                    signinButton.Enabled = true;
                    signupButton.Visible = true;
                    signupButton.Enabled = true;
                    signoutButton.Visible = false;
                    resultButton.Visible = false;
                    userNameTextBox.ReadOnly = false;
                    passwordTextBox.ReadOnly = false;
                    namePlayerLabel.Visible = false;
                    userNameLabel.Visible = true;
                    passwordLabel.Visible = true;
                    passwordTextBox.Visible = true;
                    passwordTextBox.Clear();
                    userNameTextBox.Clear();
                    listLabel.Visible = false;

                    client.closeSocket();
                    startView.hidePanelChallenge(panelChallenge);
                    listPlayer.Items.Clear();
                    buttonReload.Enabled = false;
                }
                else if(e.ReturnCode == 32)
                {
                    MessageBox.Show("Log out failed");
                    signoutButton.Enabled = true;
                    client.ListenThread(eventManager);
                }
                
            }));
        }
    }
}
