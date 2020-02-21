using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LendingSystem.ApiControllers
{
    public class ApiRepReportsController : ApiController
    {
        public Data.lendingsystemDataContext db = new Data.lendingsystemDataContext();

        [Authorize, HttpGet, Route("api/reports/loanDetailReport/list/{startDate}/{endDate}")]
        public List<ApiModels.TrnLoanModel> LoanDetailReportList(String startDate, String endDate, String filterText)
        {
            String filter = "";
            if (String.IsNullOrEmpty(filterText) == false)
            {
                filter = filterText;
            }

            var loans = from d in db.TrnLoans
                        where d.LoanDate >= Convert.ToDateTime(startDate)
                        && d.LoanDate <= Convert.ToDateTime(endDate)
                         && (d.LoanNumber.Contains(filter) == true ||
                             d.MstCustomer.FullName.Contains(filter) == true ||
                             d.MstTerm.Term.Contains(filter) == true ||
                             d.Remarks.Contains(filter) == true ||
                             d.Status.Contains(filter) == true)
                        select new ApiModels.TrnLoanModel
                        {
                            Id = d.Id,
                            LoanNumber = d.LoanNumber,
                            LoanDate = d.LoanDate.ToShortDateString(),
                            ManualLoanNumber = d.ManualLoanNumber,
                            CustomerId = d.CustomerId,
                            Customer = d.MstCustomer.FullName,
                            TermId = d.TermId,
                            Term = d.MstTerm.Term,
                            TermNumberOfMonths = d.TermNumberOfMonths,
                            PrincipalAmount = d.PrincipalAmount,
                            InterestId = d.InterestId,
                            Interest = d.MstInterest.Interest,
                            InterestPercentage = d.InterestPercentage,
                            InterestAmount = d.InterestAmount,
                            LoanAmount = d.LoanAmount,
                            NetAmount = d.NetAmount,
                            AmortizationAmount = d.AmortizationAmount,
                            PaidAmount = d.PaidAmount,
                            PenaltyAmount = d.PenaltyAmount,
                            BalanceAmount = d.BalanceAmount,
                            Remarks = d.Remarks,
                            Status = d.Status,
                            IsSubmitted = d.IsSubmitted,
                            IsCancelled = d.IsCancelled,
                            IsApproved = d.IsApproved,
                            IsDeclined = d.IsDeclined,
                            IsClosed = d.IsClosed,
                            IsLocked = d.IsLocked,
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedByUser = d.MstUser.FullName,
                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            UpdatedByUserId = d.UpdatedByUserId,
                            UpdatedByUser = d.MstUser1.FullName,
                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };

            return loans.OrderByDescending(d => d.Id).ToList();
        }

        [Authorize, HttpGet, Route("api/reports/collectionDetailReport/list/{startDate}/{endDate}")]
        public List<ApiModels.TrnCollectionModel> CollectionDetailReportList(String startDate, String endDate, String filterText)
        {
            String filter = "";
            if (String.IsNullOrEmpty(filterText) == false)
            {
                filter = filterText;
            }

            var collections = from d in db.TrnCollections
                              where d.CollectionDate >= Convert.ToDateTime(startDate)
                              && d.CollectionDate <= Convert.ToDateTime(endDate)
                              && (d.CollectionNumber.Contains(filter) == true ||
                                  d.MstCustomer.FullName.Contains(filter) == true ||
                                  d.TrnLoan.LoanNumber.Contains(filter) == true ||
                                  d.MstPayType.PayType.Contains(filter) == true ||
                                  d.Remarks.Contains(filter) == true)
                              select new ApiModels.TrnCollectionModel
                              {
                                  Id = d.Id,
                                  CollectionNumber = d.CollectionNumber,
                                  CollectionDate = d.CollectionDate.ToShortDateString(),
                                  ManualCollectionNumber = d.ManualCollectionNumber,
                                  CustomerId = d.CustomerId,
                                  Customer = d.MstCustomer.FullName,
                                  LoanId = d.LoanId,
                                  LoanNumber = d.TrnLoan.LoanNumber,
                                  PayTypeId = d.PayTypeId,
                                  PayType = d.MstPayType.PayType,
                                  PaidAmount = d.PaidAmount,
                                  PenaltyAmount = d.PenaltyAmount,
                                  Remarks = d.Remarks,
                                  IsLocked = d.IsLocked,
                                  CreatedByUserId = d.CreatedByUserId,
                                  CreatedByUser = d.MstUser.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedByUserId = d.UpdatedByUserId,
                                  UpdatedByUser = d.MstUser1.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };

            return collections.OrderByDescending(d => d.Id).ToList();
        }
    }
}
