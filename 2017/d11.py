# https://www.redblobgames.com/grids/hexagons/#coordinates-cube

dirs = {
    "n": (0, 1, -1),
    "s": (0, -1, 1),
    "ne": (1, 0, -1),
    "sw": (-1, 0, 1),
    "se": (1, -1, 0),
    "nw": (-1, 1, 0),
}


def p1(line: str):
    pos = (0, 0, 0)
    for word in line.strip().split(","):
        delta = dirs[word]
        pos = (pos[0] + delta[0], pos[1] + delta[1], pos[2] + delta[2])
    return sum(map(abs, pos)) // 2


def p2(line: str):
    pos = (0, 0, 0)
    dist = 0
    for word in line.strip().split(","):
        delta = dirs[word]
        pos = (pos[0] + delta[0], pos[1] + delta[1], pos[2] + delta[2])
        curr_dist = sum(map(abs, pos)) // 2
        if curr_dist > dist:
            dist = curr_dist
    return dist


assert p1("ne,ne,ne") == 3
assert p1("ne,ne,sw,sw") == 0
assert p1("ne,ne,s,s") == 2
assert p1("se,sw,se,sw,sw") == 3

with open("d11.txt", "r") as f:
    input = f.readline()
    assert p1(input) == 670
    assert p2(input) == 1426
