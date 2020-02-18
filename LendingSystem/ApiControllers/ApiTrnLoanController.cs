using Microsoft.AspNet.Identity;
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

        [Authorize, HttpGet, Route("api/loan/dropdown/list/customer")]
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

        [Authorize, HttpGet, Route("api/loan/dropdown/list/term")]
        public List<ApiModels.MstTermModel> DropdownListLoanTerm()
        {
            var terms = from d in db.MstTerms
                        select new ApiModels.MstTermModel
                        {
                            Id = d.Id,
                            Term = d.Term,
                            NumberOfMonths = d.NumberOfMonths,
                            DefaultInterestId = d.DefaultInterestId
                        };

            return terms.ToList();
        }

        [Authorize, HttpGet, Route("api/loan/dropdown/list/interest")]
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

        [Authorize, HttpGet, Route("api/loan/dropdown/list/user")]
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

        [Authorize, HttpGet, Route("api/loan/list/{startDate}/{endDate}/{transactionType}")]
        public List<ApiModels.TrnLoanModel> LoanList(String startDate, String endDate, String transactionType)
        {
            Boolean isClosed = false;
            if (transactionType == "Close")
            {
                isClosed = true;
            }

            if (transactionType == "All")
            {
                var loans = from d in db.TrnLoans
                            where d.LoanDate >= Convert.ToDateTime(startDate)
                            && d.LoanDate <= Convert.ToDateTime(endDate)
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
                                IsApproved = d.IsApproved,
                                IsFullyPaid = d.IsFullyPaid,
                                IsCancelled = d.IsCancelled,
                                IsClosed = d.IsClosed,
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser1.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                            };

                return loans.ToList();
            }
            else
            {
                var loans = from d in db.TrnLoans
                            where d.LoanDate >= Convert.ToDateTime(startDate)
                            && d.LoanDate <= Convert.ToDateTime(endDate)
                            && d.IsClosed == isClosed
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
                                IsApproved = d.IsApproved,
                                IsFullyPaid = d.IsFullyPaid,
                                IsCancelled = d.IsCancelled,
                                IsClosed = d.IsClosed,
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser1.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                            };

                return loans.ToList();
            }
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
                var lastLoan = from d in db.TrnLoans.OrderByDescending(d => d.Id)
                               select d;

                if (lastLoan.Any())
                {
                    var lastLoanNumber = Convert.ToInt32(lastLoan.FirstOrDefault().LoanNumber) + 0000000001;
                    loanNumber = LeadingZeroes(lastLoanNumber, 10);
                }

                var customer = from d in db.MstCustomers
                               where d.Id == objLoanModel.CustomerId
                               select d;

                if (customer.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
                }

                var term = from d in db.MstTerms
                           where d.Id == objLoanModel.TermId
                           select d;

                if (term.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Term not found.");
                }

                if (objLoanModel.PrincipalAmount > term.FirstOrDefault().LimitAmount)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, you cannot loan when the principal amount exceeds at " + term.FirstOrDefault().LimitAmount.ToString("#,##0.00"));
                }

                var getLastLoan = from d in db.TrnLoans.OrderByDescending(d => d.Id)
                                  where d.CustomerId == customer.FirstOrDefault().Id
                                  && d.BalanceAmount > 0
                                  && d.IsClosed == false
                                  select d;

                if (getLastLoan.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot proceed if you have open or pending transactions.");
                }

                Decimal principalAmount = objLoanModel.PrincipalAmount;
                Decimal interestAmount = principalAmount * (term.FirstOrDefault().MstInterest.Percentage / 100);
                Decimal loanAmount = principalAmount + interestAmount;
                Decimal netAmount = principalAmount - interestAmount;
                Decimal balanceAmount = principalAmount;
                Decimal amortizationAmount = term.FirstOrDefault().NumberOfMonths > 0 ? principalAmount / term.FirstOrDefault().NumberOfMonths : principalAmount;

                Data.TrnLoan newLoan = new Data.TrnLoan()
                {
                    LoanNumber = loanNumber,
                    LoanDate = DateTime.Today,
                    ManualLoanNumber = loanNumber,
                    CustomerId = customer.FirstOrDefault().Id,
                    TermId = term.FirstOrDefault().Id,
                    TermNumberOfMonths = term.FirstOrDefault().NumberOfMonths,
                    PrincipalAmount = principalAmount,
                    InterestId = term.FirstOrDefault().DefaultInterestId,
                    InterestPercentage = term.FirstOrDefault().MstInterest.Percentage,
                    InterestAmount = interestAmount,
                    LoanAmount = loanAmount,
                    NetAmount = netAmount,
                    AmortizationAmount = amortizationAmount,
                    PaidAmount = 0,
                    PenaltyAmount = 0,
                    BalanceAmount = balanceAmount,
                    Remarks = "Your loan application was manually processed.",
                    Status = "Open",
                    IsSubmitted = true,
                    IsApproved = true,
                    IsFullyPaid = false,
                    IsCancelled = false,
                    IsClosed = false,
                    IsLocked = false,
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
                    if (loan.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is already locked.");
                    }

                    var lockLoan = loan.FirstOrDefault();
                    lockLoan.IsLocked = true;
                    lockLoan.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    lockLoan.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan transaction not found.");
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
                    if (loan.FirstOrDefault().IsLocked == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is already unlocked.");
                    }

                    var lockLoan = loan.FirstOrDefault();
                    lockLoan.IsLocked = false;
                    lockLoan.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    lockLoan.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan transaction not found.");
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
                    if (loan.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot delete locked transactions.");
                    }

                    db.TrnLoans.DeleteOnSubmit(loan.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan transaction not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
