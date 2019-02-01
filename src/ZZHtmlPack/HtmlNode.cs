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
        private List<HtmlAttribute> _attributes = new List<HtmlAttribute>();

        private HtmlNode _endNode;
        #endregion

        public HtmlNode(HtmlNodeType type, HtmlDocument ownerDocument, int index)
        {
            NodeType = type;
            OwnerDocument = ownerDocument;

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
        public HtmlNodeType NodeType { get; internal set; }
        public HtmlDocument OwnerDocument { get; internal set; }
        public HtmlNode ParentNode { get; internal set; }
        public HtmlNode PrevNode { get; internal set; }
        public HtmlNode NextNode { get; internal set; }
        public string Name { get; set; }
        public string Id
        {
            get
            {
                HtmlAttribute att = this.GetAttribute("id");
                return att == null ? String.Empty : att.Value;
            }
            set
            {
                HtmlAttribute att = this.GetAttribute("id");
                if (att == null)
                {
                    att = this.PrependAttribute(new HtmlAttribute(this, "id", value));
                    return;
                }
                att.Value = value;
            }
        }
        public string Class
        {
            get
            {
                HtmlAttribute att = this.GetAttribute("class");
                return att == null ? String.Empty : att.Value;
            }
        }
        public bool Closed => (_endNode != null);
        public int Line { get; internal set; }
        public int LinePosition { get; internal set; }
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
        public List<HtmlAttribute> Attributes
        {
            get { return _attributes; }
            internal set => _attributes = value;
        }
        public List<HtmlNode> ChildNodes { get; internal set; }

        /// <summary>
		/// Gets a value indicating whether the current node has any attributes on the closing tag.
		/// </summary>
		public bool HasClosingAttributes
        {
            get
            {
                if ((_endNode == null) || (_endNode == this))
                {
                    return false;
                }
                if (_endNode._attributes == null)
                {
                    return false;
                }
                if (_endNode._attributes.Count <= 0)
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
		/// Gets the collection of HTML attributes for the closing tag. May not be null.
		/// </summary>
		public List<HtmlAttribute> ClosingAttributes
        {
            get
            {
                return !HasClosingAttributes ? new List<HtmlAttribute>() : _endNode.Attributes;
            }
        }
        #endregion

        #region Method
        public HtmlAttribute AppendAttribute(HtmlAttribute newAttribute)
        {
            if (newAttribute == null)
                throw new ArgumentNullException("newAttribute");
            newAttribute.OwnerNode = this;
            Attributes.Add(newAttribute);
            return newAttribute;
        }
        public HtmlAttribute PrependAttribute(HtmlAttribute newAttribute)
        {
            if (newAttribute == null)
                throw new ArgumentNullException("newAttribute");
            newAttribute.OwnerNode = this;
            Attributes.Insert(0, newAttribute);
            return newAttribute;
        }

        /// <summary>
        /// Gets the <see cref="HtmlAttribute"/> instance by attribute name,may be null
        /// </summary>
        public HtmlAttribute GetAttribute(string name)
        {
            HtmlAttribute att = Attributes.Find(a => a.Name.Equals(name));
            return att;
        }

        public HtmlAttribute SetAttributeValue(string name, string value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            HtmlAttribute att = GetAttribute("name");
            if (att == null)
            {
                return AppendAttribute(new HtmlAttribute(this, name, value));
            }
            att.Value = value;
            return att;
        }

        /// <summary>
        /// Add new class name to the remaining class value
        /// </summary>
        public void AddClass(string classValue)
        {
            HtmlAttribute classAttribute = this.GetAttribute("class");
            if (classAttribute == null)
            {
                classAttribute = this.PrependAttribute(new HtmlAttribute(this, "class", classValue));
            }
            SetAttributeValue(classAttribute.Name, classAttribute.Value + " " + classValue);
        }
        /// <summary>
        /// Set the new class value,drop the old value
        /// </summary>
        public void SetClass(string classValue)
        {
            SetAttributeValue("class", classValue);
        }

        public void AddChildNode(HtmlNode newNode)
        {
            HtmlNode preNode = ChildNodes[ChildNodes.Count - 1];
            preNode.NextNode = newNode;
            newNode.PrevNode = preNode;
            ChildNodes.Add(newNode);
        }
        public void InsertChildNode(int index, HtmlNode newNode)
        {
            if (index == 0)
            {
                newNode.NextNode = ChildNodes[0];
                ChildNodes[0].PrevNode = newNode;
            }
            else if (index > 0 || index < ChildNodes.Count)
            {
                newNode.PrevNode = ChildNodes[index - 1];
                newNode.NextNode = ChildNodes[index];
                ChildNodes[index - 1].NextNode = newNode;
                ChildNodes[index].PrevNode = newNode;
            }
            else
            {
                newNode.PrevNode = ChildNodes[index];
                ChildNodes[index].NextNode = newNode;
            }

            ChildNodes.Insert(index, newNode);
        }
        #endregion
    }
}
