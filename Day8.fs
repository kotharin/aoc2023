namespace Day8

module Part1 =

    open System.IO

    type Node = {
        Label: string
        Left: string
        Right: string
    } with 
        static member parse (line:string) =
            // split on the =
            let parts = line.Split([|'='|])
            let label = parts[0].Trim()
            // parse thes second part
            let moreParts = parts[1].Replace("(", "").Replace(")", "").Split([|','|])
            let left, right = moreParts[0].Trim(), moreParts[1].Trim()

            {Node.Label = label; Left = left; Right = right}

    // traverse to the next node based on teh instruction
    let traverseInstruction instruction nodesMap node =
        let nextNodeLabel =
            if (instruction = 'R') then
                // get the right node
                node.Right
            else node.Left
        Map.find nextNodeLabel nodesMap

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