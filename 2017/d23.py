from collections import defaultdict


def valueof(v: str, regs: dict):
    if v.isalpha():
        return regs[v]
    return int(v)


def p1(lines: list[str]):
    regs = defaultdict(int)
    ip = 0
    count = 0
    while 0 <= ip < len(lines):
        inst = lines[ip].strip().split()
        if inst[0] == "set":
            regs[inst[1]] = valueof(inst[2], regs)
            ip += 1
        elif inst[0] == "sub":
            regs[inst[1]] -= valueof(inst[2], regs)
            ip += 1
        elif inst[0] == "mul":
            regs[inst[1]] *= valueof(inst[2], regs)
            ip += 1
            count += 1
        elif inst[0] == "jnz":
            reg = valueof(inst[1], regs)
            if reg != 0:
                ip += valueof(inst[2], regs)
            else:
                ip += 1
    return count


def p2():
    # a = 1 set b = 107900, c = 124900
    # if d * e == b: f = 0
    # if f == 0: h += 1
    # while b <= g <= c: b += 17
    count = 0
    for num in range(107900, 124900 + 1, 17):
        for i in range(2, num):
            if num % i == 0:
                count += 1
                break
    return count


with open("d23.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 5929
    assert p2() == 907
