#!/usr/bin/perl

sub put_Header {
    my $fh = shift;
    print $fh "#   Copyright 2009 Moyshe BenRabi\n";
    print $fh "#\n";
    print $fh "#   Licensed under the Apache License, Version 2.0 (the \"License\");\n";
    print $fh "#   you may not use this file except in compliance with the License.\n";
    print $fh "#   You may obtain a copy of the License at\n";
    print $fh "#\n";
    print $fh "#       http://www.apache.org/licenses/LICENSE-2.0\n";
    print $fh "#\n";
    print $fh "#   Unless required by applicable law or agreed to in writing, software\n";
    print $fh "#   distributed under the License is distributed on an \"AS IS\" BASIS,\n";
    print $fh "#   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\n";
    print $fh "#   See the License for the specific language governing permissions and\n";
    print $fh "#   limitations under the License.\n";   
    print $fh "#\n";
    print $fh "#   This is always generated file. Do not edit directyly.\n";
    print $fh "#   Instead edit messagegen.pl and descr.txt\n";
    print $fh "\n";
}

sub make_FromTemplate {
    my $fi = shift;
    open FH_IN, "<", shift;
    while (<FH_IN>) {
        print $fi $_;
    }
    print $fi "\n";
    close FH_IN;
}

$messagesImports = '';
$fragmentsImports = '';
$messagesRegistrar = '';

sub make_init {
    open FB, ">", "./messages/__init__.py";
    open FH_IN, "<", "__init__.in";
    while (<FH_IN>) {
    $line = $_;
    if ($line =~ /<%IMPORTS%>/) {
        print FB "\n";
        print FB $fragmentsImports;
        print FB "\n";
        print FB $messagesImports;
        } else { if ($line =~ /<%REGISTER_DEFAULT_CLASSES%>/) {
        print FB $messagesRegistrar;
    } else {
        print FB $line;
    }}
    
    }
    close FH_IN;
    print FB "\n";
    close FB;
}

sub get_ConstName {
    my $name = shift;
    my $result = '';
    if ($name =~ /^(.*)([A-Z])(.*)([A-Z])(.*)$/) {        
    if ($1) { $result = "\U$1_$2\U$3_$4\U$5"; }
    else { if ($2) { $result = "$2\U$3_$4\U$5"; }
        else { $result = "$4\U$5"; print "$4\U$5"; } } }
    else { $result = "\U$name"; }
    return "MXP_MESSAGE_$result";
}

sub get_ClassName {
    my $name = shift;
    my $result = $name;    
    return $result;
}

sub get_ModuleName {
    my $csname = shift;
    $csname =~ s/(\w)([A-Z])/$1_\L$2/g;
    return "\L$csname";
}

sub translateType {
    my $t = shift;
    if ($t eq 'data') { $t = 'byte' }
    if ($t eq 'string') { $t = 'str' }
    return $t;
}

open FH, "<", "descr.txt";

open FB, ">", "./messages/message_base.py";
open FID, ">", "./messageID.py";
make_FromTemplate(FB, "message_base.in");
close FB;


@fragments = ();

sub isFragment {
    my $s = shift;
    for my $p (@fragments) {
    if ($s eq $p) {return 1};
    }
    return 0;
}

sub get_ImportForType {
    my $s = shift;
    if ($s eq 'uuid') {
    return "from uuid import *\n";
    }
    if (isFragment($s)) {    
    my $m = get_ModuleName($s);
    return "from $m import $s\n";
    } else {
    return '';
    }
}

sub get_DefaultForType {
    my $t = shift;
    my $r = '0';
    if (isFragment($t)) {$r = "$t()";} 
    else {if ($t eq 'string') {$r = '""';}                                    
    else {if ($t eq 'uuid') {$r = 'UUID(\'{00000000-0000-0000-0000-000000000000}\')'}}}
    return $r;
}

sub get_Size {
    my $t = shift;
    if (isFragment($t)) { return "$t.frame_data_size(frame_index)" }
    if ($t eq "string") { return 1; }
    if ($t eq "byte") { return 1; }
    if ($t eq "uint") { return 4; }
    if ($t eq "time") { return 8; }    
    if ($t eq "float") { return 8; }
    if ($t eq "uuid") { return 16; }
    if ($t eq "vector3f") { return 12; }
    if ($t eq "vector4f") { return 16; }
    return "<$t>";
}

$state = 0;
$mode = -1;

