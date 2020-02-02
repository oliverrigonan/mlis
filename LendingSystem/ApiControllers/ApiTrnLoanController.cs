﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LendingSystem.ApiControllers
{
    public class ApiTrnLoanController : ApiController
    {
        public Data.lendingsystemDataContext db = new Data.lendingsystemDataContext();

        public String LeadingZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [Authorize, HttpGet, Route("api/loan/list/{startDate}/{endDate}")]
        public List<ApiModels.TrnLoanModel> LoanList()
        {
            var loans = from d in db.TrnLoans
                        select new ApiModels.TrnLoanModel
                        {
                            Id = d.Id,
                            LoanNumber = d.LoanNumber,
                            LoanDate = d.LoanDate.ToShortDateString(),
                            ManualLoanNumber = d.ManualLoanNumber,
                            CustomerId = d.CustomerId,
                            Customer = d.MstCustomer.FullName,
                            TermId = d.TermId,
                            Term = d.LoanNumber,
                            TermNumberOfDays = d.TermNumberOfDays,
                            LoanAmount = d.LoanAmount,
                            PreviousBalanceAmount = d.PreviousBalanceAmount,
                            InterestId = d.InterestId,
                            Interest = d.MstInterest.Interest,
                            InterestPercentage = d.InterestPercentage,
                            InterestAmount = d.InterestAmount,
                            PaidAmount = d.PaidAmount,
                            PenaltyAmount = d.PenaltyAmount,
                            BalanceAmount = d.BalanceAmount,
                            DailyAmortizationAmount = d.DailyAmortizationAmount,
                            Remarks = d.Remarks,
                            PreparedByUserId = d.PreparedByUserId,
                            CheckedByUserId = d.CheckedByUserId,
                            ApprovedByUserId = d.ApprovedByUserId,
                            Status = d.Status,
                            IsLocked = d.IsLocked,
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedByUser = d.MstUser3.FullName,
                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            UpdatedByUserId = d.UpdatedByUserId,
                            UpdatedByUser = d.MstUser4.FullName,
                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                        };

            return loans.ToList();
        }

        [Authorize, HttpPost, Route("api/loan/save")]
        public HttpResponseMessage SaveLoan(ApiModels.TrnLoanModel objLoanModel)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var loanNumber = "0000000001";
                var lastLoan = from d in db.TrnLoans.OrderByDescending(d => d.Id) select d;
                if (lastLoan.Any())
                {
                    var lastLoanNumber = Convert.ToInt32(lastLoan.FirstOrDefault().LoanNumber) + 0000000001;
                    loanNumber = LeadingZeroes(lastLoanNumber, 10);
                }

                Data.TrnLoan newLoan = new Data.TrnLoan()
                {
                    Id = objLoanModel.Id,
                    LoanNumber = loanNumber,
                    LoanDate = Convert.ToDateTime(objLoanModel.LoanDate),
                    ManualLoanNumber = objLoanModel.ManualLoanNumber,
                    CustomerId = objLoanModel.CustomerId,
                    TermId = objLoanModel.TermId,
                    TermNumberOfDays = objLoanModel.TermNumberOfDays,
                    LoanAmount = objLoanModel.LoanAmount,
                    PreviousBalanceAmount = objLoanModel.PreviousBalanceAmount,
                    InterestId = objLoanModel.InterestId,
                    InterestPercentage = objLoanModel.InterestPercentage,
                    InterestAmount = objLoanModel.InterestAmount,
                    PaidAmount = objLoanModel.PaidAmount,
                    PenaltyAmount = objLoanModel.PenaltyAmount,
                    BalanceAmount = objLoanModel.BalanceAmount,
                    DailyAmortizationAmount = objLoanModel.DailyAmortizationAmount,
                    Remarks = objLoanModel.Remarks,
                    PreparedByUserId = currentUser.FirstOrDefault().Id,
                    CheckedByUserId = objLoanModel.CheckedByUserId,
                    ApprovedByUserId = objLoanModel.ApprovedByUserId,
                    Status = objLoanModel.Status,
                    IsLocked = objLoanModel.IsLocked,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.TrnLoans.InsertOnSubmit(newLoan);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPut, Route("api/loan/lock/{id}")]
        public HttpResponseMessage LockLoan(ApiModels.TrnLoanModel objLoanModel, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var loan = from d in db.TrnLoans
                           where d.Id == Convert.ToInt32(id)
                           select d;

                if (loan.Any())
                {
                    var lockLoan = loan.FirstOrDefault();
                    lockLoan.LoanDate = Convert.ToDateTime(objLoanModel.LoanDate);
                    lockLoan.ManualLoanNumber = objLoanModel.ManualLoanNumber;
                    lockLoan.CustomerId = objLoanModel.CustomerId;
                    lockLoan.TermId = objLoanModel.TermId;
                    lockLoan.TermNumberOfDays = objLoanModel.TermNumberOfDays;
                    lockLoan.LoanAmount = objLoanModel.LoanAmount;
                    lockLoan.PreviousBalanceAmount = objLoanModel.PreviousBalanceAmount;
                    lockLoan.InterestId = objLoanModel.InterestId;
                    lockLoan.InterestPercentage = objLoanModel.InterestPercentage;
                    lockLoan.InterestAmount = objLoanModel.InterestAmount;
                    lockLoan.PaidAmount = objLoanModel.PaidAmount;
                    lockLoan.PenaltyAmount = objLoanModel.PenaltyAmount;
                    lockLoan.BalanceAmount = objLoanModel.BalanceAmount;
                    lockLoan.DailyAmortizationAmount = objLoanModel.DailyAmortizationAmount;
                    lockLoan.Remarks = objLoanModel.Remarks;
                    lockLoan.IsLocked = true;
                    lockLoan.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    lockLoan.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPut, Route("api/loan/unlock/{id}")]
        public HttpResponseMessage UnlockLoan(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var loan = from d in db.TrnLoans
                           where d.Id == Convert.ToInt32(id)
                           select d;

                if (loan.Any())
                {
                    var unlockLoan = loan.FirstOrDefault();
                    unlockLoan.IsLocked = false;
                    unlockLoan.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    unlockLoan.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpDelete, Route("api/loan/delete/{id}")]
        public HttpResponseMessage DeleteLoan(String id)
        {
            try
            {
                var loan = from d in db.TrnLoans
                           where d.Id == Convert.ToInt32(id)
                           select d;

                if (loan.Any())
                {
                    db.TrnLoans.DeleteOnSubmit(loan.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}