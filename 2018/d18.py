from typing import Dict, List
from itertools import product

deltas = list(product((-1, 0, 1), (-1, 0, 1)))
deltas.remove((0, 0))


def parse(lines: List[str]):
    area = dict()
    for y, line in enumerate(lines):
        for x, c in enumerate(line):
            area[(x, y)] = c
    return area


def count_adjacent(area: Dict, cur_x: int, cur_y: int, look_for: str):
    return len(
        [
            dx
            for (dx, dy) in deltas
            if (cur_x + dx, cur_y + dy) in area
            and area[(cur_x + dx, cur_y + dy)] == look_for
        ]
    )


def change(area):
    new_area = dict()
    for (x, y), c in area.items():
        if c == ".":
            if count_adjacent(area, x, y, "|") >= 3:
                new_area[(x, y)] = "|"
            else:
                new_area[(x, y)] = c
        elif c == "|":
            if count_adjacent(area, x, y, "#") >= 3:
                new_area[(x, y)] = "#"
            else:
                new_area[(x, y)] = c
        elif c == "#":
            if (
                count_adjacent(area, x, y, "#") > 0
                and count_adjacent(area, x, y, "|") > 0
            ):
                new_area[(x, y)] = c
            else:
                new_area[(x, y)] = "."
    return new_area


def count(area):
    wood = len([v for v in area.values() if v == "|"])
    lumber = len([v for v in area.values() if v == "#"])
    return wood, lumber


def p1(lines: List[str], minutes: int):
    area = parse(lines)
    for _m in range(minutes):
        area = change(area)
    wood, lumber = count(area)
    return wood * lumber


test = [
    ".#.#...|#.",
    ".....#|##|",
    ".|..|...#.",
    "..|#.....#",
    "#.#|||#|#|",
    "...#.||...",
    ".|....|...",
    "||...#|.#|",
    "|.||||..|.",
    "...#.|..|.",
]
assert p1(test, 10) == 1147

input = open("d18.txt", "r").readlines()
assert p1(input, 10) == 360720


def p2(lines: List[str], minutes: int):
    area = parse(lines)
    history = list()
    while True:
        area = change(area)
        if area in history:
            idx = history.index(area)
            cycle = len(history) - idx
            while (idx + 1) % cycle != minutes % cycle:
                idx += 1
            wood, lumber = count(history[idx])
            break
        else:
            history.append(area)
    return wood * lumber


assert p2(input, 1000000000) == 197276  # 596 * 331
