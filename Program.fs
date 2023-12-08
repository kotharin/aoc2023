module Program =

    open Day1
    open Day2
    open Day3
    let [<EntryPoint>] main _ =
        
        let answer = Day3.Part1.solution "Day3-1.txt"
        printfn "%i" answer
        //let a = Day3.Part1.getAdjacentPositions 9 9 {X = 4; Y = 5}
        //printfn "a:%A" a
        0
