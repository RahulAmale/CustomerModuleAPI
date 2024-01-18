using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace CustomerModuleAPI.Models
{
    public partial class CustomerInfo
    {
        public Int64 ID { get; set; }
        public string? FirstName { get; set; }
        [Required]
        public string  MobileNo { get; set; }
        [EmailAddress]
        public string? EmailID { get; set; }
    }

    public class Jwt
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}
