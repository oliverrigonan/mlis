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

        [Authorize, HttpGet, Route("api/current/loan/list/{status}")]
        public List<ApiModels.TrnLoanModel> LoanHistoryList(String status)
        {
            var currentUser = from d in db.MstUsers
                              where d.AspNetUserId == User.Identity.GetUserId()
                              select d;

            var customer = from d in db.MstCustomers
                           where d.UserId == currentUser.FirstOrDefault().Id
                           select d;

            if (customer.Any())
            {
                if (status == "History")
                {
                    var loans = from d in db.TrnLoans
                                where d.CustomerId == customer.FirstOrDefault().Id
                                && d.Status != "Draft"
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
                                    PreviousAmount = d.PreviousAmount,
                                    Amount = d.Amount,
                                    NetAmount = d.NetAmount,
                                    AmortizationAmount = d.AmortizationAmount,
                                    PaidAmount = d.PaidAmount,
                                    PenaltyAmount = d.PenaltyAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    Remarks = d.Remarks,
                                    Status = d.Status,
                                    IsLocked = d.IsLocked,
                                    CreatedByUserId = d.CreatedByUserId,
                                    CreatedByUser = d.MstUser.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedByUserId = d.UpdatedByUserId,
                                    UpdatedByUser = d.MstUser1.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                };

                    return loans.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    var loans = from d in db.TrnLoans
                                where d.CustomerId == customer.FirstOrDefault().Id
                                && d.Status == status
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
                                    PreviousAmount = d.PreviousAmount,
                                    Amount = d.Amount,
                                    NetAmount = d.NetAmount,
                                    AmortizationAmount = d.AmortizationAmount,
                                    PaidAmount = d.PaidAmount,
                                    PenaltyAmount = d.PenaltyAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    Remarks = d.Remarks,
                                    Status = d.Status,
                                    IsLocked = d.IsLocked,
                                    CreatedByUserId = d.CreatedByUserId,
                                    CreatedByUser = d.MstUser.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedByUserId = d.UpdatedByUserId,
                                    UpdatedByUser = d.MstUser1.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                };

                    return loans.OrderByDescending(d => d.Id).ToList();
                }
            }
            else
            {
                return new List<ApiModels.TrnLoanModel>();
            }
        }

        [Authorize, HttpGet, Route("api/current/loan/detail/{id}")]
        public ApiModels.TrnLoanModel LoanDetail(String id)
        {
            var loan = from d in db.TrnLoans
                       where d.Id == Convert.ToInt32(id)
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
                           PreviousAmount = d.PreviousAmount,
                           Amount = d.Amount,
                           NetAmount = d.NetAmount,
                           AmortizationAmount = d.AmortizationAmount,
                           PaidAmount = d.PaidAmount,
                           PenaltyAmount = d.PenaltyAmount,
                           BalanceAmount = d.BalanceAmount,
                           Remarks = d.Remarks,
                           Status = d.Status,
                           IsLocked = d.IsLocked,
                           CreatedByUserId = d.CreatedByUserId,
                           CreatedByUser = d.MstUser.FullName,
                           CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                           UpdatedByUserId = d.UpdatedByUserId,
                           UpdatedByUser = d.MstUser1.FullName,
                           UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                       };

            return loan.FirstOrDefault();
        }

        [Authorize, HttpPost, Route("api/current/loan/apply")]
        public HttpResponseMessage ApplyLoan(ApiModels.TrnLoanModel objLoanModel)
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
                               where d.UserId == currentUser.FirstOrDefault().Id
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

                Decimal principalAmount = objLoanModel.PrincipalAmount;
                Decimal interestAmount = principalAmount * (term.FirstOrDefault().MstInterest.Percentage / 100);
                Decimal loanAmount = principalAmount + interestAmount;

                Decimal previousAmount = 0;
                var customerLastLoan = from d in db.TrnLoans.OrderByDescending(d => d.Id)
                                       where d.CustomerId == customer.FirstOrDefault().Id
                                       && d.Status == "Current"
                                       select d;

                if (customerLastLoan.Any())
                {
                    previousAmount = customerLastLoan.FirstOrDefault().BalanceAmount;
                }

                if (previousAmount > principalAmount)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, you cannot loan because you still have previous balance.");
                }

                Decimal amount = loanAmount - previousAmount;
                Decimal netAmount = (principalAmount - previousAmount) - interestAmount;
                Decimal balanceAmount = principalAmount - previousAmount;
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
                    PreviousAmount = previousAmount,
                    Amount = amount,
                    NetAmount = netAmount,
                    AmortizationAmount = amortizationAmount,
                    PaidAmount = 0,
                    PenaltyAmount = 0,
                    BalanceAmount = balanceAmount,
                    Remarks = " ",
                    Status = "Draft",
                    IsLocked = true,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.TrnLoans.InsertOnSubmit(newLoan);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newLoan.Id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPost, Route("api/current/loan/update")]
        public HttpResponseMessage UpdateLoan(ApiModels.TrnLoanModel objLoanModel)
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
                               where d.UserId == currentUser.FirstOrDefault().Id
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
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sorry, you cannot loan when the principal amount exceeds at " + term.FirstOrDefault().LimitAmount.ToString("#,##0.00"));
                }

                Decimal principalAmount = objLoanModel.PrincipalAmount;
                Decimal interestAmount = principalAmount * term.FirstOrDefault().MstInterest.Percentage;
                Decimal loanAmount = principalAmount + interestAmount;
                Decimal previousAmount = 0;
                Decimal amount = loanAmount - previousAmount;
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
                    PreviousAmount = previousAmount,
                    Amount = amount,
                    NetAmount = netAmount,
                    AmortizationAmount = amortizationAmount,
                    PaidAmount = 0,
                    PenaltyAmount = 0,
                    BalanceAmount = balanceAmount,
                    Remarks = " ",
                    Status = "Loan Application",
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
                    if (loan.FirstOrDefault().Status == "Draft")
                    {
                        var deleteLoan = loan.FirstOrDefault();
                        db.TrnLoans.DeleteOnSubmit(deleteLoan);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Loan history cannot be deleted.");
                    }
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

        [Authorize, HttpDelete, Route("api/current/loan/cancel/{id}")]
        public HttpResponseMessage CancelLoan(String id)
        {
            try
            {
                var loan = from d in db.TrnLoans
                           where d.Id == Convert.ToInt32(id)
                           select d;

                if (loan.Any())
                {
                    if (loan.FirstOrDefault().Status == "Cancelled")
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already cancelled.");
                    }
                    else
                    {
                        var cancelLoan = loan.FirstOrDefault();
                        cancelLoan.Status = "Cancelled";
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
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

        [Authorize, HttpGet, Route("api/current/loan/dropdown/list/term")]
        public List<ApiModels.MstTermModel> DropdownListLoanTerm()
        {
            var terms = from d in db.MstTerms
                        select new ApiModels.MstTermModel
                        {
                            Id = d.Id,
                            Term = d.Term,
                            NumberOfMonths = d.NumberOfMonths,
                            DefaultInterestId = d.DefaultInterestId,
                            LimitAmount = d.LimitAmount
                        };

            return terms.ToList();
        }
    }
}
