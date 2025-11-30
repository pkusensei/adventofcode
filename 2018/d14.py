init = "37"


def p1(input: int):
    recipes = str(init)
    elf1 = 0
    elf2 = 1
    while str(input) not in recipes[-7:] or len(recipes) - input < 10:
        score = int(recipes[elf1]) + int(recipes[elf2])
        recipes += str(score)
        elf1 = (elf1 + int(recipes[elf1]) + 1) % len(recipes)
        elf2 = (elf2 + int(recipes[elf2]) + 1) % len(recipes)
    print(recipes.index(str(input)))
    return recipes[input : input + 10]


assert p1(9) == "5158916779"
assert p1(5) == "0124515891"
assert p1(18) == "9251071085"
assert p1(2018) == "5941429882"
assert p1(540561) == "1413131339"  # 20254833
