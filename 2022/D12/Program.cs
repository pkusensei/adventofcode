using System.Diagnostics;

int P1(IEnumerable<string> lines)
{
    var (grid, start, end) = Parse(lines);
    return Dijkstra(grid, start, end, c => c == start);
}

int P2(IEnumerable<string> lines)
{
    var (grid, start, end) = Parse(lines);
    return Dijkstra(grid, start, end, c => grid[c] == 0);
}

(Dictionary<Coord, int>, Coord start, Coord end)
Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<Coord, int>();
    Coord start = new();
    Coord end = new();
    foreach (var (line, y) in lines.Select((line, y) => (line, y)))
    {
        foreach (var (c, x) in line.Trim().Select((c, x) => (c, x)))
        {
            int height = 0;
            var coord = new Coord(x, y);
            switch (c)
            {
                case 'S':
                    start = coord;
                    height = 0;
                    break;
                case 'E':
                    end = coord;
                    height = 'z' - 'a';
                    break;
                default:
                    height = c - 'a';
                    break;
            }
            res.Add(coord, height);
        }
    }
    return (res, start, end);
}

IEnumerable<Coord> FindAdjacent(IReadOnlyDictionary<Coord, int> grid, Coord current) =>
    new[]{current with {X=current.X-1}, current with {X=current.X+1},
          current with {Y=current.Y-1}, current with {Y=current.Y+1}}
        .Where(c => grid.ContainsKey(c));

// Copied from 2021D15 lmao
// Searching from end to start to accommodate p2
int Dijkstra(IReadOnlyDictionary<Coord, int> grid, Coord start, Coord end,
    Predicate<Coord> pred)
{
    var unvisited = grid.Keys.ToHashSet();
    var dist = grid.Keys.ToDictionary(x => x, x => int.MaxValue);
    dist[end] = 0;
    while (unvisited.Count > 0)
    {
        var minDist = dist.Where(n => unvisited.Contains(n.Key)).Select(n => n.Value).Min();
        var current = unvisited.Where(node => dist[node] == minDist).First();
        unvisited.Remove(current);

        if (pred(current))
            return dist[current];

        foreach (var node in FindAdjacent(grid, current)
                            .Where(n => unvisited.Contains(n) && grid[current] - grid[n] <= 1))
        {
            var alt = dist[current] + 1;
            if (alt < dist[node])
                dist[node] = alt;
        }
    }
    return dist[start];
}

var test = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi".Split('\n');
Debug.Assert(P1(test) == 31);
Debug.Assert(P2(test) == 29);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 534);
Debug.Assert(P2(input) == 525);

record struct Coord(int X, int Y);