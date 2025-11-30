using System.Diagnostics;
using System.Numerics;

int P1(IEnumerable<string> lines) => Solve(lines, Attempt1);
int P2(IEnumerable<string> lines) => Solve(lines, Attempt2);

int Solve(IEnumerable<string> lines, Attempt func)
{
    var grid = ParseGrid(lines);
    var path = lines.Last().Trim();
    var pos = grid.Keys.Where(k => k.Imaginary == 1).MinBy(k => k.Real);
    var dir = Complex.One;
    for (int i = 0; i < path.Length; i++)
    {
        if (char.IsLetter(path[i]))
        {
            dir *= path[i] switch
            {
                'R' => Complex.ImaginaryOne,
                _ => -Complex.ImaginaryOne,
            };
        }
        else
        {
            var currIdx = i;
            while (currIdx < path.Length && char.IsAsciiDigit(path[currIdx]))
            {
                currIdx += 1;
            }
            var steps = int.Parse(path.Substring(i, currIdx - i));
            i = currIdx - 1;
            (pos, dir) = Move(grid, pos, dir, steps, func);
        }
    }
    return Score(pos, dir);
}

int Score(Complex pos, Complex dir)
{
    var facing = 0;
    if (dir == Complex.One)
        facing = 0;
    else if (dir == -Complex.One)
        facing = 2;
    else if (dir == Complex.ImaginaryOne)
        facing = 1;
    else if (dir == -Complex.ImaginaryOne)
        facing = 3;
    return (int)(1000 * pos.Imaginary + 4 * pos.Real + facing);
}

(Complex pos, Complex dir)
Move(IReadOnlyDictionary<Complex, char> grid,
     Complex pos, Complex dir, int steps, Attempt func)
{
    for (int i = 0; i < steps; i++)
    {
        var attempt = pos + dir;
        if (grid.TryGetValue(attempt, out var c))
        { // Still in grid
            if (c == '.')
                pos = attempt;
            else
                return (pos, dir); // '#': hits a wall
        }
        else
        { // Wraps around
            (attempt, var newDir) = func(grid, pos, dir);

            if (grid[attempt] == '.')
            {
                pos = attempt;
                dir = newDir;
                // P1: dir does not change when wrapping around edges
                // this does nothing
                // vs 
                // P2: dir changes when wrapping around
                // but only when it does not hit a wall '#' right away
            }
            else
                return (pos, dir); // '#': wraps to a wall
        }
    }
    return (pos, dir);
}

(Complex attempt, Complex dir)
Attempt1(IReadOnlyDictionary<Complex, char> grid, Complex pos, Complex dir)
{
    Complex attempt = new();
    if (dir == Complex.One)
        attempt = grid.Keys.Where(k => k.Imaginary == pos.Imaginary).MinBy(k => k.Real);
    else if (dir == -Complex.One)
        attempt = grid.Keys.Where(k => k.Imaginary == pos.Imaginary).MaxBy(k => k.Real);
    else if (dir == Complex.ImaginaryOne)
        attempt = grid.Keys.Where(k => k.Real == pos.Real).MinBy(k => k.Imaginary);
    else if (dir == -Complex.ImaginaryOne)
        attempt = grid.Keys.Where(k => k.Real == pos.Real).MaxBy(k => k.Imaginary);
    return (attempt, dir);
}

