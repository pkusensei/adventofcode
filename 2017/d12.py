import networkx as nx


def parse(lines: list[str]):
    graph = nx.Graph()
    for line in lines:
        node, neighbors = line.strip().split(" <-> ")
        graph.add_edges_from(
            (int(node), int(neighbor)) for neighbor in neighbors.split(", ")
        )
    return graph


def p1(lines: list[str]):
    graph = parse(lines)
    return len(nx.node_connected_component(graph, 0))


def p2(lines: list[str]):
    graph = parse(lines)
    return nx.number_connected_components(graph)


test = [
    "0 <-> 2",
    "1 <-> 1",
    "2 <-> 0, 3, 4",
    "3 <-> 2, 4",
    "4 <-> 2, 3, 6",
    "5 <-> 6",
    "6 <-> 4, 5",
]
assert p1(test) == 6
assert p2(test) == 2

with open("d12.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 130
    assert p2(input) == 189
