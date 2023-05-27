using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi.Core.Models.Paging
{
    public class PagingModel<T> where T : class, new()
    {
        public IList<T>? Rows { get; set; }

        public int Total { get; set; }

        public PagingModel()
        { }

        public PagingModel(int total, IList<T>? rows)
        {
            Total = total;
            Rows = rows;
        }
    }
}
