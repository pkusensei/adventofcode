from collections import defaultdict
import multiprocessing
import multiprocessing.pool
import queue


def valueof(v: str, registers: dict[str, int]):
    if v.isalpha():
        return registers[v]
    else:
        return int(v)


def p1(insts: list[str]):
    registers = defaultdict(int)
    ip = 0
    frequency = 0
    while True:
        inst = insts[ip].strip()
        if inst.startswith("snd"):
            reg = inst.split()[1]
            frequency = registers[reg]
            ip += 1
        elif inst.startswith("set"):
            reg = inst.split()[1]
            v = inst.split()[2]
            registers[reg] = valueof(v, registers)
            ip += 1
        elif inst.startswith("add"):
            reg = inst.split()[1]
            v = inst.split()[2]
            registers[reg] += valueof(v, registers)
            ip += 1
        elif inst.startswith("mul"):
            reg = inst.split()[1]
            v = inst.split()[2]
            registers[reg] *= valueof(v, registers)
            ip += 1
        elif inst.startswith("mod"):
            reg = inst.split()[1]
            v = inst.split()[2]
            registers[reg] %= valueof(v, registers)
            ip += 1
        elif inst.startswith("rcv"):
            reg = inst.split()[1]
            if registers[reg] != 0:
                return frequency
            ip += 1
        elif inst.startswith("jgz"):
            reg = inst.split()[1]
            v = inst.split()[2]
            if registers[reg] > 0:
                ip += valueof(v, registers)
            else:
                ip += 1


def proc(id: int, inq, outq, insts: list[str]):
    registers = defaultdict(int)
    registers["p"] = id
    ip = 0
    count = 0

    while 0 <= ip < len(insts):
        inst = insts[ip].strip().split()
        if inst[0] == "snd":
            reg = inst[1]
            outq.put(registers[reg])
            count += 1
        elif inst[0] == "set":
            reg = inst[1]
            v = inst[2]
            registers[reg] = valueof(v, registers)
        elif inst[0] == "add":
            reg = inst[1]
            v = inst[2]
            registers[reg] += valueof(v, registers)
        elif inst[0] == "mul":
            reg = inst[1]
            v = inst[2]
            registers[reg] *= valueof(v, registers)
        elif inst[0] == "mod":
            reg = inst[1]
            v = inst[2]
            registers[reg] %= valueof(v, registers)
        elif inst[0] == "rcv":
            reg = inst[1]
            try:
                registers[reg] = inq.get(timeout=5)
            except queue.Empty:
                return count
        elif inst[0] == "jgz":
            reg = inst[1]
            v = inst[2]
            if registers[reg] > 0:
                ip += valueof(v, registers)
                continue
        ip += 1
    return count


def p2(insts: list[str]):
    pool = multiprocessing.pool.ThreadPool(processes=2)

    q1 = multiprocessing.Queue()
    q2 = multiprocessing.Queue()

    r1 = pool.apply_async(proc, (0, q1, q2, insts))
    r2 = pool.apply_async(proc, (1, q2, q1, insts))

    r1.get()
    return r2.get()


test = [
    "set a 1",
    "add a 2",
    "mul a a",
    "mod a 5",
    "snd a",
    "set a 0",
    "rcv a",
    "jgz a -1",
    "set a 1",
    "jgz a -2",
]
assert p1(test) == 4

with open("d18.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 4601
    # assert p2(input) == 6858

# What's the difference b/t this and the above?
PROGRAM = input


def run(ident, inqueue, outqueue):
    regs = defaultdict(int)
    regs["p"] = ident

    def val(v):
        try:
            return int(v)
        except ValueError:
            return regs[v]

    pc = 0
    count = 0

    while 0 <= pc < len(PROGRAM):
        cmd = PROGRAM[pc].split()
        if cmd[0] == "snd":
            outqueue.put(val(cmd[1]))
            count += 1
        elif cmd[0] == "set":
            regs[cmd[1]] = val(cmd[2])
        elif cmd[0] == "add":
            regs[cmd[1]] += val(cmd[2])
        elif cmd[0] == "mul":
            regs[cmd[1]] *= val(cmd[2])
        elif cmd[0] == "mod":
            regs[cmd[1]] %= val(cmd[2])
        elif cmd[0] == "rcv":
            try:
                regs[cmd[1]] = inqueue.get(timeout=5)
            except queue.Empty:
                return count
        elif cmd[0] == "jgz":
            if val(cmd[1]) > 0:
                pc += val(cmd[2])
                continue
        pc += 1

    return count


pool = multiprocessing.pool.ThreadPool(processes=2)

q1 = multiprocessing.Queue()
q2 = multiprocessing.Queue()

r1 = pool.apply_async(run, (0, q1, q2))
r2 = pool.apply_async(run, (1, q2, q1))

r1.get()
assert r2.get() == 6858
