module Program =

    open Day6
    
    let [<EntryPoint>] main _ =
        
        let answer = Day6.Part2.solution "Day6-1.txt"
        printfn "%i" answer
        //let a = Day3.Part1.getAdjacentPositions 9 9 {X = 4; Y = 5}
        //printfn "a:%A" a
        0
