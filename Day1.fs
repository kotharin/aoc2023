namespace Day1

open System
open System.IO

(* 
    Day1 Part1 of 2023 AOC
*)

module Shared = 
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

module Part1 =

    open Shared

    let solution inputFile = 
        let lines = File.ReadAllLines(inputFile)

        lines 
        |> Array.map(getFirstAndLastDigit)
        |> Array.sum

module Part2 =

    open Shared
    open System

    let numberMap =
        [|
            ("zero", 0);
            ("one", 1);
            ("two", 2);
            ("three", 3);
            ("four", 4);
            ("five", 5);
            ("six", 6);
            ("seven", 7);
            ("eight", 8);
            ("nine", 9);
        |]
    let replaceStringNumbers (line:string) =
        let newLine =
            Array.fold (fun (nl:string) num ->
                let sn, nv = num
                nl.Replace(sn, nv.ToString())
            ) line numberMap
        newLine
    let rec getFirstNumberRec (s:string) pos fn =
        if ((pos < s.Length) && (pos >= 0)) then
            let charAt = (char)(s.Substring(pos, 1))
            if ((charAt >= '0') && (charAt <= '9')) then
                Some ((int)charAt - 48) 
            else
                getFirstNumberRec s (fn pos) fn
        else
            None

    let rec getFirstDigitRec (line:string) leftIndex =
        // get first 5, last 5
        let charsToGet = if (line.Length <5) then line.Length else 5
        let ff = line.Substring(leftIndex,charsToGet)
        // replace the strings with humbers
        let replacedString = replaceStringNumbers ff

        // Find first number
        let dgt = getFirstNumberRec replacedString 0 incrBy1
        // if we have a number, return, else get next 5 digits
        let digit =
            match dgt with
            | Some d -> 
                d
            | _ -> 
                getFirstDigitRec line (leftIndex + 1)
        // convert digit in first 5 chars
        digit

    let rec getLastDigitRec (line:string) leftIndex =
        // get first 5, last 5
        let charsToGet = if (line.Length <5) then line.Length else 5
        let ff = line.Substring(leftIndex,charsToGet)
        // replace the strings with humbers
        let replacedString = replaceStringNumbers ff
        // Find firt number
        let dgt = getFirstNumberRec replacedString (replacedString.Length - 1) decrBy1
        // if we have a number, return, else get next 5 digits
        let digit =
            match dgt with
            | Some d -> d
            | _ -> getLastDigitRec line (leftIndex - 1)
        digit

    let getFirstAndLastDigit line =
        //printfn "line:%s" line
        let firstDigit = getFirstDigitRec line 0
        //printfn "fd:%i" firstDigit
        let rightStartIndex = if (line.Length < 5) then 0 else (line.Length - 5 )
        let lastDigit = getLastDigitRec line rightStartIndex
        //printfn "ld:%i" lastDigit
        (firstDigit * 10) +  lastDigit
    let solution inputFile =
        let lines = File.ReadAllLines(inputFile)

        lines
        |> Array.map(getFirstAndLastDigit)
        |> Array.sum
        

