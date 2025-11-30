using System.Diagnostics;

static IEnumerable<(int x1, int y1, int x2, int y2)> Parse(IEnumerable<string> lines)
{
    foreach (var line in lines)
    {
        var nums = line.Split(" -> ").Select(x => x.Split(',')).SelectMany(x => x).Select(x => Int32.Parse(x)).ToArray();
        yield return (nums[0], nums[1], nums[2], nums[3]);
    }
}

static int P1(IEnumerable<string> lines)
{
    var points = new Dictionary<(int, int), int>();
    foreach (var (x1, y1, x2, y2) in Parse(lines))
    {
        if (x1 == x2 || y1 == y2)
        {
            foreach (var (x, y) in GenerateSequence(x1, y1, x2, y2))
            {
                if (points.ContainsKey((x, y)))
                {
                    points[(x, y)] += 1;
                }
                else
                {
                    points.Add((x, y), 1);
                }
            }
        }
    }
    return points.Values.Where(x => x > 1).Count();
}

static IEnumerable<(int x, int y)> GenerateSequence(int x1, int y1, int x2, int y2)
{
    var xmin = Math.Min(x1, x2);
    var xmax = Math.Max(x1, x2);
    var ymin = Math.Min(y1, y2);
    var ymax = Math.Max(y1, y2);
    var xs = Enumerable.Range(xmin, xmax - xmin + 1);
    var ys = Enumerable.Range(ymin, ymax - ymin + 1);
    if (x1 == x2)
    {
        foreach (var y in ys)
        {
            yield return (x1, y);
        }
    }
    else if (y1 == y2)
    {
        foreach (var x in xs)
        {
            yield return (x, y1);
        }
    }
    else
    {
        if ((x1 - x2) * (y1 - y2) > 0)
        {
            foreach (var (x, y) in xs.Zip(ys))
            {
                yield return (x, y);
            }
        }
        else
        {
            foreach (var (x, y) in xs.Zip(ys.Reverse()))
            {
                yield return (x, y);
            }
        }
    }
}

static int P2(IEnumerable<string> lines)
{
    var points = new Dictionary<(int, int), int>();
    foreach (var (x1, y1, x2, y2) in Parse(lines))
    {
        foreach (var (x, y) in GenerateSequence(x1, y1, x2, y2))
        {
            if (points.ContainsKey((x, y)))
            {
                points[(x, y)] += 1;
            }
            else
            {
                points.Add((x, y), 1);
            }
        }
    }
    return points.Values.Where(x => x > 1).Count();
}

var test = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2".Split('\n').Select(s => s.Trim());
Debug.Assert(P1(test) == 5, $"{P1(test)}");

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 5197);

Debug.Assert(P2(test) == 12);
Debug.Assert(P2(input) == 18605);