using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IRSRobotView
{
    class Floodfill
    {
        int[,] Mark;
        //           0  1   2  3
        int[] dx = { 1, 0, -1, 0 };
        int[] dy = { 0, -1, 0, 1 };

        public Floodfill()
        {
            Mark = new int[10, 18];
            ResetMarks();
        }

        public void ResetMarks()
        {
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 9; x++)
                    Mark[y, x] = 0;
        }

        public void UpdateMarks(Location[,] Maze)
        {
            for(int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Maze[y, x].Content.Text = Convert.ToString(Mark[y, x]);
                }
            }
        }

        public bool Compute(int xs, int ys, int xf, int yf, Location[,] Maze)
        {
            ResetMarks();

            Mark[ys, xs] = 1;
            
            int x, y;
            if (Solve(Maze, xf, yf))
            {
                UpdateMarks(Maze);
                x = xf;
                y = yf;
                for (int N = Mark[yf, xf]; N >= 1; N--)
                {
                    if(Maze[y, x].Content.Background == Brushes.Green || Maze[y, x].Content.Background == Brushes.Red)
                    {

                    }
                    else
                    {
                        Maze[y, x].Content.Background = Brushes.Pink;
                    }
                    
                    for (int i = 0; i < 4; i++)
                    {
                        if (CanGo(x, y, dx[i], dy[i], Maze) && Mark[y + dy[i], x + dx[i]] == N - 1)
                        {
                            x += dx[i];
                            y += dy[i];
                            break;
                        }
                    }
                }
                return true;
            }
            else
            {
                UpdateMarks(Maze);
                return false;
            }
        }

        private bool Solve(Location[,] Maze, int xf, int yf)
        {
            int N = 1;
            bool NoSolution;

            do
            {
                NoSolution = true;

                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 4; y++)
                        if(Mark[y,x] == N)
                        {
                            for (int i = 0; i < 4; i++)
                                if (CanGo(x, y, dx[i], dy[i], Maze) && Mark[y + dy[i], x + dx[i]] == 0)
                                {
                                    NoSolution = false;
                                    Mark[y + dy[i], x + dx[i]] = N + 1;
                                    if (x + dx[i] == xf && y + dy[i] == yf)
                                        return true;
                                }
                        }                            
                N++;
            }
            while (NoSolution == false);
            return false;
        }
        
        private bool CanGo(int x, int y, int dx, int dy, Location[,] Maze)
        {
            if (dx == -1) return !Maze[y, x].LeftWall;
            else if (dx == 1) return !Maze[y, x + 1].LeftWall;
            else if (dy == -1) return !Maze[y, x].UpWall;
            else return !Maze[y + 1, x].UpWall;
        }
    }
}
