pub fn solve(s: &str) -> [u64; 2] {
    let mut it = s.trim().lines();
    let mut prev: Vec<_> = it
        .next()
        .map(|v| v.trim().bytes().map(|b| u64::from(b != b'.')).collect())
        .unwrap_or_default();
    let n = prev.len();
    let mut p1 = 0;
    for line in it {
        let line = line.trim().as_bytes();
        let mut curr = vec![0; n];
        for i in 0..n {
            if prev[i] > 0 {
                if line[i] == b'.' {
                    curr[i] += prev[i];
                } else {
                    p1 += 1;
                    curr[i - 1] += prev[i];
                    curr[1 + i] += prev[i];
                }
            }
        }
        prev = curr;
    }
    let p2 = prev.iter().sum();
    [p1, p2]
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = ".......S.......
    ...............
    .......^.......
    ...............
    ......^.^......
    ...............
    .....^.^.^.....
    ...............
    ....^.^...^....
    ...............
    ...^.^...^.^...
    ...............
    ..^...^.....^..
    ...............
    .^.^.^.^.^...^.
    ...............";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(solve(SAMPLE), [21, 40]);
        assert_eq!(solve(INPUT), [1553, 15811946526915]);
    }
}
