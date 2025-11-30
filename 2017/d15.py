def generator(num: int, factor: int, multiple=1):
    while True:
        num = num * factor % 2147483647
        if num % multiple == 0:
            yield num & 0xFFFF


def solve(numa: int, numb: int, turns: int, ma=1, mb=1):
    count = 0
    ga = generator(numa, 16807, ma)
    gb = generator(numb, 48271, mb)
    for _ in range(turns):
        if next(ga) == next(gb):
            count += 1
    return count


def p1(numa: int, numb: int, turns: int):
    return solve(numa, numb, turns)


def p2(numa: int, numb: int, turns: int):
    return solve(numa, numb, turns, 4, 8)


assert p1(65, 8921, 5) == 1
assert p2(65, 8921, 5_000_000) == 309

assert p1(289, 629, 40_000_000) == 638
assert p2(289, 629, 5_000_000) == 343
