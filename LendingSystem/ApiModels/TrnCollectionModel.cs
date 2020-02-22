using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingSystem.ApiModels
{
    public class TrnCollectionModel
    {
        public Int32 Id { get; set; }
        public String CollectionNumber { get; set; }
        public String CollectionDate { get; set; }
        public String ManualCollectionNumber { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public Int32 LoanId { get; set; }
        public String LoanNumber { get; set; }
        public Int32 PayTypeId { get; set; }
        public String PayType { get; set; }
        public String PaymayaAccountNumber { get; set; }
        public String GCashAccountNumber { get; set; }
        public String CreditCardNumber { get; set; }
        public String CreditCardName { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal PenaltyAmount { get; set; }
        public String Remarks { get; set; }
        public Int32 CollectorId { get; set; }
        public String Collector { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}