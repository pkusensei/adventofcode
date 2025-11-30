using System.Diagnostics;

static Coord MoveP1(Coord oldPos, string inst)
{
    var command = inst.Split(' ');
    var dir = command[0].Trim();
    var step = Int32.Parse(command[1]);
    switch (dir)
    {
        case "forward":
            return oldPos with { X = oldPos.X + step };
        case "down":
            return oldPos with { Y = oldPos.Y + step };
        case "up":
            return oldPos with { Y = oldPos.Y - step };
        default:
            throw new ArgumentException($"Invalid instruction {inst}");
    }
}

static int P1(IEnumerable<string> input)
{
    var pos = new Coord(0, 0);
    foreach (var item in input)
    {
        pos = MoveP1(pos, item);
    }
    return pos.X * pos.Y;
}

var test = @"forward 5
down 5
forward 8
up 3
down 8
forward 2".Split('\n');
Debug.Assert(P1(test) == 150);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 1804520);

static (Coord, int) MoveP2(Coord oldPos, int aim, string inst)
{
    var command = inst.Split(' ');
    var dir = command[0].Trim();
    var step = Int32.Parse(command[1]);
    switch (dir)
    {
        case "forward":
            return (new Coord(oldPos.X + step, oldPos.Y + aim * step), aim);
        case "down":
            return (oldPos, aim + step);
        case "up":
            return (oldPos, aim - step);
        default:
            throw new ArgumentException($"Invalid instruction {inst}");
    }
}

static int P2(IEnumerable<string> input)
{
    var pos = new Coord(0, 0);
    var aim = 0;
    foreach (var item in input)
    {
        (pos, aim) = MoveP2(pos, aim, item);
    }
    return pos.X * pos.Y;
}

Debug.Assert(P2(test) == 900);
Debug.Assert(P2(input) == 1971095320);

readonly record struct Coord(int X, int Y);
