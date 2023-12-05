using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._5;

public class DayFive
{
    private readonly Dictionary<string, List<int>> _default = new();

    public long ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(5, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(5, "LongInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(5, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }

    public long ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(5, "LongInput-2.txt");
        return BodyPartTwo(input);
    }

    #region Body

    private long BodyPartOne(string[] input)
    {
        var total = long.MaxValue;
        input = input.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var seedToSoilNumberOfRows = GetNumberOfRows(input, "seed-to-soil map:", "soil-to-fertilizer map:");
        var soilToFertilizerNumberOfRows = GetNumberOfRows(input, "soil-to-fertilizer map:", "fertilizer-to-water map:");
        var fertilizerToWaterNumberOfRows = GetNumberOfRows(input, "fertilizer-to-water map:", "water-to-light map:");
        var waterToLightNumberOfRows = GetNumberOfRows(input, "water-to-light map:", "light-to-temperature map:");
        var lightToTemperatureNumberOfRows = GetNumberOfRows(input, "light-to-temperature map:", "temperature-to-humidity map:");
        var temperatureToHumidityNumberOfRows = GetNumberOfRows(input, "temperature-to-humidity map:", "humidity-to-location map:");
        var humidityToLocationNumberOfRows = GetNumberOfRows(input, "humidity-to-location map:", "end");
        
        
        foreach (var number in input[0].Split(" ").Where(x => long.TryParse(x, out _)).Select(x => long.Parse(x)))
        {
            var soil = GetNextMap(input, "seed-to-soil map:", seedToSoilNumberOfRows, number);
            var fertilizer = GetNextMap(input, "soil-to-fertilizer map:", soilToFertilizerNumberOfRows, soil);
            var water = GetNextMap(input, "fertilizer-to-water map:", fertilizerToWaterNumberOfRows, fertilizer);
            var light = GetNextMap(input, "water-to-light map:", waterToLightNumberOfRows, water);
            var temperature = GetNextMap(input, "light-to-temperature map:", lightToTemperatureNumberOfRows, light);
            var humidity = GetNextMap(input, "temperature-to-humidity map:", temperatureToHumidityNumberOfRows, temperature);
            var location = GetNextMap(input, "humidity-to-location map:", humidityToLocationNumberOfRows, humidity);
            
            total = Math.Min(total, location);
        }

        return total;
    }

    private int GetNumberOfRows(string[] input, string first, string second)
    {
        var firstIndex = -1;
        var secondIndex = -1;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == first)
            {
                firstIndex = i;
            }
            else if (input[i] == second)
            {
                secondIndex = i;
            }
        }