while (<FH>) {
    $line = $_;
        $stae = 1;    
    if ($mode == 0) {
    if ($line =~ /^([A-Z])(\w*).*/) {
        $csname = get_ClassName("$1$2");
        $fragments[++$#fragments] = $csname;
        $mdname = get_ModuleName($csname);
        $fileName = "$mdname.py";
        $stae = 1;
        $constructor = "";
        $imports = '';
        $tostr = '';
        $clear = '';
        $getters = '';
        $setters = '';
        $attributes = '';
        $encoder = '';
        $decoder = '';
        $descr = '';
        $compare  = "       return True";        
        $compare1 = "       return True";
        $dataSize = '';
    } else { 
        if ($line =~ /^\s*[\n]$/) {        
        print "Writing $fileName\n";
        open FM, ">./messages/$fileName";
        $fragmentsImports .= "from $mdname import $csname\n";
        put_Header(FM);
        print FM $imports;
        print FM "\n";
        print FM "class $csname(object): \n";
        print FM "\n";
        print FM "    def __init__(self):\n";
        print FM $constructor;              
        print FM "\n";
        print FM "    def clear(self):\n";
        print FM $clear;
        print FM "        super($csname,self).clear()\n";
        print FM "\n";
        print FM "    def frame_data_size(self, frame_index):\n";
        print FM "        result = 0\n";
        print FM $dataSize;        
        print FM "        return result\n";
        print FM "\n";
        if ($getters) {
            print FM $getters;
        }
        if ($setters) {
            print FM $setters;
        }                   
        print FM "\n";
        print FM "    def serialize(self, writer):\n";
        print FM $encoder;
        print FM "\n";
        print FM "    def deserialize(self, reader):\n";
        print FM $decoder;
        print FM "\n";        
        if ($descr) {
        print FM "    def __str__(self):\n";
        print FM "        return '$csname('+$descr+')'\n";
        } else {
        print FM "    def __str__(self):\n";
        print FM "        return '$csname()'\n";            
        }
        print FM "\n";        
        print FM "    def __eq__(self,other):\n";
        print FM "$compare\n";
        print FM "\n";                          
        print FM "    def __ne__(self,other):\n";
        print FM "$compare1\n";                       
        print FM "\n";                 
        close FM;
        $state = 0;
        } else {
        if ($line !~ /^\s*[\n]$/) {          
            if ($line =~ /^\s*(\w*)\[(.*)\]\s*(\w*)$/) {
                if($2) {
                    my $t = translateType($1);            
                        if ($t eq 'str') {
                        if ($2 ne 'x') {
                        $constructor .= "        self.max_$3 = $2\n";
                        }
                        $constructor .= "        self.$3 = ''\n";
#                       $constructor .= "\n";
                        $encoder     .= "        writer.writeRange(self.$3,self.max_$3,'chr')\n";
                        $decoder     .= "        (self.$3, c) = reader.readRange(self.max_$3,'chr',1)\n";
                        if ($descr) {
                            $descr .= " \\\n                               + "
                        }
                        $descr .= "self.$3";
                        $compare     .= " and \\\n               (self.$3 == other.$3)";
                        $compare1    .= " or \\\n               (self.$3 != other.$3)";
                        $clear       .= "        self.$3 = ''\n";
                    } else {
                        my $ds = get_Size($t);
                        if ($2 ne 'x') {
                        $constructor .= "        self.max_$3 = $2\n";
                        $constructor .= "        self.$3s = [0] * self.max_$3\n";
                        }
                        else {
                        $constructor .= "        self.$3s = [0]\n";
                        }
                        $constructor .= "        self.$3_length = 0";
#                       $constructor .= "\n";
                        $clear       .= "        for i in range(0,self.$3_length):\n";
                        $clear       .= "            self.$3s[i] = 0\n";
                        $clear       .= "        self.$3_length = 0\n";
#                       $clear       .= "\n";
                        $setters     .= "    def add_$3(self, $3):\n";
                        if ($2 ne 'x') {
                        $setters     .= "        if self.$3_coun == self.max_packet_id_length:\n";
                        $setters     .= "            raise Exception(\"Too many $3s\")\n";
                        }
                        $setters     .= "        self.packet_ids[self.$3_length] = packet_id\n";
                        $setters     .= "        self.packet_id_length += 1\n";
                        $setters     .= "\n";                
                        $getters     .= "    def get_$3(self, index):\n";
                        $getters     .= "        if index >= self.$3_length:\n";
                        $getters     .= "            raise Exception(\"Out of $3 array bounds: \" + str(index))\n";
                        $getters     .= "        return self.$3s[index]\n";                
                        $getters     .= "\n";
                        if ($ds == 1) {
                        $dataSize    .= "        result += self.$3_length\n";
                        } else {
                        $dataSize    .= "        result += self.$3_length * $ds\n";
                        }
                        $encoder     .= "        writer.write(self.$3_length,'uint')\n";
                        $encoder     .= "        writer.writeRange(self.$3s,self.$3_length,'$t')\n";            
#                       $encoder     .= "\n";
                        $decoder     .= "        (self.$3_length, c) = reader.read('uint')\n";
                        $decoder     .= "        (self.$3s, c) = reader.readRange(self.$3_length,'$t',$ds)\n";                
#                       $decoder     .= "\n";                
                        if ($descr) {
                            $descr .= " \\\n                                + "
                        }
                        $descr .= "str(self.$3s)";
                        $compare     .= " and \\\n        self.$3s == other.$3s";
                        $compare1    .= " or \\\n        self.$3s != other.$3s";
                    }
                }                                           
            } else {if ($line =~ /^\s*(\w*)\s*(\w*)$/) {
                if ($2) {                
                    my $t = translateType($1);
                    my $ds = get_Size($t);
                    my $i = get_ImportForType($t);
                    my $d = get_DefaultForType($t);
                    $constructor .= "        self.$2 = $d\n";
                    $clear       .= "        self.$2 = $d\n";
                    $dataSize    .= "        result += $ds\n";        
                    if (isFragment($t)) {
                        $encoder .= "        $2.serialize(writer)\n";
                        $decoder .= "        $2.deserialize(reader)\n";
                    } else {                
                        $encoder .= "        writer.write(self.$2,'$t')\n";
                        $decoder .= "        (self.$2, c) = reader.read('$t')\n";
                    }
                    if ($descr) {
                        $descr .= " \\\n                                + "
                    }
                    $descr .= "str(self.$2)";
                    $compare     .= " and \\\n        self.$2 == other.$2";
                    $compare1    .= " or \\\n        self.$2 != other.$2";                
                    if (($i) && ($imports !~ /$t/)) {
                        $imports .= $i; 
                    }
                }
            }}           
        }}}
    } else {           
    if ($line =~ /^(.*)\((.*)\).*$/) {
        $csname = get_ClassName($1);       
        $ctname = get_ConstName($1);
        $mdname = get_ModuleName($csname);
        $fileName = "$mdname.py";
        $typeId = $2;
        $stae = 1;
        $imports = '';
        $constructor = "        self.type_code = $ctname\n";
        $tostr = '';
        $dataSize = '';
        $clear = '';
        $getters = '';
        $setters = '';
        $attributes = '';
        $encoder = '';
        $decoder = '';        
        $compare  = "\n        return (self.type_code == other.type_code)";
        $compare1 = "\n        return (self.type_code != other.type_code)";    
        $descr = "";    
    } else { 
        if ($line =~ /^s*[\n]$/) {        
        print "Writing $fileName\n";
        open FM, ">./messages/$fileName";
        $messagesImports .= "from $mdname import $csname\n";
        $messagesRegistrar .= "        self.add_message_type($csname())\n";        
        put_Header(FM);
        print FM "from message_base import *\n";
        print FM "$imports";
        print FM "\n";
        print FM "$ctname = $typeId\n";    
        print FID "$ctname = $typeId\n";    
        print FM "\n";
        print FM "class $csname(Message): \n";
        print FM "\n";
        print FM "    def __init__(self):\n";
        print FM $constructor;
        print FM "\n";
        print FM "    def clear(self):\n";
        print FM $clear;
        print FM "        super($csname,self).clear()\n";
        print FM "\n";
        print FM "    def frame_data_size(self, frame_index):\n";
        print FM "        result = 0\n";
        print FM $dataSize;
        print FM "        return result\n";
        print FM "\n";
        if ($getters) {
            print FM $getters;
        }
        if ($setters) {
            print FM $setters;
        }                   
        print FM "\n";
        print FM "    def serialize(self, writer):\n";
        if ($encoder) {        
        print FM $encoder;
        } else {
        print FM "        pass\n";
        }        
        print FM "\n";
        print FM "    def deserialize(self, reader):\n";
        if ($decoder) {        
        print FM $decoder;
        } else {
        print FM "        pass\n";
        }                
        print FM "\n";
        if ($descr) {
        print FM "    def __str__(self):\n";
        print FM "        return '$csname('+ $descr +')'\n";
        } else {
        print FM "    def __str__(self):\n";
        print FM "        return '$csname()'\n";            
        }
        print FM "\n";        
        print FM "    def __eq__(self,other):";
        print FM "$compare\n";
        print FM "\n";                 
        print FM "    def __ne__(self,other):";
        print FM "$compare1\n";                        
        print FM "\n";                 
        close FM;
        $state = 0;
        } else {        
        if ($line =~ /^\s*(\w*)\[(.*)\]\s*(\w*)$/) {            
            if($2) {
            my $t = translateType($1);            
            my $ds = get_Size($t);
            if ($t eq 'str') {
                $constructor .= "        self.max_$3 = $2\n";               
                $constructor .= "        self.$3 = ''\n";
#               $constructor .= "\n";
                $encoder     .= "        writer.writeRange(self.$3,self.max_$3,'chr')\n";
                $decoder     .= "        (self.$3, c) = reader.readRange(self.max_$3,'chr',1)\n";
                if ($descr) {
                    $descr .= " \\\n                               + "
                }
                $descr .= "self.$3";
                $compare     .= " and \\\n               (self.$3 == other.$3)";
                $compare1    .= " or \\\n               (self.$3 != other.$3)";
                $clear       .= "        self.$3 = ''\n";
            } else {
                if ($2 ne 'x') {
                    $constructor .= "        self.max_$3 = $2\n";
                    $constructor .= "        self.$3s = [0] * self.max_$3\n";
                } else {
                    $constructor .= "        self.$3s = [0]\n";
                }
                $constructor .= "        self.$3_length = 0\n";
#               $constructor .= "\n";
                $clear       .= "        for i in range(0,self.$3_length):\n";
                $clear       .= "            self.$3s[i] = 0\n";
                $clear       .= "        self.$3_length = 0\n";
#               $clear       .= "\n";
                $setters     .= "    def add_$3(self, $3):\n";
                if ($2 ne 'x') {
                    $setters     .= "        if self.$3_coun == $2:\n";
                    $setters     .= "            raise Exception(\"Too many $3s\")\n";
                }
                $setters     .= "        self.$3s[self.$3_length] = $3\n";                
                $setters     .= "        self.$3_length += 1\n";
                $setters     .= "\n";                
                $getters     .= "    def get_$3(self, index):\n";
                $getters     .= "        if index >= self.$3_length:\n";
                $getters     .= "            raise Exception(\"Out of $3s array bounds: \" + str(index))\n";
                $getters     .= "        return self.$3s[index]\n";                
                $getters     .= "\n";
                $dataSize    .= "        result += self.$3_length * $ds\n";
#               $encoder     .= "\n";
                if ($2 eq 'x') {
                    $encoder     .= "        writer.write(self.$3_length,'uint')\n";
                    $encoder     .= "        writer.writeRange(self.$3s,self.$3_length,'$t')\n";            
                } else {
                    $encoder     .= "        writer.writeRange(self.$3s,self.max_$3,'$t')\n";
                }                 
#               $encoder     .= "\n";
                if ($2 eq 'x') {
                    $decoder     .= "        (self.$3_length, c) = reader.read('uint')\n";
                    $decoder     .= "        (self.$3s, c) = reader.readRange(self.$3_length,'$t',$ds)\n";
                } else {
                    $decoder     .= "        (self.$3s, c) = reader.readRange(self.max_$3,'$t',$ds)\n";                    
                }                                 
#               $decoder     .= "\n";
                if ($descr) {
                    $descr .= " \\\n                               + "
                }
                $descr .= "str(self.$3s)";
                $compare     .= " and \\\n               (self.$3s == other.$3s)";
                $compare1    .= " or \\\n               (self.$3s != other.$3s)";
            }
            }            
        } else { if ($line =~ /^\s*(\w*)\s*(\w*)$/) {
            if ($2) {
                my $t = translateType($1);
                my $ds = get_Size($t);
                my $d = get_DefaultForType($t);
                my $i = get_ImportForType($t);
                $constructor .= "        self.$2 = $d\n";
                $clear       .= "        self.$2 = $d\n";
                $dataSize    .= "        result += $ds\n";        
                if (isFragment($t)) {
                    $encoder .= "        self.$2.serialize(writer)\n";
                    $decoder .= "        self.$2.deserialize(reader)\n";
                } else {                
                    $encoder .= "        writer.write(self.$2,'$t')\n";
                    $decoder .= "        (self.$2,c) = reader.read('$t')\n";
                }
                if ($descr) {
                    $descr .= " \\\n                                + "
                }
                $descr .= "str(self.$2)";
                $compare     .= " and \\\n               (self.$2 == other.$2)";
                $compare1    .= " or \\\n               (self.$2 != other.$2)";                            
                if (($i) && ($imports !~ /$t/)) {            
                    $imports .= $i; 
                }
            }
        }}            
        }
        }
    }
    
    if ($line =~ /^\\Fragments\n$/) {     
        print "Processing fragments.\n";
        $mode = 0;
    }

    if ($line =~ /^\\Messages\n$/) {
        print "Processing messages.\n";
        $mode = 1;
    }
        
}

make_init();
    
print "Done.";
#close FI;
close FID;
close FH;
