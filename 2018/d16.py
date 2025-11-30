from collections import defaultdict
from itertools import product
from typing import List

# inst
# opcode = inst[0]
# A = inst[1]
# B = inst[2]
# C = inst[3]


def addr(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] + registers[inst[2]]
    return registers


def addi(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] + inst[2]
    return registers


def mulr(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] * registers[inst[2]]
    return registers


def muli(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] * inst[2]
    return registers


def banr(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] & registers[inst[2]]
    return registers


def bani(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] & inst[2]
    return registers


def borr(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] | registers[inst[2]]
    return registers


def bori(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]] | inst[2]
    return registers


def setr(inst: List[int], registers: List[int]):
    registers[inst[3]] = registers[inst[1]]
    return registers


def seti(inst: List[int], registers: List[int]):
    registers[inst[3]] = inst[1]
    return registers


def gtir(inst: List[int], registers: List[int]):
    registers[inst[3]] = 1 if inst[1] > registers[inst[2]] else 0
    return registers


def gtri(inst: List[int], registers: List[int]):
    registers[inst[3]] = 1 if registers[inst[1]] > inst[2] else 0
    return registers


def gtrr(inst: List[int], registers: List[int]):
    registers[inst[3]] = 1 if registers[inst[1]] > registers[inst[2]] else 0
    return registers


def eqir(inst: List[int], registers: List[int]):
    registers[inst[3]] = 1 if inst[1] == registers[inst[2]] else 0
    return registers


def eqri(inst: List[int], registers: List[int]):
    registers[inst[3]] = 1 if registers[inst[1]] == inst[2] else 0
    return registers


def eqrr(inst: List[int], registers: List[int]):
    registers[inst[3]] = 1 if registers[inst[1]] == registers[inst[2]] else 0
    return registers


opcodes = [
    addr,
    addi,
    mulr,
    muli,
    banr,
    bani,
    borr,
    bori,
    setr,
    seti,
    gtir,
    gtri,
    gtrr,
    eqir,
    eqri,
    eqrr,
]


def parse_sect(lines: List[str]) -> List[List[List[int]]]:
    sects = list()
    sect = list()
    for line in lines:
        line = line.strip()
        if not line:
            continue
        if line.startswith("Before"):
            sect.append(list(map(int, line.split("[")[1].rstrip("]").split(", "))))
        elif line.startswith("After"):
            sect.append(list(map(int, line.split("[")[1].rstrip("]").split(", "))))
            sects.append(sect[:])
            sect.clear()
        else:
            inst = list(map(int, line.split()))
            sect.append(inst)
    return sects


def p1(samples: List[str]):
    sects = parse_sect(samples)
    count = 0
    for sect in sects:
        op_count = len([op for op in opcodes if op(sect[1], sect[0][:]) == sect[2]])
        if op_count >= 3:
            count += 1
    return count


test = ["Before: [3, 2, 1, 1]", "9 2 1 2", "After:  [3, 2, 2, 1]"]
assert p1(test) == 1

lines = open("d16p1.txt", "r").readlines()
assert p1(lines) == 618


def find_codes(input: List[str]):
    sects = parse_sect(input)
    num_ops = defaultdict(set)
    op_map = dict()
    for op in opcodes:
        for sect in sects:
            if op(sect[1], sect[0][:]) == sect[2]:
                num_ops[sect[1][0]].add(op)

    while True:
        for num, ops in num_ops.items():
            if len(ops) == 1:
                op_map[num] = ops
        for kv1, kv2 in product(op_map.items(), num_ops.items()):
            if kv1[0] != kv2[0]:
                kv2[1].difference_update(kv1[1])
        if len(op_map) == len(opcodes):
            break

    for num in op_map:
        op_map[num] = op_map[num].pop()
    return op_map


def p2(samples: List[str], program: List[str]):
    codes = find_codes(samples)
    registers = [0, 0, 0, 0]
    for line in program:
        nums = line.strip().split()
        inst = list(map(int, nums))
        registers = codes[inst[0]](inst, registers)
    return registers[0]


insts = open("d16p2.txt", "r").readlines()
assert p2(lines, insts) == 514
