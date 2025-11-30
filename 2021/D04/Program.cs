using System.Diagnostics;

var test = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7".Split('\n').ToList();

Debug.Assert(P1(test) == 4512);

var lines = File.ReadLines("input.txt").ToList();
Debug.Assert(P1(lines) == 6592);

Debug.Assert(P2(test) == 1924);
Debug.Assert(P2(lines) == 31755);

static (IList<Board> boards, IList<int> draws) Parse(IList<string> input)
{
    var draws = input[0].Split(',').Select(s => Int32.Parse(s.Trim())).ToList();
    var chunks = input.Skip(2).Where(s => !string.IsNullOrWhiteSpace(s)).Chunk(5);
    var boards = new List<Board>();

    foreach (var item in chunks)
    {
        var numbers = string.Join(' ', item).Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s.Trim()));
        boards.Add(new Board(numbers.ToArray()));
    }
    return (boards, draws);
}

static int P1(IList<string> input)
{
    var (boards, draws) = Parse(input);
    foreach (var draw in draws)
    {
        foreach (var board in boards)
        {
            if (board.MarkAndCheck(draw))
            {
                return draw * board.SumOfUnmarked();
            }
        }
    }
    throw new ArgumentException("Input error");
}

static int P2(IList<string> input)
{
    var (boards, draws) = Parse(input);
    var count = 0;
    foreach (var draw in draws)
    {
        foreach (var board in boards)
        {
            if (board.Won)
                continue;
            if (board.MarkAndCheck(draw))
            {
                count += 1;
                if (count == boards.Count)
                {
                    return board.SumOfUnmarked() * draw;
                }
            }
        }
    }
    throw new ArgumentException("Input error");
}

class Board
{
    const int SIZE = 25;
    const int ROW_SIZE = 5;
    int[] Numbers { get; } = new int[SIZE];
    bool[] Marked { get; } = new bool[SIZE];
    public bool Won { get; private set; } = default;

    public Board(int[] numbers)
    {
        Numbers = numbers;
    }

    public bool MarkAndCheck(int number)
    {
        foreach (var (value, idx) in Numbers.Select((value, idx) => (value, idx)))
        {
            if (value == number)
            {
                Marked[idx] = true;
                return Check(idx);
            }
        }
        return false;
    }

    bool Check(int index)
    {
        var first = index % ROW_SIZE;
        var col = Marked.Where((value, i) => first == i % ROW_SIZE).All(x => x);
        var rowIdx = index / ROW_SIZE;
        var row = Marked.Skip(rowIdx * ROW_SIZE).Take(ROW_SIZE).All(x => x);
        Won = col || row;
        return Won;
    }

    public int SumOfUnmarked() => Marked.Zip(Numbers).Where(v => !v.First).Select(v => v.Second).Sum();
}
