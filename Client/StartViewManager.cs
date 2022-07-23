using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class StartViewManager
    {
        private Label labelChallenge;
        private Button buttonEnter;
        private TextBox textBoxName;
        SocketManager client;
        EventManager eventManager;
        public string userName;

        public Label LabelChallenge {
            get {
                return labelChallenge;
            }

            set {
                labelChallenge = value;
            }
        }

        public TextBox TextBoxName
        {
            get
            {
                return textBoxName;
            }
            set
            {
                textBoxName = value;
            }
        }

        public Button ButtonEnter {
            get {
                return buttonEnter;
            }

            set {
                buttonEnter = value;
            }
        }

        public StartViewManager(TextBox nameBox, SocketManager client, EventManager eventManager) {
            LabelChallenge = new Label()
            {
                Text = "Enter name player: ",
                Location = new Point(17, 17),
            };
            TextBoxName = new TextBox()
            {
                Size = nameBox.Size,
                Location = new Point(14, LabelChallenge.Location.Y + 2 * LabelChallenge.Height)
            };
            ButtonEnter = new Button()
            {
                Text = "Invite",
                Location = new Point(27, TextBoxName.Location.Y + 2 * TextBoxName.Height)
            };
            this.client = client;
            this.eventManager = eventManager;
        }

        //@funtion initPanelChallenge: init the panel challenge
        //@param panelchallenge
        public void initPanelChallenge(Panel panelchallenge) {
            
            panelchallenge.Controls.Add(LabelChallenge);
            panelchallenge.Controls.Add(TextBoxName);
            panelchallenge.Controls.Add(ButtonEnter);
            panelchallenge.Visible = true;

            ButtonEnter.Click += ButtonEnter_Click;
        }

        //@funtion showPanelChallenge: show the panel challenge
        //@param panelchallenge
        public void showPanelChallenge(Panel panelchallenge) {
            panelchallenge.Visible = true;
        }

        //@funtion hidePanelChallenge: hide the panel challenger
        //@param panelchallenge
        public void hidePanelChallenge(Panel panelchallenge) {
            panelchallenge.Visible = false;
        }

        //@funtion clearNamePlayer: clear the name in the text box challenger
        public void clearNamePlayer() {
            TextBoxName.Clear();
        }

        //@funtion ButtonEnter_Click: send challenger message to server
        private void ButtonEnter_Click(object sender, EventArgs e) {                      
            string name = TextBoxName.Text;
            if (String.Compare(name, userName) == 0)
            {
                MessageBox.Show("Username player is available");
                clearNamePlayer();
                
            }
            else if (String.Compare(name, "") == 0)
            {
                MessageBox.Show("Please enter name");
            }
            else
            {
                Message mess = new Message(Cons.CHALLENGE, name.Length.ToString(Cons.SAMPLE_0000), name);
                client.sendData(mess.convertToString());
                buttonEnter.Enabled = false;
            }
            client.ListenThread(eventManager);
        }
    }
}
