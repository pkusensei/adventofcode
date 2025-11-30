using System.Diagnostics;

static IReadOnlyDictionary<(int, int), int> Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<(int, int), int>();
    foreach (var (y, line) in lines.Select((line, y) => (y, line)))
    {
        foreach (var (x, num) in line.Trim().Select((c, x) => (x, char.GetNumericValue(c))))
        {
            res.Add((x, y), (int)num);
        }
    }
    return res;
}

static int P1(IEnumerable<string> lines)
{
    var deltas = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    var points = Parse(lines);
    var lowPts = FindLowPoints(points);
    var res = points.Where(kv => lowPts.Contains(kv.Key)).Sum(kv => kv.Value + 1);
    return res;
}

var test = @"2199943210
3987894921
9856789892
8767896789
9899965678".Split('\n');
Debug.Assert(P1(test) == 15);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 545);

static IReadOnlyList<(int, int)> FindLowPoints(IReadOnlyDictionary<(int, int), int> points)
{
    var deltas = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    var res = new List<(int, int)>();
    foreach (var ((x, y), num) in points)
    {
        var isLowPoint = true;
        foreach (var (dx, dy) in deltas)
        {
            if (points.TryGetValue((x + dx, y + dy), out var neighbor) && neighbor <= num)
            {
                isLowPoint = false;
            }
        }
        if (isLowPoint)
            res.Add((x, y));
    }
    return res;
}

static HashSet<(int, int)> FindBasin(IReadOnlyDictionary<(int, int), int> points, (int x, int y) lowPoint)
{
    var deltas = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    var basin = new HashSet<(int, int)> { lowPoint };
    var attempts = new Queue<(int, int)>();
    foreach (var (dx, dy) in deltas)
    {
        var pt = (lowPoint.x + dx, lowPoint.y + dy);
        if (points.TryGetValue(pt, out var value) && value != 9 && !basin.Contains(pt))
        {
            attempts.Enqueue(pt);
        }
    }
    while (attempts.Count > 0)
    {
        var (x, y) = attempts.Dequeue();
        var inBasin = true;
        foreach (var (dx, dy) in deltas)
        {
            var neighbor = (x + dx, y + dy);
            if (points.TryGetValue(neighbor, out var value) && !basin.Contains(neighbor))
            {
                // Basins allow 4-3-2-2-5-6
                // But low points are strictly 4-3-2-5-6 (no duplicates)
                if (value < points[(x, y)])
                {
                    inBasin = false;
                }
            }
        }
        if (inBasin)
        {
            basin.Add((x, y));
            foreach (var (dx, dy) in deltas)
            {
                var pt = (x + dx, y + dy);
                if (points.TryGetValue(pt, out var value) && value != 9 && !basin.Contains(pt))
                {
                    attempts.Enqueue(pt);
                }
            }
        }
    }
    return basin;
}

static int P2(IEnumerable<string> lines)
{
    var points = Parse(lines);
    var lowPts = FindLowPoints(points);
    var basins = lowPts.Select(pt => FindBasin(points, pt).Count).ToList();
    basins.Sort();
    basins.Reverse();
    return basins[0] * basins[1] * basins[2];
}

Debug.Assert(P2(test) == 1134);
Debug.Assert(P2(input) == 950600);