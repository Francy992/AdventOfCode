using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._4;

public class DayFour
{
    private readonly Dictionary<string, List<int>> _default = new();

    public int ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(4, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    public int ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(4, "LongInput-1.txt");
        return BodyPartOne(input);
    }

    public int ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(4, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }

    public int ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(4, "LongInput-2.txt");
        return BodyPartTwo(input);
    }

    #region Body

    private int BodyPartOne(string[] input)
    {
        var total = 0;
        foreach (var str in input)
        {
            var winningNumber = str.Split("|")[0].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet();
            var numbers = str.Split("|")[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet();
            var myWinningNumber = winningNumber.Intersect(numbers).Count();
            if (myWinningNumber == 1)
                total += 1;
            else
                total += (int)Math.Pow(2, myWinningNumber - 1);
        }

        return total;
    }

    private int BodyPartTwo(string[] input)
    {
        var dict = new Dictionary<string, int>();
        // Inizialize the dictionary
        for(int i = 0; i < input.Length; i++)
        {
            dict.Add(GetKey(i + 1, 0), 1);
        }
        
        for(int i = 0; i < input.Length; i++)
        {
            var str = input[i];
            var winningNumber = str.Split("|")[0].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet();
            var numbers = str.Split("|")[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet();
            var myWinningNumber = winningNumber.Intersect(numbers).Count();
            for(int j = 0; j < myWinningNumber; j++)
            {
                var currentCopy = dict.ContainsKey(GetKey(i + 1, 0)) ? dict[GetKey(i + 1, 0)] : 1;
                if (dict.ContainsKey(GetKey(i + 1, j+1)))
                {
                    dict[GetKey(i + 1, j+1)] = dict[GetKey(i + 1, j+1)] + currentCopy;
                }
                else
                {
                    dict.Add(GetKey(i + 1, j+1), currentCopy);
                }
            }
        }

        return dict.Where(x => x.Key.Length <= input.Length).Sum(x => x.Value);
    }

    private string GetKey(int i, int j)
    {
        return $"{i + j}";
    }

    #endregion
}