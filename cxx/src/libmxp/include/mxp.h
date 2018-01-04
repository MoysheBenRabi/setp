#ifndef MXP_H
#define MXP_H

/* Copyright (c) 2009-2010 Tyrell Corporation & Moyshe Ben Rabi.

   The contents of this file are subject to the Mozilla Public License
   Version 1.1 (the "License"); you may not use this file except in
   compliance with the License. You may obtain a copy of the License at
   http://www.mozilla.org/MPL/

   Software distributed under the License is distributed on an "AS IS"
   basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
   License for the specific language governing rights and limitations
   under the License.

   The Original Code is an implementation of the Metaverse eXchange Protocol.

   The Initial Developer of the Original Code is Akos Maroy and Moyshe Ben Rabi.
   All Rights Reserved.

   Contributor(s): Akos Maroy and Moyshe Ben Rabi.

   Alternatively, the contents of this file may be used under the terms
   of the Affero General Public License (the  "AGPL"), in which case the
   provisions of the AGPL are applicable instead of those
   above. If you wish to allow use of your version of this file only
   under the terms of the AGPL and not to allow others to use
   your version of this file under the MPL, indicate your decision by
   deleting the provisions above and replace them with the notice and
   other provisions required by the AGPL. If you do not delete
   the provisions above, a recipient may use your version of this file
   under either the MPL or the AGPL.
*/

/*! \mainpage Metaverse eXchange Protocol

    This is the C++ implementation of the Metaverse eXchange Protocol.
    Currently this is in a very early stage of development.
    This implementation is provided under the AGPL license.

    - \subpage compiling
    - \subpage typical_usage
    - \subpage contributing
    - \subpage license
 */

