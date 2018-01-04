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
SUBDIRS = libmxp \
          test_libmxp \
          iot_client \
          simple_server


test.CONFIG = recursive
test.target = test
test.recurse = test_libmxp
test.recurse_target = test
QMAKE_EXTRA_UNIX_TARGETS += test

coverage.CONFIG += recursive
coverage.recurse = $$SUBDIRS
coverage.recurse_target = coverage
QMAKE_EXTRA_UNIX_TARGETS += coverage

profile.CONFIG += recursive
profile.recurse = $$SUBDIRS 
profile.recurse_target = profile
QMAKE_EXTRA_UNIX_TARGETS += profile

profile_report.CONFIG += recursive
profile_report.recurse = test_libmxp
profile_report.recurse_target = profile_report
QMAKE_EXTRA_UNIX_TARGETS += profile_report

perftools.CONFIG += recursive
perftools.recurse = $$SUBDIRS
perftools.recurse_target = perftools
QMAKE_EXTRA_UNIX_TARGETS += perftools

