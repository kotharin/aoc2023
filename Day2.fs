namespace Day2

open System
open System.IO

module Shared =

    type GameSet = {
        Red: int
        Blue: int
        Green: int
    } with
        static member Default =
            {Red =0; Blue = 0; Green = 0}
        // Parse color info
        // format 3 blue or 2 green
        static member parseColorInfo (colorData:string) =
            let parts = colorData.Trim().Split([|' '|])
            let quantity = int(parts.[0])
            let color = parts.[1].Trim()
            quantity, color
        
        // format: 2 blue, 3 green
        static member parseSetInfo (set:string) =
            let colorInfo = set.Split([|','|])
            let gameSet = 
                colorInfo
                |> Array.fold(fun gs ci ->
                    let quantity, color = GameSet.parseColorInfo ci
                    let newGS =
                        if (color = "red") then
                            {gs with Red=quantity}
                        elif (color = "green") then
                            {gs with Green=quantity}
                        else 
                            {gs with Blue=quantity}
                        
                    newGS
                ) GameSet.Default
            gameSet
        // parse the game sets info
        // format: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
        static member parse (sets:string) =
            let rgbSets = sets.Split([|';'|])
            rgbSets |> Array.map(GameSet.parseSetInfo)
        // validate
        static member isValid maxRed maxBlue maxGreen  (set:GameSet) =
            if ((set.Red <= maxRed) && (set.Blue <= maxBlue) && (set.Green <= maxGreen)) then
                true
            else
                false

        // get the color counts for the set needed to keep the set valid
        static member getColorCounts (set:GameSet) =
            set.Red, set.Blue, set.Green

    type Game = {
        Id: int
        Sets: GameSet[]
    } with
        static member parse (line:string) =
            let splitInfo = line.Split([|':'|])
            let gameId = (int)(splitInfo.[0].Split([|' '|]).[1])
            let sets = GameSet.parse splitInfo.[1]
            {
                Id = gameId
                Sets = sets
            }
        static member isValid maxRed maxBlue maxGreen (game:Game) =
            let allSetsValid =
                game.Sets
                |> Array.fold( fun valid set ->
                    let isValid = valid && GameSet.isValid maxRed maxBlue maxGreen set
                    isValid
                ) (true)
            //printfn "id:%i, valid:%b" game.Id allSetsValid
            game.Id, allSetsValid
        // get min color count to keep the game valid
        static member getColorCountPower (game:Game) =
            let reds, blues, greens =
                game.Sets
                |> Array.map(GameSet.getColorCounts)
                |> Array.unzip3
            Array.max(reds)*Array.max(blues)*Array.max(greens)  


module Part1 =

    open Shared
    let solution inputFile = 

        let maxRed,maxBlue, maxGreen = 12, 14, 13
        let isGameValid = Game.isValid maxRed maxBlue maxGreen
        let lines = File.ReadAllLines(inputFile)
        let invgsum =
            lines
            |> Array.map(Game.parse)
            |> Array.map (isGameValid)
            |> Array.filter(fun (gid,gvalid) -> 
                gvalid = true
            ) |> Array.sumBy(fst) 
        invgsum

module Part2 =
    open Shared
    let solution inputFile =
        let lines = File.ReadAllLines(inputFile)

        lines
        |> Array.map(Game.parse)
        |> Array.map(Game.getColorCountPower)
        |> Array.sum
        