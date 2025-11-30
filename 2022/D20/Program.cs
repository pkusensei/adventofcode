using System.Diagnostics;

long P1(IEnumerable<string> lines)
{
    var nums = Parse(lines).ToList();
    Update(nums);
    return Find(nums);
}

long P2(IEnumerable<string> lines)
{
    var nums = Parse(lines).Select(n => n with { Num = n.Num * 811589153 }).ToList();
    for (int i = 0; i < 10; i++)
    {
        Update(nums);
    }
    return Find(nums);
}

void Update(IList<Item> nums)
{
    var length = nums.Count;
    foreach (var item in nums)
    {
        var nIdx = (item.Num + item.Idx) % (length - 1);
        if (nIdx <= 0 && item.Num + item.Idx != 0)
            nIdx = nIdx + length - 1;
        if (nIdx < item.Idx)
        {
            foreach (var num in nums.Where(n => nIdx <= n.Idx && n.Idx < item.Idx))
                num.Idx += 1;
        }
        else if (nIdx > item.Idx)
        {
            foreach (var num in nums.Where(n => item.Idx < n.Idx && n.Idx <= nIdx))
                num.Idx -= 1;
        }
        item.Idx = (int)nIdx;
    }
}

long Find(IList<Item> nums)
{
    var zeroIdx = nums.Where(n => n.Num == 0).First().Idx;
    var indices = Enumerable.Range(1, 3).Select(n => (n * 1000 + zeroIdx) % nums.Count);
    return nums.Where(n => indices.Contains(n.Idx)).Sum(n => n.Num);
}

IEnumerable<Item> Parse(IEnumerable<string> lines)
=> lines.Select(s => int.Parse(s.Trim()))
        .Select((n, i) => new Item { Num = n, Idx = i });

var test = @"1
2
-3
3
-2
0
4".Split('\n');
Debug.Assert(P1(test) == 3);
Debug.Assert(P2(test) == 1623178306);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 8028);
Debug.Assert(P2(input) == 8798438007673);

record Item
{
    public long Num { get; init; }
    public int Idx { get; set; }
}