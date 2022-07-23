using System;
using System.Windows.Forms;

namespace Client
{
    public partial class FormRegister : Form
    {
        SocketManager client;
        EventManager eventManager;
        private string userName;
        private string password;

        public FormRegister(SocketManager client)
        {
            InitializeComponent();
            eventManager = new EventManager();
            this.client = client;
            eventManager.Signup += EventManager_Signup;
        }
        private void EventManager_Signup(object sender, SuperEventArgs e)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                client.closeSocket();
                if (e.ReturnCode == 11)
                {
                    MessageBox.Show("Register successful!");
                    this.Close();
                }
                else
                {
                    if (e.ReturnCode == 12)
                    {
                        MessageBox.Show("Username is available", "Error");

                    }
                    else if (e.ReturnCode == 13)
                    {
                        MessageBox.Show("Username or password is invalid", "Error");
                    }
                    else
                    {
                        MessageBox.Show("Register fail", "Error");
                    }
                    userNameTextBox.ReadOnly = false;
                    passwordTextBox.ReadOnly = false;
                    registerButton.Enabled = true;
                    
                }
            }));
        }
        private void registerButton_Click(object sender, EventArgs e)
        {
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
                registerButton.Enabled = false;
                userName = userNameTextBox.Text;
                password = passwordTextBox.Text;
                if (client.connectServer())
                {
                    string payload = userName + Cons.SPACE + password;
                    Message mess = new Message(Cons.SIGNUP, payload.Length.ToString(Cons.SAMPLE_0000), payload);
                    client.sendData(mess.convertToString());
                    client.ListenThread(eventManager);
                }
                else
                {
                    MessageBox.Show("Connected failed!");
                    userNameTextBox.ReadOnly = false;
                    passwordTextBox.ReadOnly = false;
                    registerButton.Enabled = true;
                }
            }
        }
    }
}

