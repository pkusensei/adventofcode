using System.Diagnostics;
using System.Text.Json.Nodes;

// Totally took this from u/encse
// https://www.reddit.com/r/adventofcode/comments/zkmyh4/2022_day_13_solutions/j012z9p/

int P1(IEnumerable<string> lines)
{
    var res = 0;
    foreach (var (line, idx) in lines.Chunk(3).Select((l, i) => (l, i + 1)))
    {
        var pac1 = JsonNode.Parse(line[0])!;
        var pac2 = JsonNode.Parse(line[1])!;
        if (Compare(pac1, pac2) < 0)
            res += idx;
    }
    return res;
}

int P2(IEnumerable<string> lines)
{
    var dividers = new[] { "[[2]]", "[[6]]" }.Select(d => JsonNode.Parse(d)).ToArray();
    var order = lines.Where(l => !string.IsNullOrWhiteSpace(l))
                     .Select(l => JsonNode.Parse(l))
                     .Concat(dividers).ToList();
    order.Sort(Compare!);
    return (order.IndexOf(dividers[0]) + 1) * (order.IndexOf(dividers[1]) + 1);
}

int Compare(JsonNode pac1, JsonNode pac2)
{
    if (pac1 is JsonValue && pac2 is JsonValue)
    {
        return (int)pac1 - (int)pac2;
    }
    else
    {
        var lst1 = pac1 as JsonArray ?? new JsonArray((int)pac1);
        var lst2 = pac2 as JsonArray ?? new JsonArray((int)pac2);
        return lst1.Zip(lst2)
            .Select(pair => Compare(pair.First!, pair.Second!))
            .FirstOrDefault(res => res != 0, lst1.Count - lst2.Count);
    }
}

var test = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]".Split('\n');
Debug.Assert(P1(test) == 13);
Debug.Assert(P2(test) == 140);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 6568);
Debug.Assert(P2(input) == 19493);