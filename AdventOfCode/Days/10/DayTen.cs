using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._10;

public class DayTen
{
    private const int Day = 10;

    public long ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(Day, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(Day, "LongInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(Day, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }

    public long ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(Day, "LongInput-2.txt");
        return BodyPartTwo(input);
    }

    #region Body

    private int BodyPartOne(string[] input)
    {
        var startPoint = GetStartPoint(input);
        return FindLongestPath(input, startPoint); 
    }

    private int FindLongestPath(string[] input, (int, int) startPoint)
    {
        var queue = new Queue<((int, int), int)>();
        var visited = new HashSet<(int, int)>();
        queue.Enqueue((startPoint, 0));
        visited.Add(startPoint);
        var longest = 0;
        var newMatrix = new List<List<string>>();
        for(var i = 0; i < input.Length; i++)
        {
            newMatrix.Add(new List<string>());
            for(var j = 0; j < input[i].Length; j++)
            {
                newMatrix[i].Add(input[i][j].ToString());
            }
        }
        while (queue.Count > 0)
        {
            var ((row, col), distance) = queue.Dequeue();

            var adjacent = GetAdjacent(input, row, col);

            // Console.WriteLine($"Adjacent for {input[row][col]}: {string.Join(", ", adjacent.Select(x => input[x.Item1][x.Item2]))}");
            foreach ((int nr, int nc) in adjacent)
            {
                if (!visited.Contains((nr, nc)))
                {
                    // Console.WriteLine($"Adjacent for {input[row][col]}: {input[nr][nc]}. Distance: {distance + 1}");
                    queue.Enqueue(((nr, nc), distance + 1));
                    if(input[nr][nc] == '.')
                    {
                        newMatrix[nr][nc] = ".";
                    }
                    else
                    {
                        newMatrix[nr][nc] = (distance + 1).ToString();
                    }
                   
                    visited.Add((nr, nc));
                    longest = Math.Max(longest, distance + 1);
                }
            }
        }
        return longest;
    }

    private List<(int, int)> GetAdjacent(string[] input, int row, int col)
    {
        var adjacent = new List<(int, int)>();
        switch (input[row][col])
        {
            case '|':
                // N
                if (row - 1 >= 0)
                {
                    adjacent.Add((row - 1, col));
                };
                // S
                if (row + 1 < input.Length)
                {
                    adjacent.Add((row + 1, col));
                };
                break;
            
            case '-':
                // E
                if (col + 1 < input[0].Length)
                {
                    adjacent.Add((row, col + 1));
                };
                // W
                if (col - 1 >= 0)
                {
                    adjacent.Add((row, col - 1));
                };
                break;
            case 'L':
                // N
                if (row - 1 >= 0)
                {
                    adjacent.Add((row - 1, col));
                };
                // E
                if (col + 1 < input[0].Length)
                {
                    adjacent.Add((row, col + 1));
                };
                break;
            case 'J':
                // N
                if (row - 1 >= 0)
                {
                    adjacent.Add((row - 1, col));
                };
                // W    
                if (col - 1 >= 0)
                {
                    adjacent.Add((row, col - 1));
                };
                break;
            
            case '7':
                // S
                if (row + 1 < input.Length)
                {
                    adjacent.Add((row + 1, col));
                };
                // W
                if (col - 1 >= 0)
                {
                    adjacent.Add((row, col - 1));
                };
                break;
            case 'F':
                // S
                if (row + 1 < input.Length)
                {
                    adjacent.Add((row + 1, col));
                };
                // E
                if (col + 1 < input[0].Length)
                {
                    adjacent.Add((row, col + 1));
                };
                break;
            case 'S':
                // N
                if (row - 1 >= 0)
                {
                    adjacent.Add((row - 1, col));
                };
                // S
                if (row + 1 < input.Length)
                {
                    adjacent.Add((row + 1, col));
                };
                // // E
                // if (col + 1 < input[0].Length)
                // {
                //     adjacent.Add((row, col + 1));
                // };
                // // W
                // if (col - 1 >= 0)
                // {
                //     adjacent.Add((row, col - 1));
                // };
                break;
        }
        return adjacent;
    }

    private (int, int) GetStartPoint(string[] input)
    {
        for(var i = 0; i < input.Length; i++)
        {
            for(var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'S')
                {
                    return (i, j);
                }
            }
        }
        throw new Exception("Start point not found");
    }

    private int FindNextValueForRow(string startRow)
    {
        var list = new List<List<int>>();
        list.Add(startRow.Split(" ").Select(x => int.Parse(x)).ToList());
        var row = list[0];
        while (row.Any(x => x != 0))
        {
            var nextRow = new List<int>();
            for(var i = 1; i < row.Count; i++)
            {
                nextRow.Add(row[i] - row[i-1]);
            }
            list.Add(nextRow);
            row = nextRow;
        }

        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (i == list.Count - 1)
            {
                list[i].Add(0);
                continue;
            }
            
            list[i].Add(list[i+1].Last() + list[i].Last());
        }
        
        return list.First().Last();
    }

    private long BodyPartTwo(string[] input)
    {
        var result = 0;
        foreach (var startRow in input)
        {
            result += FindFirstValueForRow(startRow);
        }

        return result;
    }
    
    private int FindFirstValueForRow(string startRow)
    {
        var list = new List<List<int>>();
        list.Add(startRow.Split(" ").Select(x => int.Parse(x)).ToList());
        var row = list[0];
        while (row.Any(x => x != 0))
        {
            var nextRow = new List<int>();
            for(var i = 1; i < row.Count; i++)
            {
                nextRow.Add(row[i] - row[i-1]);
            }
            list.Add(nextRow);
            row = nextRow;
        }
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (i == list.Count - 1)
            {
                list[i].Add(0);
                continue;
            }
            var newList = new List<int>();
            newList.Add(list[i].First() - list[i+1].First());
            newList.AddRange(list[i]);
            list[i] = newList;
        }
        
        return list.First().First();
    }

    #endregion
}