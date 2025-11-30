from itertools import combinations


def p1(lines: list[str]):
    res = 0
    for line in lines:
        nums = list(map(int, line.split()))
        res += max(nums) - min(nums)
    return res


def p2(lines: list[str]):
    res = 0
    for line in lines:
        nums = list(map(int, line.split()))
        for pair in combinations(nums, 2):
            dividend = max(pair)
            divisor = min(pair)
            if dividend % divisor == 0:
                res += dividend // divisor
                break

    return res


test1 = ["5 1 9 5", "7 5 3", "2 4 6 8"]
assert p1(test1) == 18

test2 = ["5 9 2 8", "9 4 7 3", "3 8 6 5"]
assert p2(test2) == 9

with open("d02.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 34925
    assert p2(input) == 221
