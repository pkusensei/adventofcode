using System.Diagnostics;
using System.Text;

static (string, IReadOnlyDictionary<string, char>) Parse(IList<string> lines)
{
    var template = lines[0].Trim();
    var rules = new Dictionary<string, char>();
    foreach (var line in lines.Skip(2))
    {
        var items = line.Split(" -> ");
        var pair = items[0].Trim();
        var element = items[1].Trim().Single();
        rules.Add(pair, element);
    }
    return (template, rules);
}

static string Insert(string template, IReadOnlyDictionary<string, char> rules)
{
    var res = new StringBuilder();
    for (int i = 0; i < template.Length - 1; i++)
    {
        res.Append(template[i]);
        var pair = template.Substring(i, 2);
        if (rules.TryGetValue(pair, out char c))
        {
            res.Append(c);
        }
    }
    res.Append(template.Last());
    return res.ToString();
}

static int P1(IList<string> lines)
{
    var (template, rules) = Parse(lines);
    for (int i = 0; i < 10; i++)
    {
        template = Insert(template, rules);
    }
    var minmax = template.GroupBy(x => x).Select(group => group.Count());
    var most = minmax.Max();
    var least = minmax.Min();
    return most - least;
}

var test = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C".Split('\n').ToList();
Debug.Assert(P1(test) == 1588);

var input = File.ReadLines("input.txt").ToList();
Debug.Assert(P1(input) == 2621);

static Int64 P2(IList<string> lines, int steps)
{
    var (template, rules) = Parse(lines);
    var counter = template.GroupBy(c => c).ToDictionary(gr => gr.Key, gr => (Int64)gr.Count());
    var pairs = new Dictionary<string, Int64>();
    for (int i = 0; i < template.Length - 1; i++)
    {
        var pair = template.Substring(i, 2);
        if (pairs.ContainsKey(pair))
            pairs[pair] += 1;
        else
            pairs.Add(pair, 1);
    }
    for (int i = 0; i < steps; i++)
    {
        var tmp = new Dictionary<string, Int64>();
        foreach (var (s, count) in pairs)
        {
            if (!rules.ContainsKey(s))
                continue;
            if (counter.ContainsKey(rules[s]))
                counter[rules[s]] += count;
            else
                counter.Add(rules[s], count);
            var pair1 = string.Concat(new[] { s[0], rules[s] });
            var pair2 = string.Concat(new[] { rules[s], s[1] });
            foreach (var p in new[] { pair1, pair2 })
            {
                if (tmp.ContainsKey(p))
                    tmp[p] += count;
                else
                    tmp.Add(p, count);
            }
        }
        pairs = tmp;
    }
    return counter.Values.Max() - counter.Values.Min();
}

Debug.Assert(P2(test, 10) == 1588);
Debug.Assert(P2(input, 10) == 2621);
Debug.Assert(P2(test, 40) == 2188189693529);
Debug.Assert(P2(input, 40) == 2843834241366);
