DELTAS: list[complex] = [1, -1, 1j, -1j]


def parse(lines: list[str]) -> dict[complex, str]:
    res = dict()
    for y, line in enumerate(lines):
        for x, c in enumerate(line):
            if c.isspace():
                continue
            res[(x + y * 1j)] = c
    return res


def solve(lines: list[str]):
    path = parse(lines)
    pos = list(filter(lambda c: c.imag == 0, path.keys()))[0]
    direction = 1j
    res = list()
    count = 1
    while True:
        pos += direction
        if pos not in path:
            break
        count += 1
        if path[pos].isalpha():
            res.append(path[pos])
        elif path[pos] in "|-":
            continue
        elif path[pos] == "+":
            prev = pos - direction
            direction = [
                delta
                for delta in DELTAS
                if pos + delta in path and (pos + delta) != prev
            ][0]

    return "".join(res), count


test = [
    r"     |          ",
    r"     |  +--+    ",
    r"     A  |  C    ",
    r" F---|----E|--+ ",
    r"     |  |  |  D ",
    r"     +B-+  +--+ ",
]
t1, t2 = solve(test)
assert t1 == "ABCDEF"
assert t2 == 38

with open("d19.txt", "r") as f:
    input = f.readlines()
    p1, p2 = solve(input)
    assert p1 == "AYRPVMEGQ"
    assert p2 == 16408
