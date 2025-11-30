using System.Diagnostics;

int P1(IEnumerable<string> lines) => lines.Select(ParseLine)
.Select(bp => bp.Id * Produce(bp, new State(24, new(0, 0, 0, 0), new Product(1, 0, 0, 0)), new()))
.Sum();

int P2(IEnumerable<string> lines) => lines.Take(3).Select(ParseLine)
.Select(bp => Produce(bp, new State(32, new(0, 0, 0, 0), new Product(1, 0, 0, 0)), new()))
.Aggregate(1, (x, y) => x * y);

int Produce(Blueprint bp, State state, Dictionary<State, int> states)
{
    if (state.Time == 0)
        return state.Prod.Geode;
    if (!states.ContainsKey(state))
    {
        states[state] = Next(bp, state).Select(ns =>
        {
            var time = state.Time - 1;
            var prod = new Product(
                ns.Prod.Ore + state.Rate.Ore,
                ns.Prod.Clay + state.Rate.Clay,
                ns.Prod.Obsidian + state.Rate.Obsidian,
                ns.Prod.Geode + state.Rate.Geode
            );
            return Produce(bp, ns with { Time = time, Prod = prod }, states);
        }).Max();
    }
    return states[state];
}

IEnumerable<State> Next(Blueprint bp, State state)
{
    var tryBuild = (Robot r, Product p) =>
        p.Ore >= r.Cost.Ore && p.Clay >= r.Cost.Clay && p.Obsidian >= r.Cost.Obsidian;
    var build = (Robot r, State s) =>
    {
        var prod = new Product(
            state.Prod.Ore - r.Cost.Ore,
            state.Prod.Clay - r.Cost.Clay,
            state.Prod.Obsidian - r.Cost.Obsidian,
            state.Prod.Geode
        );
        var rate = new Product(
            state.Rate.Ore + r.Prod.Ore,
            state.Rate.Clay + r.Prod.Clay,
            state.Rate.Obsidian + r.Prod.Obsidian,
            state.Rate.Geode + r.Prod.Geode
        );
        return s with { Prod = prod, Rate = rate };
    };

    var prev = new Product(
         state.Prod.Ore - state.Rate.Ore,
         state.Prod.Clay - state.Rate.Clay,
         state.Prod.Obsidian - state.Rate.Obsidian,
         state.Prod.Geode - state.Rate.Geode
    );

    if (!tryBuild(bp.Geode, prev) && tryBuild(bp.Geode, state.Prod))
    {
        yield return build(bp.Geode, state);
        yield break;
    }
    if (!tryBuild(bp.Obsidian, prev) && tryBuild(bp.Obsidian, state.Prod))
        yield return build(bp.Obsidian, state);
    if (!tryBuild(bp.Clay, prev) && tryBuild(bp.Clay, state.Prod))
        yield return build(bp.Clay, state);
    if (!tryBuild(bp.Ore, prev) && tryBuild(bp.Ore, state.Prod))
        yield return build(bp.Ore, state);

    yield return state;
}

Blueprint ParseLine(string line)
{
    var nums = new List<int>();
    foreach (var item in line.Split(new char[] { ' ', ':' }))
    {
        if (int.TryParse(item, out var num))
            nums.Add(num);
    }

    var ore = new Robot(new Cost(nums[1], 0, 0), new Product(1, 0, 0, 0));
    var cla = new Robot(new Cost(nums[2], 0, 0), new Product(0, 1, 0, 0));
    var obs = new Robot(new Cost(nums[3], nums[4], 0), new Product(0, 0, 1, 0));
    var geo = new Robot(new Cost(nums[5], 0, nums[6]), new Product(0, 0, 0, 1));
    return new Blueprint(nums[0], ore, cla, obs, geo);
}

var test = @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian."
.Split('\n');
Debug.Assert(P1(test) == 33);
Debug.Assert(P2(test) == 56 * 62);

var input = File.ReadLines("input.txt");
Debug.Assert((P1(input)) == 1766);
Debug.Assert(P2(input) == 30780); // Somehow produces 29754

record Cost(int Ore, int Clay, int Obsidian);
record Product(int Ore, int Clay, int Obsidian, int Geode);
record Robot(Cost Cost, Product Prod);
record Blueprint(int Id, Robot Ore, Robot Clay, Robot Obsidian, Robot Geode);
record State(int Time, Product Prod, Product Rate);