/*! \page compiling Compiling MXP

    MXP is known to compile and work on the following UNIX-like systems
    and compilers:

    - Linux (32 bit and 64 bit) using:
      - GNU g++
      - Intel C++ compiler
      - Sun Studio C++ compiler
    - MacOS X using XCode (which is a version of gcc)
    - Open Solaris, using:
      - Sun Studio C++ compiler
      - GNU g++

    Specific sections of this page include:

     - \ref compile_unix including Linux and MacOS X
     - \ref compile_lin_icc
     - \ref compile_lin_cc
     - \ref compile_solaris
       - \ref compile_solaris_gcc
     - \ref cross_compile
     - \ref compile_windows
       - \ref compile_mingw
       - \ref compile_msvc

    \section dependencies Dependnecies

    MXP is written in C++, thus you'll need a C++ compiler to use this
    library.

    This library uses qmake as a build tool, which is part of the Qt
    toolkit. This library does not depend on Qt as library. See
    http://qt.nokia.com/ for more on Qt.

    This library depends on the boost library. See http://boost.org/ for
    more on boost.

    This library depends on zlib for reading/writing zip files. See
    http://www.zlib.net/ for details. zlib is usually included on UNIX
    systems. for Windows, the MXP source tree includes zlib for convenience.

    \section compile_unix Compiling MXP on UNIX-like systems using GNU g++

    \par set up the dependencies

    Download and install Qt Creator from the Qt product site, so that you
    have a working qmake. Let's assume that Qt Create is installed under
    /opt/qtsdk-2009.03 , and that the Qt tools are contained inside under
    /opt/qtsdk-2009.03/qt

    Download boost from the boost we boost web site, and compile it for
    your platform. Refer to the boost documentation for details, but in
    general, this looks like this:

    \code
    tar xfj boost_1_40_0.tar.bz2
    cd boost_1_40_0
    ./bootstrap.sh
    ./bjam --prefix=/path/to/boost install
    \endcode

    \par compile MXP

    After the dependencies are set up, the the MXP source code, and
    do the following. These commands work for the following platform and
    compiler toolchains

    - Linux 32 and 64 bit using gcc
    - MacOS X using XCode

    In the MXP source directory, execute the following:

    \code
    # make sure qmake is accessible
    export PATH=$PATH:/opt/qtsdk-2009.03/qt/bin
    # tell the build where to find boost
    export BOOST_HOME=/path/to/boost
    # run qmake to generate Makefiles
    qmake -recursive
    # build MXP
    make
    \endcode

    Alternatively, one can start the Qt Creator application, and open
    the mxp.pro project file in it and compile. Do make sure to set
    the BOOST_HOME environment variable before doing so, or inside Qt
    Creator itself.

    \section compile_lin_icc Compiling MXP on Linux with the Intel C++ compiler

    MXP compiles using the Intel C++ compiler as well. To do this, execute
    the following in the MXP source directory:

    \code
    export PATH=$PATH:/opt/intel/Compiler/11.1/059/bin/intel64
    export PATH=$PATH:/opt/qtsdk-2009.03/qt/bin
    export BOOST_HOME=/path/to/boost
    qmake -recursive -spec linux-icc
    make
    \endcode

    \section compile_lin_cc Compiling MXP on Linux with the Sun C++ compiler

    To compile MXP using the Sun Studio C++ compiler, first download and
    install Sun Studio, for example into /opt/SunStudio.

    To compile boost using the Sun C++ compiler, download boost 1.41.0 or
    later (boost 1.40.0 is known not to work), and execute the following:

    \code
    tar xfj boost_1_41_0.tar.bz
    cd boost_1_41_0
    # put the Sun C++ into path - it has to be the first in the path
    export PATH=/opt/SunStudio/sunstudio12.1/bin:$PATH
    # specify the Sun toolset for bootstrap
    ./bootstrap.sh --with-toolset=sun
    # build & install boost, some special flags are needed
    ./bjam toolset=sun cxxflags="-D_GNU_SOURCE  -features=zla" --prefix=/path/to/boost install
    \endcode

    Obtain qmake from a Linux Qt toolset, as described in the normal Linux
    compilation instruction.

    To unpack and compile the MXP code, do the following. Specify linux-cc-32
    or linux-cc-64 as per your platform:

    \code
    export PATH=/opt/SunStudio/sunstudio12.1/bin:$PATH
    export PATH=$PATH:/opt/qtsdk-2009.03/qt/bin
    export BOOST_HOME=/path/to/boost
    qmake -recursive -spec etc/mkspec/linux-cc-64
    make
    \endcode

    \section compile_solaris Compiling on Open Solaris

    To compile on Open Solaris, one needs to install the following packages
    first:

    \code
    pfexec pkg install sunstudio SUNWhea SUNWaudh SUNWsfwhea SUNWxorg-headers
    \endcode

    As qmake doesn't come in a package on Open Solaris, and it's not available
    as a binary download from the Qt site either, one has to build qmake
    manually - but not the entire Qt library set. To have a working qmake,
    execute the following:

    \code
    wget ftp://ftp.qt.nokia.com/qt/source/qt-all-opensource-src-4.5.3.tar.gz
    /usr/gnu/bin/tar xfz qt-all-opensource-src-4.5.3.tar.gz
    cd qt-all-opensource-src
    # make sure the cc command is the compiler's cc, not gcc:
    export PATH=/usr/bin:$PATH
    ./configure --prefix=/path/to/qt --opensource
    cp -r bin /path/to/qt
    cp -r mkspecs /path/to/qt
    \endcode

    This will result in a working qmake on /path/to/qt/bin.

    To compile boost, download boost 1.41.0 (1.40.0 is known not to work),
    and execute the following:

    \code
    tar xfj boost_1_41_0.tar.bz
    cd boost_1_41_0
    # make sure the cc command is the compiler's cc, not gcc:
    export PATH=/usr/bin:$PATH
    # specify the Sun toolset for bootstrap & compilation
    ./bootstrap.sh --with-toolset=sun
    ./bjam toolset=sun --prefix=/path/to/boost install
    # for some reason, boost test is not build outright, so let's do so:
    ./bjam toolset=sun --with-test --prefix=/path/to/boost install
    \endcode

    Now get the MXP source code & compile:

    \code
    # make sure the cc command is the compiler's cc, not gcc:
    export PATH=/usr/bin:$PATH
    export PATH=$PATH:/path/to/qt/bin
    export BOOST_HOME=/path/to/boost
    # make sure to specify solaris-cc explicitly:
    qmake -recursive -spec solaris-cc
    make
    \endcode

    \subsection compile_solaris_gcc Compile on Open Solaris using g++

    To compile on Open Solaris, one needs to install the following packages
    first:

    \code
    pfexec pkg install SUNWgcc SUNWhea SUNWaudh SUNWsfwhea SUNWxorg-headers
    \endcode

    As qmake doesn't come in a package on Open Solaris, and it's not available
    as a binary download from the Qt site either, one has to build qmake
    manually - but not the entire Qt library set. To have a working qmake,
    execute the following:

    \code
    wget ftp://ftp.qt.nokia.com/qt/source/qt-all-opensource-src-4.5.3.tar.gz
    /usr/gnu/bin/tar xfz qt-all-opensource-src-4.5.3.tar.gz
    cd qt-all-opensource-src
    ./configure --prefix=/path/to/qt --opensource --platform=solaris-g++
    cp -r bin /path/to/qt
    cp -r mkspecs /path/to/qt
    \endcode

    This will result in a working qmake on /path/to/qt/bin.

    To compile boost, download boost 1.41.0 (1.40.0 is known not to work),
    and execute the following:

    \code
    tar xfj boost_1_41_0.tar.bz
    cd boost_1_41_0
    ./bootstrap.sh
    ./bjam --prefix=/path/to/boost install
    \endcode

    Now get the MXP source code & compile:

    \code
    export PATH=$PATH:/path/to/qt/bin
    export BOOST_HOME=/path/to/boost
    qmake -recursive -spec solaris-g++
    gmake
    \endcode

    \subsection cross_compile Cross-compiling MXP on Linux for Windows and MacOS

    \par cross-compiling on Linux for Windows

    For this, one needs to set up a gcc / mingw based cross-compile toolchian.
    See
    http://cross-compile.info/wiki/Toolchain:_host:_Linux,_gcc4,_target:_mingw32,_gcc3
    for more info on how to do this.

    One also needs to cross-compile boost, or copy the boost libraries compiled
    for mingw / gcc. On how to cross-compile boost, see
    http://cross-compile.info/wiki/Cross-compiling_libraries_using_bjam

    After this is set up, execute the following in the MXP source directory:

    \code
    # include the cross-compile toolchain in PATH
    export PATH=$PATH:/opt/i586-mingw32/bin
    # make sure to get qmake
    export PATH=$PATH:/opt/qtsdk-2009.03/qt/bin
    # tell the build system where to find boost
    export BOOST_HOME=/path/to/boost
    # have qmake create a Makefile that uses the cross-compile toolchain
    qmake -recursive -spec etc/mkspecs/win32-g++-i586-mingw32
    # build MXP
    make
    \endcode

    \par cross-compiling on Linux for MacOS X

    For this, one needs to set up a gcc / mingw based cross-compile toolchian.
    See
    http://cross-compile.info/wiki/Toolchain:_host:_Linux,_gcc4,_target:_MacOS_X,_gcc4
    for more info on how to do this.

    One also needs to cross-compile boost, or copy the boost libraries compiled
    for MacOS X. On how to cross-compile boost, see
    http://cross-compile.info/wiki/Cross-compiling_libraries_using_bjam

    After this is set up, execute the following in the MXP source directory:

    \code
    # include the cross-compile toolchain in PATH
    export PATH=$PATH:/opt/i686-apple-darwin9/bin
    # make sure to get qmake
    export PATH=$PATH:/opt/qtsdk-2009.03/qt/bin
    # tell the build system where to find boost
    export BOOST_HOME=/path/to/boost
    # have qmake create a Makefile that uses the cross-compile toolchain
    qmake -recursive -spec etc/mkspecs/macx-g++-i686-apple-darwin9
    # build MXP
    make
    \endcode


    \section compile_windows Compiling MXP on Windows

    MXP is known to compile and work on Windows, using the following
    compilers:

    - gcc, based on mingw, as supplied with Qt Creator
    - Microsoft Visual Studio

    \subsection compile_mingw Compiling MXP on Windows using mingw / gcc

    MXP compiles using mingw using gcc, which is shipped with Qt Creator,
    among others.

    \par set up the dependencies

    Download and install Qt Creator from the Qt product site, so that you
    have a working qmake. Let's assume that Qt Create is installed under
    C:\\Qt\\2009.03 , and that the Qt tools are contained inside under
    C:\\Qt\\2009.03\\qt . Qt will install the mingw development tools,
    including gcc, under C:\\Qt\\2009.03\\mingw

    Download boost from the boost we boost web site, and compile it for
    your platform. Refer to the boost documentation for details, but in
    general, this looks like this:

    \code
    :: make sure the mingw gcc supplied by the Qt toolkit is available
    set PATH=%PATH%;C:\Qt\2009.03\mingw\bin
    :: change into the boost source directory
    cd boost_1_40_0
    :: do bootstrapping manually so that bjam is compiled using gcc
    cd tools\jam\src
    .\build.bat gcc
    copy bin.ntx86\bjam.exe ..\..\..
    cd ..\..\..
    :: build boost using the gcc toolset
    .\bjam toolset=gcc --prefix=C:\path\to\boost install
    :: fix the installation include path
    move C:\path\to\boost\include\boost-1_40\boost C:\path\to\boost\include\boost
    rmdir C:\path\to\boost\include\boost-1_40
    \endcode

    \par compile MXP

    After the dependencies are set up, get the MXP source code, and
    do the following in the MXP source directory:

    \code
    :: make sure the mingw gcc supplied by the Qt toolkit is available
    set PATH=%PATH%;C:\Qt\2009.03\mingw\bin
    :: make sure qmake is accessible
    set PATH=%PATH%;C:\Qt\2009.03\qt\bin
    :: tell the build where to find boost
    set BOOST_HOME=C:\path\to\boost
    :: run qmake to generate Makefiles
    qmake -recursive
    :: build MXP
    mingw32-make
    \endcode

    Alternatively, one can start the Qt Creator application, and open
    the mxp.pro project file in it and compile. Do make sure to set
    the BOOST_HOME environment variable before doing so, or inside Qt
    Creator itself.

    \subsection compile_msvc Compiling MXP on Windows using MS Visual Studio

    \par set up the dependencies

    Download and install Qt Creator from the Qt product site, so that you
    have a working qmake. Let's assume that Qt Create is installed under
    C:\\Qt\\2009.03 , and that the Qt tools are contained inside under
    C:\\Qt\\2009.03\\qt

    Obtain a copy of the Microsoft Visual Studio, and install it.

    Download boost from the boost we boost web site, and compile it for
    your platform. Refer to the boost documentation for details, but in
    general, after the boost source zip file is unzipped:

    \code
    :: make sure the MS C++ compiler is available
    "C:\Program Files\Microsoft Visual Studio 9.0\VC\bin\vcvars32.bat"
    :: compile & install boost
    cd boost_1_40_0
    .\bootstrap.bat
    .\bjam --prefix=C:\path\to\boost install
    :: fix the installation include path
    move C:\path\to\boost\include\boost-1_40\boost C:\path\to\boost\include\boost
    rmdir C:\path\to\boost\include\boost-1_40
    \endcode

    \par compile MXP

    After the dependencies are set up, get the MXP source code, and
    do the following to create a Visual Studio project file and then
    compile using Visual Studio, from within the MXP source directory:

    \code
    :: make sure the MS C++ compiler is available
    "C:\Program Files\Microsoft Visual Studio 9.0\VC\bin\vcvars32.bat"
    :: make sure qmake is accessible
    set PATH=%PATH%;C:\Qt\2009.03\qt\bin
    :: tell the build where to find boost
    set BOOST_HOME=C:\path\to\boost
    :: run qmake to generate Makefiles
    qmake -recursive -spec win32-msvc2008
    :: build mxp
    devenv mxp.sln /Build
    \endcode

    Alternatively, one can open the generated Visual Studio solution file,
    mxp.sln, using the MS Visual Studio IDE, and compile from there.

 */

