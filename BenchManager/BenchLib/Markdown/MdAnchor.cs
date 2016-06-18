using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Markdown
{
    public class MdAnchor
    {
        public string Id { get; private set; }

        public string Label { get; private set; }

        public MdAnchor(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}
