module Program =

    open Day1
    open Day2
    let [<EntryPoint>] main _ =
        
        let answer = Day2.Part2.solution "Day2-2.txt"
        printfn "%i" answer
        0
