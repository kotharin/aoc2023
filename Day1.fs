namespace Day1

open System
open System.IO

(* 
    Day1 Part1 of 2023 AOC
*)
module Part1 =

    let incrBy1 x = x + 1
    let decrBy1 x = x - 1
    let getFirstAndLastDigit (line:string) =
        
        let rec getFirstNumberRec (s:string) pos fn =
            let charAt = (char)(s.Substring(pos, 1))
            if ((charAt >= '0') && (charAt <= '9')) then
                (int)charAt - 48
            else
                getFirstNumberRec s (fn pos) fn

        let first = getFirstNumberRec line 0 incrBy1
        let last = getFirstNumberRec line (line.Length - 1) decrBy1

        (first*10) + last

    let solution inputFile = 
        let lines = File.ReadAllLines(inputFile)

        lines 
        |> Array.map(fun l -> getFirstAndLastDigit l)
        |> Array.sum


