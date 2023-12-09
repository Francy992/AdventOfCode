using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._9;

public class DayNine
{
    private const int Day = 9;

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

    private long BodyPartOne(string[] input)
    {
        var result = 0;
        foreach (var startRow in input)
        {
            result += FindNextValueForRow(startRow);
        }

        return result;
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