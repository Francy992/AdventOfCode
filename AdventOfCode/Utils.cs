namespace AdventOfCode;

public static class Utils
{
    private const string InputFilePath = "Days/";

    public static string[] ReadAllLines(int day, string file)
    {
        return File.ReadAllLines(Path.Combine(InputFilePath, day.ToString(), file));
    }
}