using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaroChess_1312229.Models;

namespace CaroChess_1312229.ViewModels
{
    class BoardViewModel
    {
        public Board CurrentBoard { get; set; }

        public BoardViewModel()
        {
            CurrentBoard = new Board();
        }
    }
}
