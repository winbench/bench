using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Markdown
{
    /// <summary>
    /// This class represents a headline in a Markdown file.
    /// </summary>
    public class MdHeadline : MdAnchor
    {
        /// <summary>
        /// The headline level between 1 and 6.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="MdHeadline"/>.
        /// </summary>
        /// <param name="id">The ID of the headline anchor.</param>
        /// <param name="label">The text of the headline.</param>
        /// <param name="level">The level of the headline between 1 and 6.</param>
        public MdHeadline(string id, string label, int level)
            : base(id, label)
        {
            Level = level;
        }
    }
}
