from typing import List, Set


def parse(lines: List[str]):
    init = lines[0].split(": ")[1].strip()
    rules = set()

    for line in lines[2:]:
        if line.strip()[-1] == '#':
            rules.add(line[:5])
    return init, rules


def next(current: Set[int], rules: Set[str]):
    start = min(current)
    end = max(current)
    res = set()

    for i in range(start-3, end+4):
        pattern = ''.join(
            '#' if i+k in current else '.' for k in [-2, -1, 0, 1, 2])
        if pattern in rules:
            res.add(i)
    return res


def p1(lines: List[str]):
    init, rules = parse(lines)
    current = set(i for i, c in enumerate(init) if c == '#')
    for _i in range(20):
        current = next(current, rules)
    return sum(current)


sample = ["initial state: #..#.#..##......###...###",
          "",
          "...## => #",
          "..#.. => #",
          ".#... => #",
          ".#.#. => #",
          ".#.## => #",
          ".##.. => #",
          ".#### => #",
          "#.#.# => #",
          "#.### => #",
          "##.#. => #",
          "##.## => #",
          "###.. => #",
          "###.# => #",
          "####. => #"]
assert p1(sample) == 325

input = open("d12.txt").readlines()
assert p1(input) == 2767


def p2(lines: List[str]):
    init, rules = parse(lines)
    current = set(i for i, c in enumerate(init) if c == '#')
    last = 0
    s = 0
    for _i in range(2000):
        last = s
        current = next(current, rules)
        s = sum(current)
    return sum(current)+(50000000000-2000)*(s-last)


assert p2(input) == 2650000001362
