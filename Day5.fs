namespace Day5

open System.IO

module Shared =
    type RangeInfo = {
        SourceStart: int64
        DestinationStart: int64
        Range: int64
    } with
        static member parse (line: string)=
            let chunks = line.Split([|' '|])
            let dest = (int64)chunks.[0]
            let src = (int64)chunks.[1]
            let rng = (int64)chunks.[2]

            {SourceStart = src; DestinationStart = dest; Range = rng}
        static member empty =
            {RangeInfo.SourceStart = -1L; RangeInfo.DestinationStart = -1L; Range = 0L}
        
        static member getDestination  rangeInfo source  =
            if ((source >= rangeInfo.SourceStart) && (source <= (rangeInfo.SourceStart + rangeInfo.Range - 1L))) then
                // get the diff and then add to destination
                let diff = source - rangeInfo.SourceStart
                Some(rangeInfo.DestinationStart + diff)
            else
                None
    
    type MapInfo = {
        Ranges:RangeInfo list
        Map: Map<int64, int64>
        Type: string
    } with
        static member addRange mapInfo range =
            let newRanges = range::mapInfo.Ranges
            {mapInfo with Ranges = newRanges}
        
        static member empty =
            {Ranges = List.empty; Map = Map.empty; Type = ""}

        static member getDestination map source =
            let ranges = map.Ranges

            ranges
            |> List.map RangeInfo.getDestination
            |> List.tryPick (fun rangeFunc -> rangeFunc source)
            |> Option.defaultValue source


    type Data = {
        Seeds: int64 array
        Maps: Map<int, MapInfo>
    } with
        static member empty =
            {Seeds = Array.empty; Maps = Map.empty}

        static member addSeeds data (seeds:string) =
            let chunks = seeds.Split([|' '|])
            let seedInfo =
                chunks
                |> Array.map(int64)
            {data with Seeds = seedInfo}

        static member addToRangeMap data mapIndex mapType info =
            let rangeInfo = RangeInfo.parse info

            let map = 
                Map.tryFind mapIndex data.Maps 
                |> Option.defaultValue {MapInfo.empty with Type=mapType}

            let newMapInfo = MapInfo.addRange map rangeInfo

            // update the maps
            let newMaps = Map.add mapIndex newMapInfo data.Maps

            {data with Maps = newMaps}

    let getFinalDestination maxMaps (maps:Map<int, MapInfo>) seed =
        
        // iterate through the maps
        [|1..maxMaps|]
        |> Array.fold (fun nextValue mapIndex ->

            let map = Map.find mapIndex maps

            let dest = MapInfo.getDestination map nextValue

            dest
        ) seed

module Part1 = 

    open Shared

    let solution inputFile =
        let lines = File.ReadAllLines(inputFile)

        let data, _, _ = 
            lines
            |> Array.fold( fun (data, mapType, mapIndex) line ->

                let x = 
                    if (line.Trim() = "") then
                        // separator
                        // reset the maptype and increment the mapindex
                        (data, "", (mapIndex + 1))
                    else    
                        if (line.IndexOf(":") >= 0) then
                            // new section
                            let chunks = line.Split([|':'|])
                            let newMapType = chunks.[0]
                            let payload = chunks.[1].Trim()
                            if (newMapType = "seeds") then
                                // get the seeds info
                                let newData = Data.addSeeds data payload
                                (newData, "", 0)
                            else
                                (data, newMapType, mapIndex)
                        else
                            // line of map data
                            // add to existing map in data
                            let newData = Data.addToRangeMap data mapIndex mapType (line.Trim())
                            (newData, mapType, mapIndex)
                x
            ) (Data.empty, "", 0)

        // Go through each seed and get their final destination
        let maxMaps = data.Maps.Count

        data.Seeds
        |> Array.map(getFinalDestination maxMaps data.Maps)
        |> Array.min

module Part2 = 

    open Shared

    let solution inputFile =

        let lines = File.ReadAllLines(inputFile)

        let data, _, _ = 
            lines
            |> Array.fold( fun (data, mapType, mapIndex) line ->

                let x = 
                    if (line.Trim() = "") then
                        // separator
                        // reset the maptype and increment the mapindex
                        (data, "", (mapIndex + 1))
                    else    
                        if (line.IndexOf(":") >= 0) then
                            // new section
                            let chunks = line.Split([|':'|])
                            let newMapType = chunks.[0]
                            let payload = chunks.[1].Trim()
                            if (newMapType = "seeds") then
                                // get the seeds info
                                let newData = Data.addSeeds data payload
                                (newData, "", 0)
                            else
                                (data, newMapType, mapIndex)
                        else
                            // line of map data
                            // add to existing map in data
                            let newData = Data.addToRangeMap data mapIndex mapType (line.Trim())
                            (newData, mapType, mapIndex)
                x
            ) (Data.empty, "", 0)

        // Go through each seed and get their final destination
        let maxMaps = data.Maps.Count
    
        let seedRanges,_,_ =
            data.Seeds
            |> Array.fold( fun (list, pos, prevValue) seed ->
                if (pos%2 = 0) then
                    let newRange = [|prevValue..(prevValue + seed - 1L)|]
                    let newList = newRange::list
                    (newList, pos+1, seed)
                else
                    (list, pos + 1, seed)
            ) (List.empty, 1, 0L)
        // Loop through ranges and get destination
        seedRanges
        |> List.map(fun range ->
            let mr =
                range
                |> Array.Parallel.map(fun range -> 
                    let ret = getFinalDestination maxMaps data.Maps range
                    printfn "range done"
                    ret)
                |> Array.min
            mr
        )
        |> List.min
