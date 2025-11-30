def parse(lines: list[str]):
    res: dict[int, int] = dict()
    for line in lines:
        depth, height = line.strip().split(": ")
        res[int(depth)] = int(height)
    return res


def p1(lines: list[str]):
    firewall = parse(lines)
    return sum(
        depth * height
        for depth, height in firewall.items()
        if depth % (2 * (height - 1)) == 0
    )


def p2(lines: list[str]):
    firewall = parse(lines)
    res = 0
    while True:
        res += 1
        if any(
            (res + depth) % (2 * (height - 1)) == 0
            for depth, height in firewall.items()
        ):
            continue
        return res


test = ["0: 3", "1: 2", "4: 4", "6: 4"]
assert p1(test) == 24
assert p2(test) == 10

with open("d13.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 1960
    assert p2(input) == 3903378
