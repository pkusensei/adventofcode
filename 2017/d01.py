def p1(line: str):
    res = 0
    for idx in range(len(line) - 1):
        if line[idx] == line[idx + 1]:
            res += int(line[idx])
    if line[-1] == line[0]:
        res += int(line[0])
    return res


def p2(line: str):
    res = 0
    step = len(line) // 2
    for idx in range(step):
        if line[idx] == line[idx + step]:
            res += int(line[idx])
    return res * 2


assert p1("1122") == 3
assert p1("1111") == 4
assert p1("1234") == 0
assert p1("91212129") == 9

assert p2("1212") == 6
assert p2("1221") == 0
assert p2("123425") == 4
assert p2("123123") == 12
assert p2("12131415") == 4

with open("d01.txt", "r") as f:
    line = f.read()
    assert p1(line) == 1119
    assert p2(line) == 1420
