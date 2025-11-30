from collections import defaultdict
import re
import sys
from typing import List

sys.setrecursionlimit(3000)


def parse(lines: List[str]):
    clay = defaultdict(bool)
    for line in lines:
        a, b, c = map(int, re.findall("([\d]+)", line))
        if line.startswith("x="):
            for y in range(b, c + 1):
                clay[a + y * 1j] = True
        else:
            for x in range(b, c + 1):
                clay[x + a * 1j] = True
    return clay


clay = None
flowing = None
settled = None
ymin = None
ymax = None


def fill(pt: complex, direction=1j):
    flowing.add(pt)
    below, left, right = pt + 1j, pt - 1, pt + 1

    if not clay[below]:
        if below not in flowing and 1 <= below.imag <= ymax:
            fill(below)
        if below not in settled:
            return False
    left_filled = clay[left] or (left not in flowing and fill(left, -1))
    right_filled = clay[right] or (right not in flowing and fill(right, 1))

    if direction == 1j and left_filled and right_filled:
        settled.add(pt)

        while left in flowing:
            settled.add(left)
            left -= 1
        while right in flowing:
            settled.add(right)
            right += 1

    return (direction == -1 and (left_filled or clay[left])) or (
        direction == 1 and (right_filled or clay[right])
    )


def p1(lines: List[str]):
    global clay, flowing, settled, ymin, ymax
    clay = parse(lines)
    flowing = set()
    settled = set()
    ys = [pt.imag for pt in clay]
    ymin, ymax = min(ys), max(ys)
    fill(500)
    return len([pt for pt in flowing | settled if ymin <= pt.imag <= ymax])


def p2(lines: List[str]):
    global clay, flowing, settled, ymin, ymax
    clay = parse(lines)
    flowing = set()
    settled = set()
    ys = [pt.imag for pt in clay]
    ymin, ymax = min(ys), max(ys)
    fill(500)
    return len([pt for pt in settled if ymin <= pt.imag <= ymax])


test = [
    "x=495, y=2..7",
    "y=7, x=495..501",
    "x=501, y=3..7",
    "x=498, y=2..4",
    "x=506, y=1..2",
    "x=498, y=10..13",
    "x=504, y=10..13",
    "y=13, x=498..504",
]
assert p1(test) == 57

input = open("d17.txt", "r").readlines()
assert p1(input) == 37649

assert p2(test) == 29
assert p2(input) == 30112
