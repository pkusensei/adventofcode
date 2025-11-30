from typing import List, Tuple
import networkx as nx


def parse(lines: List[str]):
    for line in lines:
        nums = line.split(",")
        x, y, z, t = int(nums[0]), int(nums[1]), int(nums[2]), int(nums[3])
        yield (x, y, z, t)


def distance(pt1: Tuple[int, int, int, int], pt2: Tuple[int, int, int, int]):
    return sum(abs(c1 - c2) for c1, c2 in zip(pt1, pt2))


def solve(lines: List[str]):
    graph = nx.Graph()
    for pt1 in parse(lines):
        for pt2 in parse(lines):
            if distance(pt1, pt2) <= 3:
                graph.add_edge(pt1, pt2)
    return nx.number_connected_components(graph)


test1 = [
    "0,0,0,0",
    "3,0,0,0",
    "0,3,0,0",
    "0,0,3,0",
    "0,0,0,3",
    "0,0,0,6",
    "9,0,0,0",
    "12,0,0,0",
]
assert solve(test1) == 2

test2 = [
    "-1,2,2,0",
    "0,0,2,-2",
    "0,0,0,-2",
    "-1,2,0,0",
    "-2,-2,-2,2",
    "3,0,2,-1",
    "-1,3,2,2",
    "-1,0,-1,0",
    "0,2,1,-2",
    "3,0,0,0",
]
assert solve(test2) == 4

test3 = [
    "1,-1,0,1",
    "2,0,-1,0",
    "3,2,-1,0",
    "0,0,3,1",
    "0,0,-1,-1",
    "2,3,-2,0",
    "-2,2,0,0",
    "2,-2,0,-1",
    "1,-1,0,-1",
    "3,2,0,2",
]
assert solve(test3) == 3

test4 = [
    "1,-1,-1,-2",
    "-2,-2,0,1",
    "0,2,1,3",
    "-2,3,-2,1",
    "0,2,3,-2",
    "-1,-1,1,-2",
    "0,-2,-1,0",
    "-2,2,3,-1",
    "1,2,2,0",
    "-1,-2,0,-2",
]
assert solve(test4) == 8

input = open("d25.txt", "r").readlines()
assert solve(input) == 399
