using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.Response
{
    public class GenericListResponse<T> : BasicResponse
    {
        public ICollection<T> Data { get; set; }
        public IDictionary<string, object> Meta { get; set; }
    }
}
