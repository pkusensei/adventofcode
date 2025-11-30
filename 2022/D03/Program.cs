using System.Diagnostics;

int P1(IEnumerable<string> lines) => lines.Select(
    line =>
    {
        var s = line.Substring(0, line.Length / 2).Intersect(line.Substring(line.Length / 2));
        return ToScore(s.First());
    }
).Sum();

int P2(IEnumerable<string> lines) => lines.Chunk(3).Select(
    group =>
    {
        var s = group[0].Intersect(group[1]).Intersect(group[2]);
        return ToScore(s.First());
    }
).Sum();

int ToScore(char c) => c switch
{
    >= 'a' and <= 'z' => c - 'a' + 1,
    _ => c - 'A' + 27,
};

var test = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw".Split('\n', StringSplitOptions.TrimEntries);
Debug.Assert(P1(test) == 157);
Debug.Assert(P2(test) == 70);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 7727);
Debug.Assert(P2(input) == 2609);