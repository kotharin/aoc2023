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
