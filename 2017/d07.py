from collections import Counter


def name_weight(input: str):
    name = input.split(" (")[0]
    weight = input.split(" (")[1].strip(") ")
    return name, int(weight)


def parse(lines: list[str]):
    pros: dict[str, tuple[int, list[str]]] = dict()
    for line in lines:
        if "->" in line:
            items = line.split(" -> ")
            name, weight = name_weight(items[0])
            discs = list(map(lambda x: x.strip(), items[1].split(", ")))
            pros[name] = (weight, discs)
        else:
            name, weight = name_weight(line.strip())
            pros[name] = (weight, [])
    return pros


def find_root(pros: dict[str, tuple[int, list[str]]]):
    all_names = set(pro for pro in pros)
    supported = set(disc for v in pros.values() for disc in v[1])
    diff = all_names.difference(supported)
    return diff.pop()


def p1(lines: list[str]):
    pros = parse(lines)
    return find_root(pros)


def build_weight(pros: dict[str, tuple[int, list[str]]], current: str):
    res: dict[str, int] = dict()
    if len(pros[current][1]) == 0:
        return {current: pros[current][0]}
    else:
        total = pros[current][0]
        for name in pros[current][1]:
            tmp = build_weight(pros, name)
            total += tmp[name]
            res.update(tmp)
        res[current] = total
    return res


def find_imbalance(
    pros: dict[str, tuple[int, list[str]]], weights: dict[str, int], root: str
):
    current = root
    while True:
        discs = [weights[name] for name in pros[current][1]]
        counter = Counter(discs)
        if len(counter) > 1:
            for k, v in counter.items():
                if v == 1:
                    anomaly = k
                else:
                    balanced = k
            delta = anomaly - balanced
            for k, v in weights.items():
                if v == anomaly:
                    current = k
        else:
            return pros[current][0] - delta


def p2(lines: list[str]):
    pros = parse(lines)
    root = find_root(pros)
    weights = build_weight(pros, root)
    return find_imbalance(pros, weights, root)


test = [
    "pbga (66)",
    "xhth (57)",
    "ebii (61)",
    "havc (66)",
    "ktlj (57)",
    "fwft (72) -> ktlj, cntj, xhth",
    "qoyq (66)",
    "padx (45) -> pbga, havc, qoyq",
    "tknk (41) -> ugml, padx, fwft",
    "jptl (61)",
    "ugml (68) -> gyxo, ebii, jptl",
    "gyxo (61)",
    "cntj (57)",
]
assert p1(test) == "tknk"
assert p2(test) == 60

with open("d07.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == "hlhomy"
    assert p2(input) == 1505
