using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._8;

public class DayEight
{

    public long ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(8, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(8, "LongInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(8, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }

    public long ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(8, "LongInput-2.txt");
        return BodyPartTwo(input);
    }

    #region Body

    private long BodyPartOne(string[] input)
    {
        var map = new Dictionary<string, List<string>>();
        var instruction = input[0].Select(x => x).ToList();
        for (var i = 2; i < input.Length; i++)
        {
            var key = input[i].Split("=")[0].Trim();
            var left = input[i].Split("=")[1].Split(",")[0].Trim().Substring(1);
            var right = input[i].Split("=")[1].Split(",")[1].Trim().Substring(0,3);
            map.Add(key, new List<string> { left, right });
        }

        var target = "ZZZ"; //map.Last().Key;
        int steps = 0;
        var module = instruction.Count;
        var next = "AAA";
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        while (next.Trim() != target)
        {
            var currentInstruction = instruction[steps % module];
            steps++;
            var current = next;
            if (currentInstruction == 'L')
                next = map[next][0];
            else next = map[next][1];
        }
        return steps;
    }

    private long BodyPartTwo(string[] input)
    {
        var map = new Dictionary<string, List<string>>();
        var instruction = input[0].Select(x => x).ToList();
        for (var i = 2; i < input.Length; i++)
        {
            var key = input[i].Split("=")[0].Trim();
            var left = input[i].Split("=")[1].Split(",")[0].Trim().Substring(1);
            var right = input[i].Split("=")[1].Split(",")[1].Trim().Substring(0,3);
            map.Add(key, new List<string> { left, right });
        }

        var steps = 0;
        var module = instruction.Count;
        var next = map.Keys.Where(x => x[2] == 'A').ToHashSet();
        
        var map2 = new Dictionary<string, List<int>>();
        
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        while (map2.Count == 0 || map2.Any(x => x.Value.Count < 100))
        {
            var newHash = new HashSet<string>();
            foreach (var item in next)
            {
                var currentInstruction = instruction[steps % module];
                var nextTemp = currentInstruction == 'L' ? map[item][0] : map[item][1];
                newHash.Add(nextTemp);

                if (nextTemp[2] == 'Z')
                {
                    if (map2.TryGetValue(item, out var value))
                        value.Add(steps);
                    else
                        map2.Add(item, new List<int> { steps, 0 });
                }
            }
            steps++;
            next = newHash;
        }
        
        // Get Mimino comune multiple between difference of steps
        var list = new List<long>();
        foreach (var x in map2)
        {
            list.Add(x.Value[99] - x.Value[98]);
        }
        
        return FindLCM(list.ToArray());
    }
    
    private static long CalculateGCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static long CalculateLCM(long a, long b)
    {
        return (a * b) / CalculateGCD(a, b);
    }

    private static long FindLCM(long[] numbers)
    {
        if (numbers.Length < 2)
        {
            throw new ArgumentException("At least two numbers are required to find LCM.");
        }

        long lcm = numbers[0];

        for (int i = 1; i < numbers.Length; i++)
        {
            lcm = CalculateLCM(lcm, numbers[i]);
        }

        return lcm;
    }

    #endregion
}