use std::cmp::Reverse;

use itertools::{Itertools, izip};

pub fn solve(s: &str) -> [u64; 2] {
    let reds = parse(s).collect_vec();
    let mut edges = Vec::with_capacity(reds.len());
    for (&a, &b) in izip!(&reds, reds.iter().cycle().skip(1)) {
        edges.push(Rect::new(a, b));
    }
    let mut it = reds
        .iter()
        .array_combinations()
        .map(|[&a, &b]| Rect::new(a, b))
        .sorted_unstable_by_key(|r| Reverse(r.area));
    let p1 = it.next().map(|r| r.area).unwrap();
    let p2 = it
        .filter_map(|rect| {
            let xin = [rect.xs[0] + 1, rect.xs[1] - 1];
            let yin = [rect.ys[0] + 1, rect.ys[1] - 1];
            for e in &edges {
                let hor = xin[0].max(e.xs[0]) <= xin[1].min(e.xs[1]);
                let ver = yin[0].max(e.ys[0]) <= yin[1].min(e.ys[1]);
                if hor && ver {
                    return None; // Overlap!
                }
            }
            Some(rect.area)
        })
        .next()
        .unwrap();
    [p1, p2]
}

type Point = [u64; 2];
#[derive(Clone, Copy)]
struct Rect {
    xs: [u64; 2],
    ys: [u64; 2],
    area: u64,
}

impl Rect {
    fn new(a: Point, b: Point) -> Self {
        let xs = [a[0].min(b[0]), a[0].max(b[0])];
        let ys = [a[1].min(b[1]), a[1].max(b[1])];
        let area = (1 + xs[0].abs_diff(xs[1])) * (1 + ys[0].abs_diff(ys[1]));
        Rect { xs, ys, area }
    }
}

fn parse(s: &str) -> impl Iterator<Item = [u64; 2]> + Clone {
    s.trim().lines().map(|line| {
        let Some((x, y)) = line.trim().split_once(',') else {
            unreachable!()
        };
        [x, y].map(|v| v.parse().unwrap())
    })
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "7,1
    11,1
    11,7
    9,7
    9,5
    2,5
    2,3
    7,3";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(solve(SAMPLE), [50, 24]);
        assert_eq!(solve(INPUT), [4759930955, 1525241870]);
    }
}
