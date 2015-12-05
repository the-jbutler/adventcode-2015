#!/usr/bin/escript -c
-module(day41).
-export([
	main/1,
	usage/0,
	findNumber/3,
	process/3
]).

main(Args) when length(Args) == 1 ->
	R = findNumber(Args, 1, 1),
	io:format("Valid Number ~p~n", [R]);
main(Args) when length(Args) == 2 ->
	[Passphrase|Tail] = Args,
	Cores = [ list_to_integer(T) || T <- Tail],
	R = findNumber(Passphrase, Cores, Cores),
	io:format("Valid Number ~p~n", [R]);
main(_) ->
	usage().


usage() ->
	io:format("passphrase [workers]~n").


findNumber(P, C, N) when N > 1 ->
	%% Do work with multiple workers
	findNumber(P, C, N-1),
	process(P, C, N);
findNumber(P, C, _) ->
	process(P, C, 0).


process(P, C, O) ->
	Bits = 5 * 4,
	case erlang:md5([P | integer_to_list(O)]) of
		<<0:Bits, _/bitstring>> ->
			O;
		_ -> %% Recurse up in this bizitch
			process(P, C, O+C)
	end.

