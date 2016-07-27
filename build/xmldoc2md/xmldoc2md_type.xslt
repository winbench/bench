<?xml version="1.0"?>

<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform"
            xmlns:cp="urn:CRefParsing"
            xmlns:cf="urn:CRefFormatting">

  <param name="headlinePrefix" />
  <include href="xmldoc2md.xslt"/>

  <output encoding="utf-8" method="text" omit-xml-declaration="yes" standalone="yes" />

  <template match="member">
    <if test="typeparam">
      <text>**Type Parameter**

</text>
      <apply-templates select="typeparam" />
    </if>
    <if test="param">
      <value-of select="$headlinePrefix"/>
      <text>## Parameter

</text>
      <apply-templates select="param" />
      <text>
</text>
    </if>
    <if test="returns">
      <value-of select="$headlinePrefix"/>
      <text>## Return Value
</text>
      <apply-templates select="returns" />
      <text>
</text>
    </if>
    <if test="remarks">
      <value-of select="$headlinePrefix"/>
      <text>## Remarks
</text>
      <apply-templates select="remarks" />
    </if>
    <apply-templates select="example" />
    <if test="seealso">
      <value-of select="$headlinePrefix"/>
      <text>## See Also

</text>
      <apply-templates select="seealso" />
    </if>
    <text>
</text>

  </template>

  <template match="example">
    <value-of select="$headlinePrefix"/>
    <text>## Example
</text>
    <apply-templates />
    <text>

</text>
  </template>

</stylesheet>
