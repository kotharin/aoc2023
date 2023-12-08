namespace Day3

open System.IO
module Part1 =

    type Position = {
        X: int
        Y: int
    } with
        static member isValidPosition maxX maxY (position:Position) =
            if ((position.X >=0) && (position.X <=maxX) && (position.Y >=0) && (position.Y <= maxY)) then
                true
            else
                false

    // Get adjacent positions
    let getAdjacentPositions maxX maxY (position:Position) =

        [|-1..1|]
        |> Array.fold (fun positions offX ->
            let newList =
                [|-1..1|]
                |> Array.fold(fun posns offY ->
                    let newPos = {X = position.X + offX; Y = position.Y + offY}
                    if ((Position.isValidPosition maxX maxY newPos) && (newPos <> position)) then
                        // add to the list
                        newPos::posns
                    else
                        posns
                ) (positions)
            newList
        ) (List.empty)

    let getAdjacentNumbersForPositions (numberPositions:Map<Position, int>) (positions: Position list) (sharedPositions:Map<Position, List<Position>>) =
        let adjacentNumbers, _ =
            positions
            |> List.fold(fun (adjNums, sharedNumPositions) adjPos ->
                // If this position doesn't exist in the shared positions
                // that has already been "used"
                if (not (Set.contains adjPos sharedNumPositions)) then
                    let aNums = Map.tryFind adjPos numberPositions
                    aNums
                    |> Option.map(fun n ->
                        let newList = n::adjNums
                        // add the adjacent positions to not be checked
                        let sp = Map.find adjPos sharedPositions
                        // Add the ist to the map of positions not to be checked again
                        let newSharedNumPositions = Set.union (Set.ofList sp) sharedNumPositions
                
                        let newAdjNums = n::adjNums
                        newAdjNums, newSharedNumPositions)
                    |> Option.defaultValue (adjNums, sharedNumPositions)
                else (adjNums, sharedNumPositions)

            ) (List.empty, Set.empty)
        adjacentNumbers
    // Check if there are adjacent symbols
    let symbolsAdjacentToPositions (symbolPositions:Map<Position, string>) (positions: Position list) =
        positions
        |> List.fold(fun symbolCount position ->
            if (Map.containsKey position symbolPositions) then
                symbolCount + 1
            else
                symbolCount
        ) 0
    // Add the number to the appropriate positions
    let addNumberToPosition (numChars:string) (lineNumber: int) (lastCharPosition:int) (numberPosition:Map<Position, int>) (positionsSharedByNumber:Map<Position, List<Position>>) =
        // get the number
        let number = (int)(numChars.Trim())
        // get the starting position of the first char of the number
        let startPosition = lastCharPosition - (numChars.Length - 1)

        // Create an array of the offsets and
        // add the positions to the map
        let np, nsp = 
            [|0..numChars.Length - 1|]
            |> Array.fold(fun (numMap, posSharingNum) offset ->
                let newPosition = {Position.X = startPosition + offset; Y = lineNumber}
                // Keep track of the positions that share the same number.
                let newPosSharingNum = newPosition::posSharingNum
                // Add to the map
                (Map.add newPosition number numMap, newPosSharingNum )
            ) (numberPosition, List.empty)

        // create a map of the numbers sharing a position
        let newSharedPositions =
            nsp
            |> List.fold(fun map sp ->
                Map.add sp nsp map
            ) positionsSharedByNumber

        np, newSharedPositions
    let parseLine (line:string) (lineNumber:int) (numberPosition:Map<Position, int>) (symbolPosition:Map<Position, string>) (sharedPositions:Map<Position, List<Position>>) =
        let lineChars = line.ToCharArray()

        let numPos, symPos, sharedPos, _, _ =
            lineChars
            |> Array.fold( fun (np,sp, nsp, charPos, (accumChars:string) ) lc ->
                if (lc = '.') then
                    // if there was accumulated chars, add the number to the map
                    // for ALL positions. Eg: if the number is 231, you have to add
                    // to the map for each position of the number (for 2 3 and 1)
                    if (accumChars.Length > 0) then
                        let newNumberPosition, newSharedPositions = addNumberToPosition accumChars lineNumber (charPos - 1) np nsp
                        (newNumberPosition, sp, newSharedPositions, (charPos + 1), "")
                    else
                        // go to next position
                        (np, sp, nsp, (charPos + 1), "")
                elif ((lc >= '0') && (lc <= '9')) then
                    // if its a number, start collecting the chars
                    let newAccumChars = accumChars + (string)lc
                    (np, sp, nsp,  (charPos + 1), newAccumChars)
                else
                    // symbol.
                    // if there was accumulated chars, add the number to the map
                    // for ALL positions.
                    let newNumberPositionMap, newSharedPositions =
                        if (accumChars.Length > 0) then
                            addNumberToPosition accumChars lineNumber (charPos - 1) np nsp
                        else
                            np, nsp
                    // Add symbol position to map
                    let symPos = {X=charPos; Y=lineNumber}
                    let newSymbolPositionMap = Map.add symPos ((string)(lc)) sp
                    (newNumberPositionMap, newSymbolPositionMap, newSharedPositions, (charPos + 1), "")

            ) (numberPosition, symbolPosition, sharedPositions, 0, "")
        numPos, symPos, sharedPos
    let solution inputFile =

        let lines = File.ReadAllLines(inputFile)
        // get the max cols (maxX) and maxY (num of lines)
        let maxX = lines.[0].Length - 1
        let maxY = lines.Length - 1

        let numberPositions, symbolPositions, sharedNumberPositions, _ = 
            lines
            |> Array.fold( fun (np, sp, nsp, lineNumber) line ->
                let newNP, newSP, newSharedPos = parseLine line lineNumber np sp nsp
                newNP, newSP, newSharedPos, (lineNumber + 1) 
            ) (Map.empty, Map.empty, Map.empty, 0)
        
        // for each symbol position, check if there is a number adjacent to it
        (*
        let nums = 
            symbolPositions
            |> Map.keys
            |> Seq.fold (fun adjacentNumbers symbolPosition ->
                let adjacentPositions = getAdjacentPositions maxX maxY symbolPosition
                // get the numbers at that position, if present
                let adjNums = getAdjacentNumbersForPositions numberPositions adjacentPositions sharedNumberPositions
                (List.append adjacentNumbers adjNums)
            ) (List.empty)
        printfn "nums: %A" nums
        Seq.sum nums
        *)

        
        let nums =
            numberPositions
            |> Map.keys
            |> Seq.fold (fun adjacentNumbers numberPosition ->
                let adjacentPositions = getAdjacentPositions maxX maxY numberPosition
                let symCount = symbolsAdjacentToPositions symbolPositions adjacentPositions
                if (symCount > 0) then
                    // add number
                    let num = Map.find numberPosition numberPositions
                    (num*symCount)::adjacentNumbers
                else
                    adjacentNumbers
            ) (List.empty)
        printfn "nums: %A" nums
        Seq.sum nums
        