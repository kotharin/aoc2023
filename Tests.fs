module Tests

    open Xunit

    [<Fact>]
    let ``Day1 Part1`` () =
        let answer = Day1.Part1.solution "Day1-1.txt"

        Assert.Equal(54667, answer)

    [<Fact>]
    let ``Day1 Part2`` () =
        let answer = Day1.Part2.solution "Day1-2.txt"

        Assert.Equal(54203, answer)

    [<Fact>]
    let ``Day2 Part1`` () =
        let answer = Day2.Part1.solution "Day2-1.txt"

        Assert.Equal(2169, answer)

    [<Fact>]
    let ``Day2 Part2`` () =
        let answer = Day2.Part2.solution "Day2-2.txt"

        Assert.Equal(60948, answer)

    [<Fact>]
    let ``Day4 Part1`` () =
        let answer = Day4.Part1.solution "Day4-1.txt"

        Assert.Equal(20117, answer)

    [<Fact>]
    let ``Day4 Part2`` () =
        let answer = Day4.Part2.solution "Day4-1.txt"

        Assert.Equal(13768818, answer)

    [<Fact>]
    let ``Day5 Part1`` () =
        let answer = Day5.Part1.solution "Day5-1.txt"

        Assert.Equal(993500720L, answer)

    (*
    [<Fact>]
    let ``Day5 Part2`` () =
        let answer = Day5.Part2.solution "Day5-1.txt"

        Assert.Equal(46L, answer)
    *)
    [<Fact>]
    let ``Day6 Part1`` () =
        let answer = Day6.Part1.solution "Day6-1.txt"

        Assert.Equal(588588, answer)


    [<Fact>]
    let ``Day6 Part2`` () =
        let answer = Day6.Part2.solution "Day6-1.txt"

        Assert.Equal(34655848, answer)

    [<Fact>]
    let ``Day8 Part1`` () =
        let answer = Day8.Part1.solution "Day8-1.txt"

        Assert.Equal(16409, answer)

    [<Fact>]
    let ``Day9 Part1`` () =
        let answer = Day9.Part1.solution "Day9-1.txt"

        Assert.Equal(1731106378, answer)
