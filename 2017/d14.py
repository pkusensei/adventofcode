import d10


def p1(line: str):
    res = 0
    for i in range(128):
        kh = d10.p2(f"{line}-{i}")
        binary_str = f"{int(kh, 16):0128b}"
        res += sum(map(int, binary_str))
    return res


def p2(line: str):
    squares: list[list[int]] = []
    for i in range(128):
        kh = d10.p2(f"{line}-{i}")
        binary_str = f"{int(kh, 16):0128b}"
        squares.append(list(map(int, binary_str)))
    seen: set[tuple[int, int]] = set()

    def dfs(x: int, y: int):
        if (x, y) in seen:
            return
        if squares[x][y] == 0:
            return
        seen.add((x, y))
        if x > 0:
            dfs(x - 1, y)
        if y > 0:
            dfs(x, y - 1)
        if x < 127:
            dfs(x + 1, y)
        if y < 127:
            dfs(x, y + 1)

    count = 0
    for x in range(128):
        for y in range(128):
            if squares[x][y] == 0:
                continue
            if (x, y) in seen:
                continue
            count += 1
            dfs(x, y)
    return count


assert p1("flqrgnkx") == 8108
assert p2("flqrgnkx") == 1242

assert p1("ljoxqyyw") == 8316
assert p2("ljoxqyyw") == 1074
