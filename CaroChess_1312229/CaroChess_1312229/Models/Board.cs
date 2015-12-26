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
    }
}
