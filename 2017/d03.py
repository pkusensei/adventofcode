from math import sqrt, ceil


def p1(num: int):
    size = ceil(sqrt(num))
    center = ceil((size - 1) / 2)
    return max(0, center - 1 + abs(center - num % size))


assert p1(1) == 0
assert p1(12) == 3
assert p1(23) == 2
assert p1(1024) == 31
assert p1(347991) == 480

# p2 349975
# https://oeis.org/A141481
