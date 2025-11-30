using System.Diagnostics;

int P1(IList<string> lines)
{
    var (trees, rowSize, colSize) = Parse(lines);
    var invisible = 0;
    foreach (var ((row, col), tree) in trees)
    {
        var left = Enumerable.Range(0, col)
            .Select(cIdx => (row, cIdx))
            .Any(k => trees.ContainsKey(k) && trees[k] >= tree);
        var right = Enumerable.Range(col + 1, colSize - col)
            .Select(cIdx => (row, cIdx))
            .Any(k => trees.ContainsKey(k) && trees[k] >= tree);
        var up = Enumerable.Range(0, row)
            .Select(rIdx => (rIdx, col))
            .Any(k => trees.ContainsKey(k) && trees[k] >= tree);
        var down = Enumerable.Range(row + 1, rowSize - row)
            .Select(rIdx => (rIdx, col))
            .Any(k => trees.ContainsKey(k) && trees[k] >= tree);
        if (left && right && up && down)
            invisible += 1;
    }
    return trees.Count - invisible;
}

int P2(IList<string> lines)
{
    var (trees, rowSize, colSize) = Parse(lines);
    var score = 0;
    foreach (var ((row, col), tree) in trees)
    {
        var left = Enumerable.Range(0, col).Reverse()
            .Select(cIdx => (row, cIdx))
            .TakeWhile(k => trees.ContainsKey(k) && trees[k] < tree)
            .Count();
        var right = Enumerable.Range(col + 1, colSize - col)
            .Select(cIdx => (row, cIdx))
            .TakeWhile(k => trees.ContainsKey(k) && trees[k] < tree)
            .Count();
        var up = Enumerable.Range(0, row).Reverse()
            .Select(rIdx => (rIdx, col))
            .TakeWhile(k => trees.ContainsKey(k) && trees[k] < tree)
            .Count();
        var down = Enumerable.Range(row + 1, rowSize - row)
            .Select(rIdx => (rIdx, col))
            .TakeWhile(k => trees.ContainsKey(k) && trees[k] < tree)
            .Count();
        // +1 if it cannot see thru the edge
        left = left == col ? left : left + 1;
        right = col + right == colSize - 1 ? right : right + 1;
        up = up == row ? up : up + 1;
        down = down + row == rowSize - 1 ? down : down + 1;
        score = score > left * right * up * down ?
            score : left * right * up * down;
    }
    return score;
}

(Dictionary<(int row, int col), int> trees, int rowSize, int colSize)
Parse(IList<string> lines) =>
    (
        lines.SelectMany((str, row) =>
            str.Select((c, col) => ((row, col), (int)char.GetNumericValue(c)))
            ).ToDictionary(x => x.Item1, x => x.Item2),
        lines.Count,
        lines[0].Length
    );

var test = @"30373
25512
65332
33549
35390".Split('\n', StringSplitOptions.TrimEntries);
Debug.Assert(P1(test) == 21);
Debug.Assert(P2(test) == 8);

var input = File.ReadLines("input.txt").Select(x => x.Trim()).ToList();
Debug.Assert(P1(input) == 1820);
Debug.Assert(P2(input) == 385112);