/*! \page typical_usage Typical MXP usage

    The typical client user will create an mxp::net::client_communication
    object, that connects to a server, and send and receive messages from
    the mxp::message namespace, using this client object.

    \code
    #include <mxp.h>

    using namespace boost;
    using namespace mxp;

    void message_received(shared_ptr<message::message> message,
                          uint32_t                     session_id,
                          asio::ip::udp::endpoint      endpoint) {
        // handle received messages here
    }

    // create the client
    mxp::net::client_communication  client;

    // connect the event handler for received messages
    client.connect_message_handler(&message_received);

    // look up a server on host "myserver" on port 5665
    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "myserver", "5665");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);

    // open a session to the server by sending a join request
    shared_ptr<message::join_rq>    join_rq(new message::join_rq());
    // make sure to populate the join request message...
    uint32_t session_id = client.open_session(server_address, join_rq);

    // send additional messages as needed - all received messages will be
    // signaled in the message_recieved call above
    shared_ptr<message::message>    msg = ...;
    client.send(session_id, msg, true);

    // close the session when not needed anymore
    client.close_session(session_id);
    \endcode

    The typical server user of this library will create an
    mxp::net::server_communication object if he wants to run a server,
    and use it send and receive messages from the mxp::message namespace.

    \code
    #include <mxp.h>

    using namespace boost;
    using namespace mxp;

    void message_received(shared_ptr<message::message> message,
                          uint32_t                     session_id,
                          asio::ip::udp::endpoint      endpoint) {
        // handle received messages here
    }

    // sample code to bind to port 5665
    asio::ip::udp::endpoint         server_endpoint(asio::ip::udp::v4(), 5665);
    mxp::net::server_communication  server(server_endpoint);

    // connect the event handler for received messages
    server.connect_message_handler(&message_received);

    // on receiving messages from client, the message_recieved signal will
    // be notified above
    \endcode

    \see mxp::net::communication
    \see mxp::net
    \see mxp::message

    \see http://www.bubblecloud.org/
  */

