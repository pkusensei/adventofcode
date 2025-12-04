pub fn p1(s: &str) -> u64 {
    let grid = parse(s);
    let rows = grid.len() as i32;
    let cols = grid[0].len() as i32;
    let mut res = 0;
    for (r, row) in grid.iter().enumerate() {
        for (c, &v) in row.iter().enumerate() {
            if !v {
                continue;
            }
            let mut curr = 0;
            for [dr, dc] in utils::DELTAS8 {
                let nr = r as i32 + dr;
                let nc = c as i32 + dc;
                if (0..rows).contains(&nr)
                    && (0..cols).contains(&nc)
                    && grid[nr as usize][nc as usize]
                {
                    curr += 1;
                }
            }
            res += u64::from(curr < 4);
        }
    }
    res
}

pub fn p2(s: &str) -> u64 {
    let mut grid = parse(s);
    let rows = grid.len() as i32;
    let cols = grid[0].len() as i32;
    let mut res = 0;
    let mut del = vec![];
    loop {
        for (r, row) in grid.iter().enumerate() {
            for (c, &v) in row.iter().enumerate() {
                if !v {
                    continue;
                }
                let mut curr = 0;
                for [dr, dc] in utils::DELTAS8 {
                    let nr = r as i32 + dr;
                    let nc = c as i32 + dc;
                    if (0..rows).contains(&nr)
                        && (0..cols).contains(&nc)
                        && grid[nr as usize][nc as usize]
                    {
                        curr += 1;
                    }
                }
                if curr < 4 {
                    del.push([r, c]);
                }
            }
        }
        if del.is_empty() {
            break;
        }
        for [r, c] in del.drain(..) {
            res += 1;
            grid[r][c] = false;
        }
    }
    res
}

fn parse(s: &str) -> Vec<Vec<bool>> {
    s.lines()
        .filter_map(|v| {
            let v = v.trim();
            if v.is_empty() {
                None
            } else {
                Some(v.bytes().map(|b| b == b'@').collect())
            }
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "..@@.@@@@.
    @@@.@.@.@@
    @@@@@.@.@@
    @.@@@@..@.
    @@.@@@@.@@
    .@@@@@@@.@
    .@.@.@.@@@
    @.@@@.@@@@
    .@@@@@@@@.
    @.@.@@@.@.";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(p1(SAMPLE), 13);
        assert_eq!(p1(INPUT), 1537);

        assert_eq!(p2(SAMPLE), 43);
        assert_eq!(p2(INPUT), 8707);
    }
}
