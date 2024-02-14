namespace Day9

module Part1 =

    open System
    open System.IO

    type Data = {
        StartingList: int list
        DifferencesList: int list list
    } 

    // check if all nums in list are 0's
    let isAllZeros nums =
        nums
        |> List.filter(fun n -> n <> 0)
        |> List.length = 0

    let generateDifferences numbers =
        let rec genNums nums diffs =
            if (isAllZeros nums) then
                diffs
            else
                // generate the diffs for this set of numbers and add to the list
                let diff = 
                    List.pairwise nums
                    |> List.map(fun (x,y) -> y - x)
                
                let newDiffList =
                    List.append diffs [diff]

                genNums diff newDiffList

        genNums numbers List.empty

    // Parse a single line of data and convert it into
    // the Data type.
    let parseDataLine (line:string) =
        let startingNumbers =
            line.Split([|' '|])
            |> Array.map(fun s ->
               (int)s 
            )|> List.ofArray

        let diffs = generateDifferences startingNumbers

        {Data.StartingList = startingNumbers; DifferencesList = diffs}

    // Use the last row (0's) and propogate the diff up
    // to get the prediction for the first row
    let predictNextValue (data:Data) =

        // Append the starting row to the diffs
        let allRows = data.StartingList::data.DifferencesList
        let diffCount = allRows.Length

        [|diffCount - 1..-1..0|]
        |> Array.fold( fun lastDiff row ->
            // Get the value of the last element in the current row list
            let cr = allRows.[row]
            let lastValue = cr.[cr.Length - 1]
            // calculate the new diff
            lastValue + lastDiff
        ) 0


    let solution inputFile =

        let lines = File.ReadAllLines(inputFile)

        lines
        |> Array.Parallel.map(parseDataLine)
        |> Array.fold(fun sum data ->
            sum + predictNextValue data
        ) 0
