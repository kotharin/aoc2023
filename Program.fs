﻿module Program =
    
    let [<EntryPoint>] main _ =
        
        let answer = Day9.Part2.solution "Day9-1.txt"
        printfn "%i" answer
        //let a = Day3.Part1.getAdjacentPositions 9 9 {X = 4; Y = 5}
        //printfn "a:%A" a
        0
