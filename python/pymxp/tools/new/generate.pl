#!/usr/bin/perl
=pod

=cut

$mode = 1; # message generation mode.
$outdir = './';
$generator = 'python';

$_ = shift;
while($_ =~ /^-/) {
    if (($_ eq '-r')||($_ eq '--refmsg')) {
    $mode = 1 # reference message generator generation mode
    }

    if (($_ eq '-m')||($_ eq '--msg')) {
    $mode = 0 # message generator generation mode (default)
    }

    if (($_ eq '-o')||($_ eq '--out')) {
    $outdir = shift;
    }
 
    if (($_ eq '-g')||($_ eq '--generator')) {
    $generator = shift;
    }

    if (($_ eq '-h')||($_ eq '--help')) {
    print "Usage: generate.pl [-r|-m] [-o ./outdir] [-g generator] [descr.txt]\n";
    exit;
    }

    $_ = shift;
}
$infile = $_;
if (!$infile) {$infile = 'descr.txt'};

if ($mode) {
    use refmsggen;
    refmsggen::generate($outdir,$generator,$infile);
} else {
    use msggen;
    msggen::generate($outdir,$generator,$infile);
}
