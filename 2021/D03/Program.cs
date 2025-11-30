using System.Diagnostics;

static int P1(IList<string> input)
{
    var length = input[0].Length;
    var gamma = 0;
    var epsilon = 0;
    for (int i = 0; i < length; i++)
    {
        var sum = (int)input.Select(s => Char.GetNumericValue(s, length - i - 1)).Sum();
        if (sum > input.Count / 2)
        {
            gamma += (int)Math.Pow(2, i);
        }
        else
        {
            epsilon += (int)Math.Pow(2, i);
        }
    }
    return gamma * epsilon;
}

var test = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010".Split('\n').Select(s => s.Trim()).ToList();
Debug.Assert(P1(test) == 198);

var lines = File.ReadLines("input.txt").Select(s => s.Trim()).ToList();
Debug.Assert(P1(lines) == 1307354);

static int FindO2(IList<string> input, int index)
{
    if (input.Count == 1)
    {
        return Convert.ToInt32(input[0], 2);
    }
    var sum = (int)input.Select(str => Char.GetNumericValue(str, index)).Sum();
    var numToKeep = sum * 2 < input.Count ? 0 : 1;
    var strs = input.Where(s => Char.GetNumericValue(s, index) == numToKeep).ToList();
    return FindO2(strs, index + 1);
}

static int FindCO2(IList<string> input, int index)
{
    if (input.Count == 1)
    {
        return Convert.ToInt32(input[0], 2);
    }
    var sum = (int)input.Select(str => Char.GetNumericValue(str, index)).Sum();
    var numToKeep = sum * 2 >= input.Count ? 0 : 1;
    var strs = input.Where(s => Char.GetNumericValue(s, index) == numToKeep).ToList();
    return FindCO2(strs, index + 1);
}

static int P2(IList<string> input)
{
    var o2 = FindO2(input, 0);
    var co2 = FindCO2(input, 0);
    return o2 * co2;
}

Debug.Assert(P2(test) == 230);
Debug.Assert(P2(lines) == 482500);