        if(secondIndex == -1)
            secondIndex = input.Length;
        return secondIndex - firstIndex - 1;
    }

    private long GetNextMap(string[] input, string nameToFind, int numberOfRows, long toFind)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if(input[i] != nameToFind)
                continue;

            for (int j = i + 1; j <= i + numberOfRows; j++)
            {
                var start = long.Parse(input[j].Split(" ")[1]);
                var end = long.Parse(input[j].Split(" ")[0]);
                var range = long.Parse(input[j].Split(" ")[2]);
                
                if(toFind >= start && toFind <= start + range)
                    return end + toFind - start;
            }
            break;
        }

        return toFind;
    }
    
    private List<long[]> GetOptimizedNextMap(string[] input, string nameToFind, int numberOfRows)
    {
        var result = new List<long[]>();
        for (var i = 0; i < input.Length; i++)
        {
            if(input[i] != nameToFind)
                continue;

            for (var j = i + 1; j <= i + numberOfRows; j++)
            {
                var start = long.Parse(input[j].Split(" ")[1]);
                var end = long.Parse(input[j].Split(" ")[0]);
                var range = long.Parse(input[j].Split(" ")[2]);
                result.Add(new []
                {
                    end == 0 ? 0 : start, end == 0 ? start : end, range
                });
            }
            break;
        }

        return result;
    }

    private long BodyPartTwo(string[] input)
    {
        var total = long.MaxValue;
        input = input.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var seedToSoilNumberOfRows = GetNumberOfRows(input, "seed-to-soil map:", "soil-to-fertilizer map:");
        var soilToFertilizerNumberOfRows = GetNumberOfRows(input, "soil-to-fertilizer map:", "fertilizer-to-water map:");
        var fertilizerToWaterNumberOfRows = GetNumberOfRows(input, "fertilizer-to-water map:", "water-to-light map:");
        var waterToLightNumberOfRows = GetNumberOfRows(input, "water-to-light map:", "light-to-temperature map:");
        var lightToTemperatureNumberOfRows = GetNumberOfRows(input, "light-to-temperature map:", "temperature-to-humidity map:");
        var temperatureToHumidityNumberOfRows = GetNumberOfRows(input, "temperature-to-humidity map:", "humidity-to-location map:");
        var humidityToLocationNumberOfRows = GetNumberOfRows(input, "humidity-to-location map:", "end");
        
        var seedToSoil = GetOptimizedNextMap(input, "seed-to-soil map:", seedToSoilNumberOfRows);
        var soilToFertilizer = GetOptimizedNextMap(input, "soil-to-fertilizer map:", soilToFertilizerNumberOfRows);
        var fertilizerToWater = GetOptimizedNextMap(input, "fertilizer-to-water map:", fertilizerToWaterNumberOfRows);
        var waterToLight = GetOptimizedNextMap(input, "water-to-light map:", waterToLightNumberOfRows);
        var lightToTemperature = GetOptimizedNextMap(input, "light-to-temperature map:", lightToTemperatureNumberOfRows);
        var temperatureToHumidity = GetOptimizedNextMap(input, "temperature-to-humidity map:", temperatureToHumidityNumberOfRows);
        var humidityToLocation = GetOptimizedNextMap(input, "humidity-to-location map:", humidityToLocationNumberOfRows);
        
        var toCycle = input[0].Split(" ").Where(x => long.TryParse(x, out _)).Select(x => long.Parse(x)).ToList();
        long count = 0;
        // start time 
        var startTime = DateTime.Now;
        for (var i = 0; i < toCycle.Count - 1; i += 2)
        {
            var start = toCycle[i];
            var end = toCycle[i + 1];
            var countForI = 0;
            var timeForI = DateTime.Now;
            for (var number = start; number < start + end; number++)
            {
                count++;
                var soil = GetNextMap(seedToSoil, number);
                var fertilizer = GetNextMap(soilToFertilizer, soil);
                var water = GetNextMap(fertilizerToWater, fertilizer);
                var light = GetNextMap(waterToLight, water);
                var temperature = GetNextMap(lightToTemperature, light);
                var humidity = GetNextMap(temperatureToHumidity, temperature);
                var location = GetNextMap(humidityToLocation, humidity);
            
                total = Math.Min(total, location);
                
                if (count % 20000000 == 0)
                {
                    Console.WriteLine($"Count: {count} - Total: {total}. Time: {DateTime.Now - startTime}");
                }
            }
            Console.WriteLine($"Count for i: {i} - Count: {countForI}. Time: {DateTime.Now - timeForI}");
            Console.WriteLine($"Finish i: {i} - Count: {count} - Total: {total}. Time: {DateTime.Now - startTime}");
        }
        
        Console.WriteLine($"Finish - Count: {count} - Total: {total}. Time: {DateTime.Now - startTime}");
        return total;
    }

    private long GetNextMap(List<long[]> seedToSoil, long nameToFind)
    {
        foreach (var row in seedToSoil)
        {
            if (nameToFind >= row[0] && nameToFind <= row[0] + row[2])
                return nameToFind + row[1] - row[0];
        }

        return nameToFind;
    }

    #endregion
}