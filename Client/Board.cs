using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Board
    {
        private Panel board;        
        private List<List<Button>> matrix;
        private List<Player> player;      
        private List<Label> playerLabel;
        private int currentTurn;
        public int clientTurn;
        private FormPlay form;

        public Panel ChessBoard {
            get {
                return board;
            }
            set {
                board = value;
            }
        }

        public int CurrentTurn {
            get {
                return currentTurn;
            }
            set {
                currentTurn = value;
            }
        }

        public List<Player> Player {
            get {
                return player;
            }
            set {
                player = value;
            }
        }

        public List<Label> PlayerLabel
        {
            get {
                return playerLabel;
            }
            set {
                playerLabel = value;
            }
        }

        public List<List<Button>> Matrix {
            get {
                return matrix;
            }

            set {
                matrix = value;
            }
        }
        /*
        @Function:
        Create a new Board
        @Params:
        form: The form that contains this new Board
        playerLabel1: The label that hold player's 1 name
        playerLabel2: The label that hold player's 2 name
        @Return:

        */
        public Board(FormPlay form, Label playerLabel1, Label playerLabel2)
        {
            this.form = form;
            this.ChessBoard = form.panelBoard;
            this.PlayerLabel = new List<Label>()
            {
                playerLabel1,
                playerLabel2
            };

            this.Player = new List<Player>() {
                new Player(playerLabel1.Text, Properties.Resources.o),
                new Player(playerLabel2.Text, Properties.Resources.x)
            };

            EventManager.eventManager.Move += EventManager_Move;
            CurrentTurn = Constants.TURN_O;
            //Change current player name label
            PlayerLabel[CurrentTurn - 1].Font = new Font(PlayerLabel[CurrentTurn - 1].Font, FontStyle.Bold);
        }
        /*
        @Function:
        Return the coordinate of the button, respective to the board 
        @Params:
        btn: The button
        @Return:

        */
        public Point getButtonCoordinate(Button btn)
        {
            string coordinate = btn.Tag.ToString();
            int x = Convert.ToInt32(coordinate.Substring(0, coordinate.Length/2));
            int y = Convert.ToInt32(btn.Tag.ToString().Substring(coordinate.Length / 2, coordinate.Length / 2));
            Point point = new Point(x, y);
            return point;
        }
        /// <summary>
        /// <para>Return the coordinate from a string that represent a button's coordinate, respective to the board</para>
        /// <param name="btn">The string represent the button's coordinate </param>
        /// <returns>The point represent the button coordinate</returns>
        /// </summary>
        public Point getButtonCoordinate(String btn)
        {
            int x = btn[0];
            int y = btn[1];
            Point point = new Point(x, y);
            return point;
        }
        /// <summary>
        /// <para>Check if is current turn is this client's turn </para>
        /// <returns>If it is this client's turn, return true. Else return false</returns>
        /// </summary>
        public bool isClientTurn()
        {
            return this.clientTurn == this.currentTurn;
        }
        /// <summary>
        /// <para>Prepare the board layout for the match</para>
        /// <param name="boardChess">The panel control that will hold all the buttons for player to mark their moves</param>
        /// <returns>The point represent the button coordinate</returns>
        /// </summary>
        public void drawBoard(Panel boardChess) {
            boardChess.Enabled = true;
            boardChess.Controls.Clear();
            Matrix = new List<List<Button>>();
            Button previousBtn = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j <= Constants.BOARD_SIZE; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Constants.BUTTON_WIDTH,
                        Height = Constants.BUTTON_HEIGHT,
                        Location = new Point(previousBtn.Location.X + previousBtn.Width, previousBtn.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        BackColor = Color.Transparent,
                        Tag = j.ToString(Constants.SAMPLE_00) + i.ToString(Constants.SAMPLE_00)
                    };
                    btn.Click += Btn_Click;
                    boardChess.Controls.Add(btn);
                    Matrix[i].Add(btn);
                    previousBtn = btn;
                }

                // Move the last button to next row on the board's display
                previousBtn.Location = new Point(0, previousBtn.Location.Y + Constants.BUTTON_HEIGHT);
                previousBtn.Width = 0;
                previousBtn.Height = 0;
            }
        }

        /// <summary>
        /// <c>playerMakeMove</c> change the button back ground to the current player icon
        /// </summary>
        /// <param name="btn">The button to be changed</param>
        private void playerMakeMove(Button btn) {
            btn.BackgroundImage = Player[CurrentTurn-1].Mark;
        }
        /*
        @Function:
        Change this board current player to the next player, make neccessary changes to the display
        @Params:

        @Return:

        */
        /// <summary>
        /// <c></c>
        /// </summary>
        /// 
        private void changePlayer() {
            // Change current player name label
            PlayerLabel[CurrentTurn - 1].Font = new Font(PlayerLabel[CurrentTurn - 1].Font, FontStyle.Regular);
            CurrentTurn = CurrentTurn == Constants.TURN_X ? Constants.TURN_O : Constants.TURN_X;
            form.changeActivePictureBox(CurrentTurn);
            // Change other player name label
            PlayerLabel[CurrentTurn - 1].Font = new Font(PlayerLabel[CurrentTurn - 1].Font, FontStyle.Bold);
        }
        /*
        @Function:
        To fire when client receive a player move event from server
        @Params:

        @Return:
        sender: The object fired the event
        e: The event that was sent
        */
        /// <summary>
        /// <c></c>
        /// </summary>
        /// 
        private void EventManager_Move(object sender, SuperEventArgs e) {
            FormMain.App.BeginInvoke((MethodInvoker)(() =>
            {
                Point point = getButtonCoordinate(e.ReturnText);
                Button btn = Matrix[point.Y][point.X];
                if (btn.BackgroundImage != null)
                    return;
                playerMakeMove(btn);
                changePlayer();
            }));
        }
        /*
        @Function:
        To fire when a button on the board, which means a move was made
        @Params:

        @Return:
        sender: The object fired the event
        e: The event that was sent
        */
        private void Btn_Click(object sender, EventArgs e) {
            Button btn = sender as Button;
            // Check this client move is valid
            if (btn.BackgroundImage != null || !isClientTurn())
            {
                form.changeStatus("Hold your horses, champ. It's not your turn.");
                return;
            }
            Point point = getButtonCoordinate(btn);
            SocketManager.socketManager.sendData(new Message(Constants.OPCODE_PLAY, (ushort)(2 * Constants.LOCATION_SIZE), (byte)point.X, (byte)point.Y));
            playerMakeMove(btn);
            changePlayer();
        }
    }
}
