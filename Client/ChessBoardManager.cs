﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public class ChessBoardManager
    {
        private Panel chessBoard;        
        private List<Player> player;      
        private int currentPlayer;
        private List<TextBox> namePlayer;
        private List<List<Button>> matrix;
        private Message message;
        private SocketManager client;
        private EventManager eventManager;

        public Panel ChessBoard {
            get {
                return chessBoard;
            }
            set {
                chessBoard = value;
            }
        }
        public int CurrentPlayer {
            get {
                return currentPlayer;
            }
            set {
                currentPlayer = value;
            }
        }
        public Message Message {
            get {
                return message;
            }

            set {
                message = value;
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
        public List<TextBox> NamePlayer {
            get {
                return namePlayer;
            }
            set {
                namePlayer = value;
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

        public ChessBoardManager(Panel chessBoard, TextBox namePlayer1, TextBox namePlayer2, SocketManager client, EventManager eventManager)
        {
            this.client = client;
            this.ChessBoard = chessBoard;
            this.NamePlayer = new List<TextBox>()
            {
                namePlayer1,
                namePlayer2
            };
            
            this.Player = new List<Player>() {
                new Player(this.NamePlayer[0].Text, Image.FromFile("C:\\Users\\dattt\\Desktop\\caro_game\\Client\\bin\\Debug\\imagine\\x.png")),
                new Player(this.NamePlayer[1].Text, Image.FromFile("C:\\Users\\dattt\\Desktop\\caro_game\\Client\\bin\\Debug\\imagine\\o.png"))
            };

            this.eventManager = eventManager;
            this.eventManager.Move += EventManager_Move;

            CurrentPlayer = 0;
            NamePlayer[CurrentPlayer].BackColor = Color.FromArgb(100, 214, 179);
        }

        //@function getChessPoint: get a point from a button clicked
        //@param btn: the button
        //@return point: the point need to be got 
        public Point getChessPoint(Button btn)
        {
            int x = Convert.ToInt32(btn.Tag.ToString().Substring(0, Cons.LOCATION_SIZE));
            int y = Convert.ToInt32(btn.Tag.ToString().Substring(Cons.LOCATION_SIZE, Cons.LOCATION_SIZE));
            Point point = new Point(x, y);
            return point;
        }

        //@function getChessPoint: get a point with position being from string
        //@param btn: string containing the position
        //@return point: the poing need to be got
        public Point getChessPoint(String btn)
        {
            int x = Convert.ToInt32(btn.ToString().Substring(0, Cons.LOCATION_SIZE));
            int y = Convert.ToInt32(btn.ToString().Substring(Cons.LOCATION_SIZE, Cons.LOCATION_SIZE));
            Point point = new Point(x, y);
            return point;
        }



        //@function drawBoard: draw the chess board in a panel
        //@param boardChess: the panel 
        public void drawBoard(Panel boardChess) {
            boardChess.Enabled = true;
            boardChess.Controls.Clear();

            Matrix = new List<List<Button>>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Cons.BOARD_SIZE; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.BOARD_SIZE + 1; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.BUTTON_WIDTH,
                        Height = Cons.BUTTON_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString(Cons.SAMPLE_00) + j.ToString(Cons.SAMPLE_00)
                    };
                    btn.Click += Btn_Click;
                    boardChess.Controls.Add(btn);
                    Matrix[i].Add(btn);
                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Cons.BUTTON_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }

        //@function Mark: mark a position with x or o when player take a move
        //@param btn: the button containing the position
        private void Mark(Button btn) {
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
        }

        //@funtion changePlayer: change turn game
        private void changePlayer() {
            NamePlayer[CurrentPlayer].BackColor = Color.White;

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            NamePlayer[CurrentPlayer].BackColor = Color.FromArgb(100, 214, 179);
        }

        //@funtion EventManager_Move: show the move from the opponent
        private void EventManager_Move(object sender, SuperEventArgs e) {
            chessBoard.Enabled = true;
            Point point = getChessPoint(e.ReturnText);

            Button btn = Matrix[point.X][point.Y];
            if (btn.BackgroundImage != null)
                return;
            Mark(btn);

            changePlayer();
        }

        //@funtiom Btn_Click: send the move of player to the opponent
        private void Btn_Click(object sender, EventArgs e) {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null)
                return;

            Point point = getChessPoint(btn);
            Message = new Message((char) Cons.OPCODE_PLAY, (ushort) (2 * Cons.LOCATION_SIZE), (ushort) point.X, (ushort) point.Y);
            client.sendData(Message.convertToString());

            Mark(btn);

            changePlayer();

            chessBoard.Enabled = false;
            client.ListenThread(eventManager);
        }
    }
}
