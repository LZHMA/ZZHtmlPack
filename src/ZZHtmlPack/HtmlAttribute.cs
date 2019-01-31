using System;

namespace ZZHtmlPack
{
    /// <summary>
    /// Represents an HTML Attribute
    /// </summary>
    public class HtmlAttribute : IComparable
    {
        private string _name;
        private string _value;
        internal HtmlAttribute(HtmlDocument ownerdocument)
        {
            OwnerDocument = ownerdocument;
        }

        #region  Properties
        /// <summary>
        /// The qualified name of the attribute
        /// </summary>
        public string Name
        {
            get
            {
                return _name.ToLower();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _name = value;
            }
        }
        
        public string Value
        {
            get{
                return _value;
            }
            set{
                _value=value;
            }
        }

        public string OriginalName
        {
            get => _name;
        }
        /// <summary>
        /// Line number of this attribute in the document
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// The column number of this attribute in the document
        /// </summary>
        public int LinePostition { get; }

        /// <summary>
        /// The HTML document to which this attribute belongs to
        /// </summary>
        public HtmlDocument OwnerDocument { get; }// attribute can exists without a node

        /// <summary>
        /// The HTML node to which this attribute belongs to
        /// </summary>
        public HtmlNode OwnerNode { get; set; }

        /// <summary>
        /// Specifies which type of quote the data should be wrapped in
        /// </summary>
        public AttributeValueQuote QuoteType { get; set; }

        /// <summary>
        /// Gets the stream position of this attribute in this document,relative to the start of the document
        /// </summary>
        public int SteamPosition { get; }
        #endregion

        public int CompareTo(object obj)
        {
            HtmlAttribute att=obj as HtmlAttribute;
            if(att==null)
                throw new ArgumentException("obj");
            return Name.CompareTo(att.Name);
        }
    }

    /// <summary>
    /// An Enum representing different types of Quotes used for surrounding attribute values
    /// </summary>
    public enum AttributeValueQuote
    {
        /// <summary>
        /// A single quote mark '
        /// </summary>
        SingleQuote,
        /// <summary>
        /// A double quote mark "
        /// </summary>
        DoubleQuote
    }
}