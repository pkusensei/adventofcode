using System.Diagnostics;

// Segment counts
const int ZERO_COUNT = 6; // 0, 6, 9
const int ONE_COUNT = 2;
const int THREE_COUNT = 5; // 2, 3, 5
const int FOUR_COUNT = 4;
const int SEVEN_COUNT = 3;
const int EIGHT_COUNT = 7;

static int P1(IEnumerable<string> lines)
{
    return lines.Select(s => s.Split('|')[1].Split(' ').Where(digit =>
    {
        var l = digit.Trim().Length;
        return l == ONE_COUNT || l == FOUR_COUNT || l == SEVEN_COUNT || l == EIGHT_COUNT;
    }).Count()).Sum();
}

var test = @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce".Split('\n');
Debug.Assert(P1(test) == 26);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 381);

static IList<HashSet<char>> FindDigits(string line)
{
    var digits = line.Trim().Split(' ').Select(s => s.ToHashSet()).ToArray();
    var one = digits.Where(s => s.Count == ONE_COUNT).Single();
    var four = digits.Where(s => s.Count == FOUR_COUNT).Single();
    var seven = digits.Where(s => s.Count == SEVEN_COUNT).Single();
    var eight = digits.Where(s => s.Count == EIGHT_COUNT).Single();
    var nine = digits.Where(s => s.Count == ZERO_COUNT && s.IsProperSupersetOf(four)).Single();
    var zero = digits.Where(s => s.Count == ZERO_COUNT && s != nine && s.IsProperSupersetOf(seven)).Single();
    var three = digits.Where(s => s.Count == THREE_COUNT && s.IsProperSupersetOf(seven)).Single();
    var six = digits.Where(s => s.Count == ZERO_COUNT && s != zero && s != nine).Single();
    var five = digits.Where(s => s.Count == THREE_COUNT && s.IsProperSubsetOf(six)).Single();
    var two = digits.Where(s => s.Count == THREE_COUNT && s != three && s != five).Single();
    return new List<HashSet<char>> { zero, one, two, three, four, five, six, seven, eight, nine };
}

static int P2(IEnumerable<string> lines)
{
    var res = 0;
    foreach (var line in lines)
    {
        var input = line.Split('|');
        var digits = FindDigits(input[0]);
        var output = input[1].Trim().Split(' ').Reverse();
        foreach (var (outDigit, idx) in output.Select((s, idx) => (s.ToHashSet(), idx)))
        {
            foreach (var (digit, num) in digits.Select((s, i) => (s, i)))
            {
                if (digit.SetEquals(outDigit))
                {
                    res += num * (int)Math.Pow(10, idx);
                    break;
                }
            }
        }
    }

    return res;
}

Debug.Assert(P2(test) == 61229);
Debug.Assert(P2(input) == 1023686);