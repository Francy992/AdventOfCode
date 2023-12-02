﻿// See https://aka.ms/new-console-template for more information

using AdventOfCode.Days._1;
using AdventOfCode.Days._2;

Console.WriteLine("First day of Advent of Code 2023!");
var firstDay = new DayOne();
Console.WriteLine($"Result of day one for small input: {firstDay.ResolvePartOneSmallInput()}");
Console.WriteLine($"Result of day one for long input: {firstDay.ResolvePartOneLongInput()}");
Console.WriteLine($"Result of day one for small input part 2: {firstDay.ResolvePartTwoSmallInput()}");
Console.WriteLine($"Result of day one for long input part 2: {firstDay.ResolvePartTwoLongInput()}");

var secondDay = new DayTwo();
Console.WriteLine($"Result of day two for small input: {secondDay.ResolvePartOneSmallInput()}");
Console.WriteLine($"Result of day two for long input: {secondDay.ResolvePartOneLongInput()}");
Console.WriteLine($"Result of day two for small input part 2: {secondDay.ResolvePartTwoSmallInput()}");
Console.WriteLine($"Result of day two for long input part 2: {secondDay.ResolvePartTwoLongInput()}");

