# Copyright (c) 2009-2010 Tyrell Corporation & Moyshe Ben Rabi.
# 
# The contents of this file are subject to the Mozilla Public License
# Version 1.1 (the "License"); you may not use this file except in
# compliance with the License. You may obtain a copy of the License at
# http://www.mozilla.org/MPL/
# 
# Software distributed under the License is distributed on an "AS IS"
# basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
# License for the specific language governing rights and limitations
# under the License.
# 
# The Original Code is an implementation of the Metaverse eXchange Protocol.
# 
# The Initial Developer of the Original Code is Akos Maroy and Moyshe Ben Rabi.
# All Rights Reserved.
# 
# Contributor(s): Akos Maroy and Moyshe Ben Rabi.
# 
# Alternatively, the contents of this file may be used under the terms
# of the Affero General Public License (the  "AGPL"), in which case the
# provisions of the AGPL are applicable instead of those
# above. If you wish to allow use of your version of this file only
# under the terms of the AGPL and not to allow others to use
# your version of this file under the MPL, indicate your decision by
# deleting the provisions above and replace them with the notice and
# other provisions required by the AGPL. If you do not delete
# the provisions above, a recipient may use your version of this file
# under either the MPL or the AGPL.

win32-msvc* {
    CONFIG += ordered
    TEMPLATE = vcsubdirs
}
unix|win32-g++* {
    TEMPLATE = subdirs
}
SUBDIRS = src

cccc.target = cccc
cccc.commands = cccc --outdir=doc/cccc \
                `find src/libmxp -name "*.h" -o -name "*.cxx"`
QMAKE_EXTRA_UNIX_TARGETS += cccc
QMAKE_CLEAN += doc/cccc/*

cppcheck.target = cppcheck
cppcheck.commands = cppcheck --quiet --all --verbose --xml \
                    -I src/*/include src 2> tmp/cppcheck.xml
QMAKE_EXTRA_UNIX_TARGETS += cppcheck
QMAKE_CLEAN += tmp/cppcheck.xml

doxygen.target = doxygen
doxygen.commands = doxygen etc/doxygen.config
QMAKE_EXTRA_UNIX_TARGETS += doxygen
QMAKE_CLEAN += doc/doxygen/html/*

test.CONFIG = recursive
test.target = test
test.recurse = $$SUBDIRS
test.recurse_target = test
unix {
    test.commands = bin/generate_unittest_report.sh
}
QMAKE_EXTRA_UNIX_TARGETS += test
QMAKE_CLEAN += doc/unittests/*
QMAKE_CLEAN += tmp/tests.xml

coverage.CONFIG += recursive
coverage.recurse = $$SUBDIRS
coverage.recurse_target = coverage
QMAKE_EXTRA_UNIX_TARGETS += coverage

coverage_report_pre.target = coverage_report_pre
coverage_report_pre.depends = 
coverage_report_pre.commands = bin/initialize_coverage_report.sh
QMAKE_EXTRA_UNIX_TARGETS += coverage_report_pre

coverage_report.target = coverage_report
coverage_report.depends = coverage_report_pre test
coverage_report.commands = bin/generate_coverage_report.sh
QMAKE_EXTRA_UNIX_TARGETS += coverage_report
QMAKE_CLEAN += tmp/*.info

profile.CONFIG += recursive
profile.recurse = $$SUBDIRS
profile.recurse_target = profile
QMAKE_EXTRA_UNIX_TARGETS += profile

profile_report.CONFIG += recursive
profile_report.recurse = $$SUBDIRS
profile_report.recurse_target = profile_report
profile_report.depends = test
profile_report.commands = bin/generate_profile_report.sh
QMAKE_EXTRA_UNIX_TARGETS += profile_report
QMAKE_CLEAN += doc/profile/test_*.html

perftools.CONFIG += recursive
perftools.recurse = $$SUBDIRS
perftools.recurse_target = perftools
QMAKE_EXTRA_UNIX_TARGETS += perftools

