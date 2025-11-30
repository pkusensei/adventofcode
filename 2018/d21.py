from typing import List
import d16


def p1(lines: List[str], registers: List[int]):
    ip_idx = int(lines[0].split()[1])
    insts = lines[1:]
    ip = registers[ip_idx]
    while True:
        line = insts[ip]
        fname = line.split()[0]
        f = getattr(d16, fname)
        values = list(map(int, line.split()[1:]))
        values.insert(0, 0)
        registers = f(values, registers)
        ip = registers[ip_idx] + 1

        # ip == 28 i.e eqrr 4 0 2
        # compares reg[4] against reg[0]
        # set reg[0] = reg[4] here to break out
        if ip == 28:
            break

        if ip >= len(insts):
            break
        registers[ip_idx] = ip
    return registers[4]


input = open("d21.txt", "r").readlines()
assert p1(input, [0, 0, 0, 0, 0, 0]) == 7129803

# basically decompile the instructions?
def p2(magic_number: int):
    seen = set()
    c = 0
    last_unique_c = -1

    while True:
        a = c | 65536
        c = magic_number

        while True:
            c = (((c + (a & 255)) & 16777215) * 65899) & 16777215

            if 256 > a:
                if c not in seen:
                    seen.add(c)
                    last_unique_c = c
                    break
                else:
                    return last_unique_c
            else:
                a //= 256


assert p2(int(input[8].split()[1])) == 12284643
