package msggen;
=pod
Main generator package for messages.
=cut

use strict;
use warnings;

BEGIN {
    use Exporter   ();
    our ($VERSION, @ISA, @EXPORT, @EXPORT_OK, %EXPORT_TAGS);

    # set the version for version checking
    $VERSION     = 0.01;
    
    @ISA         = qw(Exporter);
    @EXPORT      = qw(&func1 &func2 &func4);
    %EXPORT_TAGS = ( );     # eg: TAG => [ qw!name1 name2! ],

    # your exported package globals go here,
    # as well as any optionally exported functions
    @EXPORT_OK   = qw(&generate);         
}

END {}

=intem
Generate stuff;
=cut
sub generate($$$) {
    my $outdir = shift;
    my $generator = shift;
    my $infile = shift;   
    
    my $header = '';
    my $content = '';
    
    my $g = {};
    if ($generator eq 'python') {
        use python;
        $g = python::spawn();
    }

    my $mode = 0;
    my $state = 0;

    my $FH_IN;
    my $FH_OUT;
    open $FH_IN, "<$infile";
    while(<$FH_IN>) {
        my $line = $_;
        if ($line =~ /\\Constants/) { 
            if ($mode != 0) {
                $content .= "\n";
            }
            $mode = 0; 
            next OUTER;
        }
        if ($line =~ /\\Fragments/) { 
            if ($mode != 1) {
                $content .= "\n";
            }
            $mode = 1; 
            next OUTER;
        }
        if ($line =~ /\\Messages/) { 
            if ($mode != 2) {
                $content .= "\n";
            }        
            $mode = 2; 
            next OUTER;
        }
        
        if (($mode == 0)&&($line =~ /^(\w*)\s*(\w*)\s*=\s*(.*)/)) {
            my $imp = $g->create_include($1);
            if ($header !~ /$imp/) {
               $header .= $imp;
            }
            $content .= $g->create_constant($1,$2,$3);
        }
        
        if (($mode == 1)||($mode == 2)) {
        }        
        
    }
        
    close $FH_IN;

    open $FH_OUT, ">$outdir/reference.py";        
    print $FH_OUT "from pymxp.messages import *\n";
    print $FH_OUT $header;
    print $FH_OUT "\n";
    print $FH_OUT $content;
    print $FH_OUT "\n";
    close $FH_OUT;
}    

return 1;
