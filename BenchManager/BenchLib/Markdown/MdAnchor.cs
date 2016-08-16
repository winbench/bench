using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Markdown
{
    /// <summary>
    /// This class represents an anchor in a Markdown file.
    /// </summary>
    public class MdAnchor
    {
        /// <summary>
        /// The anchor ID, which kan be used in an URL.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The label of the anchor.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="MdAnchor"/>.
        /// </summary>
        /// <param name="id">The ID of the anchor.</param>
        /// <param name="label">The label of the anchor.</param>
        public MdAnchor(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}
