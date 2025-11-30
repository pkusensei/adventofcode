using System.Diagnostics;

(int p1, int p2) Solve(IList<string> lines)
{
    var (grid, blizz, entry, exit, limits) = Parse(lines);
    var p1 = Search(grid, blizz, entry, exit, limits);
    var back = Search(grid, blizz, exit, entry, limits);
    var again = Search(grid, blizz, entry, exit, limits);
    return (p1, p1 + back + again);
}

int Search(IDictionary<Coord, State> grid, IEnumerable<Blizz> blizz,
           Coord start, Coord goal, int[] limits)
{
    var locs = new HashSet<Coord> { start };
    int count = 0;
    while (true)
    {
        Move(grid, blizz, limits);
        count += 1;
        var nextLocs = new HashSet<Coord>();
        foreach (var loc in locs)
        {
            nextLocs.UnionWith(loc.NextMove(grid));
        }
        if (nextLocs.Contains(goal))
            return count;
        locs = nextLocs;
    }
}

void Move(IDictionary<Coord, State> grid, IEnumerable<Blizz> blizz, int[] limits)
{
    foreach (var b in blizz)
    {
        b.Move(limits);
    }
    foreach (var k in grid.Keys)
    {
        if (grid[k] == State.Wall)
            continue;

        var bs = blizz.Select(b => b.Pos);
        if (bs.Contains(k))
            grid[k] = State.Blizz;
        else
            grid[k] = State.Empty;
    }
}

(Dictionary<Coord, State> grid, List<Blizz> blizz, Coord entry, Coord exit, int[] limits)
Parse(IList<string> lines)
{
    var grid = new Dictionary<Coord, State>();
    var blizz = new List<Blizz>();
    Coord entry = new();
    Coord exit = new();
    foreach (var (line, y) in lines.Select((l, i) => (l, i)))
    {
        foreach (var (c, x) in line.Trim().Select((c, i) => (c, i)))
        {
            switch (c)
            {
                case '#':
                    grid.Add(new(x, y), State.Wall);
                    break;
                case '>':
                    grid.Add(new(x, y), State.Blizz);
                    blizz.Add(new(x, y, Direction.Right));
                    break;
                case '<':
                    grid.Add(new(x, y), State.Blizz);
                    blizz.Add(new(x, y, Direction.Left));
                    break;
                case '^':
                    grid.Add(new(x, y), State.Blizz);
                    blizz.Add(new(x, y, Direction.Up));
                    break;
                case 'v':
                    grid.Add(new(x, y), State.Blizz);
                    blizz.Add(new(x, y, Direction.Down));
                    break;
                default:
                    grid.Add(new(x, y), State.Empty);
                    if (y == 0)
                        entry = new(x, y);
                    else if (y == lines.Count - 1)
                        exit = new(x, y);
                    break;
            }
        }
    }
    var xmin = grid.Keys.Min(c => c.X);
    var xmax = grid.Keys.Max(c => c.X);
    var ymin = grid.Keys.Min(c => c.Y);
    var ymax = grid.Keys.Max(c => c.Y);

    return (grid, blizz, entry, exit, new[] { xmin, xmax, ymin, ymax });
}

var test = @"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#".Split('\n');
var (p1, p2) = Solve(test);
Debug.Assert(p1 == 18);
Debug.Assert(p2 == 54);

var input = File.ReadLines("input.txt");
(p1, p2) = Solve(input.ToList());
Debug.Assert(p1 == 283);
Debug.Assert(p2 == 883);

class Blizz
{
    public Blizz(int x, int y, Direction dir)
    {
        Pos = new(x, y);
        Dir = dir;
    }

    public Coord Pos { get; private set; }
    public Direction Dir { get; }

    public void Move(int[] limits) => Move(limits[0], limits[1], limits[2], limits[3]);

    public void Move(int xmin, int xmax, int ymin, int ymax)
    => Pos = Dir switch
    {
        Direction.Up => Pos.Y - 1 == ymin ?
                        Pos with { Y = ymax - 1 } : Pos with { Y = Pos.Y - 1 },
        Direction.Down => Pos.Y + 1 == ymax ?
                        Pos with { Y = ymin + 1 } : Pos with { Y = Pos.Y + 1 },
        Direction.Left => Pos.X - 1 == xmin ?
                        Pos with { X = xmax - 1 } : Pos with { X = Pos.X - 1 },
        _ => Pos.X + 1 == xmax ?
                        Pos with { X = xmin + 1 } : Pos with { X = Pos.X + 1 },
    };
}

record struct Coord(int X, int Y)
{
    public IEnumerable<Coord> NextMove(IDictionary<Coord, State> grid)
    {
        var attempts = new[] { this with { X = this.X - 1 }, this with { X = this.X + 1 },
                               this with { Y = this.Y - 1 }, this with { Y = this.Y + 1 },
                               this}; // Not always possible to stay; it goes into check every time
        foreach (var a in attempts)
        {
            if (grid.TryGetValue(a, out var v) && v == State.Empty)
            {
                yield return a;
            }
        }
    }
}

enum Direction { Up, Down, Left, Right }
enum State { Empty, Wall, Blizz }