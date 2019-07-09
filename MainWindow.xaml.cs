using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IRSRobotView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Location[,] Maze;
        Location[,] ProgMaze;
        Floodfill floodfill;
        DispatcherTimer dispatcherTimer;

        int StartX, StartY, FinishX, FinishY;

        public MainWindow()
        {
            InitializeComponent();
            Maze = new Location[5, 9];

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1,1);

            GenerateMaze();

            floodfill = new Floodfill();
        }

        private void GenerateMaze()
        {
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 9; x++)
                {
                    Maze[y, x] = new Location(x,y);
                    Maze[y, x].Content.PreviewMouseDown += new MouseButtonEventHandler(this.Content_PreviewMouseDown);
                    myGrid.Children.Add(Maze[y,x].LeftLine);
                    myGrid.Children.Add(Maze[y, x].UpLine);
                    myGrid.Children.Add(Maze[y, x].Content);
                }
        }
        private void Content_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            int hoverX = ((int)((TextBlock)sender).Margin.Left - Constants.XOffset) / 40;
            int hoverY = ((int)((TextBlock)sender).Margin.Top - Constants.XOffset) / 40;

            if(Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    Maze[Constants.GreenY, Constants.GreenX].Content.Background = Brushes.White;
                    Maze[hoverY, hoverX].Content.Background = Brushes.Green;
                    Constants.GreenX = hoverX;
                    Constants.GreenY = hoverY;
                }
                else
                {
                    Maze[Constants.RedY, Constants.RedX].Content.Background = Brushes.White;
                    Maze[hoverY, hoverX].Content.Background = Brushes.Red;
                    Constants.RedX = hoverX;
                    Constants.RedY = hoverY;
                }
            }
            else
            {
                var status = (2 * Convert.ToInt32(Maze[hoverY, hoverX].LeftWall) + Convert.ToInt32(Maze[hoverY, hoverX].UpWall) + 1) % 4;

                Maze[hoverY, hoverX].LeftWall = Convert.ToBoolean(status / 2);
                Maze[hoverY, hoverX].UpWall = Convert.ToBoolean(status % 2);

                Maze[hoverY, hoverX].LeftLine.Stroke = Maze[hoverY, hoverX].LeftWall ? Brushes.CornflowerBlue : Brushes.White;
                Maze[hoverY, hoverX].UpLine.Stroke = Maze[hoverY, hoverX].UpWall ? Brushes.CornflowerBlue : Brushes.White;
            }
        }

        private void ResetMarks()
        {
            for(int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Maze[y, x].Content.Text = "0";
                    if (Maze[y, x].Content.Background == Brushes.Pink) Maze[y, x].Content.Background = Brushes.White;
                }
            }
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            ResetMarks();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartX = Constants.GreenX;
            StartY = Constants.GreenY;
            FinishX = Constants.RedX;
            FinishY = Constants.RedY;
            floodfill.Compute(StartX, StartY, FinishX, FinishY, Maze);
            //dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetMarks();
        }
        
    }
}
