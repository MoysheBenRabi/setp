#!/bin/sh
#
#  script to prepare a coverage report, by traversing sub-directoires
#  and initializing coverage information for each of them
#

RELDIR=`dirname $0`
BASEDIR=`cd $RELDIR/..; pwd`

DIRS=$(find $BASEDIR/src -maxdepth 1 -type d -not -name ".*" -not -name src)

# initialize lcov info for each subdirectory
for DIR in $DIRS; do
    echo "initializing lcov info for $DIR";
    lcov --directory $DIR/tmp \
         --base-directory $DIR \
         --output-file $DIR/tmp/base_full.info \
         --capture \
         --initial \
         --ignore-errors source > /dev/null 2>&1;
done

echo "done."

