using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class BenchUserInfo : IConfigurationPart
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public BenchUserInfo() { }

        public BenchUserInfo(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void Transfer(IDictionary<string, string> dict)
        {
            dict[PropertyKeys.UserName] = Name;
            dict[PropertyKeys.UserEmail] = Email;
        }

        public override string ToString()
        {
            return string.Format("Name='{0}', Email='{1}'", Name, Email);
        }
    }
}
