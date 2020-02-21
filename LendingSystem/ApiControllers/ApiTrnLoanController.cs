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
                            Term = d.Term
                        };

            return terms.ToList();
        }

        [Authorize, HttpGet, Route("api/loan/list/{startDate}/{endDate}/{transactionType}")]
        public List<ApiModels.TrnLoanModel> LoanList(String startDate, String endDate, String transactionType, String filterText)
        {
            Boolean isClosed = false;
            if (transactionType == "Close")
            {
                isClosed = true;
            }

            String filter = "";
            if (String.IsNullOrEmpty(filterText) == false)
            {
                filter = filterText;
            }

            if (transactionType == "All")
            {
                var loans = from d in db.TrnLoans
                            where d.LoanDate >= Convert.ToDateTime(startDate)
                            && d.LoanDate <= Convert.ToDateTime(endDate)
                            && d.IsSubmitted == true
                            && d.IsCancelled == false
                            && (d.LoanNumber.Contains(filter) == true || d.MstCustomer.FullName.Contains(filter) == true)
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
                var loans = from d in db.TrnLoans
                            where d.LoanDate >= Convert.ToDateTime(startDate)
                            && d.LoanDate <= Convert.ToDateTime(endDate)
                            && d.IsSubmitted == true
                            && d.IsCancelled == false
                            && d.IsClosed == isClosed
                            && (d.LoanNumber.Contains(filter) == true || d.MstCustomer.FullName.Contains(filter) == true)
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
        }

        [Authorize, HttpGet, Route("api/loan/detail/{id}")]
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

        [Authorize, HttpPost, Route("api/loan/create")]
        public HttpResponseMessage CreateLoan(ApiModels.TrnLoanModel objLoanModel)
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, the customer cannot loan if the principal amount exceeds at " + term.FirstOrDefault().LimitAmount.ToString("#,##0.00"));
                }

                var getLastLoan = from d in db.TrnLoans.OrderByDescending(d => d.Id)
                                  where d.CustomerId == customer.FirstOrDefault().Id
                                  && d.BalanceAmount > 0
                                  && d.IsClosed == false
                                  select d;

                if (getLastLoan.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot proceed if the customer has an open or pending previous transaction.");
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
                    Remarks = "NA",
                    Status = "Open",
                    IsSubmitted = true,
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

                return Request.CreateResponse(HttpStatusCode.OK, newLoan.Id.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPut, Route("api/loan/update/{id}")]
        public HttpResponseMessage UpdateLoan(String id, ApiModels.TrnLoanModel objLoanModel)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var term = from d in db.MstTerms
                           where d.Id == objLoanModel.TermId
                           select d;

                if (term.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Term not found.");
                }

                if (objLoanModel.PrincipalAmount > term.FirstOrDefault().LimitAmount)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, the customer cannot loan when the principal amount exceeds at " + term.FirstOrDefault().LimitAmount.ToString("#,##0.00"));
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
                    if (loan.FirstOrDefault().IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot change cancelled loan application.");
                    }

                    if (loan.FirstOrDefault().IsApproved == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot change approved loan application.");
                    }

                    if (loan.FirstOrDefault().IsDeclined == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot change declined loan application.");
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

        [Authorize, HttpPut, Route("api/loan/approve/{id}")]
        public HttpResponseMessage ApproveLoan(ApiModels.TrnLoanModel objLoanModel, String id)
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
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot approve unsubmitted loan application.");
                    }

                    if (loan.FirstOrDefault().IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot approve cancelled loan application.");
                    }

                    if (loan.FirstOrDefault().IsApproved == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This loan application was already approved.");
                    }

                    if (loan.FirstOrDefault().IsDeclined == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot approve declined loan application.");
                    }

                    if (loan.FirstOrDefault().IsClosed == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is closed.");
                    }

                    var approveLoan = loan.FirstOrDefault();
                    approveLoan.Status = "Approved";
                    approveLoan.Remarks = "Your loan application has been approved. Please bring your requirements and your co-maker at the office.";
                    approveLoan.IsApproved = true;
                    approveLoan.IsLocked = true;
                    approveLoan.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    approveLoan.UpdatedDateTime = DateTime.Now;
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

        [Authorize, HttpPut, Route("api/loan/decline/{id}")]
        public HttpResponseMessage DeclineLoan(ApiModels.TrnLoanModel objLoanModel, String id)
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
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot decline unsubmitted loan application.");
                    }

                    if (loan.FirstOrDefault().IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot decline cancelled loan application.");
                    }

                    if (loan.FirstOrDefault().IsApproved == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot decline approved loan application.");
                    }

                    if (loan.FirstOrDefault().IsDeclined == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This loan application was already declcined.");
                    }

                    if (loan.FirstOrDefault().IsClosed == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is closed.");
                    }

                    var declineLoan = loan.FirstOrDefault();
                    declineLoan.Status = "Declined";
                    declineLoan.Remarks = "Sorry but your loan application has been declined.";
                    declineLoan.IsDeclined = true;
                    declineLoan.IsLocked = true;
                    declineLoan.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    declineLoan.UpdatedDateTime = DateTime.Now;
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

        [Authorize, HttpPut, Route("api/loan/closeTransaction/{id}")]
        public HttpResponseMessage CloseLoanTransaction(ApiModels.TrnLoanModel objLoanModel, String id)
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
                    if(loan.FirstOrDefault().IsSubmitted == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot close unsubmitted loan application.");
                    }

                    if (loan.FirstOrDefault().IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot close cancelled loan application.");
                    }

                    if (loan.FirstOrDefault().IsClosed == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is already closed.");
                    }

                    var closeLoanTransaction = loan.FirstOrDefault();
                    closeLoanTransaction.Status = "Closed";
                    closeLoanTransaction.Remarks = "Sorry. Your transaction has been closed due to some reasons: \n\n" + objLoanModel.Remarks;
                    closeLoanTransaction.IsClosed = true;
                    closeLoanTransaction.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    closeLoanTransaction.UpdatedDateTime = DateTime.Now;
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
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is locked.");
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
