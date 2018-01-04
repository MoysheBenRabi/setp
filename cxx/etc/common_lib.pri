#
# Copyright (c) 2009 Tyrell Corporation.
#
# This file is part of the Tyrell Metaverse eXchange Protocol implementation
# (Tyrell MXP).
#
# Tyrell MXP is free software: you can redistribute it and/or modify
# it under the terms of the GNU Affero General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# Tyrell MXP is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU Affero General Public License for more details.
#
# You should have received a copy of the GNU Affero General Public License
# along with Tyrell MXP - see doc/agpl.txt
# If not, see <http://www.gnu.org/licenses/>.
#

INCLUDEPATH += include
CONFIG += staticlib
DESTDIR = lib
OBJECTS_DIR = tmp

*-g++* {
    QMAKE_CXXFLAGS_WARN_ON = -Wall -ansi -Wno-long-long
}
*-icc* {
    QMAKE_CXXFLAGS_WARN_ON = -Wcheck -ansi -diag-enable port-win,thread \
                             -diag-disable 1710,1711,1712
}
*-cc* {
    # use these flags, as recommended by the boost documentation for complex
    # C++ stuff,
    # see http://www.boost.org/doc/tools/build/doc/html/jam/usage.html
    QMAKE_CXXFLAGS += -library=stlport4 -features=tmplife \
                      -features=tmplrefstatic +d
    # also turn on threading
    QMAKE_CXXFLAGS += -mt
}
win32-g++* {
    # remove the -ansi option on mingw, because it fails with
    # cwchar:159: error: '::swprintf' has not been declared
    # also remove -pedantic, because it warns on "comma at the end of enums"
    QMAKE_CXXFLAGS_WARN_ON = -Wall -Wno-long-long

    # set this limit high so that ar is never invoked as "ar -M < .."
    QMAKE_LINK_OBJECT_MAX = 10000
}

win32-msvc* {
    TEMPLATE = vclib
    DEFINES += WIN32
    DEFINES += WIN32_MEAN_AND_LEAN
    DEFINES += _WIN32_WINNT=0x0501
    DEFINES += _SCL_SECURE_NO_WARNINGS
    DEFINES += _CRT_SECURE_NO_WARNINGS
    DEFINES += XMD_H
    
    DEFINES +=  UNICODE \

    INCLUDEPATH += $$(BOOST_HOME)/include \
                   ../../lib/win32-msvc/include

    build_pass:CONFIG(release, debug|release) {
        OBJECTS_DIR = release
    }
    build_pass:CONFIG(debug, debug|release) {
        TARGET = $$join(TARGET,,,d)
        DEFINES +=  _DEBUG    
        OBJECTS_DIR = debug
    }
}

win32-g++* {
    DEFINES += WIN32
    DEFINES += WIN32_MEAN_AND_LEAN
    DEFINES += _WIN32_WINNT=0x0501
    DEFINES += _SCL_SECURE_NO_WARNINGS
    TEMPLATE = lib
    INCLUDEPATH += $$(BOOST_HOME)/include \
                   ../../lib/win32-mingw/include
}
linux-* {
    DEFINES += UNIX
    DEFINES += LINUX
    TEMPLATE = lib

    INCLUDEPATH += $$(BOOST_HOME)/include
}
linux-g++-64|linux-icc*|linux-cc-64 {
    INCLUDEPATH += ../../lib/linux-x86_64/include
}
linux-g++-32|linux-icc-32|linux-cc {
    INCLUDEPATH += ../../lib/linux-i686/include
}
linux-cc* {
    QMAKE_CXXFLAGS += -D_GNU_SOURCE
}
solaris-cc*|solars-g++* {
    DEFINES += UNIX
    DEFINES += SOLARIS
    TEMPLATE = lib

    INCLUDEPATH += $$(BOOST_HOME)/include
    INCLUDEPATH += ../../lib/solaris/include
}
solaris-g++* {
    QMAKE_CXXFLAGS += -fPIC -pthreads
}
macx-g++* {
    DEFINES += UNIX
    TEMPLATE = lib

    INCLUDEPATH += $$(BOOST_HOME)/include

    INCLUDEPATH += ../../lib/darwin-i686/include
}
macx-g++ {
    INCLUDEPATH += /opt/local/include
}

PRE_TARGETDEPS = 

DEPENDPATH += $$INCLUDEPATH


*-g++* {
    coverage.CONFIG += recursive
    QMAKE_EXTRA_UNIX_TARGETS += coverage
    QMAKE_EXTRA_UNIX_TARGETS += cov_cxxflags cov_lflags
    cov_cxxflags.target  = coverage
    cov_cxxflags.depends = CXXFLAGS += -fprofile-arcs -ftest-coverage
    cov_lflags.target  = coverage
    cov_lflags.depends = LFLAGS += -fprofile-arcs -ftest-coverage
    coverage.commands = @echo "Built with coverage support..."
    build_pass|!debug_and_release:coverage.depends = all
    QMAKE_CLEAN += $(OBJECTS_DIR)/*.gcda $(OBJECTS_DIR)/*.gcno

    profile.CONFIG += recursive
    QMAKE_EXTRA_UNIX_TARGETS += profile
    QMAKE_EXTRA_UNIX_TARGETS += prof_cxxflags prof_lflags
    prof_cxxflags.target  = profile
    prof_cxxflags.depends = CXXFLAGS += -g -pg
    prof_lflags.target  = profile
    prof_lflags.depends = LFLAGS += -pg
    profile.commands = @echo "Built with profiling support..."
    build_pass|!debug_and_release:profile.depends = all

    perftools.CONFIG += perftools
    QMAKE_EXTRA_UNIX_TARGETS += perftools
    QMAKE_EXTRA_UNIX_TARGETS += perftools_cxxflags perftools_lflags
    perftools_cxxflags.target  = perftools
    perftools_cxxflags.depends =
    perftools_lflags.target  = perftools
    perftools_lflags.depends = LFLAGS += -ltcmalloc -lpthread
    perftools.commands = @echo "Built with perftools support..."
    build_pass|!debug_and_release:perftools.depends = all
}

