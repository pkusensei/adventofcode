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
        if ip >= len(insts):
            break
        registers[ip_idx] = ip
        # 3 4 5 6 8 9 11
    return registers[0]


test = [
    "#ip 0",
    "seti 5 0 1",
    "seti 6 0 2",
    "addi 0 1 0",
    "addr 1 2 3",
    "setr 1 0 0",
    "seti 8 0 4",
    "seti 9 0 5",
]
assert p1(test, [0, 0, 0, 0, 0, 0]) == 6

input = open("d19.txt", "r").readlines()
assert p1(input, [0, 0, 0, 0, 0, 0]) == 960

# print(p1(input, [1, 0, 0, 0, 0, 0]))

# So what is this doing again?
def p2(lines: List[str]):
    import re
    import collections

    a, b = map(int, [re.findall("\d+", lines[i])[1] for i in [22, 24]])
    number_to_factorize = 10551236 + a * 22 + b

    factors = collections.defaultdict(lambda: 0)
    possible_prime_divisor = 2
    while possible_prime_divisor ** 2 <= number_to_factorize:
        while number_to_factorize % possible_prime_divisor == 0:
            number_to_factorize //= possible_prime_divisor
            factors[possible_prime_divisor] += 1
        possible_prime_divisor += 1
    if number_to_factorize > 1:
        factors[number_to_factorize] += 1

    sum_of_divisors = 1
    for prime_factor in factors:
        sum_of_divisors *= (prime_factor ** (factors[prime_factor] + 1) - 1) // (
            prime_factor - 1
        )

    return sum_of_divisors


assert p2(input) == 10750428
