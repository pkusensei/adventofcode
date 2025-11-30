from typing import DefaultDict


SIZE = 300


def power(x: int, y: int, serial: int):
    rack_id = x+10
    res = rack_id*y + serial
    res *= rack_id
    hundred_digit = (res//100) % 10
    return hundred_digit - 5


assert power(3, 5, 8) == 4
assert power(122, 79, 57) == -5
assert power(217, 196, 39) == 0
assert power(101, 153, 71) == 4


def grid_power(x: int, y: int, serial: int, size: int):
    return sum(power(i, j, serial) for i in range(x, x+size) for j in range(y, y+size))


assert grid_power(33, 45, 18, 3) == 29
assert grid_power(21, 61, 42, 3) == 30


def p1(serial: int):
    grid_x = 0
    grid_y = 0
    max_power = 0
    size = 3
    for x in range(1, SIZE-size+1):
        for y in range(1, SIZE-size+1):
            tmp = grid_power(x, y, serial, size)
            if max_power < tmp:
                grid_x = x
                grid_y = y
                max_power = tmp
    return grid_x, grid_y, max_power


assert p1(18) == (33, 45, 29)
assert p1(42) == (21, 61, 30)
assert p1(1133) == (235, 14, 31)


def p2(serial: int):
    sums = DefaultDict(int)
    for y in range(1, SIZE+1):
        for x in range(1, SIZE+1):
            p = power(x, y, serial)
            sums[(x, y)] = p + sums[(x-1, y)] + \
                sums[(x, y-1)] - sums[(x-1, y-1)]
    total = 0
    resx = 0
    resy = 0
    ress = 0
    for size in range(1, SIZE+1):
        for y in range(size, SIZE+1):
            for x in range(size, SIZE+1):
                p = sums[(x, y)]-sums[(x, y-size)] - \
                    sums[(x-size, y)]+sums[(x-size, y-size)]
                if total < p:
                    total = p
                    resx = x
                    resy = y
                    ress = size
    return resx-ress+1, resy-ress+1, ress, total


assert p2(18) == (90, 269, 16, 113)
assert p2(42) == (232, 251, 12, 119)
assert p2(1133) == (237, 227, 14, 108)
