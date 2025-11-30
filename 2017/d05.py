def solve(lines: list[str], p2: bool):
    nums = list(map(int, lines))
    ip = 0
    count = 0
    while 0 <= ip < len(nums):
        count += 1
        if p2 and nums[ip] >= 3:
            tmp = ip
            ip += nums[ip]
            nums[tmp] -= 1
        else:
            tmp = ip
            ip += nums[ip]
            nums[tmp] += 1
    return count


test = "0 3 0 1 -3"
assert solve(test.split(), False) == 5
assert solve(test.split(), True) == 10

with open("d05.txt", "r") as f:
    input = f.readlines()
    assert solve(input, False) == 354121
    assert solve(input, True) == 27283023
