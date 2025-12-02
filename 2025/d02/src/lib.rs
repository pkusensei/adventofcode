pub fn solve(s: &str) -> [u64; 2] {
    let mut p1 = 0;
    let mut p2 = 0;
    for pair in s.split(',') {
        let Some((a, b)) = pair.trim().split_once('-') else {
            continue;
        };
        let [a, b] = [a, b].map(|x| x.parse::<u64>().unwrap());
        for num in a..=b {
            let width = 1 + num.ilog10();
            for i in (1..=width / 2).rev().filter(|i| width % i == 0) {
                let p = 10_u64.pow(i);
                let rem = num % p;
                let mut x = num / p;
                while x > rem && x % p == rem {
                    x /= p;
                }
                if x == rem {
                    p2 += num;
                    if 2 * i == width {
                        p1 += num;
                    }
                    break;
                }
            }
        }
    }
    [p1, p2]
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

    const INPUT: &str = include_str!("../input");

    #[test]
    fn it_works() {
        assert_eq!(solve(SAMPLE), [1227775554, 4174379265]);
        assert_eq!(solve(INPUT), [26255179562, 31680313976]);
    }
}
