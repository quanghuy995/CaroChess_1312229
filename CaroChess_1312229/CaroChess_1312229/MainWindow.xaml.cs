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

namespace CaroChess_1312229
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateChessBoard();
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
    }
}
