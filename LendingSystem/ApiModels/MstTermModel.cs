using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingSystem.ApiModels
{
    public class MstTermModel
    {
        public Int32 Id { get; set; }
        public String Term { get; set; }
        public Decimal NumberOfDays { get; set; }
        public Int32 DefaultInterestId { get; set; }
    }
}