from collections import Counter
from itertools import combinations

sample = ["abcdef",
          "bababc",
          "abbcde",
          "abcccd",
          "aabcdd",
          "abcdee",
          "ababab"]


def count_letters(input):
    return map(Counter, input)


def count_occurence(counted, num):
    return len(list(filter(lambda x: num in x.values(), counted)))


def p1(input):
    return count_occurence(count_letters(input), 2) * \
        count_occurence(count_letters(input), 3)


assert p1(sample) == 12

lines = list(map(lambda x: x.strip(), open("d02.txt").readlines()))
assert p1(lines) == 8610


def compare_str(pair):
    count = 0
    index = 0
    for idx in range(len(pair[0])):
        if pair[0][idx] != pair[1][idx]:
            count += 1
            index = idx
    if count == 1:
        return index
    else:
        return None


def p2(input):
    paired = combinations(input, 2)
    for item in paired:
        char_idx = compare_str(item)
        if char_idx is not None:
            return item[0][0:char_idx]+item[0][char_idx+1:]


sample = ["abcde",
          "fghij",
          "klmno",
          "pqrst",
          "fguij",
          "axcye",
          "wvxyz"]

assert p2(sample) == "fgij"
assert p2(lines) == "iosnxmfkpabcjpdywvrtahluy"
