using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CaroChess_1312229.Models
{
    class Board
    {
        private int _cellWidth = 40;
        private int _cellHeight = 40;
        private int _Row = 12;
        private int _Col = 12;

        public static int maxDepth = 11;
        public static int maxMove = 3;
        public int depth = 0;

        public Point[] PCMove = new Point[maxMove + 2];
        Point[] HumanMove = new Point[maxMove + 2];
        public Point[] WinMove = new Point[maxDepth + 2];
        Point[] LoseMove = new Point[maxDepth + 2];

        public int[] DScore = new int[5] { 0, 1, 9, 81, 729 };
        public int[] AScore = new int[5] { 0, 2, 18, 162, 1458 };

        public bool fWin = false;
        public int fEnd = 1;

        public EvalBoard eBoard = new EvalBoard();
        public Cells[,] Cell { get; set; }
        public enum Cells
        {
            None = 0,
            Player1 = 1,
            Player2 = 2
        };

        public Board()
        {
             Cell = new Cells[12, 12];
             for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                {
                    Cell[i, j] = Cells.None;
                }
        }
        public int cellWidth
        {
            set
            {
                _cellWidth = value;
            }
            get
            {
                return _cellWidth;
            }
        }

        public int cellHeight
        {
            set
            {
                _cellHeight = value;
            }
            get
            {
                return _cellHeight;
            }
        }
        public int Row
        {
            set
            {
                _Row = value;
            }
            get
            {
                return _Row;
            }
        }

        public int Col
        {
            set
            {
                _Col = value;
            }
            get
            {
                return _Col;
            }
        }

        public int currentCell(int x, int y)
        {
            int _currcell = x * 12 + y;
            return _currcell;
        }

        public bool checkTrueBlock(Cells[,] Cell, int x, int y)
        {
            if (Cell[x, y] == Cells.None)
                return true;
            return false;
        }

        public int ktBeforePlayer(Cells[,] Cell)//kt nguoi nao danh truoc
        {
            int dem = 0;//so so o la cua nguoi choi 1 hay nguoi choi 2
            for (int i = 0; i < _Row; i++)
            {
                for (int j = 0; j < _Col; j++)
                {
                    if (Cell[i, j] == Cells.Player1 || Cell[i, j] == Cells.Player2)
                    {
                        dem++;
                    }
                }
            }
            if (dem % 2 == 0)
                return 1;
            else
                return 2;

        }

        #region Kiem tra Win
        private bool checkHopLe(int row, int col)
        {
            if (row < 0 || row > 11 || col < 0 || col > 11)
                return false;
            return true;
        }

        public bool ktWin(Cells[,] Cell, int row, int col)
        {
            int kt1 = 0; int kt2 = 0; int kt3 = 0; int kt4 = 0;
            int k = row;
            int l = col;
            int dem1 = 0; int dem2 = 0; int dem3 = 0; int dem4 = 0;
            //kt duong cheo thư nhất
            int currPlayer = 0;
            currPlayer = ktBeforePlayer(Cell);
            //kt duong cheo 1
            while (Cell[row, col] == Cells.Player2)
            {
                while (Cell[row, col] == Cells.Player2)
                {
                    dem1++;
                    row++; col++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem1--;
                while (Cell[row, col] == Cells.Player2)
                {
                    dem1++;
                    row--; col--;
                    if (!checkHopLe(row, col))
                    {
                        kt1 = 1;
                        break;
                    }
                }
                if (kt1 == 1)
                    break;
            }

            row = k;//set lai i j ban dau
            col = l;
            //kt duong cheo thứ hai
            while (Cell[row, col] == Cells.Player2)
            {

                while (Cell[row, col] == Cells.Player2)
                {
                    dem2++;
                    row--; col++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem2--;
                while (Cell[row, col] == Cells.Player2)
                {
                    dem2++;
                    row++; col--;
                    if (!checkHopLe(row, col))
                    {
                        kt2 = 1;
                        break;
                    }
                }
                if (kt2 == 1)
                    break;
            }

            row = k;//set lai i j ban dau
            col = l;
            //kt hang ngang

            while (Cell[row, col] == Cells.Player2)
            {

                while (Cell[row, col] == Cells.Player2)
                {
                    dem3++;
                    col++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem3--;
                while (Cell[row, col] == Cells.Player2)
                {
                    dem3++;
                    col--;
                    if (!checkHopLe(row, col))
                    {
                        kt3 = 1;
                        break;
                    }
                }
                if (kt3 == 1)
                    break;
            }

            row = k;//set lai i j ban dau
            col = l;
            //kt hang doc
            while (Cell[row, col] == Cells.Player2)
            {

                while (Cell[row, col] == Cells.Player2)
                {
                    dem4++;
                    row++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem4--;
                while (Cell[row, col] == Cells.Player2)
                {
                    dem4++;
                    row--;
                    if (!checkHopLe(row, col))
                    {
                        kt4 = 1;
                        break;
                    }
                }
                if (kt4 == 1)
                    break;
            }
            //-------------------------------------------------
            //-------------------------------------------------
            row = k;
            col = l;
            while (Cell[row, col] == Cells.Player1)
            {
                while (Cell[row, col] == Cells.Player1)
                {
                    dem1++;
                    row++; col++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem1--;
                while (Cell[row, col] == Cells.Player1)
                {
                    dem1++;
                    row--; col--;
                    if (!checkHopLe(row, col))
                    {
                        kt1 = 1;
                        break;
                    }
                }
                if (kt1 == 1)
                    break;
            }

            row = k;//set lai i j ban dau
            col = l;
            //kt duong cheo thứ hai
            while (Cell[row, col] == Cells.Player1)
            {

                while (Cell[row, col] == Cells.Player1)
                {
                    dem2++;
                    row--; col++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem2--;
                while (Cell[row, col] == Cells.Player1)
                {
                    dem2++;
                    row++; col--;
                    if (!checkHopLe(row, col))
                    {
                        kt2 = 1;
                        break;
                    }
                }
                if (kt2 == 1)
                    break;
            }

            row = k;//set lai i j ban dau
            col = l;
            //kt hang ngang

            while (Cell[row, col] == Cells.Player1)
            {

                while (Cell[row, col] == Cells.Player1)
                {
                    dem3++;
                    col++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem3--;
                while (Cell[row, col] == Cells.Player1)
                {
                    dem3++;
                    col--;
                    if (!checkHopLe(row, col))
                    {
                        kt3 = 1;
                        break;
                    }
                }
                if (kt3 == 1)
                    break;
            }

            row = k;//set lai i j ban dau
            col = l;
            //kt hang doc
            while (Cell[row, col] == Cells.Player1)
            {
                while (Cell[row, col] == Cells.Player1)
                {
                    dem4++;
                    row++;
                    if (!checkHopLe(row, col))
                        break;
                }
                row = k;
                col = l;
                dem4--;
                while (Cell[row, col] == Cells.Player1)
                {
                    dem4++;
                    row--;
                    if (!checkHopLe(row, col))
                    {
                        kt4 = 1;
                        break;
                    }
                }
                if (kt4 == 1)
                    break;
            }



            if ((dem1 >= 5) || (dem2 >= 5) || (dem3 >= 5) || (dem4 >= 5))//6 con lien tiep can dc phep thang
                return true;//tra ve ket qua thang
            return false;
        }
        #endregion

        #region Kiem tra Hoa
        public int ktHoa(Cells[,] Board)
        {
            int kt = 2;
            for (int i = 0; i < _Row; i++)
            {
                for (int j = 0; j < _Col; j++)
                {
                    if (Board[i, j] != Cells.None)
                    {
                        kt = 1;//hoa
                    }
                    else
                    {
                        kt = 0;//KHONG hoa
                        return kt;
                    }
                }
            }
            return kt;
        }
        #endregion

        #region //UPDATE CHESSBOARD
        public void UpdateBoard(int x, int y, int _currentcell, Cells[,] Cell, Ellipse elip)
        {
            SolidColorBrush _color = null;
            elip.Width = _cellWidth;
            elip.Height = _cellHeight;
            if (Cell[x, y] == CaroChess_1312229.Models.Board.Cells.Player1)
            {
                _color = Brushes.SteelBlue;
            }
            if (Cell[x, y] == CaroChess_1312229.Models.Board.Cells.Player2)
            {
                _color = Brushes.Black;
            }
            elip.Fill = _color;
        }
        #endregion

        #region // AI()
        public void AI()
        {
            for (int i = 0; i < maxMove; i++)
            {
                WinMove[i] = new Point();
                PCMove[i] = new Point();
                HumanMove[i] = new Point();
            }

            depth = 0;
            FindMove();
            //MessageBox.Show(depth.ToString());
        }

        private void FindMove()
        {
            if (depth > maxDepth) return;
            depth++;
            fWin = false;
            bool fLose = false;
            Point pcMove = new Point();
            Point humanMove = new Point();
            int countMove = 0;
            EvalChessBoard(Cells.Player2, ref eBoard);

            //Lay ra MaxMove buoc di co diem cao nhat
            Point temp = new Point();
            for (int i = 0; i < maxMove; i++)
            {
                temp = eBoard.MaxPos();
                PCMove[i] = temp;
                eBoard.EBoard[(int)temp.X, (int)temp.Y] = 0;
            }

            //Lay nuoc di trong PCMove[] ra danh thu
            countMove = 0;
            while (countMove < maxMove)
            {

                pcMove = PCMove[countMove++];
                Cell[(int)pcMove.X, (int)pcMove.Y] = Cells.Player2;
                WinMove.SetValue(pcMove, depth - 1);

                //Tim cac nuoc di toi uu cua nguoi
                eBoard.ResetBoard();
                EvalChessBoard(Cells.Player1, ref eBoard);
                //Lay ra maxMove nuoc di co diem cao nhat cua nguoi
                for (int i = 0; i < maxMove; i++)
                {
                    temp = eBoard.MaxPos();
                    HumanMove[i] = temp;
                    eBoard.EBoard[(int)temp.X, (int)temp.Y] = 0;
                }
                //Danh thu cac nuoc di
                for (int i = 0; i < maxMove; i++)
                {
                    humanMove = HumanMove[i];
                    Cell[(int)humanMove.X, (int)humanMove.Y] = Cells.Player1;
                    int kt = 0;
                    kt = ktBeforePlayer(Cell);//kt nguoi choi di truoc
                    if (kt == 2)
                    {
                        bool _ktWin = false;
                        _ktWin = ktWin(Cell, (int)humanMove.X, (int)humanMove.Y);
                        if (_ktWin == true)
                        {
                            fWin = true;
                        }
                    }

                    if (kt == 2)
                    {
                        bool _ktWin = false;
                        _ktWin = ktWin(Cell, (int)humanMove.X, (int)humanMove.Y);
                        if (_ktWin == true)
                        {
                            fLose = true;
                        }
                    }
                    if (fLose)
                    {
                        Cell[(int)pcMove.X, (int)pcMove.Y] = Cells.None;
                        Cell[(int)humanMove.X, (int)humanMove.Y] = Cells.None;
                        break;
                    }
                    if (fWin)
                    {
                        Cell[(int)pcMove.X, (int)pcMove.Y] = 0;
                        Cell[(int)humanMove.X, (int)humanMove.Y] = 0;
                        return;
                    }
                    FindMove();
                    Cell[(int)humanMove.X, (int)humanMove.Y] = 0;
                }
                Cell[(int)pcMove.X, (int)pcMove.Y] = 0;

            }
        }

        public void EvalChessBoard(Cells player, ref EvalBoard eBoard)
        {
            int rw, cl, ePC, eHuman;
            eBoard.ResetBoard();

            //Danh gia theo hang
            for (rw = 0; rw < _Row; rw++)
                for (cl = 0; cl < _Col - 4; cl++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (Cell[rw, cl + i] == Cells.Player1) eHuman++;
                        if (Cell[rw, cl + i] == Cells.Player2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (Cell[rw, cl + i] == Cells.None) // Neu o chua duoc danh
                            {
                                if (eHuman == 0)
                                    if (player == Cells.Player1)
                                        eBoard.EBoard[rw, cl + i] += DScore[ePC];
                                    else eBoard.EBoard[rw, cl + i] += AScore[ePC];
                                if (ePC == 0)
                                    if (player == Cells.Player2)
                                        eBoard.EBoard[rw, cl + i] += DScore[eHuman];
                                    else eBoard.EBoard[rw, cl + i] += AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    eBoard.EBoard[rw, cl + i] *= 2;
                            }
                        }

                    }
                }

            //Danh gia theo cot
            for (cl = 0; cl < _Col; cl++)
                for (rw = 0; rw < _Row - 4; rw++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (Cell[rw + i, cl] == Cells.Player1) eHuman++;
                        if (Cell[rw + i, cl] == Cells.Player2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (Cell[rw + i, cl] == 0) // Neu o chua duoc danh
                            {
                                if (eHuman == 0)
                                    if (player == Cells.Player1)
                                        eBoard.EBoard[rw + i, cl] += DScore[ePC];
                                    else eBoard.EBoard[rw + i, cl] += AScore[ePC];
                                if (ePC == 0)
                                    if (player == Cells.Player2)
                                        eBoard.EBoard[rw + i, cl] += DScore[eHuman];
                                    else eBoard.EBoard[rw + i, cl] += AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    eBoard.EBoard[rw + i, cl] *= 2;
                            }
                        }

                    }
                }

            //Danh gia duong cheo xuong
            for (cl = 0; cl < _Col - 4; cl++)
                for (rw = 0; rw < _Row - 4; rw++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (Cell[rw + i, cl + i] == Cells.Player1) eHuman++;
                        if (Cell[rw + i, cl + i] == Cells.Player2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (Cell[rw + i, cl + i] == 0) // Neu o chua duoc danh
                            {
                                if (eHuman == 0)
                                    if (player == Cells.Player1)
                                        eBoard.EBoard[rw + i, cl + i] += DScore[ePC];
                                    else eBoard.EBoard[rw + i, cl + i] += AScore[ePC];
                                if (ePC == 0)
                                    if (player == Cells.Player2)
                                        eBoard.EBoard[rw + i, cl + i] += DScore[eHuman];
                                    else eBoard.EBoard[rw + i, cl + i] += AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    eBoard.EBoard[rw + i, cl + i] *= 2;
                            }
                        }

                    }
                }

            //Danh gia duong cheo len
            for (rw = 4; rw < _Row; rw++)
                for (cl = 0; cl < _Col - 4; cl++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (Cell[rw - i, cl + i] == Cells.Player1) eHuman++;
                        if (Cell[rw - i, cl + i] == Cells.Player2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (Cell[rw - i, cl + i] == 0) // Neu o chua duoc danh
                            {
                                if (eHuman == 0)
                                    if (player == Cells.Player1)
                                        eBoard.EBoard[rw - i, cl + i] += DScore[ePC];
                                    else eBoard.EBoard[rw - i, cl + i] += AScore[ePC];
                                if (ePC == 0)
                                    if (player == Cells.Player2)
                                        eBoard.EBoard[rw - i, cl + i] += DScore[eHuman];
                                    else eBoard.EBoard[rw - i, cl + i] += AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    eBoard.EBoard[rw - i, cl + i] *= 2;
                            }
                        }

                    }
                }

        }
        #endregion
    }
}
