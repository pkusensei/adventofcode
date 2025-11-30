using System.Diagnostics;
using System.Text;

static (List<(int x, int y)>, List<(char, int)>) Parse(IEnumerable<string> lines)
{
    var folds = new List<(char, int)>();
    var dots = new List<(int x, int y)>();
    foreach (var item in lines)
    {
        var line = item.Trim();
        if (string.IsNullOrEmpty(line))
            continue;
        if (line.StartsWith("fold along"))
        {
            var num = int.Parse(line.Split('=')[1]);
            folds.Add((line[11], num));
        }
        else
        {
            var coord = line.Split(',').Select(s => int.Parse(s)).ToList();
            dots.Add((coord[0], coord[1]));
        }
    }
    return (dots, folds);
}

static (int, int) Fold(char direction, int foldLine, (int x, int y) dot)
{
    switch (direction)
    {
        case 'x':
            if (dot.x < foldLine)
                return (dot.x, dot.y);
            var x = 2 * foldLine - dot.x;
            return (x, dot.y);
        case 'y':
            if (dot.y < foldLine)
                return (dot.x, dot.y);
            var y = 2 * foldLine - dot.y;
            return (dot.x, y);
        default:
            throw new ArgumentException($"Invalid input:{direction}, {dot}");
    }
}

static int P1(IEnumerable<string> lines)
{
    var (dots, folds) = Parse(lines);
    var res = new HashSet<(int, int)>();
    var (dir, foldLine) = folds[0];
    foreach (var item in dots)
    {
        var dot = Fold(dir, foldLine, item);
        res.Add(dot);
    }
    return res.Count;
}

var test = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5".Split('\n');
Debug.Assert(P1(test) == 17);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 810);

static void P2(IEnumerable<string> lines)
{
    var (dots, folds) = Parse(lines);
    var folded = new HashSet<(int, int)>(dots);
    foreach (var (dir, foldLine) in folds)
    {
        var tmp = new HashSet<(int, int)>();
        foreach (var item in folded)
        {
            tmp.Add(Fold(dir, foldLine, item));
        }
        folded = tmp;
    }
    var xmin = folded.Select(dot => dot.Item1).Min();
    var xmax = folded.Select(dot => dot.Item1).Max();
    var ymin = folded.Select(dot => dot.Item2).Min();
    var ymax = folded.Select(dot => dot.Item2).Max();
    var res = new List<string>();
    for (int y = ymin; y <= ymax; y++)
    {
        var builder = new StringBuilder();
        builder.Append(' ', xmax + 1 - xmin);
        for (int x = xmin; x <= xmax; x++)
        {
            if (folded.Contains((x, y)))
                builder[x - xmin] = '#';
        }
        res.Add(builder.ToString());
    }
    foreach (var item in res)
    {
        System.Console.WriteLine(item);
    }
}

P2(input); //HLBUBGFR