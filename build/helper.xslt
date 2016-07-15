<?xml version="1.0" encoding="utf-8"?>

<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform">

  <!-- HELPER -->

  <variable name="whitespace" select="'&#09;&#10;&#13; '" />

  <!-- Strips trailing whitespace characters from 'string' -->
  <template name="string-rtrim">
    <param name="string" />
    <param name="trim" select="$whitespace" />

    <variable name="length" select="string-length($string)" />

    <if test="$length &gt; 0">
      <choose>
        <when test="contains($trim, substring($string, $length, 1))">
          <call-template name="string-rtrim">
            <with-param name="string" select="substring($string, 1, $length - 1)" />
            <with-param name="trim"   select="$trim" />
          </call-template>
        </when>
        <otherwise>
          <value-of select="$string" />
        </otherwise>
      </choose>
    </if>
  </template>

  <!-- Strips leading whitespace characters from 'string' -->
  <template name="string-ltrim">
    <param name="string" />
    <param name="trim" select="$whitespace" />

    <if test="string-length($string) &gt; 0">
      <choose>
        <when test="contains($trim, substring($string, 1, 1))">
          <call-template name="string-ltrim">
            <with-param name="string" select="substring($string, 2)" />
            <with-param name="trim"   select="$trim" />
          </call-template>
        </when>
        <otherwise>
          <value-of select="$string" />
        </otherwise>
      </choose>
    </if>
  </template>

  <!-- Strips leading and trailing whitespace characters from 'string' -->
  <template name="string-trim">
    <param name="string" />
    <param name="trim" select="$whitespace" />
    <call-template name="string-rtrim">
      <with-param name="string">
        <call-template name="string-ltrim">
          <with-param name="string" select="$string" />
          <with-param name="trim"   select="$trim" />
        </call-template>
      </with-param>
      <with-param name="trim"   select="$trim" />
    </call-template>
  </template>

</stylesheet>