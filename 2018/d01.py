assert sum([+1, +1, +1]) == 3
assert sum([+1, +1, -2]) == 0
assert sum([-1, -2, -3]) == -6

lines = open("d01.txt", "r").readlines()
nums = list(map(int, lines))

assert sum(nums) == 531  # p1


def p2(nums):
    seen = set()
    total = 0
    idx = 0
    while total not in seen:
        seen.add(total)
        if idx < len(nums):
            total += nums[idx]
            idx += 1
        else:
            total += nums[0]
            idx = 1
    return total


assert p2([+1, -2, +3, +1]) == 2
assert p2([+1, -1]) == 0
assert p2([+3, +3, +4, -2, -4]) == 10
assert p2([-6, +3, +8, +5, -6]) == 5
assert p2([+7, +7, -2, -7, -4]) == 14

assert p2(nums) == 76787
