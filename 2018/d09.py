from collections import deque

# totally didn't do this
# can't even understand what the puzzle is asking :(
def solve(players: int, last_marble: int):
    scores = [0 for _i in range(players)]
    ring = deque()
    ring.appendleft(0)

    for marble in range(1, last_marble+1):
        if marble % 23 == 0:
            for _i in range(7):
                tmp = ring.pop()
                ring.appendleft(tmp)
            scores[marble % players] += marble+ring.popleft()
        else:
            for _i in range(2):
                tmp = ring.popleft()
                ring.append(tmp)
            ring.appendleft(marble)
    return max(scores)


assert solve(9, 25) == 32
assert solve(10, 1618) == 8317
assert solve(13, 7999) == 146373
assert solve(17, 1104) == 2764
assert solve(21, 6111) == 54718
assert solve(30, 5807) == 37305
assert solve(411, 71058) == 424639
assert solve(411, 71058*100) == 3516007333
