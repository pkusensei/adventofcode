using System.Diagnostics;
using System.Text;

int P1(IEnumerable<string> lines) => Parse(lines)
        .Select((x, idx) => (x, idx + 1))
        .Where(pair => (pair.Item2 + 20) % 40 == 0 && pair.Item2 <= 220)
        .Select(pair => pair.Item2 * pair.x)
        .Sum();

void P2(IEnumerable<string> lines)
{
    var sb = new StringBuilder(40);
    foreach (var (x, i) in Parse(lines).Select((x, i) => (x, i)))
    {
        var pos = i % 40;
        var sprites = Enumerable.Range(x - 1, 3);
        if (sprites.Contains(pos))
            sb.Append('#');
        else
            sb.Append(' '); // '.' is not visually friendly together with '#'
        if (pos == 39)
        {
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }
    }
}

IEnumerable<int> Parse(IEnumerable<string> lines)
{
    int x = 1;
    foreach (var line in lines)
    {
        if (line.StartsWith("noop"))
        {
            yield return x;
        }
        else if (line.StartsWith("addx"))
        {
            var v = int.Parse(line.Split(' ', StringSplitOptions.TrimEntries)[1]);
            for (int i = 0; i < 2; i++)
            {
                yield return x;
            }
            x += v;
        }
    }
}

var test = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop".Split('\n');
Debug.Assert(P1(test) == 13140);
// P2(test);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 14540);
P2(input); //EHZFZHCZ