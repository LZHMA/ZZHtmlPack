using System;
using System.Net;
using System.Text;

namespace ZZHtmlPack
{
    /// <summary>
    /// Used for downloading and parsing html from the internet
    /// </summary>
    public class HtmlWeb
    {
        #region Instance Methods
        public void LoadAsync(string url)
        {

        }

        public void LoadAsync(Uri uri, Encoding encoding, NetworkCredential credentials)
        {
            var client = new WebClient();
            if (credentials == null)
                client.UseDefaultCredentials = true;
            else
                client.Credentials = credentials;
            if (encoding != null)
                client.Encoding = encoding;
            client.DownloadStringCompleted += ClientDownloadStringCompleted;
            client.DownloadStringAsync(uri);
        }
        #endregion

        #region Event handling
        private void ClientDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs eventArgs)
        {
            try
            {
                var doc=new htmlDocument()
            }
        }
        #endregion
    }
}
