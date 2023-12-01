// See https://aka.ms/new-console-template for more information

using AdventOfCode.Days._1;

Console.WriteLine("First day of Advent of Code 2023!");
var firstDay = new DayOne();
Console.WriteLine($"Result of day one for small input: {firstDay.ResolvePartOneSmallInput()}");
Console.WriteLine($"Result of day one for long input: {firstDay.ResolvePartOneLongInput()}");
Console.WriteLine($"Result of day one for small input part 2: {firstDay.ResolvePartTwoSmallInput()}");
Console.WriteLine($"Result of day one for long input part 2: {firstDay.ResolvePartTwoLongInput()}");

