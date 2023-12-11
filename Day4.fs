namespace Day4

open System
open System.IO
module Part1 = 

    type Card = {
        Id: string
        WinningNumbers: Set<int>
        PlayerNumbers: Set<int>
    } with
        static member parse (line:string) =
            // getthe card id and the rest
            let data = line.Split([|':'|])
            let cardId = data.[0]
            // split the numbers into win/have
            let nums = data.[1].Split([|'|'|])
            //printfn "nums:%A" nums
            let winNum =
                nums.[0].Trim().Split([|' '|])
                |> Array.fold( fun s n ->
                    if (n.Trim() <> "") then
                        let num = (int)(n.Trim())
                        Set.add num s
                    else s
                ) Set.empty

            let playNum =
                nums.[1].Trim().Split([|' '|])
                |> Array.fold( fun s n ->
                    if (n.Trim() <> "") then
                        let num = (int)(n.Trim())
                        Set.add num s
                    else s
                ) Set.empty
            
            {Id = cardId; WinningNumbers = winNum; PlayerNumbers = playNum}
        static member points(card:Card) =
            let commonCount = (Set.intersect card.PlayerNumbers card.WinningNumbers) |> Set.count
            int(Math.Pow(2.0, ((float)commonCount - 1.0)))
    let solution inputFile =

        let lines = File.ReadAllLines(inputFile)

        lines 
        |> Array.map(Card.parse)
        |> Array.map(fun c ->
            c.Id, (Card.points c)
        ) |> Array.sumBy(snd)