module Program =

    open Day1
    let [<EntryPoint>] main _ =
        
        let answer = Day1.Part2.solution "Day1-2.txt"
        printfn "%i" answer
        0
