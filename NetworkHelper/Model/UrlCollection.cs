using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetworkHelper.Model
{
    [XmlRoot("Uris")]
    public class UrlCollection
    {
        [XmlArray("UriInfos")]
        [XmlArrayItem("UriInfo")]
        public List<UriInfo> AllUris { get; private set; }

        /// <summary>
        /// Current Index of Uris 
        /// </summary>
        private int _CurrentIndex { get; set; }

        #region Constructor
        public UrlCollection()
        {
            AllUris = new List<UriInfo>();
        }

        public UrlCollection(IEnumerable<UriInfo> urls)
        {
            AllUris = urls.ToList();
        }
        #endregion

        /// <summary>
        /// Export Data to File
        /// </summary>
        /// <param name="path">File path</param>
        public void Export(string path)
        {
            XmlSerializer ser = new XmlSerializer(this.GetType());
            FileStream file = File.Create(path);
            ser.Serialize(file, this);
            file.Close();
        }

        /// <summary>
        /// Import File Content to Memory
        /// </summary>
        /// <param name="path">File path</param>
        public void Import(string path)
        {
            if (!File.Exists(path)) return;
            XmlSerializer ser = new XmlSerializer(this.GetType());
            FileStream file = File.OpenRead(path);
            this.AllUris = (ser.Deserialize(file) as UrlCollection).AllUris.Where(i=>!string.IsNullOrEmpty( i.ServerIp)).ToList();
            file.Close();
        }

        /// <summary>
        /// Current Url
        /// </summary>
        public UriInfo CurrentUrl => AllUris.Count() > 0 ? AllUris[_CurrentIndex] : null;

        /// <summary>
        /// Check Current UrlInfo is the last one of Conllection 
        /// </summary>
        /// <returns></returns>
        public bool IsLast()
        {
            return AllUris.Count() == 0 || CurrentUrl == AllUris.LastOrDefault();
        }

        /// <summary>
        /// Reset the Collection content
        /// </summary>
        /// <param name="urls"></param>
        public void Reset(IEnumerable<UriInfo> urls)
        {
            if (urls.Count() == 0) return;
            _CurrentIndex = 0;
            AllUris = urls.ToList();
        }

        /// <summary>
        /// Reset the Collection content
        /// </summary>
        public void Reset(string path)
        {
            _CurrentIndex = 0;
            Import(path);
        }

        /// <summary>
        /// Add UriInfo
        /// </summary>
        /// <param name="uriInfo"></param>
        public void Add(UriInfo uriInfo)
        {
            AllUris.Add(uriInfo);
        }

        /// <summary>
        /// Move to next one
        /// </summary>
        public void MoveNext()
        {
            if (_CurrentIndex < AllUris.Count() - 1) _CurrentIndex++;
            else _CurrentIndex = 0;
        }

        /// <summary>
        /// Move to last one
        /// </summary>
        public void MoveLast()
        {
            if (_CurrentIndex > 0) _CurrentIndex--;
            else _CurrentIndex = AllUris.Count() - 1;
        }

        public void SetCurrent(UriInfo uriInfo)
        {
            int i = -1;
            _CurrentIndex = (i = AllUris.IndexOf(uriInfo)) != -1 ? i : _CurrentIndex;
        }
    }
}
