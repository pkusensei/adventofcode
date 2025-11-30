using System.Diagnostics;

int P1(IEnumerable<string> lines, int row)
{
    var (sensors, beacons) = Parse(lines);
    var reachable = FindReacheable(sensors, row);
    var (xmin, xmax) = FindRange(reachable, row);
    var covered = Enumerable.Range(xmin, xmax - xmin + 1).Count(
        x => reachable.Any(s => Distance(s.C, new Coord(x, row)) <= s.MaxDist)
    );
    var beaconNum = beacons.Distinct().Count(b => b.Y == row);
    var sensorNum = reachable.Count(s => s.C.Y == row);
    return covered - beaconNum - sensorNum;
}

Int64 P2(IEnumerable<string> lines, int limit)
{
    var (sensors, _) = Parse(lines);
    for (int y = 0; y < limit; y++)
    {
        var covered = FindReacheable(sensors, y)
            .Select(s => FindCoverage(s, y, limit))
            .OrderBy(p => p.xmin).ToList();

        bool merged = true;
        while (merged && covered.Count > 1)
        {
            merged = false;

            if (covered[0].xmin <= covered[1].xmin && covered[0].xmax >= covered[1].xmin)
            {
                var n = Math.Max(covered[0].xmax, covered[1].xmax);
                covered[0] = (covered[0].xmin, n);
                covered.RemoveAt(1);
                merged = true;
            }
        }
        if (!merged || covered[0].xmin != 0 || covered[0].xmax != limit)
        {
            return (Int64)(covered[0].xmax + 1) * 4000000 + y;
        }
    }
    return 0;
}

IEnumerable<Sensor> FindReacheable(IEnumerable<Sensor> sensors, int row)
=> sensors.Where(s => Math.Abs(s.C.Y - row) <= s.MaxDist);

(int xmin, int xmax) FindRange(IEnumerable<Sensor> sensors, int row)
{
    var xmin = sensors.Min(s => s.C.X - (s.MaxDist - Math.Abs(s.C.Y - row)));
    var xmax = sensors.Max(s => s.C.X + (s.MaxDist - Math.Abs(s.C.Y - row)));
    return (xmin, xmax);
}

(int xmin, int xmax) FindCoverage(Sensor sensor, int row, int limit)
{
    var xmin = sensor.C.X - (sensor.MaxDist - Math.Abs(sensor.C.Y - row));
    var xmax = sensor.C.X + (sensor.MaxDist - Math.Abs(sensor.C.Y - row));
    return (Math.Max(0, xmin), Math.Min(limit, xmax));
}

(List<Sensor>, List<Coord>) Parse(IEnumerable<string> lines)
{
    var sensors = new List<Sensor>();
    var beacons = new List<Coord>();
    var parse = (string s) =>
    {
        var num1 = s.Split(',')[0].Split('=')[1];
        var num2 = s.Split(',')[1].Split('=')[1];
        return new Coord(int.Parse(num1), int.Parse(num2));
    };
    foreach (var items in lines.Select(s => s.Split(':', StringSplitOptions.TrimEntries)))
    {
        var s = parse(items[0]);
        var b = parse(items[1]);
        sensors.Add(new Sensor(s, Distance(s, b)));
        beacons.Add(b);
    }
    return (sensors, beacons);
}

int Distance(Coord c1, Coord c2) => Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);

var test = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3".Split('\n');
Debug.Assert(P1(test, 10) == 26);
Debug.Assert(P2(test, 20) == 56000011);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input, 2000000) == 5367037);
Debug.Assert(P2(input, 4000000) == 11914583249288);

record struct Coord(int X, int Y);
record struct Sensor(Coord C, int MaxDist);