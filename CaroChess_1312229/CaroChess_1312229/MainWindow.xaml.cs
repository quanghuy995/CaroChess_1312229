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
        }

        bool _flagonline = false;
        int x = -1;
        int y = -1;
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
        }

        private void btnOffline_Click(object sender, RoutedEventArgs e)
        {
            _flagonline = false;
            btnOnline.IsEnabled = false;
        }

    }
}
