using System.Diagnostics;

static IEnumerable<List<(int, int)>> Parse(IEnumerable<string> lines)
{
    foreach (var line in lines)
    {
        var depth = 0;
        var sfnum = new List<(int depth, int num)>();
        foreach (var c in line.Trim())
        {
            switch (c)
            {
                case ',':
                    continue;
                case '[':
                    depth += 1;
                    break;
                case ']':
                    depth -= 1;
                    break;
                default:
                    var num = (int)char.GetNumericValue(c);
                    sfnum.Add((depth, num));
                    break;
            }
        }
        Debug.Assert(depth == 0);
        yield return sfnum;
    }
}

static List<(int, int)> Add(List<(int depth, int num)> left, List<(int depth, int num)> right)
{
    if (left.Count == 0)
        return right;
    return left.Concat(right).Select(n => (n.depth += 1, n.num)).ToList();
}

static List<(int, int)> Explode(List<(int depth, int num)> sfnum)
{
    var ((depth, num), idx) = sfnum.Select((n, i) => (n, i)).First(item => item.n.depth > 4);
    if (idx > 0)
        sfnum[idx - 1] = (sfnum[idx - 1].depth, sfnum[idx - 1].num + num);
    if (idx + 2 < sfnum.Count)
        sfnum[idx + 2] = (sfnum[idx + 2].depth, sfnum[idx + 2].num + sfnum[idx + 1].num);
    sfnum.RemoveAt(idx);
    sfnum[idx] = (4, 0);
    return sfnum;
}

static List<(int, int)> Split(List<(int depth, int num)> sfnum)
{
    var ((depth, num), idx) = sfnum.Select((n, i) => (n, i)).First(item => item.n.num > 9);
    var n1 = num / 2;
    var n2 = (num + 1) / 2;
    sfnum.RemoveAt(idx);
    sfnum.Insert(idx, (depth + 1, n2));
    sfnum.Insert(idx, (depth + 1, n1));
    return sfnum;
}

static int Magnitude(IReadOnlyList<(int depth, int num)> sfnum)
{
    var stack = new Stack<(int depth, int num)>();
    foreach (var item in sfnum)
    {
        if (stack.Count == 0)
        {
            stack.Push(item);
            continue;
        }
        else
        {
            var top = stack.Peek();
            var current = item;
            while (top.depth == current.depth)
            {
                stack.Pop();
                var d = top.depth - 1;
                var n = 3 * top.num + 2 * current.num;
                current = (d, n);
                if (stack.Count > 0)
                    top = stack.Peek();
                else
                    break;
            }
            stack.Push(current);
        }
    }
    return stack.Single().num;
}

static List<(int, int)> Reduce(List<(int depth, int num)> sfnum)
{
    while (true)
    {
        if (sfnum.Any(n => n.depth > 4))
        {
            sfnum = Explode(sfnum);
            continue;
        }
        if (sfnum.Any(n => n.num > 9))
        {
            sfnum = Split(sfnum);
            continue;
        }
        break;
    }
    return sfnum;
}

static int P1(IEnumerable<string> lines)
{
    var sfnum = new List<(int depth, int num)>();
    foreach (var num in Parse(lines))
        sfnum = Reduce(Add(sfnum, num));
    return Magnitude(sfnum);
}

static int P2(IEnumerable<string> lines)
{
    var res = 0;
    foreach (var (n1, i1) in Parse(lines).Select((n, i) => (n, i)))
    {
        foreach (var (n2, i2) in Parse(lines).Select((n, i) => (n, i)))
        {
            if (i1 != i2)
            {
                var sum = Reduce(Add(n1, n2));
                var mag = Magnitude(sum);
                if (mag > res)
                    res = mag;
            }
        }
    }
    return res;
}

var test1 = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]".Split('\n');
Debug.Assert(P1(test1) == 3488);

var test2 = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]".Split('\n');
Debug.Assert(P1(test2) == 4140);
Debug.Assert(P2(test2) == 3993);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 3892);
Debug.Assert(P2(input) == 4909);