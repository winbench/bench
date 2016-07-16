<?xml version="1.0"?>

<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform">

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
    <call-template name="cref-label">
      <with-param name="cref" select="@cref" />
    </call-template>
    <text>`][]
</text>
  </template>

  <template name="cref-label">
    <param name="cref" />
    <variable name="sign" select="substring-after($cref, ':')" />
    <choose>
      <when test="starts-with($cref, 'T:')">
        <value-of select="substring-after($sign,'.')"/>
      </when>
      <when test="starts-with($sign, 'M:')">
        <value-of select="substring-before(substring-after($sign, '.'), '(')"/>
      </when>
    </choose>
  </template>

  <template name="cref-url">

  </template>

  <template name="cref-id">

  </template>

</stylesheet>
