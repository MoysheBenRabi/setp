#!/bin/sh
#
#  script to generate a complete coverage report, by traversing sub-directoires
#  collecting coverage information from each of them, and combining these
#  into a single coverage report
#

RELDIR=`dirname $0`
BASEDIR=`cd $RELDIR/..; pwd`

DIRS=$(find $BASEDIR/src -maxdepth 1 -type d -not -name ".*" -not -name src)

# capture lcov info for each subdirectory
for DIR in $DIRS; do
    echo "capturing lcov info for $DIR";
    lcov --directory $DIR/tmp \
         --base-directory $DIR \
         --output-file $DIR/tmp/app_full.info \
         --capture \
         --ignore-errors source > /dev/null 2>&1;
done

# collect all information into a single lcov info file
for DIR in $DIRS; do
    echo "adding lcov info from $DIR";
    lcov --list $DIR/tmp/base_full.info > /dev/null 2>&1 ;
    if [ $? -eq 0 ]; then
        ADD_STATEMENT="$ADD_STATEMENT --add-tracefile $DIR/tmp/base_full.info"
    fi
    lcov --list $DIR/tmp/app_full.info > /dev/null 2>&1 ;
    if [ $? -eq 0 ]; then
        ADD_STATEMENT="$ADD_STATEMENT --add-tracefile $DIR/tmp/app_full.info"
    fi
done
lcov $ADD_STATEMENT \
     --output-file $BASEDIR/tmp/app_full.info > /dev/null 2>&1;

# clean boost and system headers from the lcov info file
echo "cleaning lcov infromation from system headers";
lcov --output-file $BASEDIR/tmp/app.info \
     --remove $BASEDIR/tmp/app_full.info "*boost*" "/usr/include/*" \
                                         "/usr/include/*/*" "*c++*" "*gcc*" \
                                         "*gmtl*" "*assimp*" \
                                         "$BASE_DIR/lib/*" "*Qt*" \
                                         "test_*" \
     > /dev/null 2>&1;

echo "generating report";
rm -rf $BASEDIR/doc/lcov/*
genhtml --output-directory $BASEDIR/doc/lcov \
        --prefix $BASEDIR/src \
        $BASEDIR/tmp/app.info > /dev/null 2>&1;

echo "cleaning up"
for DIR in $DIRS; do
    rm -f $DIR/tmp/base_full.info;
    rm -f $DIR/tmp/app_full.info;
done
rm -f $BASEDIR/tmp/app_full.info $BASEDIR/tmp/app.info

echo "done."

