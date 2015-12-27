using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CaroChess_1312229.Models
{
    class EvalBoard
    {
        public int[,] EBoard;

        public EvalBoard()
        {
            EBoard = new int[14, 14];
            ResetBoard();
        }
        public void ResetBoard()
        {
            for (int r = 0; r < 14; r++)
                for (int c = 0; c < 14; c++)
                    EBoard[r, c] = 0;
        }
        public Point MaxPos()
        {
            int Max = 0;
            Point p = new Point();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (EBoard[i, j] > Max)
                    {
                        p.X = i; p.Y = j;
                        Max = EBoard[i, j];
                    }

                }
            }
            return p;
        }
    }
}
