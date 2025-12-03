pub fn p1(s: &str) -> u64 {
    solve(s, 2)
}

pub fn p2(s: &str) -> u64 {
    solve(s, 12)
}

fn solve(s: &str, len: usize) -> u64 {
    let mut res = 0;
    for line in s.lines() {
        let line = line.trim();
        if line.is_empty() {
            continue;
        }
        let mut st = vec![];
        let n = line.len();
        for (idx, b) in line.bytes().enumerate() {
            while st.last().is_some_and(|&top| top < b) && n - idx - 1 >= len - st.len() {
                st.pop();
            }
            if st.len() < len {
                st.push(b);
            }
        }
        res += st.iter().fold(0, |acc, v| acc * 10 + u64::from(v - b'0'));
    }
    res
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "987654321111111
    811111111111119
    234234234234278
    818181911112111";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(p1(SAMPLE), 357);
        assert_eq!(p1(INPUT), 17155);

        assert_eq!(p2(SAMPLE), 3121910778619);
        assert_eq!(p2(INPUT), 169685670469164);
    }
}
