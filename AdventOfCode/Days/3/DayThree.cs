using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._3;

public class DayThree
{
    private readonly Dictionary<string, List<int>> _default = new();
    
    public int ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(3, "SmallInput-1.txt");
        return BodyPartOne(input);
    }
    
    public int ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(3, "LongInput-1.txt");
        return BodyPartOne(input);
    }   
    
    public int ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(3, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }
    
    public int ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(3, "LongInput-2.txt");
        return BodyPartTwo(input);
    } 
    
    #region Body
    private int BodyPartOne(string[] input)
    {
        var total = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var row = input[i];
            var number = "";
            var startNumberIndex = 0;
            for (var j = 0; j < row.Length; j++)
            {
                if (char.IsDigit(row[j]))
                {
                    startNumberIndex = startNumberIndex == 0 ? j : startNumberIndex;
                    number += row[j];
                    continue;
                }

                total = DoSum(number, i, startNumberIndex, j, input, total);
                startNumberIndex = 0;
                number = "";
            }
            total = DoSum(number, i, startNumberIndex, row.Length, input, total);
        }
        return total;
    }

    private int DoSum(string number, int i, int startNumberIndex, int j, string[] input, int total, bool isPartTwo = false)
    {
        if(number == "")
            return total;
                
        var shouldAdd = IsAdjacentToSymbol(i, startNumberIndex, j - 1, input, int.Parse(number), isPartTwo);
        if (shouldAdd)
        {
            total += int.Parse(number);
        }
        return total;
    }

    // private int DoSum
    private bool IsAdjacentToSymbol(int row, int startNumberI, int endNumberI, string[] input, int number, bool isPartTwo = false)
    {
        var maxI = input.Length;
        var maxJ = input[0].Length;
        var startI = startNumberI - 1 >= 0 ? startNumberI - 1 : 0;
        var endNumberJ = endNumberI + 1 < maxJ ? endNumberI + 1 : maxJ - 1;
        for (var i = startI; i <= endNumberJ; i++)
        {
            // check Above
            if (row - 1 >= 0)
            {
                var above = input[row - 1][i];
                var key = GetKey(row - 1, i);
                if(!char.IsDigit(above) && above != '.')
                {
                    if (above == '*' && isPartTwo && _default.TryGetValue(key, out var value))
                        value.Add(number);
                    return true;
                }
            }
            
            // check Below
            if (row + 1 < maxI)
            {
                var below = input[row + 1][i];
                var key = GetKey(row + 1, i);
                if(!char.IsDigit(below) && below != '.')
                {
                    if (below == '*' && isPartTwo && _default.TryGetValue(key, out var value))
                        value.Add(number);
                    return true;
                }
            }
        }
        
        // check Left
        if (startNumberI - 1 >= 0)
        {
            var left = input[row][startNumberI - 1];
            var key = GetKey(row, startNumberI - 1);
            if(!char.IsDigit(left) && left != '.')
            {
                if (left == '*' && isPartTwo && _default.TryGetValue(key, out var value))
                    value.Add(number);
                return true;
            }
        }
        
        // check Right
        if (endNumberI + 1 < maxJ)
        {
            var right = input[row][endNumberI + 1];
            var key = GetKey(row, endNumberI + 1);
            if(!char.IsDigit(right) && right != '.')
            {
                if (right == '*' && isPartTwo && _default.TryGetValue(key, out var value))
                    value.Add(number);
                return true;
            }
        }

        return false;
    }

    private int BodyPartTwo(string[] input)
    {
        var total = 0;
        _default.Clear();
        // Save all * points in dictionary
        for(int i = 0; i < input.Length; i++)
        {
            var row = input[i];
            for (var j = 0; j < row.Length; j++)
            {
                if (row[j] == '*')
                {
                    if (!_default.ContainsKey(GetKey(i, j)))
                    {
                        _default.Add(GetKey(i, j), new List<int>());
                    }
                }
            }
        }
        
        for (int i = 0; i < input.Length; i++)
        {
            var row = input[i];
            var number = "";
            var startNumberIndex = 0;
            for (var j = 0; j < row.Length; j++)
            {
                if (char.IsDigit(row[j]))
                {
                    startNumberIndex = startNumberIndex == 0 ? j : startNumberIndex;
                    number += row[j];
                    continue;
                }

                total = DoSum(number, i, startNumberIndex, j, input, total, true);
                startNumberIndex = 0;
                number = "";
            }
            total = DoSum(number, i, startNumberIndex, row.Length, input, total, true);
        }
        return _default.Values.Where(x => x.Count == 2).Sum(x => x[0] * x[1]);
    }  
    
    private string GetKey(int i, int j)
    {
        return $"{i},{j}";
    }
    
    #endregion
}