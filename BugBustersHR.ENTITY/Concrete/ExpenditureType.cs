﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class ExpenditureType
    {
        public int Id { get; set; }
        public string ExpenditureName { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }

    }
}
