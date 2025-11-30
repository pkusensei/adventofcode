import numpy as np


def to_array(line: str):
    return np.array([[c == "#" for c in l] for l in line.strip().split("/")])


def parse(lines: list[str]):
    rules = dict()
    for line in lines:
        k, v = map(to_array, line.strip().split(" => "))
        for a in (k, np.fliplr(k)):
            for r in range(4):
                rules[np.rot90(a, r).tobytes()] = v
    return rules


def enhance(grid: np.ndarray, rules: dict):
    size = len(grid)
    side = 2 if size % 2 == 0 else 3
    new_size = size * (side + 1) // side
    res = np.empty((new_size, new_size), dtype=bool)
    squares = range(0, size, side)
    new_squares = range(0, new_size, side + 1)

    for i, ni in zip(squares, new_squares):
        for j, nj in zip(squares, new_squares):
            square = grid[i : i + side, j : j + side]
            enhanced = rules[square.tobytes()]
            res[ni : ni + side + 1, nj : nj + side + 1] = enhanced
    return res


def solve(lines: list[str], turns: int):
    start = ".#./..#/###"
    rules = parse(lines)
    grid = to_array(start)
    for _i in range(turns):
        grid = enhance(grid, rules)
    return int(grid.sum())


test = ["../.# => ##./#../...", ".#./..#/### => #..#/..../..../#..#"]
assert solve(test, 2) == 12

with open("d21.txt", "r") as f:
    input = f.readlines()
    assert solve(input, 5) == 125
    assert solve(input, 18) == 1782917
