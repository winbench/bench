<?xml version="1.0"?>

<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform">

  <include href="helper.xslt"/>

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
    <text>`</text>
    <value-of select="@cref"/>
    <text>`
</text>
  </template>

  <template match="text()">
    <call-template name="string-trim">
      <with-param name="string" select="." />
    </call-template>
    <text>
</text>
  </template>

</stylesheet>