(Complex pos, Complex dir)
Attempt2(IReadOnlyDictionary<Complex, char> grid, Complex pos, Complex dir)
{
    double col = 0;
    double row = 0;
    if (dir == Complex.One)
    {
        if (InZone2(pos)) // 2R->5R
        {
            dir = -Complex.One;
            col = grid.Keys.Where(InZone5).Max(k => k.Real);
            row = grid.Keys.Where(InZone5).Max(k => k.Imaginary) - (pos.Imaginary - 1);
        }
        else if (InZone3(pos)) // 3R->2D
        {
            dir = -Complex.ImaginaryOne;
            col = grid.Keys.Where(InZone2).Min(k => k.Real)
                 + (pos.Imaginary - grid.Keys.Where(InZone3).Min(k => k.Imaginary));
            row = grid.Keys.Where(InZone2).Max(k => k.Imaginary);
        }
        else if (InZone5(pos)) // 5R->2R
        {
            dir = -Complex.One;
            col = grid.Keys.Where(InZone2).Max(k => k.Real);
            row = grid.Keys.Where(InZone5).Max(k => k.Imaginary) - pos.Imaginary + 1;
        }
        else if (InZone6(pos)) // 6R->5D
        {
            dir = -Complex.ImaginaryOne;
            col = grid.Keys.Where(InZone5).Min(k => k.Real)
                 + (pos.Imaginary - grid.Keys.Where(InZone6).Min(k => k.Imaginary));
            row = grid.Keys.Where(InZone5).Max(k => k.Imaginary);
        }
    }
    else if (dir == -Complex.One)
    {
        if (InZone1(pos)) // 1L->4L
        {
            dir = Complex.One;
            col = grid.Keys.Where(InZone4).Min(k => k.Real);
            row = grid.Keys.Where(InZone4).Max(k => k.Imaginary) - (pos.Imaginary - 1);
        }
        else if (InZone3(pos)) // 3L->4U
        {
            dir = Complex.ImaginaryOne;
            col = pos.Imaginary - grid.Keys.Where(InZone3).Min(k => k.Imaginary) + 1;
            row = grid.Keys.Where(InZone4).Min(k => k.Imaginary);
        }
        else if (InZone4(pos)) // 4L->1L
        {
            dir = Complex.One;
            col = grid.Keys.Where(InZone1).Min(k => k.Real);
            row = grid.Keys.Where(InZone4).Max(k => k.Imaginary) - pos.Imaginary + 1;
        }
        else if (InZone6(pos)) // 6L->1U
        {
            dir = Complex.ImaginaryOne;
            col = pos.Imaginary - grid.Keys.Where(InZone6).Min(k => k.Imaginary)
                + grid.Keys.Where(InZone1).Min(k => k.Real);
            row = grid.Keys.Where(InZone1).Min(k => k.Imaginary);
        }
    }
    else if (dir == Complex.ImaginaryOne)
    {
        if (InZone2(pos)) // 2D->3R
        {
            dir = -Complex.One;
            col = grid.Keys.Where(InZone3).Max(k => k.Real);
            row = grid.Keys.Where(InZone3).Min(k => k.Imaginary)
                + (pos.Real - grid.Keys.Where(InZone2).Min(k => k.Real));
        }
        else if (InZone5(pos)) // 5D->6R
        {
            dir = -Complex.One;
            col = grid.Keys.Where(InZone6).Max(k => k.Real);
            row = grid.Keys.Where(InZone6).Min(k => k.Imaginary)
                + (pos.Real - grid.Keys.Where(InZone5).Min(k => k.Real));
        }
        else if (InZone6(pos)) // 6D->2U
        {
            col = grid.Keys.Where(InZone2).Min(k => k.Real) + pos.Real - 1;
            row = grid.Keys.Where(InZone2).Min(k => k.Imaginary);
        }
    }
    else if (dir == -Complex.ImaginaryOne)
    {
        if (InZone1(pos)) // 1U->6L
        {
            dir = Complex.One;
            col = grid.Keys.Where(InZone6).Min(k => k.Real);
            row = grid.Keys.Where(InZone6).Min(k => k.Imaginary)
                + (pos.Real - grid.Keys.Where(InZone1).Min(k => k.Real));
        }
        else if (InZone2(pos)) // 2U->6D
        {
            col = pos.Real - grid.Keys.Where(InZone2).Min(k => k.Real) + 1;
            row = grid.Keys.Where(InZone6).Max(k => k.Imaginary);
        }
        else if (InZone4(pos)) // 4U->3L
        {
            dir = Complex.One;
            col = grid.Keys.Where(InZone3).Min(k => k.Real);
            row = grid.Keys.Where(InZone3).Min(k => k.Imaginary)
                + (pos.Real - grid.Keys.Where(InZone4).Min(k => k.Real));
        }
    }

    return (new Complex(col, row), dir);
}

/*
    -------
    |1 |2 |
    -------
    |3 |
 -------
 |4 |5 |
 ----
 |6 |
 ----
*/

bool InZone1(Complex c) => 51 <= c.Real && c.Real <= 100
                        && 1 <= c.Imaginary && c.Imaginary <= 50;
bool InZone2(Complex c) => 101 <= c.Real && c.Real <= 150
                        && 1 <= c.Imaginary && c.Imaginary <= 50;
bool InZone3(Complex c) => 51 <= c.Real && c.Real <= 100
                        && 51 <= c.Imaginary && c.Imaginary <= 100;
bool InZone4(Complex c) => 1 <= c.Real && c.Real <= 50
                        && 101 <= c.Imaginary && c.Imaginary <= 150;
bool InZone5(Complex c) => 51 <= c.Real && c.Real <= 100
                        && 101 <= c.Imaginary && c.Imaginary <= 150;
bool InZone6(Complex c) => 1 <= c.Real && c.Real <= 50
                        && 151 <= c.Imaginary && c.Imaginary <= 200;

Dictionary<Complex, char> ParseGrid(IEnumerable<string> lines)
{
    var grid = new Dictionary<Complex, char>();
    foreach (var (line, row) in lines.TakeWhile(s => !string.IsNullOrWhiteSpace(s))
                                     .Select((s, i) => (s, i + 1)))
    {
        foreach (var (c, col) in line.TrimEnd().Select((c, i) => (c, i + 1)))
        {
            switch (c)
            {
                case '.':
                    grid.Add(new Complex(col, row), '.');
                    break;
                case '#':
                    grid.Add(new Complex(col, row), '#');
                    break;
                default:
                    break;
            }
        }
    }
    return grid;
}

var test = @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5".Split('\n');
Debug.Assert(P1(test) == 6032);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 131052);
Debug.Assert(P2(input) == 4578);

delegate ValueTuple<Complex, Complex> Attempt(IReadOnlyDictionary<Complex, char> grid,
                                              Complex pos, Complex dir);