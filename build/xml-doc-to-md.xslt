<?xml version="1.0"?>

<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform"
            xmlns:cp="urn:CRefParsing"
            xmlns:cf="urn:CRefFormatting">

  <template match="text()">
    <value-of select="normalize-space(.)"/>
    <text>
</text>
  </template>

  <template match="c">
    <text> `</text>
    <value-of select="."/>
    <text>` </text>
  </template>

  <template match="para">
    <apply-templates />
    <text>
</text>
  </template>

  <template match="list">
    <apply-templates select="item" />
  </template>

  <template match="item">
    <text>* </text>
    <apply-templates />
    <text>
</text>
  </template>

  <template match="term">
    <text>**</text>
    <value-of select="."/>
    <text>**</text>
  </template>

  <template match="see">
    <text>[`</text>
    <value-of select="cf:FormatLabel(@cref)"/>
    <text>`][]
</text>
  </template>

  <template name="cref-label">
    <param name="cref" />
    <variable name="member-type" select="cp:MemberType($cref)" />
    <choose>
      <when test="$member-type = 'T'">
        <value-of select="cp:TypeName($cref)"/>
      </when>
      <when test="$member-type = 'M'">
        <value-of select="cp:MethodName($cref)"/>
      </when>
    </choose>
  </template>

  <template name="cref-url">

  </template>

  <template name="cref-id">

  </template>

</stylesheet>
