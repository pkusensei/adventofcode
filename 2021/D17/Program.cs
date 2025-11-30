using System.Diagnostics;

static (int x1, int x2, int y1, int y2) Parse(string line)
{
    var xs = line.Split("x=")[1].Split(", ")[0].Split("..").Select(s => int.Parse(s)).ToArray();
    var ys = line.Trim().Split("y=")[1].Split("..").Select(s => int.Parse(s)).ToArray();
    return (xs[0], xs[1], ys[0], ys[1]);
}

static int P1(string line)
{
    var (_, _, y1, _) = Parse(line);
    var vy = Math.Abs(y1) - 1;
    return Math.Abs(y1) * vy / 2;
}

static int P2(string line)
{
    var (x1, x2, y1, y2) = Parse(line);
    var vxmin = 1;
    while ((1 + vxmin) * vxmin < x1)
    {
        vxmin += 1;
    }
    var vxmax = x2;
    var vymin = y1;
    var vymax = Math.Abs(y1) - 1;
    var count = 0;
    for (int vx0 = vxmin; vx0 <= vxmax; vx0++)
    {
        for (int vy0 = vymin; vy0 <= vymax; vy0++)
        {
            var x = 0;
            var y = 0;
            var vx = vx0;
            var vy = vy0;
            while (x <= x2 && y1 <= y)
            {
                if (x >= x1 && y <= y2)
                {
                    count += 1;
                    break;
                }
                x += vx;
                y += vy;
                if (vx > 0)
                    vx -= 1;
                vy -= 1;
            }
        }
    }
    return count;
}

var test = "target area: x=20..30, y=-10..-5";
Debug.Assert(P1(test) == 45);
Debug.Assert(P2(test) == 112);

var input = "target area: x=217..240, y=-126..-69";
Debug.Assert(P1(input) == 7875);
Debug.Assert(P2(input) == 2321);