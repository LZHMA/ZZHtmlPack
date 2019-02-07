using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace ZZHtmlPack
{
    /// <summary>
    /// Represent a complete HTML document
    /// </summary>
    public partial class HtmlDocument
    {
        #region Static Members
        /// <summary>
        /// Defines the max level we would go deep into the html document
        /// </summary>
        public static int MaxDepthLevel { get; set; } = Int32.MaxValue;
        #endregion

        #region field
        private Crc32 _crc32;
        /// <summary>
        /// Collect unclosed nodes
        /// </summary>
        internal Dictionary<int, HtmlNode> OpenedNodes;

        /// <summary>
        /// Defines if output must conform to XML, instead of HTML. Default is false.
        /// </summary>
        public bool OptionOutputAsXml;
        #endregion

        /// <summary>
        /// Gets the root node of the document.
        /// </summary>
        public HtmlNode DocumentNode { get; }
        public Encoding DeclaredEncoding { get; }



        internal HtmlNode CreateNode(HtmlNodeType type, int index)
        {
            switch (type)
            {
                case HtmlNodeType.Document:
                    return new HtmlCommentNode(this)
                default:
            }
        }

        internal Encoding GetOutEncoding()
        {
            // when unspecified, use the stream encoding first
            return _declaredEncoding ?? (_streamencoding ?? OptionDefaultStreamEncoding);
        }
    }
}