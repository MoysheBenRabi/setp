package refmsggen;
=pod
Main generator package for refernse messages.
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
OUTER: while(<$FH_IN>) {
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
        # time epoch_2k = "2000.01.01"
        if (($mode == 0)&&($line =~ /^(\w*)\s*(\w*)\s*=\s*(.*)/)) {
            my $imp = $g->create_include($1);
            if ($header !~ /$imp/) {
                $header .= $imp;
            }
            $content .= $g->create_constant($1,$2,$3);
        }
        if (($mode == 1)||($mode == 2)) {
            if ($line =~ /^([A-Z])(\w*).*/ ) {        
                my $csname = $g->translate_class_name("$1$2");
                $state = 1;
                $content .= $g->begin_sub('',"gen_$csname",'');
                $content .= $g->create_instance($csname);
                next OUTER;
            }
            if ($line eq "\n") {
                $content .= $g->end_sub();
                $state = 0;           
                next OUTER;
            }        
            if ($line =~ /^\s*(\w*)\[(\d*)\]\s*(\w*)\s*=\s*(.*)/) {
            
            my $imp = $g->create_include($1);        
            my $t = $1;
            my $c = $2;
            my $n = $3;      
            my $v = $4;

            if ($t eq 'string')
            {
                $content .= $g->init_field($t,$n,$v);
                next OUTER;
            } 

            if ($v =~ /{(.*)}/) {           
                $v = "$1";            
            }
            else {
                if ($v =~ /{(.*)/) {
                    $1 =~ /^(.*)\s*,\s*$/;   
                    $v = $1;                           
             INNER: while (<$FH_IN>) {
                        my $line1 = $_;
                        $line1 =~ /^\s*(.*)/;
                        $line1 = $1;                
                        if ($line1 =~ /^\s*(.*)}/) {
                            $1 =~ /^(.*)\s*,\s*$/;                
                            $v .= ",$1";  
                            last INNER;
                        } else {   
                            $1 =~ /^(.*)\s*,\s*$/;                
                            $v .= ",$1";
                        }
                    }
                    $v =~ s/\s*//g;                                                     
                }
            }
    
            $content .= $g->init_array_field($t,$c,$n,$v);
            if ($header !~ /$imp/) {
                $header .= $imp;
            }
            next OUTER;
        }        
        if ($line =~ /^\s*(\w*)\s*(\w*)\s*=\s*(.*)/) {
            my $imp = $g->create_include($1);
            $content .= $g->init_field($1,$2,$3);
            if ($header !~ /$imp/) {
                $header .= $imp;
            }
            next OUTER;
            }
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
