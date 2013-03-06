using System.Collections.Generic;

namespace SuiteValue.UI.WP8.Model
{
    /// <summary>
    /// Helps with using LongListSelector with Grouping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Group<T> : List<T>
    {
        public Group(string title, IEnumerable<T> items)
            : base(items)
        {
            this.Title = title;
        }
        public string Title { get; set; }
    }
}