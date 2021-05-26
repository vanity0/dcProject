using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Domain.Input
{
    public class SetProductInput
    {
        public string Id { get; set; }
        public string AuditStatus { get; set; }
        public double ExpandMoney { get; set; }
    }
}
