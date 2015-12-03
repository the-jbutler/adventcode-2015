#!/usr/bin/perl

open FILE, $ARGV[0] or die "Could not open the input file.\n$!";

my @pos = (0, 0);
my %history = ("0,0", 1);

# Christ this is much nicer in Perl...
while (($bytes = read FILE, $char, 1) != 0) {
    $char eq '^' ? $pos[1] = $pos[1] + 1 :
        $char eq '>' ? $pos[0] = $pos[0] + 1 :
            $char eq 'v' ? $pos[1] = $pos[1] - 1 :
                $char eq '<' ? $pos[0] = $pos[0] - 1 : next;

    my $curPos = join ',', @pos;
    print "$curPos\n";
    $history{$curPos} += 1;
}
$sz = keys %history;
print "visited houses: $sz\n";

if (FILE != STDIN) {
    close FILE or die "Could not close input file!";
}
