import copy
from typing import List, Optional, Set


class Unit:
    def __init__(
        self,
        id: str,
        count: int,
        hp: int,
        immune: Set[str],
        weak: Set[str],
        initiative: int,
        damage_type: str,
        damage: int,
        side: int,
    ) -> None:
        self.id = id
        self.count = count
        self.hp = hp
        self.immune = immune
        self.weak = weak
        self.initiative = initiative
        self.damage_type = damage_type
        self.damage = damage
        self.side = side
        self.target = None

    @property
    def power(self):
        return self.count * self.damage

    def damage_to(self, other: "Unit"):
        if self.damage_type in other.immune:
            return 0
        elif self.damage_type in other.weak:
            return 2 * self.power
        else:
            return self.power

    @property
    def damage(self):
        return self._dmg

    @damage.setter
    def damage(self, value: int):
        self._dmg = value

    @property
    def target(self):
        return self._target

    @target.setter
    def target(self, t: Optional["Unit"]):
        assert t is None or t.side != self.side
        self._target = t

    def kill(self, num: int):
        self.count -= num


def parse(lines: List[str]):
    units: List[Unit] = []
    for line in lines:
        line = line.strip()
        if "Immune System" in line:
            next_id = 1
            side = 0
        elif "Infection" in line:
            next_id = 1
            side = 1
        elif len(line) > 0:
            words = line.split()
            count = int(words[0])
            hp = int(words[4])
            if "(" in line:
                elements = line.split("(")[1].split(")")[0]
                immune = set()
                weak = set()

                def insert(s: str):
                    words = s.split()
                    assert words[0] in ["immune", "weak"]
                    for word in words[2:]:
                        if word.endswith(","):
                            word = word[:-1]
                        if words[0] == "immune":
                            immune.add(word)
                        else:
                            weak.add(word)

                if ";" in elements:
                    s1, s2 = elements.split(";")
                    insert(s1)
                    insert(s2)
                else:
                    insert(elements)
            else:
                immune = set()
                weak = set()
            initiative = int(words[-1])
            dmg_type = words[-5]
            dmg = int(words[-6])
            id = "{}_{}".format({1: "Infection", 0: "Immune System"}[side], next_id)
            units.append(
                Unit(id, count, hp, immune, weak, initiative, dmg_type, dmg, side)
            )
            next_id += 1
    return units


def run(units: List[Unit], boost=0):
    current: List[Unit] = []
    for unit in units:
        dmg = unit.damage + (boost if unit.side == 0 else 0)
        new_u = copy.deepcopy(unit)
        new_u.damage = dmg
        current.append(new_u)

    while True:
        current.sort(key=lambda unit: (-unit.power, -unit.initiative))
        chosen = set()
        for unit in current:
            targets = sorted(
                [
                    t
                    for t in current
                    if t.side != unit.side
                    and t.id not in chosen
                    and unit.damage_to(t) > 0
                ],
                key=lambda t: (-unit.damage_to(t), -t.power, -t.initiative),
            )
            if len(targets) > 0:
                unit.target = targets[0]
                chosen.add(targets[0].id)
        current.sort(key=lambda u: -u.initiative)
        any_killed = False
        for unit in current:
            if unit.target is not None:
                dmg = unit.damage_to(unit.target)
                killed = min(unit.target.count, dmg // unit.target.hp)
                if killed > 0:
                    any_killed = True
                unit.target.kill(killed)

        current = [u for u in current if u.count > 0]
        for unit in current:
            unit.target = None
        if not any_killed:
            return 1, n1
        n0 = sum(u.count for u in current if u.side == 0)
        n1 = sum(u.count for u in current if u.side == 1)
        if n0 == 0:
            return 1, n1
        if n1 == 0:
            return 0, n0


def p1(lines: List[str]):
    units = parse(lines)
    _s, count = run(units)
    return count


def p2(lines: List[str]):
    units = parse(lines)
    boost = 1
    while True:
        side, count = run(units, boost)
        if side == 0:
            return count
        boost += 1


test = [
    "Immune System:",
    "17 units each with 5390 hit points (weak to radiation, bludgeoning) with"
    " an attack that does 4507 fire damage at initiative 2",
    "989 units each with 1274 hit points (immune to fire; weak to bludgeoning,"
    " slashing) with an attack that does 25 slashing damage at initiative 3",
    "",
    "Infection:",
    "801 units each with 4706 hit points (weak to radiation) with an attack"
    " that does 116 bludgeoning damage at initiative 1",
    "4485 units each with 2961 hit points (immune to radiation; weak to fire,"
    " cold) with an attack that does 12 slashing damage at initiative 4",
]
assert p1(test) == 5216

input = open("d24.txt", "r").readlines()
assert p1(input) == 28976

assert p2(test) == 51
assert p2(input) == 3534
