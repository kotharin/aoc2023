namespace Day6

open System.IO

module Shared =
    let waysToBeatMaxDistance time (maxDistance:int64) =
        
        // only need o do half since the numbers
        // repeat after the half way point
        let timeIntervals = [|1..time/2|]

        let count =
            timeIntervals
            |> Array.fold(fun cnt ti ->
                // Get distance the boat can travel 
                // if button pushed for the given time interval
                let distance = ((int64)ti) * (int64)(time - ti)
                if (distance > maxDistance) then
                    cnt + 1
                else cnt
            ) 0

        if (time%2 = 0) then
            count*2 - 1
        else
            count*2

module Part1 =

    open Shared
    let solution inputFile =
        
        let lines = File.ReadAllLines(inputFile)

        let times, distances = 
            lines
            |> Array.fold( fun (times, distances) line ->
                let chunks = line.Split([|':'|])
                if (chunks.[0].Trim() = "Time") then
                    let allTimes =
                        chunks.[1].Trim().Split([|' '|])
                        |> Array.map(fun s -> 
                            if (s.Length > 0) then
                                Some ((int)(s.Trim()))
                            else None
                        )
                        |> Array.choose id
                    allTimes, distances
                else
                    let allDistabces =
                        chunks.[1].Trim().Split([|' '|])
                        |> Array.map(fun s -> 
                            if (s.Length > 0) then
                                Some ((int64)(s.Trim()))
                            else None
                        )
                        |> Array.choose id
                    times, allDistabces
            ) (Array.empty, Array.empty)

        times
        |> Array.mapi(fun index time ->
            waysToBeatMaxDistance time distances.[index]
        )        
        |> Array.fold(fun prod way ->
            prod * way
        ) 1

module Part2 =

    open System.IO
    open Shared
    
    let solution inputFile =
        
        let lines = File.ReadAllLines inputFile

        let time, distance =
            lines
            |> Array.fold( fun (t,d) line ->
                let chunks = line.Split([|':'|])
                if (chunks.[0].Trim() = "Time") then
                    let ft = (int)(chunks.[1].Trim().Replace(" ", ""))
                    ft, d
                else
                    let fd = (int64)(chunks.[1].Trim().Replace(" ", ""))
                    t, fd
            ) (0, 0L)

        waysToBeatMaxDistance time distance
        