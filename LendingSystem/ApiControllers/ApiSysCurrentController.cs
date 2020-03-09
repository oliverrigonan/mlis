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
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                               ImageURL = d.ImageURL != null ? d.ImageURL.ToArray() : null
                           };

            return customer.FirstOrDefault();
        }

        [Authorize, HttpPut, Route("api/current/uploadImage")]
        public HttpResponseMessage UploadImageCustomer(ApiModels.MstCustomerModel objCustomer)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var customer = from d in db.MstCustomers
                               where d.UserId == currentUser.FirstOrDefault().Id
                               select d;

                if (customer.Any())
                {
                    byte[] imgarr = objCustomer.ImageURL;

                    var updateCustomer = customer.FirstOrDefault();
                    updateCustomer.ImageURL = imgarr;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Authorize, HttpGet, Route("api/current/loan/list/{transactionType}")]
        public List<ApiModels.TrnLoanModel> LoanHistoryList(String transactionType)
        {
            var currentUser = from d in db.MstUsers
                              where d.AspNetUserId == User.Identity.GetUserId()
                              select d;

            var customer = from d in db.MstCustomers
                           where d.UserId == currentUser.FirstOrDefault().Id
                           select d;

            if (customer.Any())
            {
                if (transactionType == "Activity")
                {
                    var loans = from d in db.TrnLoans
                                where d.CustomerId == customer.FirstOrDefault().Id
                                && d.IsClosed == false
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
                else if (transactionType == "History")
                {
                    var loans = from d in db.TrnLoans
                                where d.CustomerId == customer.FirstOrDefault().Id
                                && d.IsClosed == true
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
                else
                {
                    return new List<ApiModels.TrnLoanModel>();
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
            var declineLoan = from d in db.TrnLoans
                              where d.Id == Convert.ToInt32(id)
                              && d.IsDeclined == true
                              select d;

            if (declineLoan.Any())
            {
                var closeLoan = declineLoan.FirstOrDefault();
                closeLoan.IsClosed = true;
                db.SubmitChanges();
            }

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
                    Remarks = "Your loan application is ready to process. To continue this transaction, please click the Submit button to proceed.",
                    Status = "Open",
                    IsSubmitted = false,
                    IsCancelled = false,
                    IsApproved = false,
                    IsDeclined = false,
                    IsClosed = false,
                    IsLocked = false,
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

        [Authorize, HttpPut, Route("api/current/loan/update/{id}")]
        public HttpResponseMessage UpdateLoan(String id, ApiModels.TrnLoanModel objLoanModel)
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
                Decimal netAmount = principalAmount - interestAmount;
                Decimal balanceAmount = principalAmount;
                Decimal amortizationAmount = term.FirstOrDefault().NumberOfMonths > 0 ? principalAmount / term.FirstOrDefault().NumberOfMonths : principalAmount;

                var loan = from d in db.TrnLoans
                           where d.Id == Convert.ToInt32(id)
                           select d;

                if (loan.Any())
                {
                    if (loan.FirstOrDefault().IsSubmitted == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot change submitted loan application.");
                    }

                    if (loan.FirstOrDefault().IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot change cancelled loan application.");
                    }

                    if (loan.FirstOrDefault().IsApproved == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot change approved loan application.");
                    }

                    if (loan.FirstOrDefault().IsDeclined == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot change declined loan application.");
                    }

                    if (loan.FirstOrDefault().IsClosed == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is closed.");
                    }

                    if (loan.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is locked.");
                    }

                    var submitLoan = loan.FirstOrDefault();
                    submitLoan.TermId = term.FirstOrDefault().Id;
                    submitLoan.TermNumberOfMonths = term.FirstOrDefault().NumberOfMonths;
                    submitLoan.PrincipalAmount = principalAmount;
                    submitLoan.InterestId = term.FirstOrDefault().DefaultInterestId;
                    submitLoan.InterestPercentage = term.FirstOrDefault().MstInterest.Percentage;
                    submitLoan.InterestAmount = interestAmount;
                    submitLoan.LoanAmount = loanAmount;
                    submitLoan.NetAmount = netAmount;
                    submitLoan.AmortizationAmount = amortizationAmount;
                    submitLoan.BalanceAmount = balanceAmount;
                    submitLoan.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    submitLoan.UpdatedDateTime = DateTime.Now;
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

        [Authorize, HttpPut, Route("api/current/loan/submit/{id}")]
        public HttpResponseMessage SubmitLoan(String id, ApiModels.TrnLoanModel objLoanModel)
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
                    if (loan.FirstOrDefault().IsSubmitted == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Your loan application was already submitted. Please wait for an approval.");
                    }

                    var submitLoan = loan.FirstOrDefault();
                    submitLoan.IsSubmitted = true;
                    submitLoan.Status = "Submitted";
                    submitLoan.Remarks = "Your loan application has been successfully submitted for an approval. You will receive a text or an email once your loan application has been approved.";
                    submitLoan.IsLocked = true;
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

        [Authorize, HttpPut, Route("api/current/loan/cancel/{id}")]
        public HttpResponseMessage CancelLoan(String id, ApiModels.TrnLoanModel objLoanModel)
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
                    if (loan.FirstOrDefault().IsSubmitted == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot cancel unsubmitted loan application.");
                    }

                    if (loan.FirstOrDefault().IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Your loan application was already cancelled.");
                    }

                    if (loan.FirstOrDefault().IsDeclined == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot cancel declined loan application.");
                    }

                    if (loan.FirstOrDefault().IsClosed == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is closed.");
                    }

                    var cancelLoan = loan.FirstOrDefault();
                    cancelLoan.IsCancelled = true;
                    cancelLoan.IsClosed = true;
                    cancelLoan.Status = "Cancelled";
                    cancelLoan.Remarks = "Your loan application has been cancelled.";
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
                    if (loan.FirstOrDefault().IsSubmitted == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete submitted loan application.");
                    }

                    if (loan.FirstOrDefault().IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete cancelled loan application.");
                    }

                    if (loan.FirstOrDefault().IsApproved == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete approved loan application.");
                    }

                    if (loan.FirstOrDefault().IsDeclined == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete declined loan application.");
                    }

                    if (loan.FirstOrDefault().IsClosed == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is closed.");
                    }

                    if (loan.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is locked.");
                    }

                    var deleteLoan = loan.FirstOrDefault();
                    db.TrnLoans.DeleteOnSubmit(deleteLoan);
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
