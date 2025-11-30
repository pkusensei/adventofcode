from collections import defaultdict


def parse(lines: list[str]):
    steps = int(lines[1].split()[5])
    rules: dict[str, list] = dict()
    chunks = [lines[i : i + 10] for i in range(3, len(lines), 10)]
    for chunk in chunks:
        curr_state = chunk[0].strip()[-2]
        zero_to = int(chunk[2].strip()[-2])
        zero_dir = 1 if chunk[3].strip().endswith("right.") else -1
        zero_state = chunk[4].strip()[-2]
        one_to = int(chunk[6].strip()[-2])
        one_dir = 1 if chunk[7].strip().endswith("right.") else -1
        one_state = chunk[8].strip()[-2]
        rules[curr_state] = [
            (zero_to, zero_dir, zero_state),
            (one_to, one_dir, one_state),
        ]
    return steps, rules


def solve(lines: list[str]):
    steps, rules = parse(lines)
    tape = defaultdict(int)
    state = "A"
    pos = 0
    for _i in range(steps):
        nvalue, ndir, nstate = rules[state][tape[pos]]
        tape[pos] = nvalue
        pos += ndir
        state = nstate
    return sum(tape.values())


test = [
    "Begin in state A.",
    "Perform a diagnostic checksum after 6 steps.",
    "",
    "In state A:",
    "  If the current value is 0:",
    "    - Write the value 1.",
    "    - Move one slot to the right.",
    "    - Continue with state B.",
    "  If the current value is 1:",
    "    - Write the value 0.",
    "    - Move one slot to the left.",
    "    - Continue with state B.",
    "",
    "In state B:",
    "  If the current value is 0:",
    "    - Write the value 1.",
    "    - Move one slot to the left.",
    "    - Continue with state A.",
    "  If the current value is 1:",
    "    - Write the value 1.",
    "    - Move one slot to the right.",
    "    - Continue with state A.",
    "",
]
assert solve(test) == 3

with open("d25.txt", "r") as f:
    input = f.readlines()
    assert solve(input) == 5744
