using CaroChess_1312229.Models;
using CaroChess_1312229.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace CaroChess_1312229
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoardViewModel boardViewModel;
        public MainWindow()
        {
            InitializeComponent();
            CreateChessBoard();
            boardViewModel = new BoardViewModel();
            txtSend.GotFocus += new RoutedEventHandler(this.txtSend_GotFocus);
            txtSend.LostFocus += new RoutedEventHandler(this.txtSend_LostFocus);
        }

        private void CreateChessBoard()
        {
            SolidColorBrush defaultBrush = new SolidColorBrush(Colors.Gray);
            SolidColorBrush alternateBrush = new SolidColorBrush(Colors.White);
            for (int i = 0; i < 144; i++)
            {
                Rectangle cell = new Rectangle();
                cell.Fill = defaultBrush;
                cell.Stroke = alternateBrush;
                ChessBoard.Children.Add(cell);
            }
        }

        bool _flagonline = false;
        int x = -1;
        int y = -1;
        int tempX = -1;
        int tempY = -1;
        string PlayerAt = "1";

        Socket socket;
        private void ChessBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            #region //OFFLINE
            if (_flagonline == false)
            {
                if (radPlayervsPlayer.IsChecked == true)
                {
                    Point A = e.GetPosition(ChessBoard);
                    x = (int)(A.Y / boardViewModel.CurrentBoard.cellWidth);//toa do x cua o
                    y = (int)(A.X / boardViewModel.CurrentBoard.cellHeight);// toa do y cua o
                    int currcell = boardViewModel.CurrentBoard.currentCell(x, y);//lay o hien tai
                    if (boardViewModel.CurrentBoard.checkTrueBlock(boardViewModel.CurrentBoard.Cell, x, y) == true)
                    {
                        int kt = 0;
                        kt = boardViewModel.CurrentBoard.ktBeforePlayer(boardViewModel.CurrentBoard.Cell);//kt nguoi choi di truoc
                        if (kt == 1)
                        {
                            boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player1;
                            Ellipse elip = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip);
                            bool _ktWin = false;
                            _ktWin = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, x, y);
                            if (_ktWin == true)
                            {
                                MessageBox.Show("Violet Win !");
                                //ChessBoard.Children.Clear();
                                //CreateChessBoard(Board);
                                return;
                            }
                            
                            int _ktHoa = 2;
                            _ktHoa = boardViewModel.CurrentBoard.ktHoa(boardViewModel.CurrentBoard.Cell);
                            if (_ktHoa == 1)
                            {
                                MessageBox.Show("Hoa !");
                                //ChessBoard.Children.Clear();
                                //CreateChessBoard(Board);
                                return;
                            }
                        }
                        if (kt == 2)
                        {
                            boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player2;
                            Ellipse elip = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip);
                            bool _ktWin = false;
                            _ktWin = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, x, y);
                            if (_ktWin == true)
                            {
                                MessageBox.Show("Black Win !");
                                //ChessBoard.Children.Clear();
                                //CreateChessBoard(Board);
                                return;
                            }
                            
                            int _ktHoa = 2;
                            _ktHoa = boardViewModel.CurrentBoard.ktHoa(boardViewModel.CurrentBoard.Cell);
                            if (_ktHoa == 1)
                            {
                                MessageBox.Show("Hoa !");
                                //ChessBoard.Children.Clear();
                                //CreateChessBoard(Board);
                                return;
                            }
                        }

                    }
                    else
                        MessageBox.Show("O khong hop le !!!");
                }

                //PLAYER VS ROBOT
                if (radPlayervsComputer.IsChecked == true)
                {
                    Point A = e.GetPosition(ChessBoard);
                    x = (int)(A.Y / boardViewModel.CurrentBoard.cellWidth);//toa do x cua o
                    y = (int)(A.X / boardViewModel.CurrentBoard.cellHeight);// toa do y cua o
                    int currcell = boardViewModel.CurrentBoard.currentCell(x, y);
                    if (boardViewModel.CurrentBoard.checkTrueBlock(boardViewModel.CurrentBoard.Cell, x, y))
                    {
                        boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player1;
                        Ellipse elip = new Ellipse();
                        boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip);
                        ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                        ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip);
                        elip = null;
                        bool _ktWin = false;
                        _ktWin = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, x, y);
                        if (_ktWin == true)
                        {
                            MessageBox.Show("Violet Win !");
                            //ChessBoard.Children.Clear();
                            //CreateChessBoard(Board);
                            return;
                        }
                        int _ktHoa = 2;
                        _ktHoa = boardViewModel.CurrentBoard.ktHoa(boardViewModel.CurrentBoard.Cell);
                        if (_ktHoa == 1)
                        {
                            MessageBox.Show("Hoa !");
                            //ChessBoard.Children.Clear();
                            //CreateChessBoard(Board);
                            return;
                        }

                        //LUOT MAY DANH
                        boardViewModel.CurrentBoard.AI();
                        if (boardViewModel.CurrentBoard.fWin)
                        {
                            x = (int)boardViewModel.CurrentBoard.WinMove[0].X;
                            y = (int)boardViewModel.CurrentBoard.WinMove[0].Y;
                        }
                        else
                        {
                            boardViewModel.CurrentBoard.EvalChessBoard(CaroChess_1312229.Models.Board.Cells.Player2, ref boardViewModel.CurrentBoard.eBoard);
                            Point temp = new Point();
                            temp = boardViewModel.CurrentBoard.eBoard.MaxPos();
                            x = (int)temp.X;
                            y = (int)temp.Y;
                        }
                        boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player2;
                        elip = new Ellipse();
                        boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip);
                        ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                        ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip);
                        if (boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, x, y))
                        {
                            MessageBox.Show("Black Win !"); boardViewModel.CurrentBoard.fEnd = 2;
                            //ChessBoard.Children.Clear();
                            //CreateChessBoard(Board);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region //ONLINE
            if (_flagonline == true)
            {
                if (radPlayervsPlayer.IsChecked == true)
                {
                    Point A = e.GetPosition(ChessBoard);
                    x = (int)(A.Y / boardViewModel.CurrentBoard.cellWidth);//toa do x cua o
                    y = (int)(A.X / boardViewModel.CurrentBoard.cellHeight);// toa do y cua o
                    int currcell = boardViewModel.CurrentBoard.currentCell(x, y);//lay o hien tai
                    if (boardViewModel.CurrentBoard.checkTrueBlock(boardViewModel.CurrentBoard.Cell, x, y) == true)
                    {
                        if (PlayerAt == "0")
                        {
                            socket.Emit("MyStepIs", JObject.FromObject(new { row = x, col = y }));
                            boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player1;
                            Ellipse elip = new Ellipse();
                            //boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            //ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                            //ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip);
                        }
                        if (PlayerAt == "1")
                        {
                            socket.Emit("MyStepIs", JObject.FromObject(new { row = x, col = y }));
                            boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player1;
                            Ellipse elip = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip);
                            bool _ktWin = false;
                            _ktWin = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, x, y);
                            if (_ktWin == true)
                            {
                                MessageBox.Show("Violet Win !");
                                //ChessBoard.Children.Clear();
                                //CreateChessBoard(Board);
                                return;
                            }
                        }

                    }
                    else
                        MessageBox.Show("O khong hop le !!!");
                }

            }
            #endregion
        }


        private void btnOnline_Click(object sender, RoutedEventArgs e)
        {
            ChessBoard.Children.Clear();
            CreateChessBoard();
            _flagonline = true;
            btnOffline.IsEnabled = false;
            socket = IO.Socket("ws://gomoku-lajosveres.rhcloud.com:8000");
            string str2 = "You are the first player!";
            string str1 = null;
            socket.On("ChatMessage", (data) =>
            {
                var o = JObject.Parse(data.ToString());
                string strUser = null;
                if ((string)o["from"] != null)
                {
                    strUser = (string)o["from"];
                }
                else
                {
                    strUser = "Server";
                }

                this.Dispatcher.Invoke((Action)(() =>
                {
                    DateTime date = DateTime.Now;
                    txtChatBox.AppendText(strUser + ": " + ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() + "\n" + date.ToString() + "\n-----------------------------\n");

                }));

                if (((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() == "Welcome!")
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        socket.Emit("MyNameIs", txtPlayer.Text);
                        socket.Emit("ConnectToOtherPlayer");
                    }));
                }

                if (((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() == "This is not your turn!" || ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() == "Not allowed!")
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Rectangle rect = new Rectangle();
                        rect.Width = 40;
                        rect.Height = 40;
                        rect.Fill = Brushes.Gray;
                        rect.Stroke = Brushes.White;
                        boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.None;
                        ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                        ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), rect);
                    }));
                }

                str1 = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                if (str1.Contains(str2))//dieu kien la nguoi choi dau tien (the first)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        if (radPlayervsComputer.IsChecked == true)
                        {
                            Random r = new Random();
                            int x = r.Next(2) + 5;
                            int y = r.Next(2) + 6;
                            socket.Emit("MyStepIs", JObject.FromObject(new { row = x, col = y }));
                            boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player1;
                            int currcell = boardViewModel.CurrentBoard.currentCell(x, y);//lay o hien tai
                            Ellipse elip = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip);
                            bool _ktWin = false;
                            _ktWin = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, x, y);
                            if (_ktWin == true)
                            {
                                MessageBox.Show("Violet Win !");
                                //ChessBoard.Children.Clear();
                                //CreateChessBoard();
                                return;
                            }
                        }
                    }));
                }

            });

            socket.On("EndGame", (data) =>
            {
                var o = JObject.Parse(data.ToString());
                string strUser = null;
                if ((string)o["from"] != null)
                {
                    strUser = (string)o["from"];
                }
                else
                {
                    strUser = "Server";
                }
                this.Dispatcher.Invoke((Action)(() =>
                {
                    txtChatBox.AppendText(strUser + ": " + ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() + "\n");

                }));
            });

            socket.On("NextStepIs", (data) =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    string tempRow = ((JObject)data)["row"].ToString();
                    string tempCol = ((JObject)data)["col"].ToString();
                    tempX = Int32.Parse(tempRow);
                    tempY = Int32.Parse(tempCol);
                    PlayerAt = ((JObject)data)["player"].ToString();

                    #region //radPlayervsPlayer.IsChecked
                    if (radPlayervsPlayer.IsChecked == true)
                    {
                        if (PlayerAt == "1")
                        {
                            int currcell = boardViewModel.CurrentBoard.currentCell(tempX, tempY);//lay o hien tai
                            boardViewModel.CurrentBoard.Cell[tempX, tempY] = CaroChess_1312229.Models.Board.Cells.Player2;
                            Ellipse elip = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(tempX, tempY, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(tempX, tempY));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(tempX, tempY), elip);
                            bool _ktWin = false;
                            _ktWin = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, tempX, tempY);
                            if (_ktWin == true)
                            {
                                MessageBox.Show("Black Win !");
                                return;
                            }
                        }

                    }
                    #endregion

                    //-----------------------------------------------

                    #region //radPlayervsComputer.IsChecked
                    if (radPlayervsComputer.IsChecked == true)
                    {
                        if (PlayerAt == "1")
                        {
                            int currcell = boardViewModel.CurrentBoard.currentCell(tempX, tempY);//lay o hien tai
                            boardViewModel.CurrentBoard.Cell[tempX, tempY] = CaroChess_1312229.Models.Board.Cells.Player2;
                            Ellipse elip = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(tempX, tempY, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(tempX, tempY));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(tempX, tempY), elip);
                            bool _ktWin1 = false;
                            _ktWin1 = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, tempX, tempY);
                            if (_ktWin1 == true)
                            {
                                MessageBox.Show("Black Win !");
                                return;
                            }

                            boardViewModel.CurrentBoard.AI();
                            if (boardViewModel.CurrentBoard.fWin)
                            {
                                x = (int)boardViewModel.CurrentBoard.WinMove[0].X;
                                y = (int)boardViewModel.CurrentBoard.WinMove[0].Y;
                            }
                            else
                            {
                                boardViewModel.CurrentBoard.EvalChessBoard(CaroChess_1312229.Models.Board.Cells.Player2, ref boardViewModel.CurrentBoard.eBoard);
                                Point temp = new Point();
                                temp = boardViewModel.CurrentBoard.eBoard.MaxPos();
                                x = (int)temp.X;
                                y = (int)temp.Y;
                            }
                            socket.Emit("MyStepIs", JObject.FromObject(new { row = x, col = y }));
                            boardViewModel.CurrentBoard.Cell[x, y] = CaroChess_1312229.Models.Board.Cells.Player1;
                            currcell = boardViewModel.CurrentBoard.currentCell(x, y);
                            Ellipse elip1 = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(x, y, currcell, boardViewModel.CurrentBoard.Cell, elip1);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(x, y));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(x, y), elip1);
                            bool _ktWin = false;
                            _ktWin = boardViewModel.CurrentBoard.ktWin(boardViewModel.CurrentBoard.Cell, x, y);
                            if (_ktWin == true)
                            {
                                MessageBox.Show("Violet Win !");
                                return;
                            }
                        }
                        if (PlayerAt == "0")
                        {
                            int currcell = boardViewModel.CurrentBoard.currentCell(tempX, tempY);//lay o hien tai
                            boardViewModel.CurrentBoard.Cell[tempX, tempY] = CaroChess_1312229.Models.Board.Cells.Player1;
                            Ellipse elip = new Ellipse();
                            boardViewModel.CurrentBoard.UpdateBoard(tempX, tempY, currcell, boardViewModel.CurrentBoard.Cell, elip);
                            ChessBoard.Children.RemoveAt(boardViewModel.CurrentBoard.currentCell(tempX, tempY));
                            ChessBoard.Children.Insert(boardViewModel.CurrentBoard.currentCell(tempX, tempY), elip);
                        }
                    }
                    #endregion
                }));
            });
        }

        private void btnOffline_Click(object sender, RoutedEventArgs e)
        {
            _flagonline = false;
            btnOnline.IsEnabled = false;
        }


        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            ChessBoard.Children.Clear();
            CreateChessBoard();
            boardViewModel = new BoardViewModel();
            _flagonline = false;
            btnOnline.IsEnabled = true;
            btnOffline.IsEnabled = true;
            radPlayervsPlayer.IsChecked = false;
            radPlayervsComputer.IsChecked = false;
            txtChatBox.Clear();
        }

        #region // Chat

        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                socket.Emit("MyNameIs", txtPlayer.Text);

            }));
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_flagonline == true)
            {
                socket.Emit("ChatMessage", txtSend.Text.ToString());
                txtSend.Text = "";
            }
            if (_flagonline == false)
            {
                DateTime date = DateTime.Now;
                txtChatBox.AppendText(txtPlayer.Text + ": " + txtSend.Text + "\n" + date.ToString() + "\n-----------------------------\n");
                txtSend.Text = "";
            }
        }


        private void txtSend_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSend.Text == "Type your message here !")
            {
                txtSend.Text = "";
                txtSend.Foreground = Brushes.Black;
            }
        }

        private void txtSend_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSend.Text == "")
            {
                txtSend.Text = "Type your message here !";
                txtSend.Foreground = Brushes.LightGray;
            }
        }

        #endregion
    }
}
