from collections import Counter, defaultdict
from typing import List, Tuple


def parse_coords(lines: List[str]):
    points = map(lambda x: x.split(", "), lines)
    return list(map(lambda x: (int(x[0]), int(x[1])), points))


def get_corners(coords: List[Tuple[int, int]]):
    xs = list(map(lambda v: v[0], coords))
    ys = list(map(lambda v: v[1], coords))
    xmin = min(xs)
    xmax = max(xs)
    ymin = min(ys)
    ymax = max(ys)
    return xmin, xmax, ymin, ymax


def distance(pt1: Tuple[int, int], pt2: Tuple[int, int]):
    return abs(pt1[0]-pt2[0])+abs(pt1[1]-pt2[1])


def get_all_points(coords: List[Tuple[int, int]]):
    xmin, xmax, ymin, ymax = get_corners(coords)
    for x in range(xmin, xmax+1):
        for y in range(ymin, ymax+1):
            yield x, y


def get_all_dists(coords: List[Tuple[int, int]]):
    for x, y in get_all_points(coords):
        for coord_x, coord_y in coords:
            dist = distance((x, y), (coord_x, coord_y))
            yield ((x, y), (coord_x, coord_y), dist)


def p1(input: List[str]):
    coords = parse_coords(input)
    closest_coords = dict()
    closest_coord_dists = defaultdict(lambda: float("inf"))
    for (x, y), (coord_x, coord_y), dist in get_all_dists(coords):
        tmp = closest_coord_dists[(x, y)]
        if dist < tmp:
            closest_coords[(x, y)] = (coord_x, coord_y)
            closest_coord_dists[(x, y)] = dist
        elif dist == tmp and closest_coords[(x, y)] != (coord_x, coord_y):
            closest_coords[(x, y)] = None
    xmin, xmax, ymin, ymax = get_corners(coords)
    areas = defaultdict(int)
    for (x, y), coord in closest_coords.items():
        if coord is None:
            continue
        if x in (xmin, xmax) or y in (ymin, ymax):
            areas[coord] = float("-inf")
        areas[coord] += 1
    return max(areas.values())


sample = ["1, 1",
          "1, 6",
          "8, 3",
          "3, 4",
          "5, 5",
          "8, 9"]
assert p1(sample) == 17

lines = open("d06.txt", 'r').readlines()
assert p1(lines) == 4284


def p2(input: List[str], criterium: int):
    coords = parse_coords(input)
    dists = defaultdict(int)
    for (x, y), (coord_x, coord_y), dist in get_all_dists(coords):
        dists[(x, y)] += dist
    res = [d for d in dists.values() if d < criterium]
    return len(res)


assert p2(sample, 32) == 16
assert p2(lines, 10000) == 35490
