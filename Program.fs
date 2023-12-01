module Program =

    open Day1
    let [<EntryPoint>] main _ =
        
        let answer = Day1.Part1.solution "Day1-1.txt"
        printfn "%i" answer
        0
