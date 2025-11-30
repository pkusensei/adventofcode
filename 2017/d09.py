def end_garbage(stream: str, idx: int):
    assert stream[idx] == "<"
    while idx < len(stream):
        if stream[idx] == ">":
            count = 0
            tmpi = idx - 1
            while stream[tmpi] == "!":
                tmpi -= 1
                count += 1
            if count % 2 == 0:
                return idx
        idx += 1
    return idx


def group_score(stream: str, idx: int, out_score: int):
    assert stream[idx] == "{"
    score = out_score + 1
    total = score
    while idx < len(stream):
        idx += 1
        if stream[idx] == "{":
            s, i = group_score(stream, idx, score)
            total += s
            idx = i
        elif stream[idx] == "!":
            idx += 1
        elif stream[idx] == "<":
            idx = end_garbage(stream, idx)
        elif stream[idx] == "}":
            count = 0
            tmpi = idx - 1
            while stream[tmpi] == "!":
                tmpi -= 1
                count += 1
            if count % 2 == 0:
                return total, idx
    return total, idx - 1


def p1(stream: str):
    return group_score(stream, 0, 0)[0]


def count_cancelled(stream: str, idx: int, end: int):
    assert stream[idx] == "<"
    count = 0
    while idx < end:
        idx += 1
        if stream[idx] == "!":
            count += 2
            idx += 1
    return count


def p2(stream: str):
    idx = 0
    count = 0
    while idx < len(stream):
        if stream[idx] == "<":
            end = end_garbage(stream, idx)
            count += end - idx - count_cancelled(stream, idx, end) - 1
            idx = end
        idx += 1
    return count


assert p1(r"{}") == 1
assert p1(r"{{{}}}") == 6
assert p1(r"{{{},{},{{}}}}") == 16
assert p1(r"{<a>,<a>,<a>,<a>}") == 1
assert p1(r"{{<ab>},{<ab>},{<ab>},{<ab>}}") == 9
assert p1(r"{{<!!>},{<!!>},{<!!>},{<!!>}}") == 9
assert p1(r"{{<a!>},{<a!>},{<a!>},{<ab>}}") == 3

assert p2(r"<>") == 0
assert p2(r"<random characters>") == 17
assert p2(r"<<<<>") == 3
assert p2(r"<{!>}>") == 2
assert p2(r"<!!>") == 0
assert p2(r"<!!!>>") == 0
assert p2(r'<{o"i!a,<{i<a>') == 10


with open("d09.txt", "r") as f:
    input = f.readline()
    assert p1(input) == 23588
    assert p2(input) == 10045
