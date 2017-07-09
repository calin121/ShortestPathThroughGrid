using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPathThroughGrid {
    class Program {
        static void Main(string[] args) {
            // Input Number of Rows and Columns in the Grid:
            int numRows = 3;
            int numColumns = 4;

            // Input Start Location coordinates
            Tuple<int, int> deskLocation = new Tuple<int, int>(2, 1);

            // Input Wall Locations
            List<Tuple<int, int>> walls = new List<Tuple<int, int>>();
            walls.Add(new Tuple<int, int>(2, 2));
            walls.Add(new Tuple<int, int>(2, 3));
            walls.Add(new Tuple<int, int>(3, 1));                      

            // Input Coffee Locations
            List<Tuple<int, int>> coffeeLocations = new List<Tuple<int, int>>();
            coffeeLocations.Add(new Tuple<int, int>(1, 3));
            coffeeLocations.Add(new Tuple<int, int>(3, 2));

            // Calling the function to console log the nearest coffee location:
            int distance = DistanceToCoffee(numRows, numColumns, deskLocation, coffeeLocations, walls);
            
            if(distance == 0) {
                System.Console.WriteLine("You cannot get to a coffee location from your desk! You need to find a better desk!");
            }
            else {
            System.Console.WriteLine("Your distance from the nerest coffee location is: " + distance);
            }
        }
        public static int CheckNeighbors (int x, int y, int numRows, int numColumns, string[,] newGrid, Queue<Tuple<int, int>> queue, List<Tuple<int, int>> visited, int count, Dictionary<Tuple<int, int>, int> pathLength, int least) {
             if (x >= 0 && x < numRows && y >= 0 && y < numColumns) {
                if(newGrid[x, y] == "C") {
                    queue.Clear();
                    least = count + 1;
                    return least;
                }
                else if(newGrid[x, y] != "X" && !visited.Any(v => v.Item1 == x && v.Item2 == y)) {
                    queue.Enqueue(new Tuple<int, int>(x, y));
                    visited.Add(new Tuple<int, int>(x, y));
                    count += 1;               
                    if(count < pathLength[new Tuple<int, int>(x, y)]) {
                        pathLength[new Tuple<int, int>(x, y)] = count;
                    }
                }
            }
        return least;
        }
        public static void MarkStart (string[,] grid, Tuple<int, int> deskLocation, Dictionary<Tuple<int, int>, int>pathLength) {
            grid[(deskLocation.Item1 - 1), (deskLocation.Item2 - 1)] = "S";
            pathLength[new Tuple<int, int>(deskLocation.Item1 - 1, deskLocation.Item2 - 1)] = 0;
        }
        public static void MarkWalls (string[,] grid, List<Tuple<int, int>> walls) {
            foreach(Tuple<int, int> wall in walls) {
                        grid[(wall.Item1 - 1), (wall.Item2 - 1)] = "X";
                }
        }
        public static void MarkCoffeeLocations (string[,] grid, List<Tuple<int, int>> coffeeLocations) {
            foreach(Tuple<int, int> coffee in coffeeLocations) {
                        grid[(coffee.Item1 - 1), (coffee.Item2 - 1)] = "C";
                }
        }
        public static int DistanceToCoffee (int numRows, int numColumns, Tuple<int, int> deskLocation, List<Tuple<int, int>> coffeeLocations, List<Tuple<int, int>> walls) {
            int least = 0;
            int count = 0;
            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(new Tuple<int, int>(deskLocation.Item1 - 1, deskLocation.Item2 -1));
            List<Tuple<int, int>> visited = new List<Tuple<int, int>>();
            Dictionary<Tuple<int, int>, int> pathLength = new Dictionary<Tuple<int, int>, int>();
            string[,] newGrid = new string[numRows, numColumns];
            for (int r = 0; r < numRows; r++) {
                 for (int c = 0; c < numColumns; c++) {
                     newGrid[r, c] = "-";
                     pathLength.Add(new Tuple<int, int>(r, c), int.MaxValue);
                }
            }
            MarkStart(newGrid, deskLocation, pathLength);
            MarkWalls(newGrid, walls);
            MarkCoffeeLocations(newGrid, coffeeLocations);
            while (queue.Count != 0) {
                // establish current x and y coordinates from queue
                int x = queue.ElementAt(0).Item1;
                int y = queue.ElementAt(0).Item2;
                count = pathLength[new Tuple <int, int>(x,y)];
                queue.Dequeue();
                least = CheckNeighbors (x-1, y, numRows, numColumns, newGrid, queue, visited, count, pathLength, least);
                least = CheckNeighbors (x+1, y, numRows, numColumns, newGrid, queue, visited, count, pathLength, least);
                least = CheckNeighbors (x, y-1, numRows, numColumns, newGrid, queue, visited, count, pathLength, least);
                least = CheckNeighbors (x, y+1, numRows, numColumns, newGrid, queue, visited, count, pathLength, least);                
                count +=1;
            }
            return least;
        }
    }
}