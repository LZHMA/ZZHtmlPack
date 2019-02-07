namespace ZZHtmlPack
{
    public class HtmlCommentNode : HtmlNode
    {
        private string _comment;

        internal HtmlCommentNode(HtmlDocument ownerDocument, int index) : base(HtmlNodeType.Comment, ownerDocument, index) { }

        public string Comment
        {
            get
            {
                if (_comment == null)
                {
                    return base.InnerHtml;
                }
                return _comment;
            }
            set { _comment = value; }
        }
    }
}