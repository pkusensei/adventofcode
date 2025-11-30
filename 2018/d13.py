from collections import defaultdict
from typing import Dict, List, Tuple


class Cart:
    def __init__(self, pos: complex, dir: complex):
        self.pos = pos
        self.dir = dir
        self.turn_intersection = 0
        self.dead = False

    def move(self):
        self.pos += self.dir

    def kill(self):
        self.dead = True

    def turn(self, path: str):
        if not path:
            return
        if path == "\\":
            if self.dir.real == 0:
                self.dir *= -1j
            else:
                self.dir *= 1j
        elif path == "/":
            if self.dir.real == 0:
                self.dir *= 1j
            else:
                self.dir *= -1j
        elif path == "+":
            self.dir *= -1j * 1j ** self.turn_intersection
            self.turn_intersection = (self.turn_intersection + 1) % 3


def parse(lines: List[str]) -> Tuple[Dict[complex, str], List[Cart]]:
    tracks = defaultdict(str)
    carts = list()
    dirs = {"^": -1j, "v": 1j, "<": -1, ">": 1}
    for y, line in enumerate(lines):
        for x, c in enumerate(line.rstrip()):
            if c in "^v<>":
                dir = dirs[c]
                carts.append(Cart(x + y * 1j, dir))
            if c in "\\/+":
                tracks[(x + y * 1j)] = c
    return tracks, carts


def p1(lines: List[str]):
    tracks, carts = parse(lines)
    while True:
        carts.sort(key=lambda c: (c.pos.imag, c.pos.real))
        for i, c in enumerate(carts):
            c.move()
            if any(c2.pos == c.pos for i2, c2 in enumerate(carts) if i2 != i):
                return str(int(c.pos.real)) + "," + str(int(c.pos.imag))
            path = tracks[c.pos]
            c.turn(path)


sample1 = open("d13t1.txt", "r").readlines()
assert p1(sample1) == "7,3"

input = open("d13.txt", "r").readlines()
assert p1(input) == "83,121"


def p2(lines: List[str]):
    tracks, carts = parse(lines)
    while len(carts) > 1:
        carts.sort(key=lambda c: (c.pos.imag, c.pos.real))
        for i, c in enumerate(carts):
            if c.dead:
                continue
            c.move()
            for i2, c2 in enumerate(carts):
                if i2 != i and c2.pos == c.pos and not c2.dead:
                    c.kill()
                    c2.kill()
                    break
            if c.dead:
                continue
            path = tracks[c.pos]
            c.turn(path)
        carts = [c for c in carts if not c.dead]
    assert len(carts) == 1, "Invalid input: No cart left"
    c = carts[0]
    return str(int(c.pos.real)) + "," + str(int(c.pos.imag))


sample2 = open("d13t2.txt", "r").readlines()
assert p2(sample2) == "6,4"

assert p2(input) == "102,144"
