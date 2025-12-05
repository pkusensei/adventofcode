use std::collections::BTreeMap;

use itertools::Itertools;

pub fn solve(s: &str) -> [u64; 2] {
    let mut it = s.lines();
    let spans = it
        .by_ref()
        .take_while(|line| !line.trim().is_empty())
        .filter_map(|line| {
            line.trim()
                .split_once('-')
                .and_then(|(a, b)| a.parse().ok().zip(b.parse().ok()))
        })
        .sorted_unstable()
        .collect_vec();
    let mut map: BTreeMap<u64, u64> = BTreeMap::new();
    for (a, b) in spans {
        if let Some((_, v)) = map.range_mut(..=a).next_back()
            && a <= *v
        {
            *v = (*v).max(b);
        } else {
            map.insert(a, b);
        }
    }
    let mut p1 = 0;
    for line in it.filter_map(|v| v.trim().parse::<u64>().ok()) {
        if let Some((&a, &b)) = map.range(..=line).next_back() {
            p1 += u64::from((a..=b).contains(&line));
        }
    }
    let p2 = map.iter().map(|(k, v)| v - k + 1).sum();
    [p1, p2]
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "3-5
    10-14
    16-20
    12-18

    1
    5
    8
    11
    17
    32";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(solve(SAMPLE), [3, 14]);
        assert_eq!(solve(INPUT), [888, 344378119285354]);
    }
}
