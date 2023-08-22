using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Options
{
    public class AzureOptions
    {
        public string ResourceGroup { get; set; }
        public string Account { get; set; }
        public string Container { get; set; }
        public string ConnectionString { get; set; }
    }
}
