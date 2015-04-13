using System.Collections.Generic;
using LjubljanaBus;

namespace LjubljanaBus
{
    public class StationsInGroup<T> : List<Station>
    {
        public StationsInGroup(string category, IEnumerable<T> list)
        {
            Key = category;
            this.Items = list;
        }

        public StationsInGroup(string category)
        {
            Key = category;
        }

        public string Key { get; set; }

        public IEnumerable<T> Items { get; set; }

        public bool HasItems { get { return Count > 0; } }
    }
}
