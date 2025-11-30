using System.Diagnostics;

int Solve(IEnumerable<string> lines, Func<int[], bool> predicate) => lines.Where(
    line =>
    {
        var nums = line.Split(new char[] { ',', '-' }, StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
        return predicate(nums);
    }
).Count();

var p1 = (int[] nums) => (nums[0] - nums[2]) * (nums[1] - nums[3]) <= 0;
var p2 = (int[] nums) => !(nums[1] < nums[2] || nums[0] > nums[3]);

var test = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8".Split('\n');
Debug.Assert(Solve(test, p1) == 2);
Debug.Assert(Solve(test, p2) == 4);

var input = File.ReadLines("input.txt");
Debug.Assert(Solve(input, p1) == 588);
Debug.Assert(Solve(input, p2) == 911);