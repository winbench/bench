using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    interface IDocumentWriter
    {
        void BeginDocument();

        void EndDocument();

        void Title(string format, params object[] args);

        void BeginSection(string format, params object[] args);

        void EndSection();

        void BeginParagraph();

        void EndParagraph();

        void BeginLine();

        void EndLine();

        void Line(string format, params object[] args);

        void BeginList();

        void EndList();

        void BeginListItem();

        void EndListItem();

        void ListItem(string format, params object[] args);

        void BeginSyntaxList(string format, params object[] args);

        void EndSyntaxList();

        void BeginSyntaxListItem();

        void EndSyntaxListItem();

        void BeginDetail();

        void EndDetail();

        void BeginDefinitionList();

        void EndDefinitionList();

        void BeginDefinition(string format, params object[] args);

        void EndDefinition();

        void Definition(string key, string format, params object[] args);

        void Text(string format, params object[] arg);

        void SyntaxElement(string format, params object[] arg);

        void Keyword(string format, params object[] arg);

        void Variable(string format, params object[] arg);
    }
}
