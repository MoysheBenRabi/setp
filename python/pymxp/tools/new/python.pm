package python;
=pod
Python generator package.
=cut

use strict;
use warnings;

BEGIN {
    use Exporter   ();
    our ($VERSION, @ISA, @EXPORT, @EXPORT_OK, %EXPORT_TAGS);

    # set the version for version checking
    $VERSION     = 0.01;
    
    @ISA         = qw(Exporter);
    @EXPORT      = qw(&spawn);
    %EXPORT_TAGS = ( );     # eg: TAG => [ qw!name1 name2! ],

    # your exported package globals go here,
    # as well as any optionally exported functions
    @EXPORT_OK   = qw(&spawn &create_instance &begin_file &comment &init_field &init_array_field &begin_sub &end_sub);
    
}

END {}

=item
Create class instance
=cut
sub spawn {
    my $self = {};
    bless $self;
    $self->initialize();
    return $self;
}

=item
Constructor
=cut
sub initialize($) {
    my $self = shift;
}

=item 
Translate a class name in to a language specific form
=cut
sub translate_class_name($$) {
    my $self = shift;
    my $name = shift;
    my $result = $name;    
    return $result;
}

=item
Begin file
=cut
sub begin_file($) {
    my $self = shift;
}

=item
Add comment line
=cut
sub comment($$) {
    my $self = shift;
    return '# '.shift;
}

=item
Create a class instance
=cut
sub create_constant($$$$) {
    my $self = shift;
    my $t = shift;
    my $n = shift;
    my $v = shift;    
        
    if ($t eq 'time') {
    if ($v =~ /^"(\d*).(\d*).(\d*)"$/) {
        return "$n = datetime($1, $2, $3)\n";
    }
    } else {
    return "$n = $v";
    }           
}

=item
Create include statement
=cut
sub create_include($$) {
    my $self = shift;
    my $t = shift;
    if ($t eq "uuid") {
    return "from uuid import UUID\n";
    }
    if ($t eq "time") {
    return "from datetime import datetime\n";
    }
}
    
=item
Create a class instance
=cut
sub create_instance($$) {
    my $self = shift;
    my $i = shift;
    return "    instance = $i()\n";    
}

=item
Create field initializer for --refmsg mode
=cut
sub init_field($$$$) {
    my $self = shift;
    my $t = shift;
    my $n = shift;
    my $v = shift;    
    if ($t eq 'uuid') {
    return "    instance.$n = uuid($v)\n";
    } else {
    return "    instance.$n = $v\n";
    }
}

=item
Create array field initializer for --refmsg mode
=cut
sub init_array_field($$$$$) {
    my $self = shift;
    return '';
}

=item
Begin sub
=cut
sub begin_sub($$$$) {
    my $self = shift;
    my $r = shift;
    my $n = shift;
    my $a = shift;
    return "def $n($a):\n";
}

=item
End sub
=cut
sub end_sub($) {
    my $self = shift;
    return "    return instance\n\n";
}

return 1
