from collections import deque
from typing import List, Tuple
import networkx as nx


class Unit:
    def __init__(self, unit_type: str, x: int, y: int, damage: int):
        self.type = unit_type
        self.x = x
        self.y = y
        self.hp = 200
        self.alive = True
        self.damage = damage

    def attack(self, damage: int):
        if self.alive:
            self.hp -= damage
            if self.hp <= 0:
                self.alive = False


def neighbors(x: int, y: int):
    return [(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)]


def parse(lines: List[str], damage: int):
    grid = [list(line) for line in lines]
    units = list()
    for y, row in enumerate(grid):
        for x, c in enumerate(row):
            if c in "GE":
                units.append(Unit(c, x, y, 3 if c == "G" else damage))
                grid[y][x] = "."
    graph = nx.Graph()
    for y, row in enumerate(grid):
        for x, c in enumerate(row):
            if c == ".":
                for (newx, newy) in neighbors(x, y):
                    if (
                        0 <= newx < len(row)
                        and 0 <= newy < len(grid)
                        and grid[newy][newx] == "."
                    ):
                        graph.add_edge((x, y), (newx, newy))
    return graph, units


def find_closest(
    graph,
    excluded: List[Tuple[int, int]],
    start: Tuple[int, int],
    targets: List[Tuple[int, int]],
):
    if start not in graph:
        return [], None
    seen = set()
    queue = deque([(start, 0)])
    closest = []
    dist = None
    while len(queue) > 0:
        pos, d = queue.popleft()
        if dist is not None and d > dist:
            return closest, dist
        if pos in excluded or pos in seen:
            continue
        seen.add(pos)
        if pos in targets:
            dist = d
            closest.append(pos)
        for neighbor in graph.neighbors(pos):
            if neighbor not in seen:
                queue.append((neighbor, d + 1))
    return closest, dist


def solve(graph, units: List[Unit], elf_alive: bool):
    round = 0
    while True:
        order = sorted(units, key=lambda u: (u.y, u.x))
        for idx, unit in enumerate(order):
            if not unit.alive:
                continue

            enemies = [e for e in units if e.type != unit.type and e.alive]
            enemy_pos = [(e.x, e.y) for e in enemies]
            nearby = neighbors(unit.x, unit.y)
            enemies_in_range = [e for e in nearby if e in enemy_pos]

            if len(enemies_in_range) == 0:
                surrouding = list()
                for e in enemies:
                    surrouding.extend(neighbors(e.x, e.y))
                surrouding = [u for u in surrouding if u in graph]

                excluded = [(e.x, e.y) for e in units if e.alive and e != unit]
                closest, dist = find_closest(
                    graph, excluded, (unit.x, unit.y), surrouding
                )

                if len(closest) > 0:
                    target = min(closest, key=lambda u: (u[1], u[0]))
                    for u in sorted(nearby, key=lambda x: (x[1], x[0])):
                        _c, d = find_closest(graph, excluded, u, [target])
                        if dist - 1 == d:
                            unit.x, unit.y = u
                            break
                enemies_in_range = [
                    e for e in neighbors(unit.x, unit.y) if e in enemy_pos
                ]

            if len(enemies_in_range) > 0:
                enemies = [e for e in enemies if (e.x, e.y) in enemies_in_range]
                low_health = min(enemies, key=lambda e: (e.hp, e.y, e.x))
                low_health.attack(unit.damage)

                if elf_alive and low_health.type == "E" and not low_health.alive:
                    return False, None
                alive = set(u.type for u in units if u.alive)
                if len(alive) == 1:
                    if len(order) - 1 == idx:
                        round += 1
                    return True, round * sum(u.hp for u in units if u.alive)
        round += 1


def p1(lines: List[str]):
    graph, units = parse(lines, 3)
    _, res = solve(graph, units, False)
    return res


def p2(lines: List[str]):
    damage = 4
    while True:
        graph, units = parse(lines, damage)
        alive, res = solve(graph, units, True)
        if alive:
            return res
        damage += 1


test1 = ["#######", "#.G...#", "#...EG#", "#.#.#G#", "#..G#E#", "#.....#", "#######"]
assert p1(test1) == 27730

test2 = ["#######", "#G..#E#", "#E#E.E#", "#G.##.#", "#...#E#", "#...E.#", "#######"]
assert p1(test2) == 36334

test3 = ["#######", "#E..EG#", "#.#G.E#", "#E.##E#", "#G..#.#", "#..E#.#", "#######"]
assert p1(test3) == 39514

test4 = ["#######", "#E.G#.#", "#.#G..#", "#G.#.G#", "#G..#.#", "#...E.#", "#######"]
assert p1(test4) == 27755

test5 = ["#######", "#.E...#", "#.#..G#", "#.###.#", "#E#G#G#", "#...#G#", "#######"]
assert p1(test5) == 28944

test6 = [
    "#########",
    "#G......#",
    "#.E.#...#",
    "#..##..G#",
    "#...##..#",
    "#...#...#",
    "#.G...G.#",
    "#.....G.#",
    "#########",
]
assert p1(test6) == 18740

input = open("d15.txt", "r").readlines()
assert p1(input) == 201123


assert p2(test1) == 4988
assert p2(test3) == 31284
# assert p2(test4) == 3478, f"{p2(test4)}"
# assert p2(test5) == 6474, f"{p2(test5)}"
# assert p2(test6) == 1140, f"{p2(test6)}"
assert p2(input) == 54188
