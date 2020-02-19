using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingSystem.ApiModels
{
    public class TrnLoanModel
    {
        public Int32 Id { get; set; }
        public String LoanNumber { get; set; }
        public String LoanDate { get; set; }
        public String ManualLoanNumber { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public Int32 TermId { get; set; }
        public String Term { get; set; }
        public Decimal TermNumberOfMonths { get; set; }
        public Decimal PrincipalAmount { get; set; }
        public Int32 InterestId { get; set; }
        public String Interest { get; set; }
        public Decimal InterestPercentage { get; set; }
        public Decimal InterestAmount { get; set; }
        public Decimal LoanAmount { get; set; }
        public Decimal NetAmount { get; set; }
        public Decimal AmortizationAmount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal PenaltyAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public String Remarks { get; set; }
        public String Status { get; set; }
        public Boolean IsSubmitted { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsApproved { get; set; }
        public Boolean IsDeclined { get; set; }
        public Boolean IsClosed { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}