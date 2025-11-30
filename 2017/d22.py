from collections import defaultdict


def parse(lines: list[str]):
    plane = defaultdict(bool)
    mid = len(lines) // 2
    for y, line in enumerate(lines):
        for x, c in enumerate(line.strip()):
            if c == "#":
                plane[x + y * 1j] = True
    return plane, mid + mid * 1j


def p1(lines: list[str]):
    plane, pos = parse(lines)
    direction = -1j
    count = 0
    for _i in range(10000):
        if plane[pos]:
            direction *= 1j  # turn right
            plane[pos] = False
        else:
            direction *= -1j  # turn left
            plane[pos] = True
            count += 1
        pos += direction
    return count


def p2(lines: list[str]):
    orig, pos = parse(lines)
    plane = defaultdict(lambda: "C")
    for k, v in orig.items():
        if v:
            plane[k] = "I"
    direction = -1j
    count = 0
    for _i in range(10000000):
        if plane[pos] == "C":
            plane[pos] = "W"
            direction *= -1j
        elif plane[pos] == "W":
            plane[pos] = "I"
            count += 1
        elif plane[pos] == "I":
            plane[pos] = "F"
            direction *= 1j
        elif plane[pos] == "F":
            plane[pos] = "C"
            direction *= -1
        pos += direction
    return count


test = ["..#", "#..", "..."]
assert p1(test) == 5587
assert p2(test) == 2511944

with open("d22.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 5575
    assert p2(input) == 2511991
