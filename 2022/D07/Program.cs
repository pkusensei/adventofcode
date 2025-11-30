using System.Diagnostics;

int P1(IEnumerable<string> lines) => Parse(lines)
    .Select(e => e.Size <= 100000 ? e.Size : 0)
    .Sum();

int P2(IEnumerable<string> lines)
{
    var dirs = Parse(lines);
    var used = dirs[0].Size;
    var need = 30000000 - (70000000 - used);
    // this ! is very un-intuitive
    return dirs.Where(d => d.Size > need)!.MinBy(d => d.Size)!.Size;
}

List<Entry> Parse(IEnumerable<string> lines)
{
    var entries = new List<Entry>();
    Entry current = new Entry("/", 0);
    entries.Add(current);
    var stack = new Stack<Entry>();
    stack.Push(current);

    foreach (var line in lines.Skip(1))
    {
        if (line.StartsWith("$ cd"))
        {
            var name = line.Split(' ').Last();
            if (name == "..")
            {
                stack.Pop();
                current = stack.Peek();
            }
            else // disregarding root "/" case here
            {
                current = current.Entries.First(e => e.Name == name);
                stack.Push(current);
            }
        }
        else if (line.StartsWith("dir"))
        {
            var name = line.Split(' ')[1];
            var dir = new Entry(name, 0);
            entries.Add(dir);
            current.Entries.Add(dir);
        }
        else if (char.IsAsciiDigit(line[0]))
        {
            var file = line.Split(' ');
            var size = int.Parse(file[0]);
            var name = file[1];
            var entry = new Entry(name, size);
            current.Entries.Add(entry);
        }
    }
    return entries;
}

var test = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k".Split('\n', StringSplitOptions.TrimEntries);
Debug.Assert(P1(test) == 95437);
Debug.Assert(P2(test) == 24933642);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 1297683);
Debug.Assert(P2(input) == 5756764);

class Entry
{
    public Entry(string name, int size)
    {
        Name = name;
        Size = size;
        Entries = new List<Entry>();
    }

    public string Name { get; }
    public int Size
    {
        get => IsDir ? Entries.Select(e => e.Size).Sum() : _size;
        init => _size = value;
    }
    public bool IsDir { get => _size == 0; }
    public List<Entry> Entries { get; }

    private int _size;
}