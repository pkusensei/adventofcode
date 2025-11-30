def shrink(input: str):
    i = 0
    text = input
    while i < len(text)-2:
        if text[i] != text[i+1] and text[i].lower() == text[i+1].lower():
            res = text[0:i]+text[i+2:]
            text = res
            i = i-1 if i-1 > 0 else 0
        else:
            i += 1
    return text


sample = "dabAcCaCBAcCcaDA"
assert len(shrink(sample)) == 10

line = open("d05.txt", 'r').read()
assert len(shrink(line)) == 9462


alphabet = "abcdefghijklmnopqrstuvwxyz"


def p2(input: str):
    length = len(shrink(input))
    for letter in alphabet:
        line = input.replace(letter, '').replace(letter.upper(), '')
        size = len(shrink(line))
        if size < length:
            length = size
    return length


assert p2(sample) == 4
assert p2(line) == 4952
