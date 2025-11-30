using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

static (IImmutableList<bool> algo, HashSet<(int, int)> img)
Parse(IList<string> lines)
{
    var algo = lines[0].Trim().Select(c => c == '#').ToImmutableList();
    var res = new HashSet<(int, int)>();
    foreach (var (line, y) in lines.Skip(2).Select((s, i) => (s.Trim(), i)))
    {
        foreach (var (c, x) in line.Select((c, i) => (c, i)))
        {
            if (c == '#')
                res.Add((x, y));
        }
    }
    return (algo, res);
}

static HashSet<(int, int)>
Enhance(IImmutableList<bool> algo, HashSet<(int x, int y)> img, int turn)
{
    var deltas = new[] { (-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1) };
    var res = new HashSet<(int, int)>();
    var xmin = img.Select(k => k.x).Min();
    var xmax = img.Select(k => k.x).Max();
    var ymin = img.Select(k => k.y).Min();
    var ymax = img.Select(k => k.y).Max();

    for (int y = ymin - 1; y <= ymax + 1; y++)
    {
        for (int x = xmin - 1; x <= xmax + 1; x++)
        {
            var binary = new StringBuilder(9);
            foreach (var (dx, dy) in deltas)
            {
                var tmpx = x + dx;
                var tmpy = y + dy;
                if (turn % 2 == 0)
                {
                    if (img.Contains((tmpx, tmpy)))
                        binary.Append('1');
                    else
                        binary.Append('0');
                }
                else
                {
                    var inBound = xmin <= tmpx && tmpx <= xmax
                                  && ymin <= tmpy && tmpy <= ymax;
                    if (inBound || !algo[0]) // algo[0] == '.'
                    {
                        if (img.Contains((tmpx, tmpy)))
                            binary.Append('1');
                        else
                            binary.Append('0');
                    }
                    else // out of bound
                    {
                        binary.Append('1');
                    }
                }
            }
            var idx = Convert.ToInt32(binary.ToString(), 2);
            if (algo[idx])
                res.Add((x, y));
        }
    }
    return res;
}

static int Solve(IList<string> lines, int turns)
{
    var (algo, img) = Parse(lines);
    for (int i = 0; i < turns; i++)
        img = Enhance(algo, img, i);
    return img.Count;
}

static int P1(IList<string> lines) => Solve(lines, 2);
static int P2(IList<string> lines) => Solve(lines, 50);

var test = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###".Split('\n').ToList();
Debug.Assert(P1(test) == 35);
Debug.Assert(P2(test) == 3351);


var input = File.ReadLines("input.txt").ToList();
Debug.Assert(P1(input) == 5097);
Debug.Assert(P2(input) == 17987);