using System.Diagnostics;

var test = @"199
200
208
210
200
207
240
269
260
263".Split('\n').Select(x =>
        Int32.Parse(x)
    ).ToList();

static int p1(IList<int> nums)
{
    return nums.Skip(1).Where((value, idx) => value > nums[idx]).Count();
}

Debug.Assert(p1(test) == 7);

var input = File.ReadLines("input.txt").Select(x =>
        Int32.Parse(x)
    ).ToList();

Debug.Assert(p1(input) == 1665);

static int p2(IList<int> nums)
{
    return nums.TakeWhile((value, idx) => idx < nums.Count - 3)
    .Where((value, idx) => value + nums[idx + 1] + nums[idx + 2] < nums[idx + 1] + nums[idx + 2] + nums[idx + 3]).Count();
}

Debug.Assert(p2(test) == 5);
Debug.Assert(p2(input) == 1702);
