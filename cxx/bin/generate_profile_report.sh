#!/bin/sh
#
#  script to aggregate gprof profile reports
#

RELDIR=`dirname $0`
BASEDIR=`cd $RELDIR/..; pwd`

PROFILE_FILES=$(find $BASEDIR/src -name profile)

# capture profile info for each subdirectory
for PROFILE_FILE in $PROFILE_FILES; do
    PDIR=$(dirname $PROFILE_FILE)
    DIR=$(cd $PDIR/..; pwd)
    BASENAME=$(basename $DIR)
    echo "capturing profile info for $BASENAME";
    $BASEDIR/bin/gprof2html.py $PROFILE_FILE \
                               $BASEDIR/doc/profile/$BASENAME.html
done

echo "done."

