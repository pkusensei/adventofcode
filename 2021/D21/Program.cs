using System.Diagnostics;

static int P1(int space1, int space2)
{
    int dice = 0;
    int score1 = 0;
    int score2 = 0;
    int turns = 0;
    while (true)
    {
        space1 += Dice() + Dice() + Dice();
        space1 %= 10;
        if (space1 == 0)
            space1 = 10;
        score1 += space1;
        turns += 3;
        if (score1 >= 1000)
            break;
        space2 += Dice() + Dice() + Dice();
        space2 %= 10;
        if (space2 == 0)
            space2 = 10;
        score2 += space2;
        turns += 3;
        if (score2 >= 1000)
            break;
    }

    return Math.Min(score1, score2) * turns;

    int Dice()
    {
        dice += 1;
        if (dice > 100)
            dice = 1;
        return dice;
    }
}

static (Int64, Int64) Play(int space1, int space2, int score1, int score2, int turns, int roll1, int roll2,
                           IDictionary<(int, int, int, int, int, int, int), (Int64, Int64)> cache)
{
    var current = (space1, space2, score1, score2, turns, roll1, roll2);
    if (cache.TryGetValue(current, out var v))
        return v;
    if (turns == 3)
    {
        space1 = (space1 + roll1) % 10;
        if (space1 == 0)
            space1 = 10;
        score1 += space1;
        if (score1 >= 21)
            return (1, 0);
    }
    else if (turns == 6)
    {
        space2 = (space2 + roll2) % 10;
        if (space2 == 0)
            space2 = 10;
        score2 += space2;
        if (score2 >= 21)
            return (0, 1);
        turns = 0;
        roll1 = 0;
        roll2 = 0;
    }
    var win1 = 0L;
    var win2 = 0L;
    for (int dice = 1; dice < 4; dice++)
    {
        if (turns < 3)
        {
            var (w1, w2) = Play(space1, space2, score1, score2, turns + 1, roll1 + dice, roll2, cache);
            win1 += w1;
            win2 += w2;
        }
        else
        {
            var (w1, w2) = Play(space1, space2, score1, score2, turns + 1, roll1, roll2 + dice, cache);
            win1 += w1;
            win2 += w2;
        }
    }
    cache[current] = (win1, win2);
    return (win1, win2);
}

static Int64 P2(int space1, int space2)
{
    var (w1, w2) = Play(space1, space2, 0, 0, 0, 0, 0,
                        new Dictionary<(int, int, int, int, int, int, int), (Int64, Int64)>());
    return Math.Max(w1, w2);
}


Debug.Assert(P1(4, 8) == 739785);
Debug.Assert(P2(4, 8) == 444356092776315);

Debug.Assert(P1(3, 7) == 1006866);
Debug.Assert(P2(3, 7) == 273042027784929);