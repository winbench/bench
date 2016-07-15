<?xml version="1.0"?>

<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform">

  <include href="xml-doc-to-md.xslt"/>

  <output encoding="utf-8" method="text" omit-xml-declaration="yes" standalone="yes" />

  <template match="/member">
    <text>## Method `</text>
    <value-of select="@name"/>
    <text>`
</text>
    <apply-templates select="summary"/>
    <apply-templates select="remarks"/>
  </template>

  <template match="summary">
    <apply-templates />
  </template>

  <template match="remarks">
    <text>### Remarks
</text>
  </template>

</stylesheet>
