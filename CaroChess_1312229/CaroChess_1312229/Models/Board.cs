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
                _color = Brushes.DarkViolet;
            }
            if (Cell[x, y] == CaroChess_1312229.Models.Board.Cells.Player2)
            {
                _color = Brushes.Black;
            }
            elip.Fill = _color;
        }
        #endregion
    }
}
