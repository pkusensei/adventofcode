from collections import defaultdict


def cond(registers: dict[str, int], name: str, op: str, value: str):
    v = int(value.strip())
    if op == ">":
        return registers[name] > v
    elif op == "<":
        return registers[name] < v
    elif op == ">=":
        return registers[name] >= v
    elif op == "<=":
        return registers[name] <= v
    elif op == "==":
        return registers[name] == v
    elif op == "!=":
        return registers[name] != v
    else:
        raise ValueError(f"Invalid operator '{op}'")


def operate(registers: dict[str, int], name: str, op: str, value: str):
    v = int(value.strip())
    if op == "inc":
        registers[name] += v
    elif op == "dec":
        registers[name] -= v
    else:
        raise ValueError(f"Invalid operator '{op}'")


def p1(lines: list[str]):
    registers: defaultdict[str, int] = defaultdict(int)
    for line in lines:
        words = line.split()
        if cond(registers, words[4], words[5], words[6]):
            operate(registers, words[0], words[1], words[2])
    return max(registers.values())


def p2(lines: list[str]):
    registers: defaultdict[str, int] = defaultdict(int)
    res = 0
    for line in lines:
        words = line.split()
        if cond(registers, words[4], words[5], words[6]):
            operate(registers, words[0], words[1], words[2])
        if max(registers.values()) > res:
            res = max(registers.values())
    return res


test = [
    "b inc 5 if a > 1",
    "a inc 1 if b < 5",
    "c dec -10 if a >= 1",
    "c inc -20 if c == 10",
]
assert p1(test) == 1
assert p2(test) == 10

with open("d08.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 5849
    assert p2(input) == 6702
