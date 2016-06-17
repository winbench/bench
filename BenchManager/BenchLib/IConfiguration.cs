using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IConfiguration : IPropertyCollection, IGroupedPropertyCollection
    {
        void SetValue(string name, string value);

        void SetValue(string name, string[] value);

        void SetValue(string name, bool value);

        void SetValue(string name, int value);

        void SetGroupValue(string group, string name, string value);

        void SetGroupValue(string group, string name, string[] value);

        void SetGroupValue(string group, string name, bool value);

        void SetGroupValue(string group, string name, int value);

        string GetStringValue(string name);

        string GetStringValue(string name, string def);

        string GetStringGroupValue(string group, string name);

        string GetStringGroupValue(string group, string name, string def);

        string[] GetStringListValue(string name);

        string[] GetStringListValue(string name, string[] def);

        string[] GetStringListGroupValue(string group, string name);

        string[] GetStringListGroupValue(string group, string name, string[] def);

        bool GetBooleanValue(string name);

        bool GetBooleanValue(string name, bool def);

        bool GetBooleanGroupValue(string group, string name);

        bool GetBooleanGroupValue(string group, string name, bool def);

        int GetInt32Value(string name);

        int GetInt32Value(string name, int def);

        int GetInt32GroupValue(string group, string name);

        int GetInt32GroupValue(string group, string name, int def);
    }
}
