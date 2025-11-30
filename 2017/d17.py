from collections import deque


def p1(step: int):
    nums = [0]
    idx = 0
    current = 1
    while current <= 2017:
        idx = (idx + step) % len(nums) + 1
        nums.insert(idx, current)
        current += 1
    return nums[idx + 1]


def p2(step: int):
    nums = deque([0])
    for num in range(1, 50_000_001):
        nums.rotate(-step)
        nums.append(num)
    return nums[nums.index(0) + 1]


assert p1(3) == 638
assert p1(348) == 417
assert p2(348) == 34334221
