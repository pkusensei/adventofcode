using System.Diagnostics;

int P1(IEnumerable<string> input) => Parse(input).Max();

int P2(IEnumerable<string> input) => Parse(input).OrderByDescending(x => x).Take(3).Sum();

List<int> Parse(IEnumerable<string> input)
{
    var values = new List<int>();
    var current = 0;
    foreach (var item in input)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            values.Add(current);
            current = 0;
        }
        else
        {
            current += int.Parse(item);
        }
    }
    if (current != 0)
        values.Add(current);
    return values;
}

var test = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000".Split('\n');

Debug.Assert(P1(test) == 24000);
Debug.Assert(P2(test) == 45000);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 70369);
Debug.Assert(P2(input) == 203002);
