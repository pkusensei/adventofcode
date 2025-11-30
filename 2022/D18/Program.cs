using System.Diagnostics;

int P1(IEnumerable<string> lines)
{
    var res = 0;
    var seen = new List<Coord>();

    foreach (var curr in Parse(lines))
    {
        res += 6;
        var touched = seen.Count(c => c.Distance(curr) == 1);
        res -= touched * 2;
        seen.Add(curr);
    }
    return res;
}

int P2(IEnumerable<string> lines)
{
    var grid = Parse(lines).ToHashSet();
    var min = grid.SelectMany(c => new[] { c.X, c.Y, c.Z }).Min() - 1;
    var max = grid.SelectMany(c => new[] { c.X, c.Y, c.Z }).Max() + 1;
    var start = new Coord(min, min, min);
    var queue = new Queue<Coord>();
    queue.Enqueue(start);
    var seen = new HashSet<(Coord, Coord)> { (new Coord(0, 0, 0), start) };
    int res = 0;

    while (queue.TryDequeue(out var p))
    {
        foreach (var d in Coord.Deltas)
        {
            var neighbor = p.Neighbor(d);
            if (neighbor.OutOfBounds(min, max))
                continue;

            if (seen.Add((d, neighbor)))
            {
                if (grid.Contains(neighbor))
                {
                    res += 1;
                }
                else
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
    }
    return res;
}

IEnumerable<Coord> Parse(IEnumerable<string> lines) => lines.Select(
    line => new Coord(line.Split(',', StringSplitOptions.TrimEntries)
                          .Select(int.Parse).ToArray())
);

var test = @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5".Split('\n');
Debug.Assert(P1(test) == 64);
Debug.Assert(P2(test) == 58);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 4580);
Debug.Assert(P2(input) == 2610);

record struct Coord(int X, int Y, int Z)
{
    public Coord(int[] nums) :
        this(nums[0], nums[1], nums[2])
    { }

    public static IEnumerable<Coord> Deltas
    => new (int x, int y, int z)[]
            {(-1, 0, 0), (0, -1, 0), (0, 0, -1),
            (1, 0, 0),  (0, 1, 0),  (0, 0, 1)}
            .Select(d => new Coord(d.x, d.y, d.z));

    public Coord Neighbor(Coord delta)
    => new Coord(X + delta.X, Y + delta.Y, Z + delta.Z);

    public int Distance(Coord c)
    => Math.Abs(X - c.X) + Math.Abs(Y - c.Y) + Math.Abs(Z - c.Z);

    public bool OutOfBounds(int min, int max)
    => X < min || X > max || Y < min || Y > max || Z < min || Z > max;
}