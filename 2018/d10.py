from typing import List
import sys


class Point:
    def __init__(self, x, y, vx, vy):
        self.x = x
        self.y = y
        self.vx = vx
        self.vy = vy

    def __str__(self):
        return f"Pos {(self.x, self.y)} Vel {(self.vx, self.vy)}"

    def move(self):
        self.x += self.vx
        self.y += self.vy

    def at(self, x, y):
        return self.x == x and self.y == y


def parse_line(line: str):
    tmp = line.split("=<")
    pos = tmp[1]
    vel = tmp[2]
    x = int(pos.split(',')[0])
    y = int(pos.split(',')[1].split('>')[0])
    vx = int(vel.split(',')[0])
    vy = int(vel.split(',')[1].split('>')[0])
    return Point(x, y, vx, vy)


def parse(lines: List[str]):
    points = [parse_line(line) for line in lines]
    return points


def draw(points: List[Point], xmin, xmax, ymin, ymax):
    length = xmax - xmin+1
    count = ymax - ymin+1
    for y in range(count):
        for x in range(length):
            if any(p.at(x+xmin, y+ymin) for p in points):
                sys.stdout.write('#')
            else:
                sys.stdout.write(' ')
            sys.stdout.flush()
        sys.stdout.write('\n')


def get_corners(points: List[Point]):
    xmin = min(point.x for point in points)
    xmax = max(point.x for point in points)
    ymin = min(point.y for point in points)
    ymax = max(point.y for point in points)
    return xmin, xmax, ymin, ymax


def solve(lines: List[str], ylimit: int):
    pts = parse(lines)
    xmin, xmax, ymin, ymax = get_corners(pts)
    count = 0
    while ymax-ymin > ylimit:
        for pt in pts:
            pt.move()
        xmin, xmax, ymin, ymax = get_corners(pts)
        count += 1
    draw(pts, xmin, xmax, ymin, ymax)
    return count


sample = ["position=< 9,  1> velocity=< 0,  2>",
          "position=< 7,  0> velocity=<-1,  0>",
          "position=< 3, -2> velocity=<-1,  1>",
          "position=< 6, 10> velocity=<-2, -1>",
          "position=< 2, -4> velocity=< 2,  2>",
          "position=<-6, 10> velocity=< 2, -2>",
          "position=< 1,  8> velocity=< 1, -1>",
          "position=< 1,  7> velocity=< 1,  0>",
          "position=<-3, 11> velocity=< 1, -2>",
          "position=< 7,  6> velocity=<-1, -1>",
          "position=<-2,  3> velocity=< 1,  0>",
          "position=<-4,  3> velocity=< 2,  0>",
          "position=<10, -3> velocity=<-1,  1>",
          "position=< 5, 11> velocity=< 1, -2>",
          "position=< 4,  7> velocity=< 0, -1>",
          "position=< 8, -2> velocity=< 0,  1>",
          "position=<15,  0> velocity=<-2,  0>",
          "position=< 1,  6> velocity=< 1,  0>",
          "position=< 8,  9> velocity=< 0, -1>",
          "position=< 3,  3> velocity=<-1,  1>",
          "position=< 0,  5> velocity=< 0, -1>",
          "position=<-2,  2> velocity=< 2,  0>",
          "position=< 5, -2> velocity=< 1,  2>",
          "position=< 1,  4> velocity=< 2,  1>",
          "position=<-2,  7> velocity=< 2, -2>",
          "position=< 3,  6> velocity=<-1, -1>",
          "position=< 5,  0> velocity=< 1,  0>",
          "position=<-6,  0> velocity=< 2,  0>",
          "position=< 5,  9> velocity=< 1, -2>",
          "position=<14,  7> velocity=<-2,  0>",
          "position=<-3,  6> velocity=< 2, -1>"]
assert solve(sample, 8) == 3

input = open("d10.txt", 'r').readlines()
assert solve(input, 10) == 10521
