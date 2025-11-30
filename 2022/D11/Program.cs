using System.Diagnostics;

int P1(IEnumerable<string> lines)
{
    var units = Parse(lines);
    for (int i = 0; i < 20; i++)
    {
        foreach (var unit in units)
        {
            unit.Process(units);
        }
    }
    return units.Select(u => u.InspectTimes)
                .OrderByDescending(x => x).Take(2).Aggregate((x, y) => x * y);
}

Int64 P2(IEnumerable<string> lines)
{
    var units = Parse(lines);
    var level = units.Select(u => u.Divisor).Aggregate(1, (x, y) => x * y);
    for (int i = 0; i < 10000; i++)
    {
        foreach (var unit in units)
        {
            unit.Process(units, level);
        }
    }
    return units.Select(u => (Int64)u.InspectTimes)
                .OrderByDescending(x => x).Take(2).Aggregate((x, y) => x * y);
}

List<Unit> Parse(IEnumerable<string> lines)
{
    var res = new List<Unit>();
    foreach (var chunk in lines.Chunk(7))
    {
        var items = chunk[1].Split(':')[1]
                            .Split(',', StringSplitOptions.TrimEntries)
                            .Select(Int64.Parse).ToList();
        var op = GetOp(chunk[2]);
        var divisor = int.Parse(chunk[3].Split("by ", StringSplitOptions.TrimEntries)[1]);
        var tt = int.Parse(chunk[4].Split("monkey ", StringSplitOptions.TrimEntries)[1]);
        var tf = int.Parse(chunk[5].Split("monkey ", StringSplitOptions.TrimEntries)[1]);
        res.Add(new Unit
        {
            Items = items,
            Op = op,
            Divisor = divisor,
            TargetT = tt,
            TargetF = tf
        });
    }
    return res;
}

Func<Int64, Int64> GetOp(string line)
{
    var s = line.Split("old ", StringSplitOptions.TrimEntries)[1];
    Func<Int64, Int64, Int64> op = s[0] switch
    {
        '*' => (Int64 x, Int64 y) => x * y,
        _ => (Int64 x, Int64 y) => x + y,
    };

    var operand = s.Split(' ')[1];
    if (Int64.TryParse(operand, out var res))
        return (Int64 x) => op(x, res);
    else
        return (Int64 x) => op(x, x);
}

var test = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1".Split('\n');
Debug.Assert(P1(test) == 10605);
Debug.Assert(P2(test) == 2713310158);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 56595);
Debug.Assert(P2(input) == 15693274740);

class Unit
{
    public List<Int64> Items { get; init; } = null!;
    public Func<Int64, Int64> Op { get; init; } = null!;
    public int Divisor { get; init; }
    public int TargetT { get; init; }
    public int TargetF { get; init; }
    public int InspectTimes { get; private set; } = 0;

    public void Process(List<Unit> units) => Process(units, 0);

    public void Process(List<Unit> units, int level)
    {
        foreach (var item in Items)
        {
            // This could produce Int64 values with Int32 param
            // The culprit is likely "old * old"
            var newItem = Op(item);

            if (level == 0) // p1
                newItem /= 3;
            else            // p2
                newItem %= level;

            if (newItem % Divisor == 0)
                units[TargetT].Items.Add(newItem);
            else
                units[TargetF].Items.Add(newItem);
        }
        InspectTimes += Items.Count;
        Items.Clear();
    }
}