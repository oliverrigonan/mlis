using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingSystem.ApiModels
{
    public class TrnLoanChecklistModel
    {
        public Int32 Id { get; set; }
        public Int32 LoanId { get; set; }
        public Int32 CheckListId { get; set; }
        public String CheckList { get; set; }
        public String Remarks { get; set; }
        public byte[] ImageAttachment { get; set; }
    }
}