using System.Diagnostics;

int Solve(IEnumerable<string> lines, Pred pred)
{
    var grid = Parse(lines);
    int low = grid.Keys.Select(c => c.Y).Max();
    while (pred(grid, low, new Coord(500, 0)))
    {
    }
    return grid.Values.Count(s => s == 'o');
}

int P1(IEnumerable<string> lines) => Solve(lines, Fall);
int P2(IEnumerable<string> lines) => Solve(lines, Fall2);

bool Fall(IDictionary<Coord, char> grid, in int low, Coord point)
{
    while (true)
    {
        var pNext = point with { Y = point.Y + 1 };
        var pLeft = pNext with { X = pNext.X - 1 };
        var pRight = pNext with { X = pNext.X + 1 };
        if (!grid.ContainsKey(pNext))
        {
            point = pNext;
        }
        else if (!grid.ContainsKey(pLeft))
        {
            point = pLeft;
        }
        else if (!grid.ContainsKey(pRight))
        {
            point = pRight;
        }
        else
        {
            grid[point] = 'o';
            return true; // falls to rest
        }

        if (point.Y >= low)
            return false;// falls out
    }
}

bool Fall2(IDictionary<Coord, char> grid, in int low, Coord point)
{
    while (true)
    {
        var pNext = point with { Y = point.Y + 1 };
        var pLeft = pNext with { X = pNext.X - 1 };
        var pRight = pNext with { X = pNext.X + 1 };
        if (!grid.ContainsKey(pNext))
        {
            point = pNext;
        }
        else if (!grid.ContainsKey(pLeft))
        {
            point = pLeft;
        }
        else if (!grid.ContainsKey(pRight))
        {
            point = pRight;
        }
        else
        {
            grid[point] = 'o';
            if (point.Y == 0)
            {
                return false; // blocked
            }
            return true; // falls to rest
        }
        if (point.Y == low + 1)
        {
            grid[point] = 'o';
            return true;
        }
    }
}

Dictionary<Coord, char> Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<Coord, char>();
    foreach (var pairs in lines.Select(line => line.Split("->", StringSplitOptions.TrimEntries)))
    {
        var points = pairs.Select(p =>
        {
            var nums = p.Split(',').Select(int.Parse);
            return new Coord(nums.First(), nums.Last());
        }).ToList();

        foreach (var (p, idx) in points.Select((p, i) => (p, i)).SkipLast(1))
        {
            var pNext = points[idx + 1];
            if (p.X == pNext.X)
            {
                // the scan can go upwards... WTF
                for (int y = Math.Min(p.Y, pNext.Y); y <= Math.Max(p.Y, pNext.Y); y++)
                {
                    res[new Coord(p.X, y)] = '#';
                }
            }
            if (p.Y == pNext.Y)
            {
                for (int x = Math.Min(p.X, pNext.X); x <= Math.Max(p.X, pNext.X); x++)
                {
                    res[new Coord(x, p.Y)] = '#';
                }
            }
        }
        res[points.Last()] = '#';
    }

    return res;
}

var test = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9".Split('\n');
Debug.Assert(P1(test) == 24);
Debug.Assert(P2(test) == 93);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 763);
Debug.Assert(P2(input) == 23921);

record struct Coord(int X, int Y);
delegate bool Pred(IDictionary<Coord, char> grid, in int low, Coord point);