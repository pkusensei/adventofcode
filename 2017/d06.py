def solve(line: str):
    nums = list(map(int, line.split()))
    count = 0
    seen: list[str] = list()
    first = 0
    while True:
        curr_dist = " ".join(map(str, nums))
        if curr_dist not in seen:
            seen.append(curr_dist)
        else:
            first = seen.index(curr_dist)
            break
        count += 1
        maxv = max(nums)
        maxi = nums.index(maxv)
        nums[maxi] = 0
        idx = maxi + 1
        while maxv > 0:
            maxv -= 1
            if idx >= len(nums):
                idx = 0
            nums[idx] += 1
            idx += 1
    return count, count - first


test = "0 2 7 0"
p1, p2 = solve(test)
assert p1 == 5
assert p2 == 4

input = "11	11	13	7	0	15	5	5	4	4	1	1	7	1	15	11"
p1, p2 = solve(input)
assert p1 == 4074
assert p2 == 2793
