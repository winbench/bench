using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Markdown
{
    public class MdHeadline : MdAnchor
    {
        public int Level { get; private set; }

        public MdHeadline(string id, string label, int level)
            : base(id, label)
        {
            Level = level;
        }
    }
}
