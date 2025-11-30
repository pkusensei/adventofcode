using System.Diagnostics;

static char Match(char c) => c switch
{
    '(' => ')',
    '[' => ']',
    '{' => '}',
    '<' => '>',
    _ => throw new ArgumentException($"Invalid input {c}")
};

static bool IsOpen(char c) => c switch
{
    '(' => true,
    '[' => true,
    '{' => true,
    '<' => true,
    _ => false
};

static bool IsClose(char c) => c switch
{
    ')' => true,
    ']' => true,
    '}' => true,
    '>' => true,
    _ => false
};


static int P1(IEnumerable<string> lines)
{
    var scores = new Dictionary<char, int>() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
    int res = 0;
    foreach (var line in lines)
    {
        var chars = new Stack<char>();
        foreach (var c in line.Trim())
        {
            if (IsOpen(c))
            {
                chars.Push(c);
            }
            else
            {
                var top = chars.Pop();
                if (c != Match(top))
                {
                    if (IsClose(c))
                        res += scores[c];
                }
            }
        }
    }
    return res;
}

static Int64 P2(IEnumerable<string> lines)
{
    var scores = new Dictionary<char, int>() { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };
    var res = new List<Int64>();
    foreach (var line in lines)
    {
        var chars = new Stack<char>();
        var score = 0L;
        var corrupted = false;
        foreach (var c in line)
        {
            if (IsOpen(c))
            {
                chars.Push(c);
            }
            else
            {
                var top = chars.Pop();
                if (c != Match(top))
                {
                    corrupted = true;
                    break;
                }
            }
        }
        while (chars.Count > 0 && !corrupted)
        {
            var c = chars.Pop();
            if (IsOpen(c))
                score = score * 5 + scores[c];
        }
        if (score > 0)
        {
            res.Add(score);
        }
    }
    res.Sort();
    return res[res.Count / 2];
}

var test = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]".Split();
Debug.Assert(P1(test) == 26397);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 323691);

Debug.Assert(P2(test) == 288957L);
Debug.Assert(P2(input) == 2858785164L);
