using System;
using System.Text.RegularExpressions;

namespace Mastersign.XmlDoc.Xsl
{
    public class CRefParsing
    {
        private static readonly Regex T_TypeNameP = new Regex(@"^T\:.*\.(.*?)(?:`\d+)?$");

        private static readonly Regex M_MethodNameP = new Regex(@"^M\:.*\.(.*?)\(.*$");

        private static readonly Regex CRefPattern = new Regex(@"^(?<type>\w)\:(?<def>.*)");

        private static readonly Regex TypePattern = new Regex(@"^(?:(?<ns>.+)\.)?(?<name>[^\.]+?)$");

        private static readonly Regex MethodPattern = new Regex(@"^(?<type>.+?)\.(?<name>.*)\((?<args>.*)\)$");

        private static readonly Regex PropertyPattern = new Regex(@"^(?<type>.+?)\.(?<name>.*)$");

        public string MemberType(string cref)
        {
            int p = cref.IndexOf(':');
            return p < 0 ? string.Empty : cref.Substring(0, p);
        }

        public string TypeName(string cref)
        {
            return T_TypeNameP.Replace(cref, "$1");
        }

        public string MethodName(string cref)
        {
            return M_MethodNameP.Replace(cref, "$1");
        }
    }

    public class CRefFormatting
    {
        private static readonly CRefParsing parsing = new CRefParsing();

        public string FormatLabel(string cref)
        {
            switch (parsing.MemberType(cref))
            {
                case "T": return parsing.TypeName(cref);
                case "M": return parsing.MethodName(cref);
                default: return "UNKNOWN_MEMBER_TYPE";
            }
        }
    }
}