using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FormAccount : Form
    {
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        public FormAccount()
        {
            InitializeComponent();
            EventManager.eventManager.SignUp += EventManager_SignUp;
            EventManager.eventManager.SignIn += EventManager_SignIn;
            this.FormClosed += new FormClosedEventHandler(FormAccount_FormClosed);

            this.userNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.userNameTextBox_Validating);
            this.passwordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.passwordTextBox_Validating);
        }
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        private void FormAccount_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        private void EventManager_SignUp(object sender, SuperEventArgs e)
        {
            FormMain.App.BeginInvoke((MethodInvoker)(() =>
            {
                switch (e.ReturnCode)
                {
                    case Constants.OPCODE_SIGN_UP_SUCESS:
                        MessageBox.Show("Account has been created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case Constants.OPCODE_SIGN_UP_INVALID_USERNAME:
                        MessageBox.Show("Invalid username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case Constants.OPCODE_SIGN_UP_INVALID_PASSWORD:
                        MessageBox.Show("Invalid password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case Constants.OPCODE_SIGN_UP_DUPLICATED_USERNAME:
                        MessageBox.Show("Username is already used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default:
                        MessageBox.Show("Sign up failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }));
        }
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        private void EventManager_SignIn(object sender, SuperEventArgs e)
        {
            FormMain.App.BeginInvoke((MethodInvoker)(() =>
            {
                if (e.ReturnCode == Constants.OPCODE_SIGN_IN_SUCESS)
                {
                    this.FormClosed -= FormAccount_FormClosed;
                    this.Close();
                    SocketManager.socketManager.sendData(new Message(Constants.OPCODE_LIST));
                    SocketManager.socketManager.sendData(new Message(Constants.OPCODE_INFO, (ushort) userNameTextBox.Text.Length, userNameTextBox.Text));
                    FormMain.App.setPlayerName(this.userNameTextBox.Text);
                    FormManager.openForm(Constants.FORM_MAIN);
                return;
                }
                switch (e.ReturnCode)
                {
                    case Constants.OPCODE_SIGN_IN_ALREADY_LOGGED_IN:
                        MessageBox.Show("Already login!");
                        break;
                    case Constants.OPCODE_SIGN_IN_INVALID_PASSWORD:
                        MessageBox.Show("Invalid password!");
                        break;
                    case Constants.OPCODE_SIGN_IN_USERNAME_NOT_FOUND:
                        MessageBox.Show("Can't find username!");
                        break;
                    case Constants.OPCODE_SIGN_IN_WRONG_PASSWORD:
                        MessageBox.Show("Invalid password!");
                        break;
                    default:
                        MessageBox.Show("Login failed!", "Error");
                        break;
                }
            }));
            return;
        }
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        private void signInButton_Click(object sender, EventArgs e)
        {
            string username = userNameTextBox.Text;
            for (int i = username.Length; i < 20; i++)
            {
                username = username + " ";
            }
            string payload = username + Constants.SPACE + passwordTextBox.Text;
            Message sentMessage = new Message(Constants.OPCODE_SIGN_IN, (ushort)payload.Length, payload);
            SocketManager.socketManager.sendData(sentMessage);
        }
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        private void signUpButton_Click(object sender, EventArgs e)
        {
            string username = userNameTextBox.Text;
            for (int i = username.Length; i < 20; i++)
            {
                username = username + " ";
            }
            string payload = username + Constants.SPACE + passwordTextBox.Text;
            Message sentMessage = new Message(Constants.OPCODE_SIGN_UP, (ushort)payload.Length, payload);
            SocketManager.socketManager.sendData(sentMessage);
        }
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        void userNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (userNameTextBox.Text.Length == 0)
            {
                error = "Please enter an username!";
                e.Cancel = true;
            }
            else if (userNameTextBox.Text.Contains(" "))
            {
                error = "Username contains invalid character!";
                e.Cancel = true;
            }
            errorProvider1.SetError((Control)sender, error);
        }
        /// <summary>
        /// <para></para>
        /// 
        /// <returns></returns>
        /// </summary>
        void passwordTextBox_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (userNameTextBox.Text.Length == 0)
            {
                error = "Please enter a password!";
                e.Cancel = true;
            }
            else if (userNameTextBox.Text.Contains(" "))
            {
                error = "Password contains invalid character!";
                e.Cancel = true;
            }
            errorProvider1.SetError((Control)sender, error);
        }

    }
}
