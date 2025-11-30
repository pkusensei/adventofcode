using System.Diagnostics;

int Solve(string line, int count) => count + 1 + line.Select((_, idx) => idx)
    .First(
        idx => line.Skip(idx + 1).Take(count).Distinct().Count() == count
    );

int P1(string line) => Solve(line, 4);
int P2(string line) => Solve(line, 14);

var test = "mjqjpqmgbljsphdztnvjfqwrcgsmlb";
Debug.Assert(P1(test) == 7);
Debug.Assert(P2(test) == 19);

var input = File.ReadAllText("input.txt");
Debug.Assert(P1(input) == 1238);
Debug.Assert(P2(input) == 3037);