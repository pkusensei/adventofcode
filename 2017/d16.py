def p1(pros: str, insts: str):
    lst = list(pros)
    insts = insts.strip().split(",")
    for inst in insts:
        if inst[0] == "s":
            num = int(inst[1:])
            lst = lst[-num:] + lst[:-num]
        elif inst[0] == "x":
            ia, ib = map(int, inst[1:].split("/"))
            lst[ia], lst[ib] = lst[ib], lst[ia]
        elif inst[0] == "p":
            pe = inst[1:].split("/")[0]
            pb = inst[1:].split("/")[1]
            ie = lst.index(pe)
            ib = lst.index(pb)
            lst[ie], lst[ib] = lst[ib], lst[ie]
    return "".join(lst)


def p2(pros: str, insts: str, turns: int):
    seen = list()
    current = pros
    while current not in seen:
        seen.append(current)
        current = p1(current, insts)
    first = seen.index(current)
    cycle = len(seen) - first
    idx = (turns - first) % cycle + first
    return seen[idx]


assert p1("abcde", "s1,x3/4,pe/b") == "baedc"

line = "abcdefghijklmnop"
with open("d16.txt", "r") as f:
    input = f.readline()
    assert p1(line, input) == "lgpkniodmjacfbeh"
    assert p2(line, input, 1_000_000_000) == "hklecbpnjigoafmd"