/*! \page contributing Contributing to MXP

    All contributions to this library are very welcome. If you'd do so,
    please consider the following first, not comprehensive list.

    \section coding_style Coding style

    MXP is written using a standard-oriented C++ approach, with the following
    in mind:

    - portability, across platfroms and across compilers
    - following coding style of the C++ standard library / STL / boost library

    Please make sure that the contrubtions you're planning to make are made
    with a similar mindset. Yes, it does take time and effort to make sure
    a piece of C++ code compiles on a number of C++ compilers, like the
    GNU g++, Intel C++ compiler, or Microsoft Visual C++. But it is worth the
    effort - the more portable the code, better in quality it is.

    \par consider compiler warnings, from all compilers

    Don't disregard, but consider compiler warnings, from various compilers.
    They are a great help, and offer you hints on how to improve your code.

    \par use namespaces

    The code is organized into embedded namespaces, like mxp::message, etc.
    Please continue using this approach. The namespaces are reflected in the
    directory structure as well: include files for each namespace are
    contained in a separate include subdirectory, e.g. mxp/message for the
    mxp::message namespace.

    \par use lower case identifiers, with underscore separation

    For class and type names, use lower case names, with the underscore
    character as word separator, e.g. some_type, and not SomeType or
    CSomeType.

    \par don't use hungarian notation

    Don't use the hungarian notation for variable or argument names,
    it really makes the code difficult to read, while adding nothing
    in usefulness.

    \section code_formatting Code formatting

    Groupwork is best done if there are agreed guidelines that each group
    member follows. This includes code formatting as well. Some generic
    code formatting quidelines used in this library:

    \par indentation - no tabs - use 4 spaces

    For indentation, don't use tabs - use 4 spaces instead. don't use tab
    characters in any part of the source code - period. Do indent your code.

    \par use compound statements on control flow structures

    For control flow structures, like if, for, while, always use compound
    statements, for example:

    \code
    if (...) {
        ...
    }
    \endcode

    \par line length - 80 characters at most

    Use lines that are 80 characters in length at most. Break the line if it
    would become longer. factor out your code into a separate function if
    it's already indented too deep.

    \par enclose include files with defines - no \#pragma once

    For include files, make sure to enclose them with \#ifndef \#define ..
    \#endif statements, to make sure their content is only included once. do not
    use the non-portable \#pragma once statement.

    for example:

    \code
    #ifndef MXP_SOME_INCLUDE_H
    #define MXP_SOME_INCLUDE_H
    ...
    #endif
    \endcode

    \section literature Recommended reading

    The following books give enlightening and insightful read on C++, and thus
    are highly recommended:

    - Scott Meyers: Effective C++, Effective STL, More Effective C++,
      http://www.aristeia.com/books.html
    - Herb Sutter & Andrei Alexandrescu: C++ Coding Standards,
      http://www.gotw.ca/publications/c++cs.htm
    - Nicolai Josuttis: The C++ Standard Library,
      http://www.josuttis.com/libbook/

 */

