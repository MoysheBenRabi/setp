/*
 * Copyright (c) 2009-2010 Tyrell Corporation.
 *
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 *
 * The Original Code is an implementation of the Metaverse eXchange Protocol.
 *
 * The Initial Developer of the Original Code is Akos Maroy.
 * All Rights Reserved.
 *
 * Contributor(s): Akos Maroy.
 *
 * Alternatively, the contents of this file may be used under the terms
 * of the Affero General Public License (the  "AGPL"), in which case the
 * provisions of the AGPL are applicable instead of those
 * above. If you wish to allow use of your version of this file only
 * under the terms of the AGPL and not to allow others to use
 * your version of this file under the MPL, indicate your decision by
 * deleting the provisions above and replace them with the notice and
 * other provisions required by the AGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file
 * under either the MPL or the AGPL.
 */
package mxp.iot;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Enumeration;
import java.util.zip.ZipEntry;
import java.util.zip.ZipFile;

import junit.framework.TestCase;

/**
 * Inter-operability package / message test.
 * Generates a reference zip file based on this Java implementation, and
 * compares the generated zip file against an expected zip file.
 *
 * @see <a href="http://iot.bubblecloud.org/">MXP IOT</a>
 * @see <a href="http://iot.bubblecloud.org/reference_messages.aspx">
 *      Reference Messages</a>
 */
public class CompareZipTest extends TestCase {
    /**
     * The name of the zip file containing the reference packets.
     */
    private static final String REFERENCE_ZIP_FILE_NAME =
                                        "var/mxp_0_5_reference_messages.zip";

    /**
     * Compare the entries in two zip files.
     * Actually, all entries from file1 are taken and compared to entries of
     * the same name in file2. Thus, if there are additional entries in
     * file2, they are just disregarded.
     *
     * @param file1 one of the zip files.
     * @param file2 the other zip file.
     * @return true if the entries in the two zip files are the same.
     * @throws IOException on I/O errors
     */
    private boolean compareEntries(ZipFile   file1,
                                   ZipFile   file2) throws IOException {

        Enumeration<? extends ZipEntry> entries = file1.entries();
        while (entries.hasMoreElements()) {
            ZipEntry entry1 = entries.nextElement();
            assertNotNull(entry1);
            ZipEntry entry2 = file2.getEntry(entry1.getName());
            assertNotNull(entry2);

            assertEquals(entry1.getName() + " size difference",
                         entry1.getSize(), entry2.getSize());

            InputStream is1 = file1.getInputStream(entry1);
            InputStream is2 = file2.getInputStream(entry2);
            int value1;
            int value2;
            while ((value1 = is1.read()) != -1) {
                value2 = is2.read();
                assertTrue("premature end of " + entry2.getName(),
                            value2 != -1);
                assertEquals("differences in " + entry1.getName(),
                             value1, value2);
            }
        }

        return true;
    }

    /**
     * Generate a reference zip file containing all reference packages,
     * and compare it against a saved zip file, containing for-sure
     * reference message packets.
     *
     * @throws IOException on I/O errors
     */
    public void testCompareZipFiles() throws IOException {
        // generate the reference packets ourselves
        File             tmpZipFile = File.createTempFile("mxp_compare_test",
                                                          "zip");
        tmpZipFile.deleteOnExit();
        FileOutputStream fos = new FileOutputStream(tmpZipFile);
        GenerateMessages.generateMessages(fos);
        fos.flush();
        fos.close();

        // load the two zip files
        ZipFile               file1 = new ZipFile(tmpZipFile);
        ZipFile               file2 = new ZipFile(REFERENCE_ZIP_FILE_NAME);

        // make sure they have the same number of entries
        int file1Entries = 0;
        int file2Entries = 0;
        for (Enumeration<? extends ZipEntry> entries = file1.entries();
             entries.hasMoreElements(); entries.nextElement()) {
            ++file1Entries;
        }
        for (Enumeration<? extends ZipEntry> entries = file2.entries();
             entries.hasMoreElements(); entries.nextElement()) {
            ++file2Entries;
        }
        assertEquals(file1Entries, file2Entries);

        // compare the entries themselves
        compareEntries(file1, file2);
    }
}
