using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._2;

public class DayTwo
{
    private readonly Dictionary<string, int> _default = new()
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 }
    };
    
    public int ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(2, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    private int BodyPartOne(string[] input)
    {
        var total = 0;
        foreach (var game in input)
        {
            var gameNumber = GetGameNumber(game);
            var isPossible = true;
            foreach (var subGame in game.Split(":")[1].Split(";"))
            {
                var cubes = subGame.Split(",");
                foreach (var cube in cubes)
                {
                    var color = cube.Trim().Split(" ")[1].Trim();
                    var targetNumber = _default[color];
                    var number = int.Parse(cube.Trim().Split(" ")[0].Trim());
                    if (number <= targetNumber) continue;
                    isPossible = false;
                    break;
                }
                if(!isPossible)
                    break;
            }
            
            if(isPossible)
                total += gameNumber;
        }

        return total;
    }

    private int GetGameNumber(string game)
    {
        // Example Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        return int.Parse(game.Split(":")[0].Split(" ")[1]);
    }

    public int ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(2, "LongInput-1.txt");
        return BodyPartOne(input);
    }   
    
    public int ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(2, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }
    
    private int BodyPartTwo(string[] input)
    {
        var total = 0;
        foreach (var game in input)
        {
            var dictForGame = new Dictionary<string, int>();
            foreach (var subGame in game.Split(":")[1].Split(";"))
            {
                var cubes = subGame.Split(",");
                foreach (var cube in cubes)
                {
                    var color = cube.Trim().Split(" ")[1].Trim();
                    var number = int.Parse(cube.Trim().Split(" ")[0].Trim());
                    
                    if (dictForGame.ContainsKey(color))
                    {
                        dictForGame[color] = Math.Max(dictForGame[color], number);
                    }
                    else
                    {
                        dictForGame.Add(color, number);
                    }
                }
            }
            var currentMultiple = 1;
            foreach (var (key, value) in dictForGame)
            {
                currentMultiple *= value;
            }
            total += currentMultiple;
        }

        return total;
    }
    
    public int ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(2, "LongInput-2.txt");
        return BodyPartTwo(input);
    }   
}