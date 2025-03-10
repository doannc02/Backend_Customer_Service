using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IChiba.Customer.Application.Common.BaseRequest;

    public class QueryPage
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
        public string Sort { get; set; }
    }

