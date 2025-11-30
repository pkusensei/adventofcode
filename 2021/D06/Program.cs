using System.Diagnostics;

static Dictionary<int, Int64> OneTurn(Dictionary<int, Int64> nums)
{
    var res = new Dictionary<int, Int64>();
    foreach (var (dayLeft, count) in nums)
    {
        if (dayLeft == 0)
        {
            res.Add(8, count);
        }
        else if (dayLeft - 1 != 6)
        {
            res.Add(dayLeft - 1, count);
        }
    }
    var sum = nums.Where(kv => kv.Key == 7 || kv.Key == 0).Select(kv => kv.Value).Sum();
    if (sum > 0)
    {
        res.Add(6, sum);
    }
    return res;
}

static Int64 Solve(IEnumerable<int> input, int days)
{
    var count = input.GroupBy(x => x).ToDictionary(x => x.Key, x => (Int64)x.Count());
    for (int i = 0; i < days; i++)
    {
        count = OneTurn(count);
    }
    return count.Values.Sum();
}

var test = "3,4,3,1,2".Split(',').Select(s => Int32.Parse(s));
Debug.Assert(Solve(test, 18) == 26);
Debug.Assert(Solve(test, 80) == 5934);

var input = File.ReadLines("input.txt").First().Split(',').Select(s => Int32.Parse(s));
Debug.Assert(Solve(input, 80) == 345793);

Debug.Assert(Solve(test, 256) == 26984457539);
Debug.Assert(Solve(input, 256) == 1572643095893);