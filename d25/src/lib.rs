pub fn solve(s: &str) -> u64 {
    let [locks, keys] = parse(s);
    let mut res = 0;
    for lock in locks.iter() {
        for key in keys.iter() {
            if lock.iter().zip(key.iter()).all(|(a, b)| a & b == 0) {
                res += 1;
            }
        }
    }
    res
}

fn parse(s: &str) -> [Vec<Vec<u8>>; 2] {
    let [mut locks, mut keys] = [0, 1].map(|_| vec![]);
    for lines in s.split("\n\n") {
        let curr: Vec<_> = lines
            .lines()
            .map(|line| {
                line.bytes()
                    .fold(0, |acc, b| (acc << 1) | u8::from(b == b'#'))
            })
            .collect();
        if lines.starts_with('#') {
            locks.push(curr);
        } else {
            keys.push(curr);
        }
    }
    [locks, keys]
}

#[cfg(test)]
mod tests {
    use super::*;

    const TEST: &str = r#"#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####"#;

    const INPUT: &str = include_str!("../input.txt");

    #[test]
    fn it_works() {
        assert_eq!(solve(TEST), 3);
        assert_eq!(solve(INPUT), 2586);
    }
}
