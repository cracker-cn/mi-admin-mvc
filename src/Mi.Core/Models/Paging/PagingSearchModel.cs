using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi.Core.Models.Paging
{
    public class PagingSearchModel
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
