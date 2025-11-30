using System.Diagnostics;

int Solve(IEnumerable<string> lines, int count)
{
    var res = new HashSet<Point>();
    var points = Enumerable.Range(0, count).Select(_ => new Point(0, 0)).ToArray();
    res.Add(points.Last());

    foreach (var line in lines.Select(l => l.Split(' ', StringSplitOptions.TrimEntries)))
    {
        var dir = ToDir(line[0][0]);
        var steps = int.Parse(line[1]);

        for (int step = 0; step < steps; step++)
        {
            points[0] = Move(dir, points[0]);

            for (int i = 1; i < points.Length; i++)
            {
                if (InTouch(points[i], points[i - 1]))
                    continue;

                if (points[i - 1].X == points[i].X)
                {
                    var nDir = points[i - 1].Y - points[i].Y < 0 ? Dir.Up : Dir.Down;
                    points[i] = Move(nDir, points[i]);
                }
                else if (points[i - 1].Y == points[i].Y)
                {
                    var nDir = points[i - 1].X - points[i].X < 0 ? Dir.Left : Dir.Right;
                    points[i] = Move(nDir, points[i]);
                }
                else
                {
                    var x = points[i - 1].X - points[i].X < 0 ? points[i].X - 1 : points[i].X + 1;
                    var y = points[i - 1].Y - points[i].Y < 0 ? points[i].Y - 1 : points[i].Y + 1;
                    points[i] = new Point(x, y);
                }
            }

            res.Add(points.Last());
        }
    }

    return res.Count;
}

int P1(IEnumerable<string> lines) => Solve(lines, 2);
int P2(IEnumerable<string> lines) => Solve(lines, 10);

bool InTouch(Point a, Point b)
{
    if (a.X == b.X)
        return Math.Abs(a.Y - b.Y) < 2;
    else if (a.Y == b.Y)
        return Math.Abs(a.X - b.X) < 2;
    else
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) <= 2;
}

Point Move(Dir dir, Point p) => dir switch
{
    Dir.Right => p with { X = p.X + 1 },
    Dir.Left => p with { X = p.X - 1 },
    Dir.Up => p with { Y = p.Y - 1 },
    _ => p with { Y = p.Y + 1 },
};

Dir ToDir(char c) => c switch
{
    'R' => Dir.Right,
    'L' => Dir.Left,
    'U' => Dir.Up,
    _ => Dir.Down
};

var test = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2".Split('\n');
Debug.Assert(P1(test) == 13);
Debug.Assert(P2(test) == 1);

var test2 = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20".Split('\n');
Debug.Assert(P2(test2) == 36);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 6464);
Debug.Assert(P2(input) == 2604);

record struct Point(int X, int Y);

enum Dir
{
    Right,
    Left,
    Up,
    Down,
}