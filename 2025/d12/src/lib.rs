pub fn solve(s: &str) -> u64 {
    let mut res = 0;
    for [a, b, sum] in parse(s) {
        let [a, b] = [a, b].map(|v| v / 3);
        res += u64::from(a * b >= sum);
    }
    res
}

fn parse(s: &str) -> impl Iterator<Item = [i32; 3]> {
    s.trim()
        .lines()
        .map(|line| line.trim())
        .skip_while(|line| line.is_empty() || line.ends_with(':') || line.contains(['#', '.']))
        .map(|line| {
            let mut it = line
                .split(|c: char| !c.is_ascii_digit())
                .filter_map(|s| if !s.is_empty() { s.parse().ok() } else { None });
            let mut shape = [0; 3];
            shape[0] = it.next().unwrap();
            shape[1] = it.next().unwrap();
            shape[2] = it.sum();
            shape
        })
}

#[cfg(test)]
mod tests {
    use super::*;

    const _SAMPLE: &str = "0:
    ###
    ##.
    ##.

    1:
    ###
    ##.
    .##

    2:
    .##
    ###
    ##.

    3:
    ##.
    ###
    ##.

    4:
    ###
    #..
    ###

    5:
    ###
    .#.
    ###

    4x4: 0 0 0 0 2 0
    12x5: 1 0 1 0 2 2
    12x5: 1 0 1 0 3 2";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        // assert_eq!(solve(SAMPLE), 2);
        assert_eq!(solve(INPUT), 433);
    }
}
