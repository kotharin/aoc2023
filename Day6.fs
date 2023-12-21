namespace Day6

open System.IO
module Part1 =

    // beat:[|33; 49; 14; 26|] 588588
    // beat:[|33; 49; 14; 26|]

    let waysToBeatMaxDistance time maxDistance =
        
        // only need o do half since the numbers
        // repeat after the half way point
        let timeIntervals = [|1..time/2|]

        let count =
            timeIntervals
            |> Array.map(fun ti ->
                // Get distance the boat can travel 
                // if button pushed for the given time interval
                ti * (time - ti)
            )
            |> Array.filter(fun d -> d > maxDistance)
            |> Array.length

        if (time%2 = 0) then
            count*2 - 1
        else
            count*2
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
                                Some ((int)(s.Trim()))
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
        