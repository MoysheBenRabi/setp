package template;

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
    @EXPORT_OK   = qw(&make_FromTemplate);
    
}

END {}

sub make_FromTemplate($) {
    my $fi = shift;
    open FH_IN, "<", shift;
    while (<FH_IN>) {
    print $fi $_;
    }
    print $fi "\n";
    close FH_IN;
}

return 1;
