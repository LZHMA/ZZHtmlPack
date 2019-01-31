using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ZZHtmlPack
{
    [DebuggerDisplay("Name:{OriginalName}")]
    public partial class HtmlNode
    {
        #region Static Members
        private static readonly string HtmlNodeTypeNameComment = "#comment";
        private static readonly string HtmlNodeTypeNameDocument = "#document";
        private static readonly string HtmlNodeTypeNameText = "#text";
        public static Dictionary<string, HtmlElementFlag> ElementsFlags;

        static HtmlNode()
        {
            ElementsFlags = new Dictionary<string, HtmlElementFlag>();

            //tags whose content may be anything
            ElementsFlags.Add("script", HtmlElementFlag.CData);
            ElementsFlags.Add("style", HtmlElementFlag.CData);
            ElementsFlags.Add("noxhtml", HtmlElementFlag.CData);

            //tags that can not contain other tags
            ElementsFlags.Add("base", HtmlElementFlag.Empty);
            ElementsFlags.Add("link", HtmlElementFlag.Empty);
            ElementsFlags.Add("meta", HtmlElementFlag.Empty);
            ElementsFlags.Add("isindex", HtmlElementFlag.Empty);
            ElementsFlags.Add("hr", HtmlElementFlag.Empty);
            ElementsFlags.Add("col", HtmlElementFlag.Empty);
            ElementsFlags.Add("img", HtmlElementFlag.Empty);
            ElementsFlags.Add("param", HtmlElementFlag.Empty);
            ElementsFlags.Add("embed", HtmlElementFlag.Empty);
            ElementsFlags.Add("frame", HtmlElementFlag.Empty);
            ElementsFlags.Add("wbr", HtmlElementFlag.Empty);
            ElementsFlags.Add("bgsound", HtmlElementFlag.Empty);
            ElementsFlags.Add("spacer", HtmlElementFlag.Empty);
            ElementsFlags.Add("keygen", HtmlElementFlag.Empty);
            ElementsFlags.Add("area", HtmlElementFlag.Empty);
            ElementsFlags.Add("input", HtmlElementFlag.Empty);
            ElementsFlags.Add("basefont", HtmlElementFlag.Empty);

            ElementsFlags.Add("form", HtmlElementFlag.CanOverlap);

            // they sometimes contain, and sometimes they don 't...
            ElementsFlags.Add("option", HtmlElementFlag.Empty);

            // tag whose closing tag is equivalent to open tag:
            // <p>bla</p>bla will be transformed into <p>bla</p>bla
            // <p>bla<p>bla will be transformed into <p>bla<p>bla and not <p>bla></p><p>bla</p> or <p>bla<p>bla</p></p>
            //<br> see above
            ElementsFlags.Add("br", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
        }
        #endregion

        #region Field
        private HtmlAttributeCollection _attributes;
        private HtmlDocument _ownerDocument;

        private HtmlNode _parentNode;
        //todo : childnodes
        private HtmlNode _endNode;
        #endregion

        public HtmlNode(HtmlNodeType type, HtmlDocument ownerDocument, int index)
        {
            NodeType = type;
            _ownerDocument = ownerDocument;

            switch (type)
            {
                case HtmlNodeType.Comment:
                    Name = HtmlNodeTypeNameComment;
                    _endNode = this;
                    break;
                case HtmlNodeType.Document:
                    Name = HtmlNodeTypeNameDocument;
                    _endNode = this;
                    break;
                case HtmlNodeType.Text:
                    Name = HtmlNodeTypeNameText;
                    _endNode = this;
                    break;
            }

        }

        #region Property
        public HtmlNodeType NodeType { get; private set; }
        public string Name { get; set; }
        public bool Closed => (_endNode != null);
        public bool HasAttributes
        {
            get
            {
                if (_attributes == null)
                    return false;
                if (_attributes.Count <= 0)
                    return false;
                return true;
            }
        }
        public HtmlAttributeCollection Attributes
        {
            get
            {
                if (!HasAttributes)
                {
                    _attributes = new HtmlAttributeCollection(this);
                }
                return _attributes;
            }
            private set => _attributes = value;
        }
        #endregion

    }
}
