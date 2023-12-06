using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._6;

public class DaySix
{
    private readonly Dictionary<string, List<int>> _default = new();

    public int ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(6, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    public int ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(6, "LongInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(6, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }

    public long ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(6, "LongInput-2.txt");
        return BodyPartTwo(input);
    }

    #region Body

    private int BodyPartOne(string[] input)
    {
        var result = 1;
        var races = input[0].Split(":")[1].Split(" ").Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Zip(input[1].Split(":")[1].Split(" ").Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                , (x, y) => new { Time = int.Parse(x), Distance = int.Parse(y) })
            .ToList();

        foreach (var race in races)
        {
            var counter = 0;
            for (int i = 1; i < race.Time; i++)
            {
                var currentDistance = (race.Time - i) * i;
                if (currentDistance > race.Distance)
                    counter++;
            }

            if (counter > 0)
                result *= counter;
        }
        return result;
    }

    private long BodyPartTwo(string[] input)
    {
        var result = 1;
        var races = input[0].Split(":")[1].Split(" ").Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Zip(input[1].Split(":")[1].Split(" ").Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                , (x, y) => new { Time = long.Parse(x), Distance = long.Parse(y) })
            .ToList();

        foreach (var race in races)
        {
            var counter = 0;
            for (var i = 1; i < race.Time; i++)
            {
                var currentDistance = (race.Time - i) * i;
                if (currentDistance > race.Distance)
                    counter++;
            }

            if (counter > 0)
                result *= counter;
        }
        return result;
    }

    #endregion
}