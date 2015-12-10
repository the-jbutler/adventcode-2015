open System.IO;

let matchOp a b op : uint16 = 
    match op with
    | "AND" -> a &&& b
    | "OR" -> a ||| b
    | "LSHIFT" -> a <<< int32 b
    | "RSHIFT" -> a >>> int32 b
    | "NOT" -> ~~~a
    | _ -> raise(System.NotSupportedException("Unknown operation provided"))

let rec getValue (wires : Map<string, uint16>) key commands =
    printfn "Look for target '%s'" key
    try
        let tmp = uint16 key
        printfn "Found: %d" tmp
        let out = wires.Add(key, tmp)
        out
    with
    | _ -> 
        match wires.TryFind(key) with
        | Some(x) -> printfn "value!"; wires
        | None -> 
            printfn "No value :("
            let tmp = processCommandWithTarget wires commands key commands
            tmp
and processCommandWithTarget wires commands target state =
    match state with
    | command::tail -> 
        //printfn "Processing %A" command
        match command with
        | (out, left, None, None) when out = target -> 
            printfn "Found target '%s'" target
            let leftVal = getValue wires left commands
            let newWires = leftVal.Add(out, leftVal.[left])
            newWires
        | (out, left, None, op) when out = target ->
            printfn "Found target '%s'" target
            let leftVal = getValue wires left commands
            let result = matchOp leftVal.[left] 0us op.Value
            let newWires = leftVal.Add(out, result)
            newWires
        | (out, left, right, op) when out = target ->
            printfn "Found target '%s'" target
            let leftVal = getValue wires left commands
            let rightVal = getValue leftVal right.Value commands
            let result = matchOp rightVal.[left] rightVal.[right.Value] op.Value
            let newWires = rightVal.Add(out, result)
            newWires
        | (_, _, _, _) -> processCommandWithTarget wires commands target tail
    | [] -> Map.empty

let lineToTuples (line : string) = 
    let parts = line.Split([|' '|],  System.StringSplitOptions.RemoveEmptyEntries) |> Seq.toList
    match parts with
    | [a; "->"; b] -> 
        (b, a, None, None)
    | [a; op; b; "->"; c] -> 
        (c, a, Some(b), Some(op))
    | ["NOT"; a; "->"; b] -> 
        (b, a, None, Some("NOT"))
    | _ -> raise(System.NotSupportedException("Could not process line"))

let rec readLines lines =
    match lines with
    | head :: tail -> 
        (lineToTuples head) :: readLines tail
    | [] -> []

let readFile filePath = 
    let lines = System.IO.File.ReadAllLines filePath |> Seq.toList
    readLines lines

let args = fsi.CommandLineArgs
let result = readFile args.[1]
System.Console.ReadLine() |> ignore
let r = processCommandWithTarget Map.empty result args.[2] result

//printfn "%A" result
printfn "%s: %d" args.[2] (r.TryFind args.[2]).Value
