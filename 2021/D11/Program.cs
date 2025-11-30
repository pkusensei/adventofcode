using System.Diagnostics;

static (Dictionary<(int, int), int>, int) Step(Dictionary<(int, int), int> grid)
{
    var deltas = new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
    var flashes = new HashSet<(int, int)>();
    var tmp = new Queue<(int, int)>();

    foreach (var pt in grid.Keys)
    {
        grid[pt] += 1;
        if (grid[pt] > 9)
        {
            flashes.Add(pt);
            tmp.Enqueue(pt);
        }
    }
    while (tmp.Count > 0)
    {
        var current = tmp.Dequeue();
        var points = deltas.Select(delta => (delta.Item1 + current.Item1, delta.Item2 + current.Item2))
            .Where(pt => grid.ContainsKey(pt) && !flashes.Contains(pt));
        foreach (var pt in points)
        {
            grid[pt] += 1;
            if (grid[pt] > 9)
            {
                flashes.Add(pt);
                tmp.Enqueue(pt);
            }
        }
    }
    foreach (var pt in flashes)
    {
        grid[pt] = 0;
    }

    return (grid, flashes.Count);
}

static Dictionary<(int, int), int> Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<(int, int), int>();
    foreach (var (line, y) in lines.Select((line, idx) => (line, idx)))
    {
        foreach (var (c, x) in line.Trim().Select((c, idx) => (c, idx)))
        {
            res.Add((x, y), (int)char.GetNumericValue(c));
        }
    }
    return res;
}

static int P1(IEnumerable<string> lines, int steps)
{
    var grid = Parse(lines);
    var count = 0;
    for (int i = 0; i < steps; i++)
    {
        var res = Step(grid);
        grid = res.Item1;
        count += res.Item2;
    }
    return count;
}

var test = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526".Split('\n');
Debug.Assert(P1(test, 10) == 204);
Debug.Assert(P1(test, 100) == 1656);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input, 100) == 1683);

static int P2(IEnumerable<string> lines)
{
    var grid = Parse(lines);
    var count = 0;
    while (true)
    {
        count += 1;
        var res = Step(grid);
        grid = res.Item1;
        if (grid.Values.All(x => x == 0))
        {
            return count;
        }
    }
}

Debug.Assert(P2(test) == 195);
Debug.Assert(P2(input) == 788);