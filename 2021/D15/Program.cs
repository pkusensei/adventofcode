using System.Diagnostics;

static IReadOnlyDictionary<(int x, int y), int> Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<(int, int), int>();
    foreach (var (line, y) in lines.Select((s, iy) => (s, iy)))
    {
        foreach (var (c, x) in line.Trim().Select((c, ix) => (c, ix)))
        {
            res.Add((x, y), (int)char.GetNumericValue(c));
        }
    }
    return res;
}

static IReadOnlyList<(int, int)> Neighbors(IReadOnlyDictionary<(int, int), int> graph, (int x, int y) node)
{
    return new[] { (-1, 0), (1, 0), (0, -1), (0, 1) }
        .Select(delta => (delta.Item1 + node.x, delta.Item2 + node.y))
        .Where(node => graph.ContainsKey(node))
        .ToList();
}

static int Dijkstra(IReadOnlyDictionary<(int, int), int> graph, (int, int) start, (int, int) end)
{
    var unvisited = graph.Keys.ToHashSet();
    var dist = graph.Keys.ToDictionary(n => n, n => int.MaxValue);
    dist[start] = 0;
    while (unvisited.Count > 0)
    {
        var minDist = dist.Where(n => unvisited.Contains(n.Key)).Select(n => n.Value).Min();
        var current = unvisited.Where(node => dist[node] == minDist).First();
        unvisited.Remove(current);
        if (current == end)
            return dist[end];
        foreach (var neighbor in Neighbors(graph, current).Where(n => unvisited.Contains(n)))
        {
            var alt = dist[current] + graph[neighbor];
            if (alt < dist[neighbor])
            {
                dist[neighbor] = alt;
            }
        }
    }
    return dist[end];
}

static int P1(IEnumerable<string> lines)
{
    var graph = Parse(lines);
    var start = (0, 0);
    var end = graph.Keys.MaxBy(n => n.x + n.y);
    return Dijkstra(graph, start, end);
}

var test = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581".Split('\n');
Debug.Assert(P1(test) == 40);

var input = File.ReadAllLines("input.txt");
Debug.Assert(P1(input) == 589);

static int DijkstraPQ(IReadOnlyDictionary<(int, int), int> graph, (int, int) start, (int, int) end)
{
    var next = new PriorityQueue<(int, int), int>();
    var dist = graph.Keys.ToDictionary(n => n, n => int.MaxValue);
    var visited = new HashSet<(int, int)>();
    dist[start] = 0;
    next.Enqueue(start, 0);
    while (next.Count > 0)
    {
        var current = next.Dequeue();
        if (visited.Contains(current))
            continue;
        visited.Add(current);
        if (current == end)
            return dist[end];
        foreach (var neighbor in Neighbors(graph, current))
        {
            var alt = dist[current] + graph[neighbor];
            if (alt < dist[neighbor])
                dist[neighbor] = alt;
            if (dist[neighbor] != int.MaxValue)
                next.Enqueue(neighbor, dist[neighbor]);
        }
    }
    return dist[end];
}

static int P2(IEnumerable<string> lines)
{
    var graph = Parse(lines).ToDictionary(n => n.Key, n => n.Value);
    var start = (0, 0);
    var xsize = graph.Keys.MaxBy(n => n.x).x + 1;
    var ysize = graph.Keys.MaxBy(n => n.y).y + 1;
    for (int y = 0; y < 5 * ysize; y++)
    {
        for (int x = 0; x < 5 * xsize; x++)
        {
            if (graph.ContainsKey((x, y)))
                continue;
            var orig = graph[(x % xsize, y % ysize)];
            var num = orig + y / ysize + x / xsize;
            if (num > 9)
                num = num % 9;
            graph.Add((x, y), num);
        }
    }
    return DijkstraPQ(graph, start, (5 * xsize - 1, 5 * ysize - 1));
}

Debug.Assert(P2(test) == 315);
Debug.Assert(P2(input) == 2885);