def p1(lines: list[str]):
    count = 0
    for line in lines:
        words = line.split()
        unique = set(words)
        if len(words) == len(unique):
            count += 1
    return count


def p2(lines: list[str]):
    count = 0
    for line in lines:
        words = line.split()
        unique = set(str(sorted(word)) for word in words)
        if len(unique) == len(words):
            count += 1
    return count


with open("d04.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 466
    assert p2(input) == 251
