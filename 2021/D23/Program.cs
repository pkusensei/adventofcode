using System.Diagnostics;

static State Parse(string text)
{
    var world = text.Where(c => c == '.' || ('A' <= c && c <= 'D')).ToArray();
    return new State(world, 0);
}

static bool RoomOrganized(State state, int room)
{
    for (int i = state.Depth - 1; i >= 0; i -= 1)
    {
        var c = state.World[state.HallwaySize + i * 4 + room];
        if (c == '.')
            return true;
        if (c != 'A' + room)
            return false;
    }
    return true;
}

static bool Success(State state)
{
    if (state.World.Take(state.HallwaySize).Any(c => c != '.'))
        return false;
    for (int room = 0; room < 4; room++)
    {
        if (!RoomOrganized(state, room))
            return false;
    }
    return true;
}

static void PushRoom(char[] world, int hallwaySize, int depth, int room, char c)
{
    for (int i = depth - 1; i >= 0; i -= 1)
    {
        var index = hallwaySize + i * 4 + room;
        if (world[index] == '.')
        {
            world[index] = c;
            return;
        }
    }
    throw new ArgumentException($"Cannot push '{c}' to full room #{room}");
}

static int RoomCount(State state, int room)
{
    var count = 0;
    for (int i = state.Depth - 1; i >= 0; i -= 1)
    {
        if (state.World[state.HallwaySize + i * 4 + room] == '.')
            return count;
        count += 1;
    }
    return count;
}

static char PeekRoom(State state, int room)
{
    for (int i = 0; i < state.Depth; i++)
    {
        var idx = state.HallwaySize + i * 4 + room;
        if (state.World[idx] != '.')
            return state.World[idx];
    }
    throw new ArgumentException($"Cannot peek into empty room #{room}");
}

static void PopRoom(char[] world, int hallwaySize, int depth, int room)
{
    for (int i = 0; i < depth; i++)
    {
        var idx = hallwaySize + i * 4 + room;
        if (world[idx] != '.')
        {
            world[idx] = '.';
            return;
        }
    }
    throw new AggregateException($"Cannot pop from empty room #{room}");
}

static IList<State> GetNeighbors(State state)
{
    var cost = new int[] { 1, 10, 100, 1000 };

    var res = new List<State>();
    for (int i = 0; i < state.HallwaySize; i++)
    {
        if (state.World[i] == '.')
            continue;
        var current = state.World[i];
        var targetIdx = current - 'A';
        var canMove = RoomOrganized(state, targetIdx);
        if (!canMove)
            continue;

        var targetPos = targetIdx * 2 + 2;
        var direction = targetPos > i ? 1 : -1;
        for (int j = direction; Math.Abs(j) <= Math.Abs(targetPos - i); j += direction)
        {
            if (state.World[i + j] != '.')
            {
                canMove = false;
                break;
            }
        }
        if (!canMove)
            continue;
        var world = new char[state.World.Length];
        state.World.CopyTo(world, 0);
        world[i] = '.';
        PushRoom(world, state.HallwaySize, state.Depth, targetIdx, current);
        var energy = state.Energy
                     + (Math.Abs(targetPos - i) + (state.Depth - RoomCount(state, targetIdx))) * cost[targetIdx];
        res.Add(new State(world, energy));
    }
    if (res.Count > 0)
        return res;

    for (int room = 0; room < 4; room++)
    {
        if (RoomOrganized(state, room))
            continue;

        var current = PeekRoom(state, room);
        var targetIdx = current - 'A';
        var energy = state.Energy + (state.Depth - RoomCount(state, room) + 1) * cost[targetIdx];
        var roomPos = room * 2 + 2;
        foreach (var direction in new[] { -1, 1 })
        {
            var dist = direction;
            while (roomPos + dist >= 0 && roomPos + dist < state.HallwaySize && state.World[roomPos + dist] == '.')
            {
                var dest = roomPos + dist;
                if (dest == 2 || dest == 4 || dest == 6 || dest == 8)
                {
                    dist += direction;
                    continue;
                }

                var world = new char[state.World.Length];
                state.World.CopyTo(world, 0);
                world[roomPos + dist] = current;
                PopRoom(world, state.HallwaySize, state.Depth, room);

                res.Add(new State(world, energy + Math.Abs(dist) * cost[targetIdx]));
                dist += direction;
            }
        }
    }

    return res;
}

static int Organize(State state)
{
    var queue = new PriorityQueue<State, int>();
    queue.Enqueue(state, 0);
    var visited = new HashSet<string>();
    while (queue.Count > 0)
    {
        var node = queue.Dequeue();
        var world = new string(node.World);
        if (visited.Contains(world))
            continue;
        if (Success(node))
            return node.Energy;
        visited.Add(world);
        queue.EnqueueRange(GetNeighbors(node).Select(n => (n, n.Energy)));
    }
    throw new Exception();
}

static int Solve(string text)
{
    var state = Parse(text);
    return Organize(state);
}

var test = @"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########";
Debug.Assert(Solve(test) == 12521);

var test2 = @"#############
#...........#
###B#C#B#D###
  #D#C#B#A#
  #D#B#A#C#
  #A#D#C#A#
  #########";
Debug.Assert(Solve(test2) == 44169);

var input = @"#############
#...........#
###D#D#B#A###
  #C#A#B#C#
  #########";
Debug.Assert(Solve(input) == 16508);
// 16058 43626

var input2 = @"#############
#...........#
###D#D#B#A###
  #D#C#B#A#
  #D#B#A#C#
  #C#A#B#C#
  #########";
Debug.Assert(Solve(input2) == 43626);

record State
{
    public State(char[] world, int energy)
    {
        World = world;
        Energy = energy;
        HallwaySize = world.Count(c => c == '.');
        Depth = (World.Length - HallwaySize) / 4;
    }

    public char[] World { get; }
    public int Energy { get; }
    public int HallwaySize { get; }
    public int Depth { get; }
}
