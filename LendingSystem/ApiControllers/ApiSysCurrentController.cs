using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LendingSystem.ApiControllers
{
    public class ApiSysCurrentController : ApiController
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

        [Authorize, HttpGet, Route("api/current/detail")]
        public ApiModels.MstCustomerModel GetCurrentUserCustomerDetail()
        {
            var currentUser = from d in db.MstUsers
                              where d.AspNetUserId == User.Identity.GetUserId()
                              select d;

            var customer = from d in db.MstCustomers
                           where d.UserId == currentUser.FirstOrDefault().Id
                           select new ApiModels.MstCustomerModel
                           {
                               Id = d.Id,
                               FullName = d.FullName,
                               BirthDate = d.BirthDate.ToShortDateString(),
                               Gender = d.Gender,
                               Address = d.Address,
                               ContactNumber = d.ContactNumber,
                               Photo = d.Photo,
                               UserId = d.UserId,
                               IsLocked = d.IsLocked,
                               CreatedByUserId = d.CreatedByUserId,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedByUserId = d.UpdatedByUserId,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return customer.FirstOrDefault();
        }

        [Authorize, HttpGet, Route("api/current/loan/history/list")]
        public List<ApiModels.TrnLoanModel> LoanHistoryList()
        {
            var currentUser = from d in db.MstUsers
                              where d.AspNetUserId == User.Identity.GetUserId()
                              select d;

            var customer = from d in db.MstCustomers
                           where d.UserId == currentUser.FirstOrDefault().Id
                           select d;

            if (customer.Any())
            {
                var loans = from d in db.TrnLoans
                            where d.CustomerId == customer.FirstOrDefault().Id
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
            else
            {
                return new List<ApiModels.TrnLoanModel>();
            }
        }

        [Authorize, HttpPost, Route("api/current/loan/apply")]
        public HttpResponseMessage ApplyLoan(ApiModels.TrnLoanModel objLoanModel)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var customer = from d in db.MstCustomers
                               where d.UserId == currentUser.FirstOrDefault().Id
                               select d;

                if (customer.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
                }

                var loanNumber = "0000000001";
                var lastLoan = from d in db.TrnLoans.OrderByDescending(d => d.Id) select d;
                if (lastLoan.Any())
                {
                    var lastLoanNumber = Convert.ToInt32(lastLoan.FirstOrDefault().LoanNumber) + 0000000001;
                    loanNumber = LeadingZeroes(lastLoanNumber, 10);
                }

                Data.TrnLoan newLoan = new Data.TrnLoan()
                {
                    LoanNumber = loanNumber,
                    LoanDate = DateTime.Today,
                    ManualLoanNumber = loanNumber,
                    CustomerId = customer.FirstOrDefault().Id,
                    TermId = objLoanModel.TermId,
                    TermNumberOfDays = objLoanModel.TermNumberOfDays,
                    LoanAmount = objLoanModel.LoanAmount,
                    PreviousBalanceAmount = objLoanModel.PreviousBalanceAmount,
                    InterestId = objLoanModel.InterestId,
                    InterestPercentage = objLoanModel.InterestPercentage,
                    InterestAmount = objLoanModel.InterestAmount,
                    PaidAmount = 0,
                    PenaltyAmount = 0,
                    BalanceAmount = objLoanModel.BalanceAmount,
                    DailyAmortizationAmount = objLoanModel.DailyAmortizationAmount,
                    Remarks = objLoanModel.Remarks,
                    PreparedByUserId = currentUser.FirstOrDefault().Id,
                    CheckedByUserId = objLoanModel.CheckedByUserId,
                    ApprovedByUserId = objLoanModel.ApprovedByUserId,
                    Status = "Pending",
                    IsLocked = true,
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

        [Authorize, HttpDelete, Route("api/current/loan/delete/{id}")]
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

        [Authorize, HttpGet, Route("api/current/loan/dropdown/list/customer")]
        public List<ApiModels.MstCustomerModel> DropdownListLoanCustomer()
        {
            var customers = from d in db.MstCustomers
                            select new ApiModels.MstCustomerModel
                            {
                                Id = d.Id,
                                FullName = d.FullName
                            };

            return customers.ToList();
        }

        [Authorize, HttpGet, Route("api/current/loan/dropdown/list/term")]
        public List<ApiModels.MstTermModel> DropdownListLoanTerm()
        {
            var terms = from d in db.MstTerms
                        select new ApiModels.MstTermModel
                        {
                            Id = d.Id,
                            Term = d.Term,
                            NumberOfDays = d.NumberOfDays,
                            DefaultInterestId = d.DefaultInterestId
                        };

            return terms.ToList();
        }

        [Authorize, HttpGet, Route("api/current/loan/dropdown/list/interest")]
        public List<ApiModels.MstInterestModel> DropdownListLoanInterest()
        {
            var interests = from d in db.MstInterests
                            select new ApiModels.MstInterestModel
                            {
                                Id = d.Id,
                                Interest = d.Interest,
                                Percentage = d.Percentage
                            };

            return interests.ToList();
        }

        [Authorize, HttpGet, Route("api/current/loan/dropdown/list/user")]
        public List<ApiModels.MstUserModel> DropdownListLoanUser()
        {
            var users = from d in db.MstUsers
                        select new ApiModels.MstUserModel
                        {
                            Id = d.Id,
                            FullName = d.FullName
                        };

            return users.ToList();
        }
    }
}
