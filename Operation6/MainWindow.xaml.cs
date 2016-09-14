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

namespace Operation6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool[,] filled = { {false,false,false },{false,false,false },{false,false,false } };
        int[,] wincheck = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        bool PlayerOneTurn = true;
        bool gameover = false;
        int playedcount = 0;
        int player1wins = 0;
        int player2wins = 0;
        int draws = 0;
        int gamesleft;
        bool oneplayer = false;
        bool skipAITurn = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TicTacToe_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!gameover)
            {
                var point = Mouse.GetPosition(TicTacToe);

                int row = 0;
                int col = 0;
                double accumulatedHeight = 0.0;
                double accumulatedWidth = 0.0;

                // calc row mouse was over
                foreach (var rowDefinition in TicTacToe.RowDefinitions)
                {
                    accumulatedHeight += rowDefinition.ActualHeight;
                    if (accumulatedHeight >= point.Y)
                        break;
                    row++;
                }

                // calc col mouse was over
                foreach (var columnDefinition in TicTacToe.ColumnDefinitions)
                {
                    accumulatedWidth += columnDefinition.ActualWidth;
                    if (accumulatedWidth >= point.X)
                        break;
                    col++;
                }
                if (!filled[row, col])
                {
                    playedcount++;
                    filled[row, col] = true;
                    Image imgControl = new Image();
                    if (PlayerOneTurn)
                    {
                        imgControl.Source = new BitmapImage(new Uri("X.png", UriKind.Relative));
                        PlayerOneTurn = false;
                        wincheck[row, col] = 1;
                        if (checkwin())
                        {
                            gameover = true;
                            MessageBox.Show("X Wins");
                            player1wins++;
                            Player1Wins.Text = player1wins.ToString();
                            Restart();
                            skipAITurn = true;
                        }
                        else
                        {
                            if (playedcount >= 9)
                            {
                                gameover = true;
                                MessageBox.Show("Draw");
                                draws++;
                                Draws.Text = draws.ToString();
                                Restart();
                                skipAITurn = true;
                            }
                            else
                            {
                                imgControl.Margin = new Thickness(10);
                                Grid.SetColumn(imgControl, col);
                                Grid.SetRow(imgControl, row);
                                TicTacToe.Children.Add(imgControl);
                            }
                        }
                    }
                    else
                    {
                        imgControl.Source = new BitmapImage(new Uri("O.png", UriKind.Relative));
                        PlayerOneTurn = true;
                        wincheck[row, col] = 2;
                        if (checkwin())
                        {
                            gameover = true;
                            MessageBox.Show("O Wins");
                            player2wins++;
                            Player2Wins.Text = player2wins.ToString();
                            Restart();
                        }
                        else
                        {
                            if (playedcount >= 9)
                            {
                                gameover = true;
                                MessageBox.Show("Draw");
                                Draws.Text = draws.ToString();
                                Restart();
                            }
                            else
                            {
                                imgControl.Margin = new Thickness(10);
                                Grid.SetColumn(imgControl, col);
                                Grid.SetRow(imgControl, row);
                                TicTacToe.Children.Add(imgControl);
                            }
                        }
                    }
                    if (oneplayer && !skipAITurn)
                    {
                        AITakeTurn();
                    }
                    skipAITurn = false;
                }
            }
        }

        private void AITakeTurn()
        {
            int row=0, col=0;
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(!filled[i,j])
                    {
                        row = i;
                        col = j;
                        break;
                    }
                }
            }
            playedcount++;
            filled[row, col] = true;
            Image imgControl = new Image();

            imgControl.Source = new BitmapImage(new Uri("O.png", UriKind.Relative));
            PlayerOneTurn = true;
            wincheck[row, col] = 2;
            if (checkwin())
            {
                gameover = true;
                MessageBox.Show("O Wins");
                player2wins++;
                Player2Wins.Text = player2wins.ToString();
                Restart();
            }
            else
            {
                if (playedcount >= 9)
                {
                    gameover = true;
                    MessageBox.Show("Draw");
                    Draws.Text = draws.ToString();
                    Restart();
                }
                else
                {
                    imgControl.Margin = new Thickness(10);
                    Grid.SetColumn(imgControl, col);
                    Grid.SetRow(imgControl, row);
                    TicTacToe.Children.Add(imgControl);
                }
            }
        }

        private bool checkwin()
        {
            //Player 1
            //start at top left corner
            if (wincheck[0, 0] == 1)
            {
                //check top row
                if (wincheck[0, 1] == 1)
                {
                    if (wincheck[0, 2] == 1)
                    {
                        return true;
                    }
                }
                //check left col
                else if (wincheck[1, 0] == 1)
                {
                    if (wincheck[2, 0] == 1)
                    {
                        return true;
                    }
                }
                //check main diag
                else if ((wincheck[1, 1] == 1))
                {
                    if (wincheck[2, 2] == 1)
                    {
                        return true;
                    }
                }
            }
            //check middle row
            if (wincheck[1, 0] == 1)
            {
                if ((wincheck[1, 1] == 1))
                {
                    if (wincheck[1, 2] == 1)
                    {
                        return true;
                    }
                }
            }
            //check bottom row
            if (wincheck[2, 0] == 1)
            {
                if ((wincheck[2, 1] == 1))
                {
                    if (wincheck[2, 2] == 1)
                    {
                        return true;
                    }
                }
            }
            //check middle col
            if (wincheck[0, 1] == 1)
            {
                if ((wincheck[1, 1] == 1))
                {
                    if (wincheck[2, 1] == 1)
                    {
                        return true;
                    }
                }
            }
            //check middle col
            if (wincheck[0, 2] == 1)
            {
                if ((wincheck[1, 2] == 1))
                {
                    if (wincheck[2, 2] == 1)
                    {
                        return true;
                    }
                }
            }
            //check left diag
            if (wincheck[0, 2] == 1)
            {
                if ((wincheck[1, 1] == 1))
                {
                    if (wincheck[2, 0] == 1)
                    {
                        return true;
                    }
                }
            }

            //Player 2
            //start at top left corner
            if (wincheck[0, 0] == 2)
            {
                //check top row
                if (wincheck[0, 1] == 2)
                {
                    if (wincheck[0, 2] == 2)
                    {
                        return true;
                    }
                }
                //check left col
                else if (wincheck[1, 0] == 2)
                {
                    if (wincheck[2, 0] == 2)
                    {
                        return true;
                    }
                }
                //check main diag
                else if ((wincheck[1, 1] == 2))
                {
                    if (wincheck[2, 2] == 2)
                    {
                        return true;
                    }
                }
            }
            //check middle row
            if (wincheck[1, 0] == 2)
            {
                if ((wincheck[1, 1] == 2))
                {
                    if (wincheck[1, 2] == 2)
                    {
                        return true;
                    }
                }
            }
            //check bottom row
            if (wincheck[2, 0] == 2)
            {
                if ((wincheck[2, 1] == 2))
                {
                    if (wincheck[2, 2] == 2)
                    {
                        return true;
                    }
                }
            }
            //check middle col
            if (wincheck[0, 1] == 2)
            {
                if ((wincheck[1, 1] == 2))
                {
                    if (wincheck[2, 1] == 2)
                    {
                        return true;
                    }
                }
            }
            //check middle col
            if (wincheck[0, 2] == 2)
            {
                if ((wincheck[1, 2] == 2))
                {
                    if (wincheck[2, 2] == 2)
                    {
                        return true;
                    }
                }
            }
            //check left diag
            if (wincheck[0, 2] == 2)
            {
                if ((wincheck[1, 1] == 2))
                {
                    if (wincheck[2, 0] == 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Restart()
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    filled[i, j] = false;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    wincheck[i, j] = 0;
                }
            }
            PlayerOneTurn = true;
            gameover = false;
            playedcount = 0;
            TicTacToe.Children.Clear();
            gamesleft--;
            if (gamesleft <= 0)
            {
                hideGame();
                playedcount = 0;
                player1wins = 0;
                player2wins = 0;
                draws = 0;
                Player1Wins.Text = "0";
                Player2Wins.Text = "0";
                Draws.Text = "0";
                gameover = true;
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            hideGame();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ResponseTextBox.Text, out gamesleft))
            {
                showGame();
                gameover = false;
                oneplayer = AITrue.IsChecked.Value;
            }
            else
            {
                MessageBox.Show("Invalid number of games");
            }
        }
        private void hideGame()
        {
            Pregame.Visibility = Visibility.Visible;
            Player1Wins.Visibility = Visibility.Hidden;
            Player2Wins.Visibility = Visibility.Hidden;
            Draws.Visibility = Visibility.Hidden;
            player1label.Visibility = Visibility.Hidden;
            player2label.Visibility = Visibility.Hidden;
            drawslabel.Visibility = Visibility.Hidden;
            TicTacToe.Visibility = Visibility.Hidden;
        }
        private void showGame()
        {
            Pregame.Visibility = Visibility.Hidden;
            Player1Wins.Visibility = Visibility.Visible;
            Player2Wins.Visibility = Visibility.Visible;
            Draws.Visibility = Visibility.Visible;
            player1label.Visibility = Visibility.Visible;
            player2label.Visibility = Visibility.Visible;
            drawslabel.Visibility = Visibility.Visible;
            TicTacToe.Visibility = Visibility.Visible;
        }
    }
}
