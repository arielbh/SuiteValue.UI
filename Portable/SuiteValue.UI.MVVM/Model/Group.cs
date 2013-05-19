using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuiteValue.UI.MVVM.Model
{
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
