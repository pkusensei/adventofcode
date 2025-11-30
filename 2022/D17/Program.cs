using System.Diagnostics;

long P1(string line)
{
    int count = 2022;
    var grid = Enumerable.Range(0, 7).Select(x => new Coord(x, 0)).ToList();
    var lineIdx = 0;
    var length = line.Length;

    for (int i = 0; i < count; i++)
    {
        var height = grid.Select(c => c.Y).Max() + 4;
        var block = Block.Create(height, i % 5 + 1);

        while (true)
        {
            block.Move(grid, line[lineIdx % length]);
            lineIdx += 1;
            if (!block.Fall(grid))
                break;
        }
    }

    return grid.Select(c => c.Y).Max();
}

// No it doesn't work. Assertions gonna fail
long P2(string line)
{
    var (cycle, cHeight) = FindCycle(line);
    var count = 1000000000000 % cycle;

    var grid = Enumerable.Range(0, 7).Select(x => new Coord(x, 0)).ToList();
    var lineIdx = 0;
    var length = line.Length;

    for (int i = 0; i < count; i++)
    {
        var height = grid.Select(c => c.Y).Max() + 4;
        var block = Block.Create(height, i % 5 + 1);

        while (true)
        {
            block.Move(grid, line[lineIdx % length]);
            lineIdx += 1;
            if (!block.Fall(grid))
                break;
        }
    }

    return grid.Select(c => c.Y).Max() + 1000000000000 / cycle * cHeight;
}

(int, long) FindCycle(string line)
{
    var grid = Enumerable.Range(0, 7).Select(x => new Coord(x, 0)).ToList();
    var lineIdx = 0;
    var length = line.Length;
    var seen = new Dictionary<(int, int), long>();

    var count = 0;
    while (true)
    {
        var bIdx = count % 5;
        var height = grid.Select(c => c.Y).Max() + 4;
        var block = Block.Create(height, bIdx);

        while (true)
        {
            lineIdx = lineIdx % length;
            block.Move(grid, line[lineIdx % length]);
            lineIdx += 1;
            if (!block.Fall(grid))
                break;
        }
        if (seen.TryGetValue((bIdx, lineIdx), out var last))
        {
            var curr = grid.Select(c => c.Y).Max();
            return (count, curr - last);
        }
        else
        {
            seen[(bIdx, lineIdx)] = grid.Select(c => c.Y).Max();
        }
        count += 1;
    }
}

var test = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
Debug.Assert(P1(test) == 3068);
Debug.Assert(P2(test) == 1514285714288);

var input = File.ReadAllText("input.txt").Trim();
Debug.Assert(P1(input) == 3083);
Debug.Assert(P2(input) == 1532183908048);

record struct Coord(long X, long Y);
class Block
{
    public Coord[] Filled { get; private set; } = null!;

    public bool Fall(List<Coord> grid)
    {
        var next = Filled.Select(c => c with { Y = c.Y - 1 });

        if (next.Intersect(grid).Count() > 0)
        {
            // falls to rest
            grid.AddRange(Filled);
            return false;
        }
        else
        {
            Filled = next.ToArray();
            return true;
        }
    }

    public bool Move(List<Coord> grid, char move)
    {
        var next = move switch
        {
            '<' => Filled.Select(c => c with { X = c.X - 1 }),
            _ => Filled.Select(c => c with { X = c.X + 1 }),
        };
        if (next.Intersect(grid).Count() > 0
        || next.Any(c => c.X < 0 || c.X > 6))
        {
            return false;
        }
        else
        {
            Filled = next.ToArray();
            return true;
        }
    }

    public static Block Create(long height, int type)
    => type switch
    {
        1 => new Block
        {
            Filled = new[]{
                        new Coord(2, height),
                        new Coord(3, height),
                        new Coord(4, height),
                        new Coord(5, height),
                    },
        },
        2 => new Block
        {
            Filled = new[]{
                        new Coord(3, height),
                        new Coord(2, height+1),
                        new Coord(3, height+1),
                        new Coord(4, height+1),
                        new Coord(3, height+2),
                    },
        },
        3 => new Block
        {
            Filled = new[]{
                        new Coord(2, height),
                        new Coord(3, height),
                        new Coord(4, height),
                        new Coord(4, height+1),
                        new Coord(4, height+2),
                    },
        },
        4 => new Block
        {
            Filled = new[]{
                        new Coord(2, height),
                        new Coord(2, height+1),
                        new Coord(2, height+2),
                        new Coord(2, height+3),
                    },
        },
        _ => new Block
        {
            Filled = new[]{
                        new Coord(2, height),
                        new Coord(3, height),
                        new Coord(2, height+1),
                        new Coord(3, height+1),
                    },
        },
    };
}