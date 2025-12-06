use itertools::Itertools;

pub fn p1(s: &str) -> u64 {
    let mut grid = vec![];
    for line in s.trim().lines() {
        let curr = line.split_whitespace().collect_vec();
        grid.push(curr);
    }
    let ops = grid.pop().unwrap();
    let mut res = 0;
    for (col, &op) in ops.iter().enumerate() {
        res += grid.iter().fold(if op == "+" { 0 } else { 1 }, |acc, v| {
            if op == "+" {
                acc + v[col].parse::<u64>().unwrap()
            } else {
                acc * v[col].parse::<u64>().unwrap()
            }
        });
    }
    res
}

pub fn p2(s: &str) -> u64 {
    let it = s.trim().lines();
    let n = it.clone().map(|v| v.len()).max().unwrap();
    let mut ops = it
        .clone()
        .last()
        .map(|v| v.trim().split_whitespace().rev().collect_vec())
        .unwrap();
    let grid = it
        .take_while(|v| v.trim().starts_with(|c: char| c.is_ascii_digit()))
        .collect_vec();
    let mut res = 0;
    let mut nums = vec![];
    for col in 0..n {
        let mut curr = 0;
        for row in &grid {
            let b = row.bytes().nth(col).unwrap_or(0);
            if b.is_ascii_digit() {
                curr = curr * 10 + u64::from(b - b'0');
            }
        }
        if curr > 0 {
            nums.push(curr);
        } else {
            let op = ops.pop().unwrap();
            res += nums
                .drain(..)
                .fold(if op == "+" { 0 } else { 1 }, |acc, v| {
                    if op == "+" { acc + v } else { acc * v }
                });
        }
    }
    let op = ops.pop().unwrap();
    res += nums
        .drain(..)
        .fold(if op == "+" { 0 } else { 1 }, |acc, v| {
            if op == "+" { acc + v } else { acc * v }
        });
    res
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "
123 328  51 64
 45 64  387 23
  6 98  215 314
*   +   *   +";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(p1(SAMPLE), 4277556);
        assert_eq!(p1(INPUT), 4771265398012);

        assert_eq!(p2(SAMPLE), 3263827);
        assert_eq!(p2(INPUT), 10695785245101);
    }
}
