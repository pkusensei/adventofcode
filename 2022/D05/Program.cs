using System.Diagnostics;

string P1(IEnumerable<string> lines)
{
    var (sts, insts) = Parse(lines);
    var stacks = sts.Select(s => new Stack<char>(s)).ToArray();

    foreach (var inst in insts)
    {
        for (int i = 0; i < inst[0]; i++)
        {
            var c = stacks[inst[1] - 1].Pop();
            stacks[inst[2] - 1].Push(c);
        }
    }
    return string.Concat(stacks.Select(s => s.Peek()));
}

string P2(IEnumerable<string> lines)
{
    var (stacks, insts) = Parse(lines);
    foreach (var inst in insts)
    {
        var numToKeep = stacks[inst[1] - 1].Count - inst[0];
        var keep = stacks[inst[1] - 1].Take(numToKeep).ToList();
        stacks[inst[2] - 1].AddRange(stacks[inst[1] - 1].Skip(numToKeep));
        stacks[inst[1] - 1] = keep;
    }
    return string.Concat(stacks.Select(s => s.Last()));
}

(List<char>[], int[][]) Parse(IEnumerable<string> lines)
{
    var numStacks = (lines.First().Length + 1) / 4;
    var stacks = Enumerable.Range(0, numStacks).Select(x => new List<char>()).ToArray();
    foreach (var line in lines)
    {
        if (string.IsNullOrWhiteSpace(line))
            break;

        foreach (var (c, idx) in line.Select((c, idx) => (c, idx)))
        {
            if (char.IsLetter(c))
                stacks[idx / 4].Add(c);
        }
    }
    foreach (var s in stacks)
    {
        s.Reverse();
    }
    var insts = lines.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1)
        .Select(line => line.Split(new[] { "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse).ToArray())
        .ToArray();
    return (stacks, insts);
}

var test = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2".Split('\n');
Debug.Assert(P1(test) == "CMZ");
Debug.Assert(P2(test) == "MCD");

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == "FCVRLMVQP");
Debug.Assert(P2(input) == "RWLWGJGFD");