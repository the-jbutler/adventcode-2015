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
        tmp
    with
    | _ -> 
        match wires.TryFind(key) with
        | Some(x) -> printfn "value!"; x
        | None -> 
            printfn "No value :("
            let tmp = processCommandWithTarget wires commands key commands
            getValue tmp key commands
and processCommandWithTarget wires commands target state =
    match state with
    | command::tail -> 
        //printfn "Processing %A" command
        match command with
        | (out, left, None, None) when out = target -> 
            printfn "Found target '%s'" target
            let leftVal = getValue wires left commands
            let newWires = wires.Add(out, leftVal)
            newWires
        | (out, left, None, op) when out = target ->
            printfn "Found target '%s'" target
            let leftVal = getValue wires left commands
            let result = matchOp leftVal 0us op.Value
            let newWires = wires.Add(out, result)
            newWires
        | (out, left, right, op) when out = target ->
            printfn "Found target '%s'" target
            let leftVal = getValue wires left commands
            let rightVal = getValue wires right.Value commands
            let result = matchOp leftVal rightVal op.Value
            let newWires = wires.Add(out, result)
            newWires
            //processCommandWithTarget newWires commands target tail
        | (_, _, _, _) -> processCommandWithTarget wires commands target tail
    | [] -> Map.empty

let processCommand commands =
    match commands with
    | command::tail -> 
        match command with
        | (out, left, None, None) -> 
            []
        | (out, left, None, op) ->
            []
        | (out, left, right, op) ->
            []
    | [] -> []

let linesToTuples (line : string) = 
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
        (linesToTuples head) :: readLines tail
    | [] -> []

let readFile filePath = 
    let lines = System.IO.File.ReadAllLines filePath |> Seq.toList
    readLines lines

let args = fsi.CommandLineArgs
let result = readFile args.[1]
System.Console.ReadLine() |> ignore
let r = processCommandWithTarget Map.empty result "dr" result

printfn "%A" result

//printfn "%s: %d" args.[2] (result.TryFind args.[2]).Value
//let r = Map.fold (fun map k v -> printfn "%s:\t%d" k v) () result