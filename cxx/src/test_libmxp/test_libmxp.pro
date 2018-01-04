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
TARGET = test_libmxp
QT -= core \
    gui
HEADERS += src/test_deserialization.h \
    src/test_serialization.h \
    src/test_two_way_serialization.h \
    src/test_message_serialization.h \
    src/test_message_frame.h \
    src/test_packet.h \
    src/test_helpers.h \
    src/test_packetizer.h \
    src/test_connection.h \
    src/test_communication.h \
    src/reference_messages.h \
    src/ioapi.h \
    src/crypt.h \
    src/zip.h \
    src/unzip.h \
    src/test_iot_zip.h \
    src/test_session_store.h \
    src/test_bubble.h
SOURCES += src/test_deserialization.cxx \
    src/test_serialization.cxx \
    src/test_two_way_serialization.cxx \
    src/test_message_serialization.cxx \
    src/main.cxx \
    src/test_message_frame.cxx \
    src/test_packet.cxx \
    src/test_packetizer.cxx \
    src/test_connection.cxx \
    src/test_communication.cxx \
    src/reference_messages.cxx \
    src/ioapi.c \
    src/zip.c \
    src/unzip.c \
    src/test_iot_zip.cxx \
    src/test_session_store.cxx \
    src/test_bubble.cxx
RESOURCES +=
LIBS += -L../libmxp/lib
INCLUDEPATH += ../libmxp/include
CONFIG += console
CONFIG -= qt
win32-msvc* {
    CONFIG(release, debug|release):LIBS += -lmxp
    CONFIG(debug, debug|release):LIBS += -lmxpd
}
win32-g++*:LIBS += -lmxp
unix:LIBS += -lmxp \
    -lz
linux-g++*:
macx-g++:
PRE_TARGETDEPS = ../libmxp
include(../../etc/common_test.pri)
