module Program =

    open Day8
    
    let [<EntryPoint>] main _ =
        
        let answer = Day8.Part2.solution "Day8-1.txt"
        printfn "%i" answer
        //let a = Day3.Part1.getAdjacentPositions 9 9 {X = 4; Y = 5}
        //printfn "a:%A" a
        0
