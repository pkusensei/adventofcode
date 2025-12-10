use std::collections::VecDeque;

use fxhash::FxHashSet;
use itertools::Itertools;
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

fn parse(s: &str) -> impl Iterator<Item = (i32, Vec<i32>, Vec<i32>)> {
    s.trim().lines().map(parse_line)
}

fn parse_line(line: &str) -> (i32, Vec<i32>, Vec<i32>) {
    let mut target = 0;
    let mut buttons = vec![];
    let mut req = vec![];
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
                    req.push(cost);
                }
            }
            _ => (),
        }
    }
    (target, buttons, req)
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
        assert_eq!(p1(SAMPLE), 7);
        assert_eq!(p1(INPUT), 477);

        assert_eq!(p2(SAMPLE), 33);
        assert_eq!(p2(INPUT), 17970);
    }
}
