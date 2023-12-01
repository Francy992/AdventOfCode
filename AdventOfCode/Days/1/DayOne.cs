using System.Text.RegularExpressions;

namespace AdventOfCode.Days._1;

public class DayOne
{
    private Dictionary<string, int> _mapping = new()
    {
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        {"5", 5},
        {"6", 6},
        {"7", 7},
        {"8", 8},
        {"9", 9},
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9}
    };
    public int ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(1, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    private int BodyPartOne(string[] input)
    {
        var total = 0;
        foreach (var calibration in input)
        {
            var numbers = calibration.Where(x => Char.IsDigit(x)).ToList();
            var number = int.Parse($"{numbers.First()}{numbers.Last()}");
            total += number;
        }

        return total;
    }

    public int ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(1, "LongInput-1.txt");
        return BodyPartOne(input);
    }   
    
    public int ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(1, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }

    private int BodyPartTwo(string[] input)
    {
        var total = 0;
        foreach (var calibration in input)
        {
            var first = 0;
            var last = 0;
            var substrings = GetSubstrings(calibration);
            foreach (var sub in substrings)
            {
                if(!_mapping.TryGetValue(sub, out var currentValue))
                    continue;
                
                first = currentValue;
                break;
            }
            for(int i = substrings.Count - 1; i >= 0; i--)
            {
                if(!_mapping.TryGetValue(substrings[i], out var currentValue))
                    continue;
                
                last = currentValue;
                break;
            }
            
            var number = int.Parse($"{first}{last}");
            total += number;
        }

        return total;
    }

    private List<string> GetSubstrings(string input)
    {
        var substrings = new List<string>();
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input.Length - i; j++)
            {
                substrings.Add(input.Substring(i, j + 1));
            }
        }

        return substrings;
    }
    
    public int ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(1, "LongInput-2.txt");
        return BodyPartTwo(input);
    }   
}