/*! \page license License

   Copyright (c) 2009-2010 Tyrell Corporation & Moyshe Ben Rabi.

   The contents of this file are subject to the Mozilla Public License
   Version 1.1 (the "License"); you may not use this file except in
   compliance with the License. You may obtain a copy of the License at
   http://www.mozilla.org/MPL/

   Software distributed under the License is distributed on an "AS IS"
   basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
   License for the specific language governing rights and limitations
   under the License.

   The Original Code is an implementation of the Metaverse eXchange Protocol.

   The Initial Developer of the Original Code is Akos Maroy and Moyshe Ben Rabi.
   All Rights Reserved.

   Contributor(s): Akos Maroy and Moyshe Ben Rabi.

   Alternatively, the contents of this file may be used under the terms
   of the Affero General Public License (the  "AGPL"), in which case the
   provisions of the AGPL are applicable instead of those
   above. If you wish to allow use of your version of this file only
   under the terms of the AGPL and not to allow others to use
   your version of this file under the MPL, indicate your decision by
   deleting the provisions above and replace them with the notice and
   other provisions required by the AGPL. If you do not delete
   the provisions above, a recipient may use your version of this file
   under either the MPL or the AGPL.
 */

/** \namespace mxp

    This is the C++ implementation of the Metaverse eXchange Protocol.
    Currently this is in a very early stage of development.
    This implementation is provided under the AGPL license.

    To see the typical usage of this library, refer to \ref typical_usage
  */

#include <mxp/serialization.h>
#include <mxp/message.h>
#include <mxp/packet.h>
#include <mxp/net.h>
#include <mxp/services.h>

#endif

