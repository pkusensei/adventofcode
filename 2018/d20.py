import networkx as nx

directions = {"N": 1, "E": 1j, "S": -1, "W": -1j}


def parse(line: str):
    graph = nx.Graph()
    pos = {0}
    starts = {0}
    ends = set()
    stack = list()
    for c in line[1:-1]:
        if c == "|":
            ends.update(pos)
            pos = starts
        elif c in "NSEW":
            graph.add_edges_from((p, p + directions[c]) for p in pos)
            pos = {p + directions[c] for p in pos}
        elif c == "(":
            stack.append((starts, ends))
            starts = pos
            ends = set()
        elif c == ")":
            pos.update(ends)
            starts, ends = stack.pop()
    return graph


def p1(line: str):
    graph = parse(line)
    lengths = nx.algorithms.shortest_path_length(graph, 0)
    return max(lengths.values())


def p2(line: str):
    graph = parse(line)
    lengths = nx.algorithms.shortest_path_length(graph, 0)
    return len([l for l in lengths.values() if l >= 1000])


assert p1("^WNE$") == 3
assert p1("^ENWWW(NEEE|SSE(EE|N))$") == 10
assert p1("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$") == 18
assert p1("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$") == 23
assert p1("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$") == 31

input = open("d20.txt", "r").read()
assert p1(input) == 3810
assert p2(input) == 8615
