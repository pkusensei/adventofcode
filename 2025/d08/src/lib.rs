use itertools::{Itertools, izip};

pub fn solve(s: &str, mut connect: i32) -> [u64; 2] {
    let points = parse(s);
    let n = points.len();
    let mut dsu = DSU::new(n);
    let mut it = (0..n)
        .array_combinations()
        .sorted_unstable_by_key(|&[a, b]| {
            izip!(points[a], points[b])
                .map(|(v1, v2)| v1.abs_diff(v2).pow(2))
                .sum::<u64>()
        });
    for [a, b] in it.by_ref() {
        dsu.union(a, b);
        connect -= 1;
        if connect <= 0 {
            break;
        }
    }
    let p1 = (0..n)
        .map(|v| {
            let root = dsu.find(v);
            (root, dsu.size[root])
        })
        .unique()
        .map(|(_, v)| v)
        .k_largest_relaxed(3)
        .product::<u64>();
    let mut p2 = 0;
    for [a, b] in it {
        dsu.union(a, b);
        if dsu.n == 1 {
            p2 = points[a][0] * points[b][0];
            break;
        }
    }
    [p1, p2]
}

struct DSU {
    parent: Vec<usize>,
    size: Vec<u64>,
    n: usize,
}

impl DSU {
    fn new(n: usize) -> Self {
        Self {
            parent: (0..n).collect(),
            size: vec![1; n],
            n,
        }
    }

    fn find(&mut self, v: usize) -> usize {
        if self.parent[v] != v {
            self.parent[v] = self.find(self.parent[v]);
        }
        self.parent[v]
    }

    fn union(&mut self, x: usize, y: usize) -> bool {
        let [rx, ry] = [x, y].map(|v| self.find(v));
        if rx == ry {
            return false;
        }
        if self.size[rx] < self.size[ry] {
            self.size[ry] += self.size[rx];
            self.parent[rx] = ry;
        } else {
            self.size[rx] += self.size[ry];
            self.parent[ry] = rx;
        }
        self.n -= 1;
        true
    }
}

fn parse(s: &str) -> Vec<[u64; 3]> {
    s.trim()
        .lines()
        .map(|line| {
            let [a, b, c] = line
                .trim()
                .split(',')
                .map(|num| num.parse().unwrap())
                .collect::<Vec<_>>()[..]
            else {
                unreachable!()
            };
            [a, b, c]
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "162,817,812
    57,618,57
    906,360,560
    592,479,940
    352,342,300
    466,668,158
    542,29,236
    431,825,988
    739,650,466
    52,470,668
    216,146,977
    819,987,18
    117,168,530
    805,96,715
    346,949,466
    970,615,88
    941,993,340
    862,61,35
    984,92,344
    425,690,689";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(solve(SAMPLE, 10), [40, 25272]);
        assert_eq!(solve(INPUT, 1000), [181584, 8465902405]);
    }
}
