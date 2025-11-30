using System.Diagnostics;

static IReadOnlyDictionary<string, HashSet<string>> Parse(IEnumerable<string> lines)
{
    var graph = new Dictionary<string, HashSet<string>>();
    foreach (var pair in lines.Select(line => line.Split('-')))
    {
        var left = pair[0].Trim();
        var right = pair[1].Trim();
        if (graph.TryGetValue(left, out var vr))
            vr.Add(right);
        else
            graph.Add(left, new HashSet<string>(new[] { right }));

        if (graph.TryGetValue(right, out var vl))
            vl.Add(left);
        else
            graph.Add(right, new HashSet<string>(new[] { left }));
    }
    return graph;
}

static int CountPath(IReadOnlyDictionary<string, HashSet<string>> graph, IList<string> trail, bool repeat)
{
    if (trail.Last() == "end")
        return 1;

    var count = 0;
    foreach (var pt in graph[trail.Last()])
    {
        var newTrail = trail.Concat(new[] { pt }).ToList();
        // upper or non visited lower
        if (!(pt.All(char.IsLower) && trail.Contains(pt)))
        {
            count += CountPath(graph, newTrail, repeat);
        }
        // once visited lower
        else if (repeat && trail.Count(x => x == pt) == 1 && pt != "start")
        {
            count += CountPath(graph, newTrail, false);
        }
    }
    return count;
}

static int P1(IEnumerable<string> lines)
{
    var graph = Parse(lines);
    return CountPath(graph, new[] { "start" }, false);
}

static int P2(IEnumerable<string> lines)
{
    var graph = Parse(lines);
    return CountPath(graph, new[] { "start" }, true);
}

var test1 = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end".Split('\n');
Debug.Assert(P1(test1) == 10);

var test2 = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc".Split('\n');
Debug.Assert(P1(test2) == 19);

var test3 = @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW".Split('\n');
Debug.Assert(P1(test3) == 226);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 5920);

Debug.Assert(P2(test1) == 36);
Debug.Assert(P2(test2) == 103);
Debug.Assert(P2(test3) == 3509);
Debug.Assert(P2(input) == 155477);