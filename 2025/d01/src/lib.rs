pub fn solve(s: &str) -> [i32; 2] {
    let mut prev = 50;
    let mut p1 = 0;
    let mut p2 = 0;
    for line in s.lines().map(|s| s.trim()) {
        if line.is_empty() {
            continue;
        }
        let num: i32 = line[1..].parse().unwrap();
        let rem = num % 100;
        p2 += num / 100;
        prev = if line.as_bytes()[0] == b'R' {
            let curr = (prev + rem) % 100;
            p2 += i32::from(curr < prev);
            curr
        } else {
            let curr = (prev - rem).rem_euclid(100);
            if prev != 0 && (curr > prev || curr == 0) {
                p2 += 1;
            }
            curr
        };
        p1 += i32::from(prev == 0);
    }
    [p1, p2]
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = r"L68
    L30
    R48
    L5
    R60
    L55
    L1
    L99
    R14
    L82";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(solve(SAMPLE), [3, 6]);
        assert_eq!(solve(INPUT), [984, 5657]);
    }
}
