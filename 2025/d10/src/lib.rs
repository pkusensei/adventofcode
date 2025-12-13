use itertools::Itertools;

// Credits to
// u/tenthmascot for [Bifurcate algo](https://www.reddit.com/r/adventofcode/comments/1pk87hl/2025_day_10_part_2_bifurcate_your_way_to_victory/)
// And to
// u/_RcCookie_ for [Java code in reference](https://github.com/Rc-Cookie/Advent-of-Code-25/blob/6ea776bf1de2be72e5f206f587ddd78f3f7ed87f/src/main/java/de/rccookie/aoc/aoc25/Solution10.java)
pub mod bifurcate {
    use super::*;
    use fxhash::FxHashMap;

    pub fn p1(s: &str) -> u32 {
        parse(s)
            .map(|(target, buttons, _)| {
                combinations(target, &buttons)
                    .iter()
                    .map(|v| v.count_ones())
                    .min()
                    .unwrap()
            })
            .sum()
    }

    pub fn p2(s: &str) -> u32 {
        parse(s)
            .map(|(_, buttons, jolts)| solve(&buttons, &jolts))
            .sum()
    }

    fn solve(button_masks: &[i32], jolts: &[i32]) -> u32 {
        let n = jolts.len();
        let buttons = button_masks
            .iter()
            .map(|mask| {
                let mut butts = vec![];
                for bit in 0..n {
                    if mask & (1 << bit) > 0 {
                        butts.push(bit);
                    }
                }
                butts
            })
            .collect_vec();
        dfs(&jolts, &button_masks, &buttons, &mut FxHashMap::default()).unwrap()
    }

    fn dfs(
        target: &[i32],
        button_masks: &[i32],
        buttons: &[Vec<usize>],
        combo_cache: &mut FxHashMap<i32, Vec<i32>>,
    ) -> Option<u32> {
        if target.iter().all(|&v| v == 0) {
            return Some(0);
        }
        let parity = target.iter().rev().fold(0, |acc, v| (acc << 1) | v & 1);
        let v = combo_cache
            .entry(parity)
            .or_insert_with(|| combinations(parity, &button_masks))
            .clone();
        let mut res = None;
        'out: for combo in v {
            let count = combo.count_ones();
            let mut curr = target.to_vec();
            for (i, butts) in buttons.iter().enumerate() {
                if combo & (1 << i) > 0 && !press(butts, &mut curr) {
                    continue 'out;
                }
            }
            for v in curr.iter_mut() {
                *v >>= 1;
            }
            if let Some(temp) = dfs(&curr, button_masks, buttons, combo_cache) {
                let temp = (temp << 1) + count;
                let v = res.get_or_insert(temp);
                *v = (*v).min(temp)
            }
        }
        res
    }

    fn press(butts: &[usize], curr: &mut [i32]) -> bool {
        for &b in butts {
            curr[b] -= 1;
            if curr[b] < 0 {
                return false;
            }
        }
        true
    }

    fn combinations(target: i32, buttons: &[i32]) -> Vec<i32> {
        let mut res = vec![];
        if 0 == target {
            res.push(0);
        }
        let max: i32 = 1 << buttons.len();
        let mut combo = 0;
        let mut pattern = 0;
        for mask in 1..max {
            let bit = mask & (-mask); // find button to toggle
            combo ^= bit; // toggle that button, e.g (1,3)
            pattern ^= buttons[bit.trailing_zeros() as usize]; // add button's effect
            if pattern == target {
                res.push(combo);
            }
        }
        res
    }
}

// integer linear programming with `microlp` crate
pub mod bfs_and_ilp {
    use super::*;
    use fxhash::FxHashSet;
    use microlp::{ComparisonOp, LinearExpr, OptimizationDirection, Problem};

    pub fn p1(s: &str) -> u64 {
        parse(s)
            .map(|(target, buttons, _)| bfs(target, &buttons))
            .sum()
    }

    pub fn p2(s: &str) -> u64 {
        parse(s)
            .filter_map(|(_, buttons, req)| solve(buttons, req))
            .sum()
    }

    fn solve(buttons: Vec<i32>, req: Vec<i32>) -> Option<u64> {
        let mut problem = Problem::new(OptimizationDirection::Minimize);
        let vars = (0..buttons.len())
            .map(|_| problem.add_integer_var(1.0, (0, i32::MAX)))
            .collect_vec();
        for (constraint, &req) in req.iter().enumerate() {
            let mut equation = LinearExpr::empty();
            for (variable, &butt) in buttons.iter().enumerate() {
                if butt & (1 << constraint) > 0 {
                    equation.add(vars[variable], 1.0);
                }
            }
            problem.add_constraint(equation, ComparisonOp::Eq, f64::from(req));
        }
        Some(problem.solve().ok()?.objective().round() as u64)
    }

    fn bfs(target: i32, buttons: &[i32]) -> u64 {
        use std::collections::VecDeque;

        let mut queue = VecDeque::from([(0, 0)]);
        let mut seen = FxHashSet::default();
        seen.insert(0);
        while let Some((node, step)) = queue.pop_front() {
            if node == target {
                return step;
            }
            for &butt in buttons {
                let next = node ^ butt;
                if seen.insert(next) {
                    queue.push_back((next, 1 + step));
                }
            }
        }
        0
    }
}

fn parse(s: &str) -> impl Iterator<Item = (i32, Vec<i32>, Vec<i32>)> {
    s.trim().lines().map(parse_line)
}

fn parse_line(line: &str) -> (i32, Vec<i32>, Vec<i32>) {
    let mut target = 0;
    let mut buttons = vec![];
    let mut jolts = vec![];
    for chunk in line.trim().split_ascii_whitespace() {
        match chunk.bytes().next() {
            Some(b'[') => {
                // emm endianness?
                for b in chunk.bytes().rev() {
                    if b == b'.' {
                        target = (target << 1) | 0;
                    } else if b == b'#' {
                        target = (target << 1) | 1;
                    }
                }
            }
            Some(b'(') => {
                let curr = chunk
                    .trim_matches(['(', ')'])
                    .split(',')
                    .fold(0, |acc, button| {
                        let bit = button.parse::<i32>().unwrap();
                        acc | (1 << bit)
                    });
                buttons.push(curr);
            }
            Some(b'{') => {
                for cost in chunk.trim_matches(['{', '}']).split(',') {
                    let cost = cost.parse().unwrap();
                    jolts.push(cost);
                }
            }
            _ => (),
        }
    }
    (target, buttons, jolts)
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
    [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
    [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(bfs_and_ilp::p1(SAMPLE), 7);
        assert_eq!(bfs_and_ilp::p1(INPUT), 477);

        assert_eq!(bfs_and_ilp::p2(SAMPLE), 33);
        assert_eq!(bfs_and_ilp::p2(INPUT), 17970);

        assert_eq!(bifurcate::p1(SAMPLE), 7);
        assert_eq!(bifurcate::p1(INPUT), 477);

        assert_eq!(bifurcate::p2(SAMPLE), 33);
        assert_eq!(bifurcate::p2(INPUT), 17970);
    }
}
