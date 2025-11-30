from collections import defaultdict


def p1(lines: list[str]):
    accs = list()
    for line in lines:
        tmp = line.strip().split("a=<")[1].strip(">").split(",")
        a = sum(map(lambda s: abs(int(s)), tmp))
        accs.append(a)
    mina = min(accs)
    return accs.index(mina)


class Particle:
    def __init__(
        self,
        pos: tuple[int, int, int],
        vel: tuple[int, int, int],
        acc: tuple[int, int, int],
    ):
        self.pos = pos
        self.vel = vel
        self.acc = acc

    def tick(self):
        self.vel = tuple(v + a for v, a in zip(self.vel, self.acc))
        self.pos = tuple(p + v for p, v in zip(self.pos, self.vel))


def p2(lines: list[str]):
    particles: dict[int, Particle] = {}
    for id, line in enumerate(lines):
        ps = line.strip().split(", ")[0].lstrip("p=<").strip(">").split(",")
        vs = line.strip().split(", ")[1].lstrip("v=<").strip(">").split(",")
        accs = line.strip().split(", ")[2].lstrip("a=<").strip(">").split(",")
        p = Particle(
            (int(ps[0]), int(ps[1]), int(ps[2])),
            (int(vs[0]), int(vs[1]), int(vs[2])),
            (int(accs[0]), int(accs[1]), int(accs[2])),
        )
        particles[id] = p
    for _i in range(1000):
        poses = defaultdict(list)
        for id, p in particles.items():
            p.tick()
            poses[p.pos].append(id)
        for ids in poses.values():
            if len(ids) > 1:
                for id in ids:
                    particles.pop(id)
    return len(particles)


test1 = ["p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>", "p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>"]
assert p1(test1) == 0

test2 = [
    "p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>",
    "p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>",
    "p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>",
    "p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>",
]
assert p2(test2) == 1

with open("d20.txt", "r") as f:
    input = f.readlines()
    assert p1(input) == 144
    assert p2(input) == 477
