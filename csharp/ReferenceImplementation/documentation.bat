call mdoc update -i MXP/bin/Debug/MXP.xml -o doc/xml -L lib lib/MXP.dll
call mdoc update -i MXPTests/bin/Debug/MXPTests.xml -o doc/xml -L lib lib/MXPTests.dll
call mdoc export-html -o doc/html doc/xml