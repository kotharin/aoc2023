module Program =

    open Day1
    open Day2
    let [<EntryPoint>] main _ =
        
        let answer = Day2.Part1.solution "Day2-1.txt"
        printfn "%i" answer
        0
