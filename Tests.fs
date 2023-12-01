module Tests

    open Xunit

    [<Fact>]
    let ``Day1 Part1`` () =
        let answer = Day1.Part1.solution "test"

        Assert.Equal("hello test", answer)

