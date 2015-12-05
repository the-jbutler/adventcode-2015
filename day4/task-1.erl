#!/usr/bin/escript

main(Args) when length(Args) == 1 ->
	R = findNumber(Args, 1),
	io:format("Valid Number ~p~n", R);
main(Args) when length(Args) == 2 ->
	[Passphrase|Tail] = Args,
	[Cores|_] = Tail,
	R = findNumber(Passphrase, Cores),
	io:format("Valid Number ~p~n", R);
main(_) ->
	usage().


usage() ->
	io:format("provide a passphrase~n").


findNumber(P, C) ->
	io:format("Finding lowest valid number for ~s with ~p workers~n", [P, C]),
	launchProcess(P, C, C).


launchProcess(P, C, N) when N > 1 ->
	%% Do work
	process(P, C, N),
	launchProcess(P, C, N-1);
launchProcess(P, C, N) ->
	process(P, C, 0),
	io:format("wat").


process(P, C, O) ->
	MD = getMD5(P ++ integer_to_list(O)),
	T = isValid(MD, 0),
	case T of
		true ->
			O;
		false ->
			process(P, C, O+C)
	end.

%% Check the specified string to see if it's a valid hash
%% for our needs.
isValid([H|T], I) when [H] =:= "0" ->
%%	io:format("~p  ~s~s~n", [I, [H], T]),
	isValid(T, I+1);
isValid(_, I) when I >= 5 ->
	true;
isValid(_, I) when I < 5 ->
	false.

%% Gets the MD5 hash for the specified string.
getMD5(P) ->
	io:format("~s~n", [P]),
	MD = erlang:md5(P),
	MDList = binary_to_list(MD),
	lists:flatten(listsToHex(MDList)).

%% Process an entire list of numbers into hex.
listsToHex(L) ->
	lists:map(fun(X) -> intToHex(X) end, L).

%% Convert an integer into a hexadecimal string.
intToHex(I) when I < 256 ->
	[hex(I div 16), hex(I rem 16)].

%% Get the hex value for an integer number.
%% Decimal numbers just return the number.
%% Anything > 9 returns the hex character instead.
hex(N) when N < 10 ->
	$0 + N;
hex(N) when N >= 10, N < 16 ->
	$a + (N-10).
