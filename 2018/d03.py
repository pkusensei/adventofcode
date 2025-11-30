from typing import List
from itertools import product
from collections import Counter


def parse_claim(claim: str):
    items = claim.split(' ')
    id = int(items[0][1:])
    x = int(items[2].split(',')[0])
    y = int(items[2].split(',')[1].strip(':'))
    [width, height] = map(int, items[3].split('x'))
    return id, x, y, width, height


def find_overlapped(claims: List[str]):
    all = Counter()
    for claim in claims:
        _id, x, y, width, height = parse_claim(claim)
        x_coords = [x+i for i in range(width)]
        y_coords = [y+i for i in range(height)]
        coords = set(product(x_coords, y_coords))
        all.update(coords)
    return set([key for key in all.keys() if all[key] > 1])


def p1(claims: List[str]):
    return len(find_overlapped(claims))


sample = ["#1 @ 1,3: 4x4",
          "#2 @ 3,1: 4x4",
          "#3 @ 5,5: 2x2"]
assert p1(sample) == 4

lines = list(map(lambda x: x.strip(), open("d03.txt").readlines()))
assert p1(lines) == 118539


def p2(claims: List[str]):
    overlapped = find_overlapped(claims)
    for claim in claims:
        id, x, y, width, height = parse_claim(claim)
        x_coords = [x+i for i in range(width)]
        y_coords = [y+i for i in range(height)]
        coords = set(product(x_coords, y_coords))
        if len(overlapped.intersection(coords)) == 0:
            return id


assert p2(sample) == 3
assert p2(lines) == 1270
