from typing import List
import networkx as nx


def build_graph(input: List[str]):
    graph = nx.DiGraph()
    for line in input:
        graph.add_edge(line[5], line[36])
    return graph


def p1(input: List[str]):
    return "".join(nx.lexicographical_topological_sort(build_graph(input)))


sample = ["Step C must be finished before step A can begin.",
          "Step C must be finished before step F can begin.",
          "Step A must be finished before step B can begin.",
          "Step A must be finished before step D can begin.",
          "Step B must be finished before step E can begin.",
          "Step D must be finished before step E can begin.",
          "Step F must be finished before step E can begin."]
assert p1(sample) == "CABDFE"

lines = open("d07.txt", 'r').readlines()
assert p1(lines) == "GJFMDHNBCIVTUWEQYALSPXZORK"


def p2(input: List[str], workers: int, interval: int):
    graph = build_graph(input)
    task_times = []
    tasks = []
    time = 0
    while task_times or graph:
        available_tasks = [
            node for node in graph if node not in tasks and graph.in_degree(node) == 0]
        if available_tasks and len(task_times) < workers:
            task = min(available_tasks)
            task_times.append(ord(task) - ord('A')+interval)
            tasks.append(task)
        else:
            min_time = min(task_times)
            completed = [tasks[i]
                         for i, v in enumerate(task_times) if v == min_time]
            task_times = [v - min_time for v in task_times if v > min_time]
            tasks = [t for t in tasks if t not in completed]
            time += min_time
            graph.remove_nodes_from(completed)
    return time


assert p2(sample, 2, 1) == 15
assert p2(lines, 5, 61) == 1050
