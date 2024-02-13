namespace Day8

module Shared =

    type Node = {
        Label: string
        Left: string
        Right: string
        EndsWithZ: bool
    } with 
        static member parse (line:string) =
            // split on the =
            let parts = line.Split([|'='|])
            let label = parts[0].Trim()
            // parse thes second part
            let moreParts = parts[1].Replace("(", "").Replace(")", "").Split([|','|])
            let left, right = moreParts[0].Trim(), moreParts[1].Trim()
            let ewz =
                if (label.Substring(2,1) = "Z") then
                    true
                else false
            {Node.Label = label; Left = left; Right = right; EndsWithZ = ewz}

    // traverse to the next node based on teh instruction
    let traverseInstruction instruction nodesMap node =
        let nextNodeLabel =
            if (instruction = 'R') then
                // get the right node
                node.Right
            else node.Left
        Map.find nextNodeLabel nodesMap

module Part1 =

    open System.IO
    open Shared
    let countStepTillEnd (instructions:char array) nodesMap start finish =

        let rec countTillFinish insPos currentNode count =
            if (currentNode = finish) then
                count
            else
                // get the next instruction
                let newInsPos =
                    if (insPos >= instructions.Length) then
                        // Reset back to 0
                        0
                    else insPos
                let newInstruction = instructions[newInsPos]
                let nextNode = traverseInstruction  newInstruction nodesMap currentNode
                countTillFinish (newInsPos + 1) nextNode (count + 1)

        countTillFinish 0 start 0
        
    let solution inputFile =
        let lines = File.ReadAllLines(inputFile)

        let instructions = lines[0].ToCharArray()

        let nodesMap = 
            Array.skip 2 lines
            |> Array.map(fun l  ->
                let node = Node.parse l
                node.Label, node)
            |> Map.ofArray

        let startingNode = Map.find "AAA" nodesMap
        let finishingNode = Map.find "ZZZ" nodesMap

        let steps = countStepTillEnd instructions nodesMap startingNode finishingNode

        steps

module Part2 =

    open System.IO
    open Shared
    open FSharp.Collections
    open System

    let allNodesEndWith nodes =
        let compliantNodes =
            nodes
            |> Array.Parallel.filter (fun n -> n.EndsWithZ)

        let cl = compliantNodes.Length
        if ((cl > 0) && (compliantNodes.Length % 4 = 0)) then
            printfn "----- ewz: %i" compliantNodes.Length

        compliantNodes.Length = nodes.Length

    let countSteps (instructions:char array) nodesMap startingNodes =
        
        let rec countAllSteps insPos currentNodes count =
            if ((count> 0) && (allNodesEndWith currentNodes)) then
                printfn "end node: %A" currentNodes
                count
            else
                //if (count%1000000 = 0) then
                    //printfn "step count:%i" count
                // move nodes one more instruction
                let newInsPos =
                    if (insPos >= instructions.Length) then
                        0
                    else insPos
                let newInstruction = instructions[newInsPos]

                let newCurrentNodes =
                    currentNodes
                    |> Array.Parallel.map(fun node ->
                        traverseInstruction newInstruction nodesMap node
                    )
                //printfn "new nodes:%A" newCurrentNodes
                countAllSteps (newInsPos + 1) newCurrentNodes (count + 1)
        
        countAllSteps 0 startingNodes 0

    let solution inputFile =
        let lines = File.ReadAllLines(inputFile)

        let instructions = lines[0].ToCharArray()

        let nodesList, allANodes = 
            Array.skip 2 lines
            |> Array.fold(fun (nodes, allANodes) l ->
                let node = Node.parse l
                let newNodes =  (node.Label, node)::nodes
                let newAllANodes =
                    if (node.Label.Substring(2,1) = "A") then
                        node::allANodes
                    else allANodes

                newNodes, newAllANodes
            ) (List.empty, List.empty)

        let nodesMap = Map.ofList nodesList
        let allANodesArray = allANodes |> Array.ofList
        printfn "starting nodes: %i" (allANodesArray.Length)

        
        let commonCycle =
            allANodesArray
            |> Array.map(fun node ->
                countSteps instructions nodesMap [|node|]
            )

        // find the LCM of the below numbers
        printfn "cc:%A" commonCycle

        0