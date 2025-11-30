from typing import List


class Node:
    def __init__(self, start: int, end: int):
        self.start = start
        self.end = end
        self.children = list()

    def __str__(self):
        return f"[{self.start} {self.end})"

    def add_children(self, children):
        self.children.extend(children)

    def value(self, nums: List[int]):
        count = nums[self.start+1]
        entries = nums[self.end-count:self.end]
        if nums[self.start] == 0:
            return sum(entries)
        res = 0
        for entry in entries:
            if entry <= len(self.children):
                res += self.children[entry-1].value(nums)
        return res


def find_nodes(nums: List[int], start_idx: int):
    if nums[start_idx] == 0:
        length = nums[start_idx+1]+2
        return [Node(start_idx, start_idx+length)]  # [start..end)
    else:
        nodes = list()
        children = list()
        num_child = nums[start_idx]
        next_start = start_idx+2
        for i in range(num_child):
            new_nodes = find_nodes(nums, next_start)
            nodes.extend(new_nodes)
            children.append(new_nodes[0])
            next_start = new_nodes[0].end
        current = Node(start_idx, next_start+nums[start_idx+1])
        current.add_children(children)
        nodes.insert(0, current)
        return nodes


def p1(nums: List[int]):
    nodes = find_nodes(nums, 0)
    res = 0
    for node in nodes:
        count = nums[node.start+1]
        res += sum(nums[node.end-count:node.end])
    return res


sample = list(map(int, "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2".split()))
assert p1(sample) == 138

input = list(map(int, open("d08.txt", 'r').read().split()))
assert p1(input) == 40908


def p2(nums: List[int]):
    nodes = find_nodes(nums, 0)
    return nodes[0].value(nums)


assert p2(sample) == 66
assert p2(input) == 25910
