from typing import List, Tuple
from queue import PriorityQueue


def parse(lines: List[str]):
    for line in lines:
        r = line.split(">, r=")[1]
        x, y, z = line.split(">, r=")[0].split("=<")[1].split(",")
        yield (int(x), int(y), int(z)), int(r)


def distance(pt1: Tuple[int, int, int], pt2: Tuple[int, int, int]):
    return sum(abs(p1 - p2) for p1, p2 in zip(pt1, pt2))


def p1(lines: List[str]):
    pts = list()
    pos = (0, 0, 0)
    radius = 0
    for pt, r in parse(lines):
        pts.append((pt, r))
        if r > radius:
            radius = r
            pos = pt
    return len([pt for pt in pts if distance(pos, pt[0]) <= radius])


test1 = [
    "pos=<0,0,0>, r=4",
    "pos=<1,0,0>, r=1",
    "pos=<4,0,0>, r=3",
    "pos=<0,2,0>, r=1",
    "pos=<0,5,0>, r=3",
    "pos=<0,0,3>, r=1",
    "pos=<1,1,1>, r=1",
    "pos=<1,1,2>, r=1",
    "pos=<1,3,1>, r=1",
]
assert p1(test1) == 7

input = open("d23.txt", "r").readlines()
assert p1(input) == 602


def p2(lines: List[str]):
    pts = parse(lines)
    queue = PriorityQueue()
    for (x, y, z), r in pts:
        dist = abs(x) + abs(y) + abs(z)
        queue.put((max(0, dist - r), 1))
        queue.put((dist + r + 1, -1))
    count = 0
    max_count = 0
    res = 0
    while not queue.empty():
        dist, e = queue.get()
        count += e
        if count > max_count:
            max_count = count
            res = dist
    return res


test2 = [
    "pos=<10,12,12>, r=2",
    "pos=<12,14,12>, r=2",
    "pos=<16,12,12>, r=4",
    "pos=<14,14,14>, r=6",
    "pos=<50,50,50>, r=200",
    "pos=<10,10,10>, r=5",
]
assert p2(test2) == 36
assert p2(input) == 110620102
