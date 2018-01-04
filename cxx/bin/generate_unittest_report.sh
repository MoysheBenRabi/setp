#!/bin/sh
#
#  combine the unit test results available, and generate an HTML unit
#  test report
#

RELDIR=`dirname $0`
BASEDIR=`cd $RELDIR/..; pwd`

TEST_FILES=`find $BASEDIR/src -name TEST-*.xml`

echo "<testsuites>" > $BASEDIR/tmp/tests.xml;
for TEST_FILE in $TEST_FILES; do
    cat $TEST_FILE | tail -n +3 | head -n -1 >> $BASEDIR/tmp/tests.xml;
done
echo "</testsuites>" >> $BASEDIR/tmp/tests.xml;

xsltproc --output $BASEDIR/doc/unittests/index.html \
         $BASEDIR/etc/junit-noframes.xsl $BASEDIR/tmp/tests.xml

