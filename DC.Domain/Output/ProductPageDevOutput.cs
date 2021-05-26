using DC.Domain.Global;
using System.Collections.Generic;

namespace DC.Domain.Output
{

    public class ProdDevOutput
    {
        public PageModel<ProductPageDevOutput> PorductList { get; set; }
        public List<DwzOutput> DwzList { get; set; }
    }

    public class ProductPageDevOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string AliasName { get; set; }
        public string LinkModel { get; set; }
        public double ExtendMoney { get; set; }
        public string Ecode { get; set; }
    }
}
