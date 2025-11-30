from typing import Tuple
import networkx as nx


def make_grid(depth: int, target: Tuple[int, int]):
    grid = dict()
    for x in range(target[0] + 1):
        for y in range(target[1] + 1):
            if (x == 0 and y == 0) or (x == target[0] and y == target[1]):
                geo = 0
            elif x == 0:
                geo = y * 48271
            elif y == 0:
                geo = x * 16807
            else:
                geo = grid[(x - 1, y)][0] * grid[(x, y - 1)][0]
            erosion = (geo + depth) % 20183
            risk = erosion % 3
            grid[(x, y)] = (erosion, risk)
    return {pt: risk for pt, (_e, risk) in grid.items()}


def p1(depth: int, target: Tuple[int, int]):
    grid = make_grid(depth, target)
    return sum(grid.values())


assert p1(510, (10, 10)) == 114
assert p1(3339, (10, 715)) == 7915


def p2(depth: int, target: Tuple[int, int]):
    corner = (target[0] + 100, target[1] + 100)
    grid = make_grid(depth, corner)
    graph = nx.Graph()
    rocky, wet, narrow = 0, 1, 2
    torch, gear, neither = 0, 1, 2
    reg_items = {rocky: (torch, gear), wet: (gear, neither), neither: (torch, neither)}
    deltas = [(0, 1), (0, -1), (1, 0), (-1, 0)]

    for x, y in grid.keys():
        items = reg_items[grid[(x, y)]]
        graph.add_edge((x, y, items[0]), (x, y, items[1]), weight=7)
        for dx, dy in deltas:
            new_x = x + dx
            new_y = y + dy
            if (new_x, new_y) in grid:
                nitems = reg_items[grid[(new_x, new_y)]]
                for item in set(items).intersection(set(nitems)):
                    graph.add_edge((x, y, item), (new_x, new_y, item), weight=1)

    return nx.dijkstra_path_length(graph, (0, 0, torch), (target[0], target[1], torch))


# assert p2(510, (10, 10)) == 45, f"{p2(510, (10, 10))}" # 48 ??
assert p2(3339, (10, 715)) == 980
