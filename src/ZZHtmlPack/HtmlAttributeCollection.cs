using System;
using System.Collections;
using System.Collections.Generic;

namespace ZZHtmlPack
{
    public class HtmlAttributeCollection : IList<HtmlAttribute>
    {
        private HtmlNode _ownerNode;
        private Dictionary<string, HtmlAttribute> hashItems = new Dictionary<string, HtmlAttribute>();
        private List<HtmlAttribute> items = new List<HtmlAttribute>();

        public HtmlAttributeCollection(HtmlNode ownernode)
        {
            _ownerNode = ownernode;
        }
        #region Prperties
        /// <summary>
        /// Gets the node to which these attributes belongs to 
        /// </summary>
        public HtmlNode OwnerNode { get; }

        /// <summary>
        /// Gets a given attribute from the list using its name
        /// </summary>
        public HtmlAttribute this[string name]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException("name");
                HtmlAttribute value;
                return hashItems.TryGetValue(name.ToLower(), out value) ? value : null;
            }
            set
            {

            }
        }
        #endregion

        public int Count => items.Count;
        public bool IsReadOnly => false;

        /// <summary>
        /// The attribute at the specified index
        /// </summary>
        public HtmlAttribute this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        /// <summary>
        /// Adds supplied item to the collection
        /// </summary>
        /// <param name="item">the attribute item to be added</param>
        public void Add(HtmlAttribute item)
        {
            Append(item);
        }

        public void Clear()
        {
            hashItems.Clear();
            items.Clear();
        }

        public bool Contains(HtmlAttribute item)
        {
            return items.Contains(item);
        }

        public void CopyTo(HtmlAttribute[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(HtmlAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");
            int index = items.IndexOf(attribute);
            if (index == -1)
                return false;
            RemoveAt(index);
            return true;
        }

        public int IndexOf(HtmlAttribute item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, HtmlAttribute item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            hashItems[item.Name] = item;
            item.OwnerNode = OwnerNode;
            items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            HtmlAttribute attribute = items[index];
            hashItems.Remove(attribute.Name);
            items.RemoveAt(index);
        }

        IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #region Public Method
        /// <summary>
        /// Inserts the specified attribute as the last attribute in the collection
        /// </summary>
        /// <param name="newAttribute">The attribute to insert</param>
        /// <returns>the appended attribute</returns>
        public HtmlAttribute Append(HtmlAttribute newAttribute)
        {
            if (newAttribute == null)
                throw new ArgumentNullException("newAttribute");
            hashItems[newAttribute.Name] = newAttribute;
            newAttribute.OwnerNode = OwnerNode;
            items.Add(newAttribute);

            return newAttribute;
        }

        /// <summary>
        /// Checks for existence of attribute by given name
        /// </summary>
        public bool Contains(string name)
        {
            foreach (var item in items)
            {
                if (item.Name.Equals(name.ToLower()))
                    return true;
            }
            return false;
        }

        public HtmlAttribute Prepend(HtmlAttribute newAttribute)
        {
            Insert(0, newAttribute);
            return newAttribute;
        }
        #endregion
    }
}