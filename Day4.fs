namespace Day4

open System
open System.IO

module Shared =

    type Card = {
        Id: int
        WinningNumbers: Set<int>
        PlayerNumbers: Set<int>
    } with
        static member parse (line:string) =
            // getthe card id and the rest
            let data = line.Split([|':'|])
            let cardId = (int) (data.[0].Replace("Card","").Trim())
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
        static member childCards (card:Card) =
            let count = (Set.intersect card.PlayerNumbers card.WinningNumbers) |> Set.count
            // start with the current card id and add above count to get child Ids
            [(card.Id + 1)..(card.Id + count)]

module Part1 = 

    open Shared
    let solution inputFile =

        let lines = File.ReadAllLines(inputFile)

        lines 
        |> Array.map(Card.parse)
        |> Array.map(fun c ->
            c.Id, (Card.points c)
        ) |> Array.sumBy(snd)

module Part2 = 

    open Shared

    let getChildrenCount (childCardsCache:Map<int, List<int>>) (childCountMap:Map<int, int>) (card:Card) =
        let childCards = Map.find card.Id childCardsCache
        let childCardsCount = 
            childCards
            |> List.fold (fun count cc ->
                (Map.find cc childCountMap) + count
            ) 0
        let thisCardCount = childCardsCount + 1
        Map.add card.Id thisCardCount childCountMap, thisCardCount
    let solution inputFile =
        let lines = File.ReadAllLines(inputFile)

        let cards = 
            lines
            |> Array.map(Card.parse)

        let allCardsMap =
            cards
            |> Array.map(fun c -> c.Id, c)
            |> Map.ofArray

        // Get the child cards cache
        let childCardsCacheMap =
            cards
            |> Array.fold(fun map card ->
                let childList = Card.childCards card
                // add to the cache
                Map.add card.Id childList map 
            ) Map.empty

        let rowCount = Array.length lines

        // get all children for each card and add
        let countMap, totalCount =
            [|rowCount .. -1 .. 1|]
            |> Array.fold(fun  (countMap, countTotal) c ->
                let card = Map.find c allCardsMap
                // get child count
                let newMap, childCount = getChildrenCount childCardsCacheMap countMap card
                newMap, (countTotal + childCount)
            ) (Map.empty,0)
        totalCount