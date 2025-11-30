// https://topaz.github.io/paste/#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
// Didn't do this
// It reads so dull it's not even funny

using System.Diagnostics;

static List<HashSet<Vec3>> Parse(IEnumerable<string> lines)
{
    var res = new List<HashSet<Vec3>>();
    foreach (var item in lines)
    {
        var line = item.Trim();
        if (string.IsNullOrEmpty(line))
            continue;
        else if (line.Contains("---"))
            res.Add(new HashSet<Vec3>());
        else
        {
            Debug.Assert(res.Count > 0);
            var tmp = line.Split(',').Select(s => int.Parse(s)).ToArray();
            res.Last().Add(new Vec3(tmp[0], tmp[1], tmp[2]));
        }
    }
    return res;
}

static Vec3 Rotate(Vec3 pt, Vec3 up, int rotation)
{
    var reoriented = up switch
    {
        (0, 1, 0) => pt,
        (0, -1, 0) => (pt.X, -pt.Y, -pt.Z),
        (1, 0, 0) => (pt.Y, pt.X, -pt.Z),
        (-1, 0, 0) => (pt.Y, -pt.X, pt.Z),
        (0, 0, 1) => (pt.Y, pt.Z, pt.X),
        (0, 0, -1) => (pt.Y, -pt.Z, -pt.X),
        _ => throw new ArgumentException($"Invalid Vec3 up = {up}"),
    };
    return rotation switch
    {
        0 => reoriented,
        1 => (reoriented.Z, reoriented.Y, -reoriented.X),
        2 => (-reoriented.X, reoriented.Y, -reoriented.Z),
        3 => (-reoriented.Z, reoriented.Y, reoriented.X),
        _ => throw new ArgumentException("Invalid rotation")
    };
}

static Vec3 Translate(Vec3 p, Vec3 v) => (p.X + v.X, p.Y + v.Y, p.Z + v.Z);
static Vec3 Diff(Vec3 p1, Vec3 p2) => (p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

static (HashSet<Vec3> aligned, Vec3 Translation, Vec3 up, int rotation)?
Align(IReadOnlySet<Vec3> beacons1, IReadOnlySet<Vec3> beacons2)
{
    var axes = new[] { (1, 0, 0), (-1, 0, 0), (0, 1, 0), (0, -1, 0), (0, 0, 1), (0, 0, -1) };
    foreach (var axis in axes)
    {
        for (int rotation = 0; rotation < 4; rotation++)
        {
            var rotatedBeacons2 = beacons2.Select(b => Rotate(b, axis, rotation))
                                          .ToHashSet();
            foreach (var b1 in beacons1)
            {
                foreach (var b2 in rotatedBeacons2)
                {
                    var diff = Diff(b1, b2);
                    var transformedB2 = rotatedBeacons2.Select(b => Translate(b, diff))
                                                       .ToHashSet();
                    var inter = transformedB2.Intersect(beacons1).ToHashSet();
                    if (inter.Count >= 12)
                        return (transformedB2, diff, axis, rotation);
                }
            }
        }
    }
    return null;
}

static (List<HashSet<Vec3>> scans, List<HashSet<Vec3>> scanners)
Reduce(IList<HashSet<Vec3>> scans, IList<HashSet<Vec3>> scanners)
{
    var toRemove = new HashSet<int>();
    for (int i = 0; i < scans.Count - 1; i++)
    {
        for (int j = i + 1; j < scans.Count; j++)
        {
            if (toRemove.Contains(j))
                continue;
            var alignment = Align(scans[i], scans[j]);
            if (alignment != null)
            {
                foreach (var s in scanners[j])
                {
                    var scanner = Translate(alignment.Value.Translation,
                                            Rotate(s, alignment.Value.up, alignment.Value.rotation));
                    scanners[i].Add(scanner);
                }
                scans[i].UnionWith(alignment.Value.aligned);
                toRemove.Add(j);
            }
        }
    }
    return (scans.Where((s, i) => !toRemove.Contains(i)).ToList(),
            scanners.Where((s, i) => !toRemove.Contains(i)).ToList());
}

static (List<HashSet<Vec3>> scans, List<HashSet<Vec3>> scanners)
Solve(IEnumerable<string> lines)
{
    var scans = Parse(lines);
    var scanners = scans.Select(_ => new HashSet<Vec3> { (0, 0, 0) }).ToList();
    while (scans.Count > 1)
        (scans, scanners) = Reduce(scans, scanners);
    return (scans, scanners);
}

static int P1(IEnumerable<string> lines)
{
    var (scans, _) = Solve(lines);
    return scans[0].Count;
}

static int Distance(Vec3 p1, Vec3 p2)
{
    var d = Diff(p1, p2);
    return Math.Abs(d.X) + Math.Abs(d.Y) + Math.Abs(d.Z);
}

static int P2(IEnumerable<string> lines)
{
    var (_, scanners) = Solve(lines);
    var scannerList = scanners[0].ToList();
    var res = 0;
    for (int i = 0; i < scannerList.Count - 1; i++)
    {
        for (int j = i + 1; j < scannerList.Count; j++)
        {
            var dist = Distance(scannerList[i], scannerList[j]);
            if (res < dist)
                res = dist;
        }
    }
    return res;
}

var test = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14".Split('\n');
Debug.Assert(P1(test) == 79);
Debug.Assert(P2(test) == 3621);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 445);
Debug.Assert(P2(input) == 13225);

record struct Vec3(int X, int Y, int Z)
{
    public static implicit operator Vec3((int x, int y, int z) v) => new Vec3(v.x, v.y, v.z);
}