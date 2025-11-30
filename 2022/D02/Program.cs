using System.Diagnostics;

int P1(IEnumerable<string> lines) => lines.Select(
        line =>
        {
            int a = ToScore(line[0]);
            int b = ToScore(line[2]);
            if (a == b)
                return 3 + b;
            else if ((a + 1) % 3 == b % 3)
                return 6 + b;
            else
                return 0 + b;
        }
    ).Sum();

int P2(IEnumerable<string> lines) => lines.Select(
        line => line[2] switch
        {
            'X' => 0 + (ToScore(line[0]) + 1) % 3 + 1,
            'Z' => 6 + ToScore(line[0]) % 3 + 1,
            _ => 3 + ToScore(line[0])
        }
    ).Sum();

int ToScore(char c) => c switch
{
    'A' or 'X' => 1,
    'B' or 'Y' => 2,
    _ => 3,
};

var test = @"A Y
B X
C Z".Split('\n', StringSplitOptions.TrimEntries);
Debug.Assert(P1(test) == 15);
Debug.Assert(P2(test) == 12);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 10310);
Debug.Assert(P2(input) == 14859);