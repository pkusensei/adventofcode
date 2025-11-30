using System.Diagnostics;

// Gave up on this
// [Totally confused face.jpg]
// 😵😵😵
static Int64 Solve(IList<string> lines, bool p1 = true)
{
    var pairs = new List<(int, int)>();
    foreach (var i in Enumerable.Range(0, 14))
    {
        pairs.Add((int.Parse(lines[i * 18 + 5][6..]), int.Parse(lines[i * 18 + 15][6..])));
    }
    var stack = new Stack<(int, int)>();
    var keys = new Dictionary<int, (int, int)>();

    foreach (var (pair, idx) in pairs.Select((pair, i) => (pair, i)))
    {
        if (pair.Item1 > 0)
        {
            stack.Push((idx, pair.Item2));
        }
        else
        {
            var (j, addr) = stack.Pop();
            keys[idx] = (j, addr + pair.Item1);
        }
    }
    var output = new Dictionary<int, int>();

    if (p1)
    {
        foreach (var (k, v) in keys)
        {
            output[k] = Math.Min(9, 9 + v.Item2);
            output[v.Item1] = Math.Min(9, 9 - v.Item2);
        }
    }
    else
    {
        foreach (var (k, v) in keys)
        {
            output[k] = Math.Max(1, 1 + v.Item2);
            output[v.Item1] = Math.Max(1, 1 - v.Item2);
        }
    }
    var result = long.Parse(string.Join("", output.OrderBy(x => x.Key).Select(x => x.Value)));

    return result;
}



var input = File.ReadLines("input.txt").ToList();
Debug.Assert(Solve(input) == 74929995999389);
Debug.Assert(Solve(input, false) == 11118151637112);