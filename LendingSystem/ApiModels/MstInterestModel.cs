using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingSystem.ApiModels
{
    public class MstInterestModel
    {
        public Int32 Id { get; set; }
        public String Interest { get; set; }
        public Decimal Percentage { get; set; }
    }
}