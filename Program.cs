using System;

namespace ConsolePractice
{
    internal class Program
    {
        public class Cells
        {
            public int Neighbors;
            public bool IsBomb;
            public bool IsOpen;
        }
        public static Cells[,] Grid = new Cells[10,10];
        public static Random Rnd = new Random();
        public static int Bombs;
        public static int Fields;
        public static bool IsOnPlay;
        
        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            GridGenerator();
            Console.WriteLine("Number of bombs: " + Bombs.ToString());
            while (IsOnPlay)
            {
                Console.Clear();
                Show();
                Selector();
                if (Fields != -1) continue;
                IsOnPlay = false;
                Show();
                Console.WriteLine("Win!");
            }
        }
       
        public static void GridGenerator()
        {
            for (var row = 0; row < 10; row++)
                for (var col = 0; col < 10; col++)
                {
                    Grid[row,col] = new Cells();
                    var chanser = Rnd.Next(0,7);
                    if (chanser == 3)
                    {
                        Grid[row, col].IsBomb = true;
                        Bombs++;
                    }
                }

            Fields = 100 - Bombs;
            for (var row = 0; row < 10; row++)
                for (var col = 0; col < 10; col++)
                    if (Grid[row, col].IsBomb)
                        for (var yoff = row - 1; yoff < row + 2; yoff++)
                            for (var xoff = col - 1; xoff < col + 2; xoff++)
                                if (yoff > -1 && yoff < 10 && xoff > -1 && xoff < 10)
                                    Grid[yoff, xoff].Neighbors++;

            IsOnPlay = true;
        }

        public static void Selector()
        {

            Console.Write("Row: ");
            string Row = Console.ReadLine();
            Console.Write("Coll: ");
            string Col = Console.ReadLine();
            int row, col;
            bool Rsuc = int.TryParse(Row, out row);
            bool Csuc = int.TryParse(Col, out col);
            if (Rsuc && Csuc)
            {
                if (row >= 10 || row <= -1 || col >= 10 || col <= -1) return;
                if (Grid[row, col].IsBomb)
                {
                    IsOnPlay = false;
                    Grid[row, col].IsOpen = true;
                    Show();
                    Console.WriteLine("Loss.\nPress any key for restart...");
                    Console.ReadKey();
                    Main();
                }

                Grid[row, col].IsOpen = true;
                if (Grid[row, col].Neighbors == 0)
                {
                    SpaceChecker(row, col);
                }

                Fields--;
            }
        }

        public static void SpaceChecker(int row, int col)
        {
            for (var yoff = row - 1; yoff <= row + 1; yoff++)
                for (var xoff = col - 1; xoff <= col + 1; xoff++)
                {
                    if (yoff <= -1 || yoff >= 10 || xoff <= -1 || xoff >= 10) continue;
                    if (Grid[yoff,xoff].Neighbors == 0 && Grid[yoff,xoff].IsOpen == false)
                    {
                        Grid[yoff, xoff].IsOpen = true;
                        Fields--;
                        SpaceChecker(yoff,xoff);
                    }

                    if (Grid[yoff, xoff].IsBomb) continue;
                    Fields--;
                    Grid[yoff, xoff].IsOpen = true;
                }
        }

        public static void Show()
        {
            for (var row = 0; row < 10; row++)
            {
                for (var col = 0; col < 10; col++)
                {
                    if      (Grid[row,col].IsOpen && Grid[row,col].IsBomb == false) Console.Write( $".{Grid[row,col].Neighbors.ToString()}.");
                    else if (Grid[row, col].IsBomb && Grid[row,col].IsOpen) Console.Write(".b.");
                    else Console.Write(".+.");
                }
                Console.WriteLine();
            }
        }
    }
}
