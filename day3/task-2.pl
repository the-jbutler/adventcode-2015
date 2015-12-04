#!/usr/bin/perl
use strict;
use warnings;

open IN_FILE, $ARGV[0] or die "Could not open the input file.\n$!";

my @pos = ([0, 0], [0, 0]);
my %history = ("0,0", 1);

# Christ this is much nicer in Perl...
while ((read IN_FILE, my $chars, 2) != 0) {
    my $whichPos = 0;
    for my $char (split //, $chars) {
        $char eq '^' ? $pos[$whichPos][1] = $pos[$whichPos][1] + 1 :
            $char eq '>' ? $pos[$whichPos][0] = $pos[$whichPos][0] + 1 :
                $char eq 'v' ? $pos[$whichPos][1] = $pos[$whichPos][1] - 1 :
                    $char eq '<' ? $pos[$whichPos][0] = $pos[$whichPos][0] - 1 : next;

        my $curPosStr = join ',', @{$pos[$whichPos]};
        $history{$curPosStr} += 1;
        $whichPos += 1;
    }
}
my $sz = keys %history;
print "visited houses: $sz\n";

# We outie.
close IN_FILE or die "Could not close input file!";
