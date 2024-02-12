namespace Day7

module Shared =

    type Rank =
        | HighCard = 1
        | OnePair = 2
        | TwoPair = 3
        | ThreeOfAKind = 4
        | FullHouse = 5
        | FourOfAKind = 6
        | FiveOfAKind = 7

    let is2PairOr3ofAKind (cards:string array) =
        let rec is2or3 (remainingCards:string array) =
            // take first card and remove all instances in the hand
            let first = remainingCards.[0]
            let newRemainingCards =
                remainingCards
                |> Array.filter (fun c -> c <> first)
            if (remainingCards.Length = 5) then
                if (newRemainingCards.Length = 2) then
                    Rank.ThreeOfAKind
                elif (newRemainingCards.Length = 3) then
                    Rank.TwoPair
                else
                    is2or3 newRemainingCards
            else
                if (newRemainingCards.Length = 2) then
                    Rank.TwoPair
                else
                    Rank.ThreeOfAKind
        is2or3 cards

    let getRank (cards:string array) =
        let set = Set.ofArray cards

        match Set.count set with
        | 1 -> Rank.FiveOfAKind
        | 2 -> 
            // four of a kind or full house
            // Remove any one of the cards
            // If what remains is a of length 1 or 4 
            // its 4 of a kind
            let replace = Set.minElement set
            let replacedCards =
                cards
                |> Array.filter(fun c -> c = replace)
            if ((replacedCards.Length = 4) || (replacedCards.Length = 1)) then
                Rank.FourOfAKind
            else
                Rank.FullHouse
        | 3 -> 
            // Three of a kind or 2 pair
            is2PairOr3ofAKind cards
        | 4 ->  Rank.OnePair
        | _ -> Rank.HighCard

    type Hand = {
        Cards: string array
        Bid: int
        TypeRank: Rank
    } with
        static member parse (line:string) =
            let chunks = line.Split([|' '|])

            let cards =
                chunks.[0].Trim().ToCharArray()
                |> Array.map(string)
            
            let bid = int(chunks.[1].Trim())

            let rank = getRank cards

            {Cards = cards; Bid = bid; TypeRank = rank}
    let getCardValue (card:string) =
        if (card = "T") then
            10
        elif (card = "J") then
            11
        elif (card = "Q") then
            12
        elif (card = "K") then
            13
        elif (card = "A") then
            14
        else
            (int)card
        
module Part1 =
    open System.IO
    open Shared

    
    //let rankHands (hands:Hand[]) =
        
        //let rec sortAndGroupuByCol col (unsorted:(string*) (sorted: Hand list) =
            // If sorting by the last column
            // the array should be fully sorted
    //        if (col = 4) then
    //            let tsa = 

    //    ()
    
    let solution inputFile =
        
        let lines = File.ReadAllLines inputFile

        let hands =
            lines
            |> Array.map(Hand.parse)
            |> Array.groupBy (fun hand -> hand.TypeRank)
            |> Array.sortBy fst
        printfn "hands:%A" hands
        0