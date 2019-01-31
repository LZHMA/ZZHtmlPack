using System;
using System.Collections;
using System.Collections.Generic;

namespace ZZHtmlPack
{
    public class HtmlNodeCollection : IList<HtmlNode>
    {
        private readonly HtmlNode _parentNode;
        private readonly List<HtmlNode> _items = new List<HtmlNode>();

        public HtmlNodeCollection(HtmlNode parentNode)
        {
            _parentNode = parentNode;
        }

        public HtmlNode this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(HtmlNode item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(HtmlNode item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(HtmlNode[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<HtmlNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(HtmlNode item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, HtmlNode item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(HtmlNode item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}