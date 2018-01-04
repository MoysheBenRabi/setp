<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" indent="yes"/>

<xsl:template match="/TestLog/TestSuite">
    <testsuites>
    <xsl:call-template name="testsuite"/>
    </testsuites>
</xsl:template>

<xsl:template name="testsuite">
    <xsl:for-each select="TestSuite">
        <testsuite>
            <xsl:attribute name="errors">
                <xsl:value-of select="count(TestCase/Exception)"/>
            </xsl:attribute>
            <xsl:attribute name="failures">
                <xsl:value-of select="count(TestCase/Error)"/>
            </xsl:attribute>
            <xsl:attribute name="tests">
                <xsl:value-of select="count(TestCase)"/>
            </xsl:attribute>
            <xsl:attribute name="package">
                <xsl:value-of select="@name"/>
            </xsl:attribute>
            <xsl:attribute name="name">
                <xsl:value-of select="@name"/>
            </xsl:attribute>
            <xsl:attribute name="time">
                <xsl:value-of select="sum(TestingTime) div 1000000"/>
            </xsl:attribute>

            <xsl:call-template name="testcase"/>
        </testsuite>
    </xsl:for-each>
</xsl:template>

<xsl:template name="testcase">
    <xsl:for-each select="TestCase">
        <testcase>
            <xsl:attribute name="classname">
                <xsl:value-of select="substring-before(@name, '::')"/>
            </xsl:attribute>
            <xsl:attribute name="name">
                <xsl:value-of select="substring-after(@name, '::')"/>
            </xsl:attribute>
            <xsl:attribute name="time">
                <xsl:value-of select="TestingTime div 1000000"/>
            </xsl:attribute>
            <xsl:choose>
                <xsl:when test="Error">
                    <failure>
                        <xsl:attribute name="message">
                            <xsl:value-of select="Error"/>
                        </xsl:attribute>
                        <xsl:attribute name="type">
                            error
                        </xsl:attribute>
                        <xsl:value-of select="Error"/>
                        File: <xsl:value-of select="Error/@file"/>
                        Line: <xsl:value-of select="Error/@line"/>
                    </failure>
                </xsl:when>
                <xsl:when test="Exception">
                    <error>
                        <xsl:attribute name="message">
                            <xsl:value-of select="Exception"/>
                        </xsl:attribute>
                        <xsl:attribute name="type">
                            <xsl:value-of select="substring-before(Exception, ': ')"/>
                        </xsl:attribute>
                        <xsl:value-of select="Exception"/>
                        File: <xsl:value-of select="Exception/LastCheckpoint/@file"/>
                        Line: <xsl:value-of select="Exception/LastCheckpoint/@line"/>
                    </error>
                </xsl:when>
            </xsl:choose>
        </testcase>
    </xsl:for-each>
</xsl:template>

<xsl:template match="text()|@*"/>

</xsl:stylesheet>
