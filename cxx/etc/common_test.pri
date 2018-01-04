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

DESTDIR = bin
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

    # also tell the same to the linker
    QMAKE_LFLAGS += -library=stlport4 -features=tmplife \
                    -features=tmplrefstatic +d
    QMAKE_LFLAGS += -mt
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
    TEMPLATE = vcapp
    DEFINES += WIN32
    DEFINES += WIN32_MEAN_AND_LEAN
    DEFINES += _WIN32_WINNT=0x0501
    DEFINES += _SCL_SECURE_NO_WARNINGS
    DEFINES += _CRT_SECURE_NO_WARNINGS

    DEFINES +=  UNICODE

    INCLUDEPATH += $$(BOOST_HOME)/include \
                   ../../lib/win32-msvc/include
    LIBS += -L$$(BOOST_HOME)/lib \
            -L../../lib/win32-msvc/lib

    build_pass:CONFIG(release, debug|release) {
        OBJECTS_DIR = release

        LIBS += -lzlib
    }
    build_pass:CONFIG(debug, debug|release) {
        TARGET = $$join(TARGET,,,d)
        DEFINES +=  _DEBUG
        OBJECTS_DIR = debug

        LIBS += -lzlibd
    }
}

win32-g++* {
    TEMPLATE = app
    DEFINES += WIN32
    DEFINES += WIN32_MEAN_AND_LEAN
    DEFINES += _WIN32_WINNT=0x0501
    DEFINES += _SCL_SECURE_NO_WARNINGS
    INCLUDEPATH += $$(BOOST_HOME)/include \
                   ../../lib/win32-mingw/include
    LIBS += -L$$(BOOST_HOME)/lib \
            -L../../lib/win32-mingw/lib
}
win32-g++ {
    LIBS += -llibboost_thread-mgw34-mt \
            -llibboost_system-mgw34-mt \
            -llibboost_unit_test_framework-mgw34-mt \
            -llibboost_test_exec_monitor-mgw34-mt \
            -lws2_32 -lmswsock -lz
}
win32-g++-* {
    LIBS += -llibboost_thread_win32 \
            -llibboost_system \
            -llibboost_unit_test_framework \
            -llibboost_test_exec_monitor \
            -lws2_32 -lmswsock -lz
}

linux-* {
    TEMPLATE = app
    DEFINES += UNIX
    DEFINES += LINUX

    INCLUDEPATH += $$(BOOST_HOME)/include
    LIBS += -L$$(BOOST_HOME)/lib \
            -lboost_thread \
            -lboost_system \
            -lpthread
}
linux-g++-64|linux-icc*|linux-cc-64 {
    INCLUDEPATH += ../../lib/linux-x86_64/include
    LIBS += -L../../lib/linux-x86_64/lib
}
linux-g++-32|linux-icc-32|linux-cc {
    INCLUDEPATH += ../../lib/linux-i686/include
    LIBS += -L../../lib/linux-i686/lib
}
linux-cc* {
    QMAKE_CXXFLAGS += -D_GNU_SOURCE
}
solaris-cc*|solars-g++* {
    TEMPLATE = app
    DEFINES += UNIX
    DEFINES += SOLARIS

    INCLUDEPATH += $$(BOOST_HOME)/include
    LIBS += -L$$(BOOST_HOME)/lib \
            -lboost_thread \
            -lboost_system \
            -lboost_unit_test_framework \
            -lboost_test_exec_monitor \
            -lsocket -lnsl

    INCLUDEPATH += ../../lib/solaris/include
    LIBS += -L../../lib/solaris/lib
}
solaris-g++* {
    QMAKE_CXXFLAGS += -fPIC -pthreads
    QMAKE_LFLAGS += -fPIC -pthreads
}
macx-g++* {
    DEFINES += UNIX
    TEMPLATE = app

    INCLUDEPATH += $$(BOOST_HOME)/include
    LIBS += -L$$(BOOST_HOME)/lib

    INCLUDEPATH += ../../lib/darwin-i686/include
    LIBS += -L../../lib/darwin-i686/lib
}
macx-g++ {
    INCLUDEPATH += /opt/local/include
    LIBS+=-L/opt/local/lib

    LIBS += -lboost_thread-mt \
            -lboost_system-mt \
}
macx-g++-i686-apple-darwin9 {
    LIBS += -lboost_thread \
            -lboost_system
}

DEPENDPATH += $$INCLUDEPATH


linux-* {
    test.target = test
    test.depends = $$DESTDIR/$$TARGET.exe
    unix {
        test.commands = $$DESTDIR/$$TARGET --output_format=XML --log_level=all  > tmp/test_log 2> tmp/test_report; \
                        xsltproc ../../etc/utf2junit.xsl tmp/test_log > tmp/TEST-$${TARGET}.xml
    }
    QMAKE_EXTRA_UNIX_TARGETS += test
    QMAKE_CLEAN += tmp/test_* tmp/TEST-*
}

win32-g++* {
    test.target = test
    test.depends = $$DESTDIR/$$TARGET.exe
    win32 {
        test.commands = $$DESTDIR\\$$TARGET --output_format=XML --log_level=all  > tmp/test_log 2> tmp/test_report; 
        # TODO: execute xslt transformation on windows as well
    }
    QMAKE_EXTRA_UNIX_TARGETS += test
    QMAKE_CLEAN += tmp/test_* tmp/TEST-*
}

macx-g++* {
    test.target = test
    test.depends = all
    unix {
        test.commands = $$DESTDIR/$${TARGET}.app/Contents/MacOS/$$TARGET --output_format=XML --log_level=all  > tmp/test_log 2> tmp/test_report; \
                        xsltproc ../../etc/utf2junit.xsl tmp/test_log > tmp/TEST-$${TARGET}.xml
    }
    QMAKE_EXTRA_UNIX_TARGETS += test
    QMAKE_CLEAN += tmp/test_* tmp/TEST-*
}

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
    coverage.commands = @echo "Built with profiling support..."
    build_pass|!debug_and_release:profile.depends = all

    profile_report.target = profile_report
    profile_report.depends = test
    profile_report.commands = gprof $$DESTDIR/$$TARGET > tmp/profile;
    QMAKE_EXTRA_UNIX_TARGETS += profile_report
    QMAKE_CLEAN += gmon.out tmp/profile

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

