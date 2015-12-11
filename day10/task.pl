#!/usr/bin/env perl

my $rounds = $ARGV[1] || 1;
my $val = $ARGV[0];

print "Performing $rounds rounds on '$val'\n";

for ($round = 1; $round <= $rounds; $round++) {
	print "Starting round $round\n";
	my $result;
	while ($val =~ /(.)(\1*)/g) {
		$result = sprintf "%s%s%s", $result, length($2)+1, $1;
	}
	$val = $result;
	print "Round $round result length: " . length($val) . "\n";
}
