from collections import defaultdict
from typing import Optional


def parse(lines: list[str]):
    pieces: dict[int, set[int]] = defaultdict(set)
    for line in lines:
        nums = list(map(int, line.strip().split("/")))
        pieces[nums[0]].add(nums[1])
        pieces[nums[1]].add(nums[0])
    return pieces


def build(pieces: dict[int, set[int]], bridge: Optional[list[tuple[int, int]]]):
    bridge = bridge if bridge is not None else [(0, 0)]
    current = bridge[-1][1]
    for b in pieces[current]:
        if not ((current, b) in bridge or (b, current) in bridge):
            new_bridge = bridge + [(current, b)]
            yield new_bridge
            yield from build(pieces, new_bridge)


def solve(lines: list[str]):
    pieces = parse(lines)
    res = list()
    for bridge in build(pieces, None):
        res.append((len(bridge), sum(x + y for x, y in bridge)))
    return res


def p1(lines: list[str]):
    res = solve(lines)
    res.sort(key=lambda x: x[1])
    return res[-1][1]


def p2(lines: list[str]):
    res = solve(lines)
    res.sort()
    return res[-1][1]


test = ["0/2", "2/2", "2/3", "3/4", "3/5", "0/1", "10/1", "9/10"]
assert p1(test) == 31
assert p2(test) == 19

with open("d24.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 1656
    assert p2(input) == 1642
