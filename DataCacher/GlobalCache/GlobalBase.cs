using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DataCacher.GlobalCache
{
   public abstract class GlobalBase
    {
        protected MemoryCache CatchContain { get; set; }
        private static readonly object _Locker = new { };

        public GlobalBase()
        {
            InitialContain();
        }

        /// <summary>
        /// Initial Memory Cache Container
        /// </summary>
        protected internal virtual void InitialContain()
        {
            CatchContain = new System.Runtime.Caching.MemoryCache(this.GetType().Name);
        }

        /// <summary>
        /// Add item to Cache
        /// </summary>
        /// <param name="key">catch key </param>
        /// <param name="value">catch value</param>
        protected internal virtual void AddItem(string key, object value)
        {
            lock (_Locker)
            {
                CatchContain.Add(key, value, DateTimeOffset.MaxValue);
            }
        }

        /// <summary>
        /// Set(insert or update) item to Cache
        /// </summary>
        /// <param name="key">catch key </param>
        /// <param name="value">catch value</param>
        protected internal virtual void SetItem(string key, object value)
        {
            lock (_Locker)
            {
                if (value == null && value.GetType() != typeof(string)) return;
                CatchContain.Set(key, value, DateTimeOffset.MaxValue);
            }
        }

        /// <summary>
        /// Remove item from Cache
        /// </summary>
        /// <param name="key">catch key</param>
        protected internal virtual void RemoveItem(string key)
        {
            lock (_Locker)
            {
                CatchContain.Remove(key);
            }
        }

        /// <summary>
        /// Get value from Cache
        /// </summary>
        /// <param name="key">catch key</param>
        /// <param name="remove">remove value after get it</param>
        /// <returns></returns>
        protected internal virtual object GetItem(string key, bool remove)
        {
            lock (_Locker)
            {
                var res = CatchContain[key];
                if (res != null)
                {
                    if (remove == true)
                        CatchContain.Remove(key);
                }
                //else
                //{
                //    throw new Exception("CachingProvider-GetItem: Don't contains key: " + key);
                //}
                return res;
            }
        }
    }
}
