using System.Diagnostics;

(int p1, int p2) Solve(IEnumerable<string> lines)
{
    var p1 = 0;
    var p2 = 0;
    var units = Parse(lines);
    while (true)
    {
        p2 += 1;
        var proposed = new Dictionary<Coord, int>();
        foreach (var unit in units)
        {
            if (unit.Propose(units))
            {
                var p = unit.Proposed;
                if (proposed.ContainsKey(p))
                    proposed[p] += 1;
                else
                    proposed[p] = 1;
            }
        }
        if (proposed.Values.Count(v => v == 1) == 0)
            return (p1, p2);

        foreach (var unit in units)
        {
            if (proposed.TryGetValue(unit.Proposed, out var count) && count == 1)
                unit.Move();
        }
        if (p2 == 10)
        {
            var xmin = units.Min(u => u.Pos.X);
            var xmax = units.Max(u => u.Pos.X);
            var ymin = units.Min(u => u.Pos.Y);
            var ymax = units.Max(u => u.Pos.Y);
            p1 = (ymax - ymin + 1) * (xmax - xmin + 1) - units.Count;
        }
    }
}

List<Unit> Parse(IEnumerable<string> lines)
{
    var res = new List<Unit>();
    foreach (var (line, y) in lines.Select((l, i) => (l, i)))
    {
        foreach (var (c, x) in line.Trim().Select((c, i) => (c, i)))
        {
            if (c == '#')
                res.Add(new Unit(x, -y));
        }
    }
    return res;
}

var test = @".......#......
.....###.#....
...#...#.#....
....#...##....
...#.###......
...##.#.##....
....#..#......".Split('\n');
var (p1, p2) = Solve(test);
Debug.Assert(p1 == 110);
Debug.Assert(p2 == 20);

var input = File.ReadLines("input.txt");
(p1, p2) = Solve(input);
Debug.Assert(p1 == 3925);
Debug.Assert(p2 == 903);  // It's correct; just takes forever to run

record struct Coord(int X, int Y)
{
    public static Coord[] Deltas()
    => new Coord[] { new(-1, 1), new(0, 1), new(1, 1), new(1, 0),
                     new(1, -1), new(0, -1), new(-1, -1), new(-1, 0) };

    public static IEnumerable<Coord> Deltas(Direction dir)
    => dir switch
    {
        Direction.North => Deltas().Where(c => c.Y == 1),
        Direction.South => Deltas().Where(c => c.Y == -1),
        Direction.West => Deltas().Where(c => c.X == -1),
        _ => Deltas().Where(c => c.X == 1),
    };

    public static Coord operator +(Coord a, Coord b)
    => new(a.X + b.X, a.Y + b.Y);
}

class Unit
{
    public Unit(int x, int y)
    {
        Pos = new Coord(x, y);
        FirstDir = Direction.North;
    }

    public Coord Pos { get; private set; }
    public Coord Proposed { get; private set; }
    public Direction FirstDir { get; private set; }

    public void Move() => Pos = Proposed;

    public bool Propose(IEnumerable<Unit> units)
    {
        var startDir = FirstDir;
        FirstDir = (Direction)(((int)FirstDir + 1) % 4);

        bool empty = true;
        foreach (var d in Coord.Deltas())
        {
            if (units.Any(u => u.Pos == Pos + d))
            {
                empty = false;
                break;
            }
        }
        if (empty)
        {
            Proposed = Pos;
            return false; // empty around; no move
        }

        var dir = startDir;
        while (!TryDir(dir, units))
        {
            dir = (Direction)(((int)dir + 1) % 4);
            if (dir == startDir)
            {
                Proposed = Pos;
                return false; // being surrounded; no move
            }
        }
        Proposed = dir switch
        {
            Direction.North => Pos with { Y = Pos.Y + 1 },
            Direction.South => Pos with { Y = Pos.Y - 1 },
            Direction.West => Pos with { X = Pos.X - 1 },
            _ => Pos with { X = Pos.X + 1 },
        };
        return true;
    }

    bool TryDir(Direction dir, IEnumerable<Unit> units)
    {
        foreach (var delta in Coord.Deltas(dir))
        {
            var pos = Pos + delta;
            if (units.Any(u => u.Pos == pos))
                return false;
        }
        return true;
    }
}

enum Direction { North = 0, South, West, East }