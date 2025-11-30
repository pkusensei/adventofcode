using System.Diagnostics;

static (IReadOnlyDictionary<(int x, int y), char>, int width, int height) Parse(IList<string> lines)
{
    var res = new Dictionary<(int, int), char>();
    foreach (var (row, y) in lines.Select((line, i) => (line, i)))
    {
        foreach (var (c, x) in row.Trim().Select((c, x) => (c, x)))
        {
            res.Add((x, y), c);
        }
    }
    return (res, lines[0].Trim().Length, lines.Count);
}

static IReadOnlyDictionary<(int x, int y), char> MoveEast(IReadOnlyDictionary<(int x, int y), char> orig, int width)
{
    var res = new Dictionary<(int, int), char>(orig);
    foreach (var ((x, y), c) in orig.Where(kv => kv.Value == '>'))
    {
        var xidx = (x + 1) % width;
        if (orig[(xidx, y)] == '.')
        {
            res[(xidx, y)] = '>';
            res[(x, y)] = '.';
        }
    }
    return res;
}

static IReadOnlyDictionary<(int x, int y), char> MoveSouth(IReadOnlyDictionary<(int x, int y), char> orig, int height)
{
    var res = new Dictionary<(int, int), char>(orig);
    foreach (var ((x, y), c) in orig.Where(kv => kv.Value == 'v'))
    {
        var yidx = (y + 1) % height;
        if (orig[(x, yidx)] == '.')
        {
            res[(x, yidx)] = 'v';
            res[(x, y)] = '.';
        }
    }
    return res;
}

static int Solve(IList<string> lines)
{
    var (map, width, height) = Parse(lines);
    var count = 0;
    while (true)
    {
        count += 1;
        var tmp = MoveEast(map, width);
        tmp = MoveSouth(tmp, height);
        if (Equals(map, tmp))
            break;
        map = tmp;
    }
    return count;

    bool Equals(IReadOnlyDictionary<(int, int), char> a, IReadOnlyDictionary<(int, int), char> b)
    {
        var res = true;
        if (a.Count == b.Count)
        {
            foreach (var (k1, v1) in a)
            {
                if (b.TryGetValue(k1, out var v2) && v2 != v1)
                {
                    res = false;
                }
            }
        }
        return res;
    }
}

var test = @"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>".Split('\n').ToList();
Debug.Assert(Solve(test) == 58);

var input = File.ReadLines("input.txt").ToList();
Debug.Assert(Solve(input) == 305);