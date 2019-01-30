using System;

namespace ZZHtmlPack
{
    /// <summary>
    /// Represent a complete HTML document
    /// </summary>
    public partial class HtmlDocument
    {
        #region field
        /// <summary>
        /// Defines the max level we would go deep into the html document
        /// </summary>
        private static int _maxDepthLevel = int.MaxValue;
        #endregion
    }
}