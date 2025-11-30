using System.Diagnostics;

static int Solve(IEnumerable<int> input, Func<int, int, int> calc)
{
    var min = input.Min();
    var max = input.Max();
    var nums = Enumerable.Range(min, max - min);
    return nums.Select(target => input.Select(pos => calc(pos, target)).Sum()).Min();
}

Func<int, int, int> p1 = (pos, target) => Math.Abs(pos - target);
Func<int, int, int> p2 = (pos, target) => Enumerable.Range(1, p1(pos, target)).Sum();

var test = new[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 };
Debug.Assert(Solve(test, p1) == 37);

var line = File.ReadLines("input.txt").First().Split(',').Select(s => Int32.Parse(s));
Debug.Assert(Solve(line, p1) == 349812);

Debug.Assert(Solve(test, p2) == 168);
Debug.Assert(Solve(line, p2) == 99763899);