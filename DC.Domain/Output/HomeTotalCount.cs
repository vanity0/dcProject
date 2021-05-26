using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Domain.Output
{
    public class HomeTotalCount
    {
        public long ProductCount { get; set; }
        public long DevCount { get; set; }
        public long PvCount { get; set; }
        public long UvCount { get; set; }
        public long CpaCount { get; set; }
        public long IosCount { get; set; }
        public long AndroidCount { get; set; }
    }
}
