from collections import Counter
from typing import List


def find_guard(line: str):
    return int(line.split('#')[1].split(' ')[0])


def find_minute(line: str):
    return int(line.split(']')[0].split(':')[1])


def count_minute(lines: List[str]):
    index = 0
    all = dict()
    while index < len(lines):
        if "Guard" in lines[index]:
            id = find_guard(lines[index])
            if id not in all:
                all[id] = Counter()
            index += 1
        while index < len(lines) and "Guard" not in lines[index]:
            start = find_minute(lines[index])
            index += 1
            end = find_minute(lines[index])
            index += 1
            all[id].update(range(start, end))
    return all


def find_max_by_total(all):
    count = dict()
    for id, counter in all.items():
        count[id] = sum(counter.values())
    return max(count, key=count.get)


def solve(input: List[str], find_f):
    counted = count_minute(input)
    id = find_f(counted)
    minute = max(counted[id], key=counted[id].get)
    return id*minute


sample = ["[1518-11-01 00:00] Guard #10 begins shift",
          "[1518-11-01 00:05] falls asleep",
          "[1518-11-01 00:25] wakes up",
          "[1518-11-01 00:30] falls asleep",
          "[1518-11-01 00:55] wakes up",
          "[1518-11-01 23:58] Guard #99 begins shift",
          "[1518-11-02 00:40] falls asleep",
          "[1518-11-02 00:50] wakes up",
          "[1518-11-03 00:05] Guard #10 begins shift",
          "[1518-11-03 00:24] falls asleep",
          "[1518-11-03 00:29] wakes up",
          "[1518-11-04 00:02] Guard #99 begins shift",
          "[1518-11-04 00:36] falls asleep",
          "[1518-11-04 00:46] wakes up",
          "[1518-11-05 00:03] Guard #99 begins shift",
          "[1518-11-05 00:45] falls asleep",
          "[1518-11-05 00:55] wakes up"]

assert solve(sample, find_max_by_total) == 240

lines = open("d04.txt", 'r').readlines()
lines.sort()
assert solve(lines, find_max_by_total) == 109659


def find_max_by_minute(all):
    count = dict()
    for id, counter in all.items():
        if len(counter) > 0:
            count[id] = max(counter.values())
    return max(count, key=count.get)


assert solve(sample, find_max_by_minute) == 4455
assert solve(lines, find_max_by_minute) == 36371
