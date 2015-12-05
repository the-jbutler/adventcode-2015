#!/usr/bin/escript -c
-module(day41).
-export([
	main/1,
	usage/0,
	findNumber/4,
	process/4
]).

main(Args) when length(Args) == 1 ->
	R = findNumber(Args, 5, 1, 1),
	io:format("Valid Number ~p~n", [R]);
main(Args) when length(Args) == 2 ->
	[Passphrase|Tail] = Args,
	[Zeroes|_] = [ list_to_integer(H) || H <- Tail ],
	R = findNumber(Passphrase, Zeroes, 1, 1),
	io:format("Valid Number ~p~n", [R]);
main(Args) when length(Args) == 3 ->
	[Passphrase|Tail] = Args,
	[Zeroes|TailTwo] = [ list_to_integer(H) || H <- Tail ],
	[Cores|_] = [ list_to_integer(H) || H <- TailTwo ],
	R = findNumber(Passphrase, Zeroes, Cores, Cores),
	io:format("Valid Number ~p~n", [R]);
main(_) ->
	usage().


usage() ->
	io:format("passphrase [leading zeros (5)] [workers (1)]~n").


findNumber(P, Z, C, N) when N > 1 ->
	%% Do work with multiple workers
	findNumber(P, Z, C, N-1),
	process(P, Z, C, N);
findNumber(P, Z, C, _) ->
	io:format("leading zeros: ~p~n", [Z]),
	process(P, Z, C, 0).


process(P, Z, C, O) ->
	Bits = 4*Z,
	case erlang:md5([P | integer_to_list(O)]) of
		<<0:Bits, _/bitstring>> ->
			O;
		_ -> %% Recurse up in this bizitch
			process(P, Z, C, O+C)
	end.

