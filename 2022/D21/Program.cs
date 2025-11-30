using System.Diagnostics;

long P1(IEnumerable<string> lines)
{
    var (root, _) = Parse(lines);
    return root.Num;
}

long P2(IEnumerable<string> lines)
{
    var (root, humn) = Parse(lines);
    var left = root.Left!;
    var right = root.Right!;
    if (left.HasH)
        left.Num = right.Num;
    else if (right.HasH)
        right.Num = left.Num;
    return humn.Num;
}

(OpNode, NumNode) Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<string, Node>();
    NumNode? humn = null;
    foreach (var line in lines)
    {
        var items = line.Split(' ', StringSplitOptions.TrimEntries);
        var name = items[0].Substring(0, 4);
        if (items.Length == 2)
        {
            var num = long.Parse(items[1]);
            var node = new NumNode { Name = name, Num = num };
            res.Add(name, node);
            if (name == "humn")
                humn = node;
        }
        else
        {
            res.Add(name, new OpNode
            {
                LName = items[1],
                RName = items[3],
                Op = items[2][0],
            });
        }
    }
    foreach (var v in res.Values)
    {
        if (v is OpNode node)
        {
            node.Left = res[node.LName];
            node.Right = res[node.RName];
        }
    }
    return ((OpNode)res["root"]!, humn!);
}

var test = @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32".Split('\n');
Debug.Assert(P1(test) == 152);
Debug.Assert(P2(test) == 301);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 152479825094094);
Debug.Assert(P2(input) == 3360561285172);

abstract class Node
{
    public abstract long Num { get; set; }
    public abstract bool HasH { get; }
}

sealed class NumNode : Node
{
    public string Name { get; init; } = null!;
    public override long Num { get; set; }
    public override bool HasH => Name == "humn";
}

sealed class OpNode : Node
{
    public override long Num
    {
        get => Op switch
        {
            '+' => Left!.Num + Right!.Num,
            '-' => Left!.Num - Right!.Num,
            '*' => Left!.Num * Right!.Num,
            _ => Left!.Num / Right!.Num,
        };
        set
        {
            if (Left!.HasH)
            {
                Left!.Num = Op switch
                {
                    '+' => value - Right!.Num,
                    '-' => value + Right!.Num,
                    '*' => value / Right!.Num,
                    _ => value * Right!.Num,
                };
            }
            else if (Right!.HasH)
            {
                Right!.Num = Op switch
                {
                    '+' => value - Left!.Num,
                    '-' => Left!.Num - value,
                    '*' => value / Left!.Num,
                    _ => value / Left!.Num,
                };
            }
        }
    }

    public string LName { get; init; } = null!;
    public string RName { get; init; } = null!;
    public Node? Left { get; set; }
    public Node? Right { get; set; }
    public char Op { get; init; }
    public override bool HasH => Left!.HasH || Right!.HasH;
}