open System.IO;

(*
    Based on the string provided in 'op', retrun the result of that 
    operation. E.x: 'ADD' will add numbers 'a' and 'b', 'NOT' will invert 
    number 'a'.
*)
let matchOp a b op : uint16 = 
    match op with
    | "AND" -> a &&& b
    | "OR" -> a ||| b
    | "LSHIFT" -> a <<< int32 b
    | "RSHIFT" -> a >>> int32 b
    | "NOT" -> ~~~a
    | _ -> raise(System.NotSupportedException("Unknown operation provided"))

let rec getValue (wires : Map<string, uint16>) key commands =
    try
        wires.Add(key, uint16 key)
    with
    | _ -> 
        match wires.TryFind key with
        | Some(x) -> wires
        | None -> processCommandWithTarget wires commands key commands
and processCommandWithTarget wires commands target state =
    match state with
    | command::tail -> 
        match command with
        // These three only work if they are for the target wire.
        | (out, left, None, None) when out = target -> 
            // Handle normal assignments.
            let leftVal = getValue wires left commands
            leftVal.Add(out, leftVal.[left])
        | (out, left, None, op) when out = target ->
            // Handle urany operations (NOT, etc.)
            let leftVal = getValue wires left commands
            let result = matchOp leftVal.[left] 0us op.Value
            leftVal.Add(out, result)
        | (out, left, right, op) when out = target ->
            // Handle binary operations (AND, OR, etc.)
            let leftVal = getValue wires left commands
            let rightVal = getValue leftVal right.Value commands
            let result = matchOp rightVal.[left] rightVal.[right.Value] op.Value
            rightVal.Add(out, result)
        // Continue building up the mapping of wires if this is not the target.
        | (_, _, _, _) -> processCommandWithTarget wires commands target tail
    | [] -> Map.empty

(*
    Parse a string into a tuple by breaking it down into a list of elements 
    (delimited by ' ') and pattern matching that element list.
*)
let lineToTuples (line : string) = 
    let parts = line.Split([|' '|],  System.StringSplitOptions.RemoveEmptyEntries) |> Seq.toList
    match parts with
    | [a; "->"; b] -> (b, a, None, None)
    | [a; op; b; "->"; c] -> (c, a, Some(b), Some(op))
    | ["NOT"; a; "->"; b] -> (b, a, None, Some("NOT"))
    | _ -> raise(System.NotSupportedException("Could not process line"))

(*
    Convert a list of strings to a list of tuples representing the strings 
    split on ' '.
*)
let rec readLines lines =
    match lines with
    | head :: tail -> (lineToTuples head) :: readLines tail
    | [] -> []

let readFile filePath = 
    let lines = System.IO.File.ReadAllLines filePath |> Seq.toList
    readLines lines

// Perform one run to get our initial value for this task.
let args = fsi.CommandLineArgs
let result = readFile args.[1]
let r = processCommandWithTarget Map.empty result args.[2] result
// Use the answer from the previous execution as a base for the new answer.
let modified = Map.empty.Add(args.[3], r.[args.[2]])
let r2 = processCommandWithTarget modified result args.[2] result
printfn "Value of wire %s: %d" args.[2] (r2.TryFind args.[2]).Value
