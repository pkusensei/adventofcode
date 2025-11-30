using System.Diagnostics;

string Solve(IEnumerable<string> lines) => Encode(Parse(lines).Sum());

string Encode(long num)
{
    var digits = new List<char>();
    while (num != 0)
    {
        var curr = (num + 2) % 5 - 2;
        var digit = curr switch
        {
            -2 => '=',
            -1 => '-',
            0 => '0',
            1 => '1',
            _ => '2',
        };
        digits.Insert(0, digit);
        num = (num + 2) / 5;
    }
    return string.Join("", digits);
}

IEnumerable<long> Parse(IEnumerable<string> lines)
{
    foreach (var line in lines.Select(l => l.Trim().Reverse()))
    {
        long res = 0;
        foreach (var (c, idx) in line.Select((c, i) => (c, i)))
        {
            var digit = c switch
            {
                '=' => -2,
                '-' => -1,
                '0' => 0,
                '1' => 1,
                _ => 2,
            };
            res += digit * (long)Math.Pow(5, idx);
        }
        yield return res;
    }
}

var test = @"1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122".Split('\n');
Debug.Assert(Solve(test) == "2=-1=0");

var input = File.ReadLines("input.txt");
Debug.Assert(Solve(input) == "2=0-2-1-0=20-01-2-20");