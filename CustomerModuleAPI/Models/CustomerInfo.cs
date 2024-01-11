using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CustomerModuleAPI.Models
{
    public partial class CustomerInfo
    {
        public Int64? Id { get; set; }
        public string? Name { get; set; }
        public string? EmailID { get; set; }
        public string? MobileNo1 { get; set; }
        public string? MobileNo2 { get; set; }
        public string? Address { get; set; }
    }
}
