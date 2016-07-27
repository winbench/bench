using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Mastersign.XmlDoc
{
    // https://msdn.microsoft.com/en-us/library/fsbx0t7x.aspx
    // https://msdn.microsoft.com/en-us/library/5ast78ax.aspx

    public enum CRefKind
    {
        Unknown,
        Invalid,
        Error,
        Namespace,
        Type,
        Field,
        Method,
        Property,
        Event,
    }

    public class CRefParsingResult
    {
        public string Source { get; private set; }

        public CRefKind Kind { get; private set; }

        public CRefParsingResult(string source, CRefKind kind)
        {
            Source = source;
            Kind = kind;
        }
    }

    public class CRefErrorMessage : CRefParsingResult
    {
        public string Message { get; private set; }

        public CRefErrorMessage(string source, string message)
            : base(source, CRefKind.Error)
        {
            Message = message;
        }
    }

    public class CRefNamespace : CRefParsingResult
    {
        public string Namespace { get; private set; }

        protected CRefNamespace(string source, CRefKind kind, string ns)
            : base(source, kind)
        {
            Namespace = ns;
        }

        public CRefNamespace(string source, string ns)
            : this(source, CRefKind.Namespace, ns)
        {
        }
    }

    public class CRefType : CRefNamespace
    {
        public string TypeName { get; private set; }

        public string FullTypeName
        {
            get
            {
                return string.IsNullOrEmpty(Namespace)
                    ? TypeName
                    : Namespace + "." + TypeName;
            }
        }

        protected CRefType(string source, CRefKind kind, string ns, string type)
            : base(source, kind, ns)
        {
            TypeName = type;
        }

        public CRefType(string source, string ns, string type)
            : this(source, CRefKind.Type, ns, type)
        {
        }
    }

    public abstract class CRefMember : CRefType
    {
        public string MemberName { get; private set; }

        protected CRefMember(string source, CRefKind kind, string ns, string type, string name)
            : base(source, kind, ns, type)
        {
            MemberName = name;
        }
    }

    public class CRefField : CRefMember
    {
        public CRefField(string source, string ns, string type, string name)
            : base(source, CRefKind.Field, ns, type, name)
        {
        }
    }

    public class CRefArgumentType
    {
        public string Namespace { get; private set; }

        public string TypeName { get; private set; }

        public string FullTypeName
        {
            get
            {
                return string.IsNullOrEmpty(Namespace)
                    ? TypeName
                    : Namespace + "." + TypeName;
            }
        }

        public string Modifiers { get; private set; }

        public CRefArgumentType(string ns, string type, string mod)
        {
            Namespace = ns;
            TypeName = type;
            Modifiers = mod;
        }
    }

    public class CRefMethod : CRefMember
    {
        public CRefArgumentType[] Arguments { get; private set; }

        public CRefArgumentType ReturnType { get; private set; } // only in use with casting operators

        public CRefMethod(string source, string ns, string type, string name, CRefArgumentType[] arguments, CRefArgumentType returnType)
            : base(source, CRefKind.Method, ns, type, name)
        {
            Arguments = arguments;
            ReturnType = returnType;
        }
    }

    public class CRefProperty : CRefMember
    {
        public CRefArgumentType[] Arguments { get; private set; }

        public CRefProperty(string source, string ns, string type, string name, CRefArgumentType[] arguments)
            : base(source, CRefKind.Property, ns, type, name)
        {
            Arguments = arguments;
        }
    }

    public class CRefEvent : CRefMember
    {
        public CRefEvent(string source, string ns, string type, string name)
            : base(source, CRefKind.Event, ns, type, name)
        {
        }
    }

    public class CRefParsing
    {
        private static readonly Regex CRefPattern
            = new Regex(@"^(?<kind>\w)\:(?<def>.*)$");

        private static readonly Regex NamespacePattern
            = new Regex(@"^(?<ns>[^\.]+(?:\.[^\.]+)*?)$");

        private static readonly Regex TypePattern
            = new Regex(@"^(?:(?<ns>[^\.]+(?:\.[^\.]+)*?)\.)?(?<type>[^\.\(]+?)$");

        private static readonly Regex MethodPattern
            = new Regex(@"^(?:(?<ns>[^\.\(]+(?:\.[^\.]+)*?)\.)?(?<type>[^\.\(]+?)\.(?<name>[^\.\(]+)(?:\((?<args>.+?)\)(?:~(?<ret>.+))?)?$");

        private static readonly Regex FieldPattern
            = new Regex(@"^(?:(?<ns>[^\.]+(?:\.[^\.]+)*?)\.)?(?<type>[^\.\(]+?)\.(?<name>[^\.\(]+)$");

        private static readonly Regex PropertyPattern
            = new Regex(@"^(?:(?<ns>[^\.\(]+(?:\.[^\.]+)*?)\.)?(?<type>[^\.\(]+?)\.(?<name>[^\.\(]+)(?:\((?<args>.+?)\))?$");

        private static readonly Regex EventPattern
            = new Regex(@"^(?:(?<ns>[^\.]+(?:\.[^\.]+)*?)\.)?(?<type>[^\.\(]+?)\.(?<name>[^\.\(]+)$");

        private static readonly Regex ArgumentTypePattern
            = new Regex(@"^(?:(?<ns>[^\.]+(?:\.[^\.]+)*?)\.)?(?<type>[^\.\(]+?)(?<mod>(?:\*|@|\^|\[[\d,\:\?]*\])*)$");

        private static CRefKind ParseKind(string kind)
        {
            switch (kind)
            {
                case "N": return CRefKind.Namespace;
                case "T": return CRefKind.Type;
                case "F": return CRefKind.Field;
                case "P": return CRefKind.Property;
                case "M": return CRefKind.Method;
                case "E": return CRefKind.Event;
                default: return CRefKind.Unknown;
            }
        }

        private static string EmptyToNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        private static CRefArgumentType ParseArgumentType(string type)
        {
            if (type == null) return null;
            var m = ArgumentTypePattern.Match(type);
            if (!m.Success) return null;
            return new CRefArgumentType(
                EmptyToNull(m.Groups["ns"].Value),
                m.Groups["type"].Value,
                EmptyToNull(m.Groups["mod"].Value));
        }

        private static CRefArgumentType[] ParseArgumentList(string args)
        {
            if (string.IsNullOrEmpty(args)) return new CRefArgumentType[0];
            var parts = args.Split(',');
            var result = new List<CRefArgumentType>();
            foreach (var part in parts)
            {
                result.Add(ParseArgumentType(part));
            }
            return result.ToArray();
        }

        public static CRefParsingResult Parse(string cref)
        {
            if (cref == null) throw new ArgumentNullException("cref");
            if (cref.StartsWith("!"))
            {
                var message = cref.Substring(1).TrimStart();
                return new CRefErrorMessage(cref, message);
            }
            var crefM = CRefPattern.Match(cref);
            if (!crefM.Success)
            {
                return new CRefParsingResult(cref, CRefKind.Invalid);
            }
            var kind = ParseKind(crefM.Groups["kind"].Value);
            var def = crefM.Groups["def"].Value;
            Match m = default(Match);
            switch (kind)
            {
                case CRefKind.Namespace:
                    m = NamespacePattern.Match(def);
                    return new CRefNamespace(cref,
                        EmptyToNull(m.Groups["ns"].Value));
                case CRefKind.Type:
                    m = TypePattern.Match(def);
                    return new CRefType(cref,
                        EmptyToNull(m.Groups["ns"].Value),
                        m.Groups["type"].Value);
                case CRefKind.Field:
                    m = FieldPattern.Match(def);
                    return new CRefField(cref,
                        EmptyToNull(m.Groups["ns"].Value),
                        m.Groups["type"].Value,
                        m.Groups["name"].Value);
                case CRefKind.Method:
                    m = MethodPattern.Match(def);
                    return new CRefMethod(cref,
                        EmptyToNull(m.Groups["ns"].Value),
                        m.Groups["type"].Value,
                        m.Groups["name"].Value,
                        ParseArgumentList(m.Groups["args"].Value),
                        ParseArgumentType(m.Groups["ret"].Value));
                case CRefKind.Property:
                    m = PropertyPattern.Match(def);
                    return new CRefProperty(cref,
                        EmptyToNull(m.Groups["ns"].Value),
                        m.Groups["type"].Value,
                        m.Groups["name"].Value,
                        ParseArgumentList(m.Groups["args"].Value));
                case CRefKind.Event:
                    m = EventPattern.Match(def);
                    return new CRefEvent(cref,
                        EmptyToNull(m.Groups["ns"].Value),
                        m.Groups["type"].Value,
                        m.Groups["name"].Value);
                case CRefKind.Invalid:
                    return new CRefParsingResult(cref, kind);
                default:
                    return new CRefParsingResult(cref, CRefKind.Unknown);
            }
        }

        public string ErrorMessage(string cref)
        {
            var error = Parse(cref) as CRefErrorMessage;
            return error != null ? error.Message ?? string.Empty : string.Empty;
        }

        public string MemberKind(string cref)
        {
            var result = Parse(cref);
            return result.Kind.ToString();
        }

        public string Namespace(string cref)
        {
            var ns = Parse(cref) as CRefNamespace;
            return ns != null ? ns.Namespace ?? string.Empty : string.Empty;
        }

        public string TypeName(string cref)
        {
            var type = Parse(cref) as CRefType;
            return type != null ? type.TypeName ?? string.Empty : string.Empty;
        }

        public string MemberName(string cref)
        {
            var member = Parse(cref) as CRefMember;
            return member != null ? member.MemberName ?? string.Empty : string.Empty;
        }

        public CRefArgumentType[] Arguments(string cref)
        {
            var result = Parse(cref);
            var method = result as CRefMethod;
            if (method != null) return method.Arguments;
            var property = result as CRefProperty;
            return property != null ? property.Arguments : null;
        }

        public CRefArgumentType ReturnType(string cref)
        {
            var method = Parse(cref) as CRefMethod;
            return method != null ? method.ReturnType : null;
        }
    }

    public class CRefFormatting
    {
        private static Regex ArgumentListPattern = new Regex(@"\((?<args>.+?)\)");

        private static Regex GenericTypePattern = new Regex(@"`(?<cnt>\d+)");

        private static Regex GenericMethodPattern = new Regex(@"``(?<cnt>\d+)");

        private static Regex TypeParamRefPattern = new Regex(@"^`(?<no>\d+)$");

        private static Regex MethodTypeParamRefPattern = new Regex(@"^``(?<no>\d+)$");

        private const int HASH_LENGTH = 4; // Bytes

        public CRefFormatting()
        {
            FileNameExtension = ".md";
            UrlBase = "";
            UrlFileNameExtension = ".html";
        }

        public string FileNameExtension { get; set; }

        public string UrlBase { get; set; }

        public string UrlFileNameExtension { get; set; }

        private CRefParsingResult CurrentCRef { get; set; }

        private HashAlgorithm md5 = MD5.Create();

        public string CurrentContextCRef
        {
            get { return CurrentCRef.Source; }
            set { CurrentCRef = CRefParsing.Parse(value); }
        }

        public Assembly[] Assemblies { get; set; }

        private XmlDocument[] xmlDocs;

        public XmlDocument[] XmlDocs
        {
            get { return xmlDocs; }
            set
            {
                xmlDocs = value;
                CountRawUrls();
            }
        }

        private readonly Dictionary<string, int> fileUrlCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, int> fullUrlCounts = new Dictionary<string, int>();

        private delegate bool ElementCriteria(XmlElement node);

        private void CountRawUrls()
        {
            fullUrlCounts.Clear();
            if (XmlDocs == null) return;
            foreach (var xmlDoc in XmlDocs)
            {
                var memberEls = xmlDoc.SelectNodes("/doc/members/member");
                foreach (XmlElement mEl in memberEls)
                {
                    var cref = mEl.GetAttribute("name");
                    var parsed = CRefParsing.Parse(cref);
                    if (parsed.Kind == CRefKind.Type)
                    {
                        var normalizedFilePart = NormalizeForUrl(UrlFilePart(parsed));
                        int fileCnt;
                        if (fileUrlCounts.TryGetValue(normalizedFilePart, out fileCnt))
                        {
                            fileUrlCounts[normalizedFilePart] = fileCnt + 1;
                        }
                        else
                        {
                            fileUrlCounts[normalizedFilePart] = 1;
                        }
                    }
                }
                memberEls = xmlDoc.SelectNodes("/doc/members/member");
                foreach (XmlElement mEl in memberEls)
                {
                    var cref = mEl.GetAttribute("name");
                    var parsed = CRefParsing.Parse(cref);
                    if (parsed.Kind != CRefKind.Type)
                    {
                        var normalizedUrl = NormalizeForUrl(
                            CombineFileAndAnchor(UrlFilePartUnique(parsed), UrlAnchorPart(parsed)));
                        int fullCnt;
                        if (fullUrlCounts.TryGetValue(normalizedUrl, out fullCnt))
                        {
                            fullUrlCounts[normalizedUrl] = fullCnt + 1;
                        }
                        else
                        {
                            fullUrlCounts[normalizedUrl] = 1;
                        }
                    }
                }
            }
        }

        private string Hashed(string value)
        {
            var hash = md5.ComputeHash(Encoding.Unicode.GetBytes(value));
            var sb = new StringBuilder();
            for (int i = 0; i < Math.Min(hash.Length, HASH_LENGTH); i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private string NormalizeForUrl(string name)
        {
            if (name == null) return null;
            var result = name;
            result = result.Replace("#", "");
            result = result.Replace('.', '-');
            result = result.Replace('_', '-');
            result = GenericMethodPattern.Replace(result, "");
            result = GenericTypePattern.Replace(result, "");
            result = result.ToLowerInvariant();
            return result;
        }

        private string UrlAnchorPart(CRefParsingResult parsed)
        {
            if (parsed.Kind == CRefKind.Field ||
                parsed.Kind == CRefKind.Event ||
                parsed.Kind == CRefKind.Property ||
                parsed.Kind == CRefKind.Method)
            {
                var member = (CRefMember)parsed;
                return member.MemberName;
            }
            else
            {
                return null;
            }
        }

        private string UrlFilePart(CRefParsingResult parsed)
        {
            if (parsed.Kind == CRefKind.Namespace)
            {
                var ns = (CRefNamespace)parsed;
                return "ns_" + ns.Namespace;
            }
            else if (parsed is CRefType)
            {
                var type = (CRefType)parsed;
                return type.FullTypeName;
            }
            else
            {
                return Hashed(parsed.Source);
            }
        }

        private bool IsManifoldFile(CRefParsingResult cref)
        {
            var normalizedUrlFilePart = NormalizeForUrl(UrlFilePart(cref));
            int cnt;
            return fileUrlCounts.TryGetValue(normalizedUrlFilePart, out cnt)
                ? cnt > 1
                : false;
        }

        private string UrlFilePartUnique(CRefParsingResult parsed)
        {
            var urlFilePart = UrlFilePart(parsed);
            return IsManifoldFile(parsed)
                ? urlFilePart + "_" + Hashed(urlFilePart)
                : urlFilePart;
        }

        private string CombineFileAndAnchor(string filePart, string anchorPart)
        {
            return anchorPart != null
                ? filePart + "#" + anchorPart
                : filePart;
        }

        private bool IsManifoldUrl(CRefParsingResult cref)
        {
            var normalizedUrl = NormalizeForUrl(
                CombineFileAndAnchor(UrlFilePartUnique(cref), UrlAnchorPart(cref)));
            int cnt;
            return fullUrlCounts.TryGetValue(normalizedUrl, out cnt)
                ? cnt > 1
                : false;
        }

        private string UrlAnchorPartUnique(CRefParsingResult parsed)
        {
            var anchorPart = UrlAnchorPart(parsed);
            if (anchorPart == null) return null;
            return IsManifoldUrl(parsed)
                ? anchorPart + "_" + Hashed(parsed.Source)
                : anchorPart;
        }

        public string Anchor(string cref)
        {
            return NormalizeForUrl(UrlAnchorPartUnique(CRefParsing.Parse(cref)));
        }

        public string Url(string cref)
        {
            var parsed = CRefParsing.Parse(cref);
            return UrlBase + CombineFileAndAnchor(
                NormalizeForUrl(UrlFilePartUnique(parsed)) + UrlFileNameExtension,
                NormalizeForUrl(UrlAnchorPartUnique(parsed)));
        }

        public string CRefTypeName(Type t)
        {
            return t.FullName.Replace('+', '.');
        }

        public string CRef(Type t)
        {
            return "T:" + CRefTypeName(t);
        }

        public string FileName(string cref)
        {
            return NormalizeForUrl(UrlFilePartUnique(CRefParsing.Parse(cref)))
                + FileNameExtension;
        }

        public string FileName(Type t)
        {
            return FileName(CRef(t));
        }

        private XmlElement FindFirstElement(string xpath, ElementCriteria criteria)
        {
            if (XmlDocs == null) return null;
            foreach (var xmlDoc in XmlDocs)
            {
                var nodeSet = xmlDoc.SelectNodes(xpath);
                foreach (XmlElement el in nodeSet)
                {
                    if (criteria(el)) return el;
                }
            }
            return null;
        }

        private string[] FindTypeParameterNames(string cref, int n)
        {
            var memberEl = FindFirstElement("/doc/members/member",
                el => el.HasAttribute("name")
                   && el.GetAttribute("name") == cref);
            var result = new string[n];
            var i = 0;
            if (memberEl != null)
            {
                var typeParamEls = memberEl.SelectNodes("typeparam");
                if (typeParamEls != null)
                {
                    foreach (XmlElement el in typeParamEls)
                    {
                        result[i] = el.GetAttribute("name");
                        i++;
                        if (i >= n) break;
                    }
                }
            }
            for (; i < n; i++)
            {
                result[i] = "?"; // "T" + (i + 1);
            }
            return result;
        }

        private string[] GetTypeParameterNames(CRefType cref)
        {
            var typeCRef = "T:" + ((CRefType)cref).FullTypeName;
            var matches = GenericTypePattern.Matches(typeCRef);
            var names = new List<string>();
            foreach (Match m in matches)
            {
                var n = int.Parse(m.Groups["cnt"].Value);
                names.AddRange(FindTypeParameterNames(
                    typeCRef.Substring(0, m.Index + m.Length), n));
            }
            return names.ToArray();
        }

        private string[] GetMethodTypeParameterNames(CRefMember cref)
        {
            var m = GenericMethodPattern.Match(cref.Source);
            if (m.Success)
            {
                var n = int.Parse(m.Groups["cnt"].Value);
                return FindTypeParameterNames(cref.Source, n);
            }
            else
            {
                return new string[0];
            }
        }

        private string ReplaceTypeParameter(string cref)
        {
            var p = cref.IndexOf('(');
            var head = p > 0 ? cref.Substring(0, p) : cref;
            var tail = p > 0 ? cref.Substring(p) : string.Empty;
            head = GenericTypePattern.Replace(head, m =>
            {
                var n = int.Parse(m.Groups["cnt"].Value);
                return "<"
                    + string.Join(", ", FindTypeParameterNames(
                    cref.Substring(0, m.Index + m.Length), n))
                    + ">";
            });
            return head + tail;
        }

        private string ReplaceMethodTypeParameter(string cref)
        {
            var p = cref.IndexOf('(');
            var head = p > 0 ? cref.Substring(0, p) : cref;
            var tail = p > 0 ? cref.Substring(p) : string.Empty;
            head = GenericMethodPattern.Replace(head, m =>
            {
                var n = int.Parse(m.Groups["cnt"].Value);
                return "<"
                    + string.Join(", ", FindTypeParameterNames(cref, n))
                    + ">";
            });
            return head + tail;
        }

        private string FormatGenerics(string cref)
        {
            var parsed = CRefParsing.Parse(cref);
            var result = cref;
            if (parsed.Kind == CRefKind.Method || parsed.Kind == CRefKind.Property)
            {
                result = ReplaceMethodTypeParameter(cref);
            }
            result = ReplaceTypeParameter(result);
            var colon = result.IndexOf(':');
            if (colon > 0) result = result.Substring(colon + 1);
            return result;
        }

        private string ShortName(string name)
        {
            switch (name)
            {
                case "System.String":
                    return "string";
                case "System.Boolean":
                    return "bool";
                case "System.SByte":
                    return "sbyte";
                case "System.Byte":
                    return "byte";
                case "System.Int16":
                    return "short";
                case "System.UInt16":
                    return "ushort";
                case "System.Int32":
                    return "int";
                case "System.UInt32":
                    return "uint";
                case "System.Int64":
                    return "long";
                case "System.UInt64":
                    return "ulong";
                case "System.Single":
                    return "float";
                case "System.Double":
                    return "double";
            }
            return name.Contains(".")
                ? name.Substring(name.LastIndexOf('.') + 1)
                : name;
        }

        private string FormatArgument(CRefMember member, CRefArgumentType arg, bool nameOnly)
        {
            if (arg == null) return "?";
            var methodTypeParamMatch = MethodTypeParamRefPattern.Match(arg.FullTypeName);
            if (methodTypeParamMatch.Success)
            {
                var methodTypeParams = GetMethodTypeParameterNames(member);
                var no = int.Parse(methodTypeParamMatch.Groups["no"].Value);
                return methodTypeParams != null && methodTypeParams.Length > no
                    ? methodTypeParams[no]
                    : "?";
            }
            var typeParamMatch = TypeParamRefPattern.Match(arg.FullTypeName);
            if (typeParamMatch.Success)
            {
                var typeParams = GetTypeParameterNames(member);
                var no = int.Parse(typeParamMatch.Groups["no"].Value);
                return typeParams != null && typeParams.Length > no
                    ? typeParams[no]
                    : "?";
            }
            var result = arg.FullTypeName;
            if (nameOnly) result = ShortName(result);
            return result;
        }

        private string FormatArguments(CRefMember member, string crefTemplate, bool nameOnly = false)
        {
            var result = crefTemplate;
            CRefArgumentType[] arguments = null;
            if (member.Kind == CRefKind.Method) arguments = ((CRefMethod)member).Arguments;
            if (member.Kind == CRefKind.Property) arguments = ((CRefProperty)member).Arguments;
            var argList = string.Empty;
            if (arguments != null)
            {
                var list = new string[arguments.Length];
                for (int i = 0; i < arguments.Length; i++)
                {
                    list[i] = FormatArgument(member, arguments[i], nameOnly);
                }
                argList = string.Join(", ", list);
            }
            var p = result.IndexOf('(');
            result = p > 0 ? result.Substring(0, p) : result;
            if (member.Kind == CRefKind.Method)
            {
                result += "(" + argList + ")";
            }
            else if (member.Kind == CRefKind.Property && argList.Length > 0)
            {
                result += "[" + argList + "]";
            }
            return result;
        }

        public string EscapeMarkdown(string text)
        {
            return text
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }

        public string Label(string cref)
        {
            var result = CRefParsing.Parse(cref);
            switch (result.Kind)
            {
                case CRefKind.Namespace: return ((CRefNamespace)result).Namespace;
                case CRefKind.Type: return ShortName(FormatGenerics(cref));
                case CRefKind.Field: return ((CRefMember)result).MemberName;
                case CRefKind.Method:
                    var m = (CRefMethod)result;
                    if (m.MemberName == "#ctor")
                    {
                        return ShortName(FormatArguments(m, FormatGenerics(cref).Replace(".#ctor", ""), true));
                    }
                    else if (m.MemberName == "#cctor")
                    {
                        return ShortName(FormatArguments(m, FormatGenerics(cref).Replace(".#cctor", ""), true))
                            + " (static)";
                    }
                    else
                    {
                        return ShortName(FormatArguments(m, FormatGenerics(cref), true));
                    }
                case CRefKind.Property:
                    var p = (CRefProperty)result;
                    return ShortName(FormatArguments(p, FormatGenerics(cref), true));
                case CRefKind.Event: return ((CRefMember)result).MemberName;
                default: return "UNKNOWN_KIND_OF_MEMBER";
            }
        }

        public string FullLabel(string cref)
        {
            var result = CRefParsing.Parse(cref);
            switch (result.Kind)
            {
                case CRefKind.Namespace: return ((CRefNamespace)result).Namespace;
                case CRefKind.Type: return FormatGenerics(cref);
                case CRefKind.Field: return FormatGenerics(cref);
                case CRefKind.Method:
                    var m = (CRefMethod)result;
                    if (m.MemberName == "#ctor")
                    {
                        return FormatArguments(m, FormatGenerics(cref).Replace(".#ctor", ""), false);
                    }
                    else if (m.MemberName == "#cctor")
                    {
                        return FormatArguments(m, FormatGenerics(cref).Replace(".#cctor", ""), false)
                            + " (static)";
                    }
                    else
                    {
                        return FormatArguments(m, FormatGenerics(cref), false);
                    }
                case CRefKind.Property:
                    var p = (CRefProperty)result;
                    return FormatArguments(p, FormatGenerics(cref), false);
                case CRefKind.Event: return FormatGenerics(cref);
                default: return "UNKNOWN_KIND_OF_MEMBER";
            }
        }

        public string RemoveIndentation(string text)
        {
            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var minIndentation = text.Length;
            foreach (var l in lines)
            {
                if (l.Trim().Length == 0) continue;
                var trimmedLine = l.TrimStart();
                var indentation = l.Length - trimmedLine.Length;
                if (minIndentation > indentation) minIndentation = indentation;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().Length == 0) lines[i] = string.Empty;
                else lines[i] = lines[i].Substring(minIndentation);
            }
            return string.Join(Environment.NewLine, lines).TrimEnd();
        }
    }
}
