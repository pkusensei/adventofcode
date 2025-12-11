use std::collections::VecDeque;

use fxhash::FxHashMap;

pub fn p1(s: &str) -> u64 {
    let map = parse(s);
    let mut queue = VecDeque::from(["you"]);
    let mut res = 0;
    while let Some(node) = queue.pop_front() {
        if node == "out" {
            res += 1;
            continue;
        }
        for &next in map.get(node).unwrap_or(&vec![]) {
            queue.push_back(next);
        }
    }
    res
}

pub fn p2(s: &str) -> u64 {
    let map = parse(s);
    let mut indegs = map
        .values()
        .flatten()
        .fold(FxHashMap::default(), |mut acc, node| {
            *acc.entry(*node).or_insert(0) += 1;
            acc
        });
    let mut queue = VecDeque::from(["svr"]);
    let mut freq = FxHashMap::default();
    freq.insert("svr", [1, 0, 0]); // [all paths, fft, fft&&dac]
    while let Some(node) = queue.pop_front() {
        let currf = freq.entry(node).or_insert([0; 3]);
        // sequence depends on input
        if node == "fft" {
            currf[1] += currf[0];
        } else if node == "dac" {
            currf[2] += currf[1];
        }
        let currf = *currf;
        for &next in map.get(node).unwrap_or(&vec![]) {
            let nf = freq.entry(next).or_default();
            for (nfv, cv) in nf.iter_mut().zip(currf) {
                *nfv += cv;
            }
            let deg = indegs.entry(next).or_insert(0);
            *deg -= 1;
            if *deg == 0 {
                queue.push_back(next);
            }
        }
    }
    freq.get("out").map(|f| f[2]).unwrap_or_default()
}

fn parse(s: &str) -> FxHashMap<&str, Vec<&str>> {
    s.trim()
        .lines()
        .map(|line| {
            let (a, b) = line.trim().split_once(": ").unwrap();
            (a, b.split_ascii_whitespace().collect())
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "aaa: you hhh
    you: bbb ccc
    bbb: ddd eee
    ccc: ddd eee fff
    ddd: ggg
    eee: out
    fff: out
    ggg: out
    hhh: ccc fff iii
    iii: out";

    const SAMPLE2: &str = "svr: aaa bbb
    aaa: fft
    fft: ccc
    bbb: tty
    tty: ccc
    ccc: ddd eee
    ddd: hub
    hub: fff
    eee: dac
    dac: fff
    fff: ggg hhh
    ggg: out
    hhh: out";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(p1(SAMPLE), 5);
        assert_eq!(p1(INPUT), 555);

        assert_eq!(p2(SAMPLE2), 2);
        assert_eq!(p2(INPUT), 502447498690860);
    }
}
