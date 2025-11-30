# totally didn't do this
# reads as boring as it can be
def reverse(nums: list[int], repeat: int):
    knot = list(range(256))
    pos = 0
    skip = 0
    for _i in range(repeat):
        for i in nums:
            temp = []
            for j in range(i):
                temp.append(knot[(pos + j) % 256])
            for j in range(i):
                knot[(pos + i - 1 - j) % 256] = temp[j]
            pos += skip + i
            skip += 1
    return knot


def densehash(knot: list[int]):
    dense = [0] * 16
    for i in range(16):
        dense[i] = knot[16 * i]
        for m in range(1, 16):
            dense[i] ^= knot[16 * i + m]
    return dense


def kh(dense: list[int]):
    knothash = ""
    for i in dense:
        if len(hex(i)[2:]) == 2:
            knothash += hex(i)[2:]
        else:
            knothash += "0" + hex(i)[2:]
    return knothash


def p1(line: str):
    nums = list(map(int, line.split(",")))
    knot = reverse(nums, 1)
    return knot[0] * knot[1]


def p2(line: str):
    nums = list()
    for i in range(len(line)):
        nums.append(ord(line[i]))
    nums += [17, 31, 73, 47, 23]
    sparce = reverse(nums, 64)
    dense = densehash(sparce)
    return kh(dense)


input = "230,1,2,221,97,252,168,169,57,99,0,254,181,255,235,167"
assert p1(input) == 2928
assert p2(input) == "0c2f794b2eb555f7830766bf8fb65a16"
