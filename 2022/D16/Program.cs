using System.Diagnostics;

int P1(IEnumerable<string> lines)
{
    var valves = Parse(lines);
    var keep = FindKeepers(valves);
    return MaxFlow(valves["AA"], keep, 30);
}

int P2(IEnumerable<string> lines)
{
    var valves = Parse(lines);
    var keep = FindKeepers(valves);
    return MaxFlow2(new[] { valves["AA"], valves["AA"] }, keep, new[] { 26, 26 });
}

// Took from u/MarvelousShade
// https://www.reddit.com/r/adventofcode/comments/zn6k1l/2022_day_16_solutions/j0gr45o/

int MaxFlow(Valve curr, Valve[] keep, int left)
{
    int mf = 0;

    foreach (var v in keep)
    {
        var newTime = left - curr.Distances[v] - 1;
        if (newTime > 0)
        {
            var flow = newTime * v.Rate
                    + MaxFlow(v, keep.Where(x => x != v).ToArray(), newTime);
            if (mf < flow)
                mf = flow;
        }
    }
    return mf;
}

int MaxFlow2(Valve[] curr, Valve[] keep, int[] left)
{
    int mf = 0;
    var pick = left[0] > left[1] ? 0 : 1;
    var curV = curr[pick];

    foreach (var v in keep)
    {
        var newTime = left[pick] - curV.Distances[v] - 1;
        if (newTime > 0)
        {
            var newTimes = new[] { newTime, left[1 - pick] };
            var newV = new[] { v, curr[1 - pick] };
            var flow = newTime * v.Rate
                    + MaxFlow2(newV, keep.Where(x => x != v).ToArray(), newTimes);
            if (mf < flow)
                mf = flow;
        }
    }
    return mf;
}

Valve[] FindKeepers(IReadOnlyDictionary<string, Valve> valves)
=> valves.Values.Where(v => v.Rate > 0).ToArray();

Dictionary<string, Valve> Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<string, Valve>();
    foreach (var line in lines.Select(l => l.Split(';')))
    {
        var name = line[0].Substring(6, 2);
        var rate = int.Parse(line[0].Split('=')[1]);
        var lst = line[1].Split("to ")[1]
            .Split(' ', StringSplitOptions.TrimEntries).Skip(1)
            .Select(s => s.Trim(',')).ToList();
        res.Add(name, new Valve
        {
            Name = name,
            Rate = rate,
            To = lst
        });
    }
    foreach (var v in res.Values)
    {
        var curr = res[v.Name];
        curr.Distances[v] = 0;
        UpdateDistance(res, curr, v);
    }
    return res;
}

void UpdateDistance(IReadOnlyDictionary<string, Valve> valves,
                    Valve? curr, Valve target)
{
    var visited = new HashSet<Valve>();

    while (curr != null && visited.Count < valves.Count)
    {
        visited.Add(curr);
        int dist = curr.Distances[target] + 1;
        foreach (var valve in curr.To.Select(name => valves[name]))
        {
            if (visited.Contains(valve))
                continue;

            if (valve.Distances.ContainsKey(target))
            {
                if (dist < valve.Distances[target])
                    valve.Distances[target] = dist;
            }
            else
            {
                valve.Distances[target] = dist;
            }
        }
        curr = valves.Values.Where(v => !visited.Contains(v) && v.Distances.ContainsKey(target))
            .OrderBy(v => v.Distances[target]).FirstOrDefault();
    }
}

var test = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II".Split('\n');
Debug.Assert(P1(test) == 1651);
Debug.Assert(P2(test) == 1707);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 2181);
Debug.Assert(P2(input) == 2824); // Somehow produces 2786

class Valve
{
    public string Name { get; init; } = null!;
    public int Rate { get; init; }
    public List<string> To { get; init; } = null!;
    public Dictionary<Valve, int> Distances { get; init; } = new();
}