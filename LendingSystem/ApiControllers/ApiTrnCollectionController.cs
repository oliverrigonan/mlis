using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LendingSystem.ApiControllers
{
    public class ApiTrnCollectionController : ApiController
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

        public void UpdateLoanTransaction(Int32 loanId)
        {
            try
            {
                Decimal totalPaidAmount = 0;
                Decimal totalPaneltyAmount = 0;
                Decimal totalBalanceAmount = 0;

                var collections = from d in db.TrnCollections
                                  where d.LoanId == loanId
                                  select d;

                if (collections.Any())
                {
                    totalPaidAmount = collections.Sum(d => d.PaidAmount);
                    totalPaneltyAmount = collections.Sum(d => d.PenaltyAmount);

                    var loan = from d in db.TrnLoans
                               where d.Id == loanId
                               select d;

                    if (loan.Any())
                    {
                        totalBalanceAmount = loan.FirstOrDefault().PrincipalAmount - (totalPaidAmount + totalPaneltyAmount);

                        var updateLoan = loan.FirstOrDefault();
                        updateLoan.PaidAmount = totalPaidAmount;
                        updateLoan.PenaltyAmount = totalPaneltyAmount;
                        updateLoan.BalanceAmount = totalBalanceAmount;
                        db.SubmitChanges();
                    }
                    else
                    {
                        throw new Exception("Empty loans.");
                    }
                }
                else
                {
                    throw new Exception("Empty collections.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize, HttpGet, Route("api/collection/dropdown/list/customer")]
        public List<ApiModels.MstCustomerModel> DropdownListCollectionCustomer()
        {
            var customers = from d in db.MstCustomers
                            select new ApiModels.MstCustomerModel
                            {
                                Id = d.Id,
                                FullName = d.FullName
                            };

            return customers.ToList();
        }

        [Authorize, HttpGet, Route("api/collection/dropdown/list/loanNumber/{customerId}")]
        public List<ApiModels.TrnLoanModel> DropdownListCollectionLoanNumber(String customerId)
        {
            var loans = from d in db.TrnLoans
                        where d.CustomerId == Convert.ToInt32(customerId)
                        && d.BalanceAmount > 0
                        select new ApiModels.TrnLoanModel
                        {
                            Id = d.Id,
                            LoanNumber = d.LoanNumber,
                            BalanceAmount = d.BalanceAmount
                        };

            return loans.ToList();
        }

        [Authorize, HttpGet, Route("api/collection/dropdown/list/payType")]
        public List<ApiModels.MstPayTypeModel> DropdownListLoanPayType()
        {
            var payTypes = from d in db.MstPayTypes
                           select new ApiModels.MstPayTypeModel
                           {
                               Id = d.Id,
                               PayType = d.PayType
                           };

            return payTypes.ToList();
        }

        [Authorize, HttpGet, Route("api/collection/list/{startDate}/{endDate}")]
        public List<ApiModels.TrnCollectionModel> CollectionList(String startDate, String endDate, String filterText)
        {
            String filter = "";
            if (String.IsNullOrEmpty(filterText) == false)
            {
                filter = filterText;
            }

            var collections = from d in db.TrnCollections
                              where d.CollectionDate >= Convert.ToDateTime(startDate)
                              && d.CollectionDate <= Convert.ToDateTime(endDate)
                              && (d.CollectionNumber.Contains(filter) == true || d.TrnLoan.LoanNumber.Contains(filter) == true || d.Remarks.Contains(filter) == true)
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

        [Authorize, HttpPost, Route("api/collection/add")]
        public HttpResponseMessage AddCollection(ApiModels.TrnCollectionModel objCollection)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var customer = from d in db.MstCustomers
                               where d.Id == objCollection.CustomerId
                               select d;

                if (customer.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
                }

                var loan = from d in db.TrnLoans
                           where d.Id == objCollection.LoanId
                           && d.CustomerId == customer.FirstOrDefault().Id
                           select d;

                if (loan.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan transaction not found.");
                }

                var payType = from d in db.MstPayTypes
                              where d.Id == objCollection.PayTypeId
                              select d;

                if (payType.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Pay type not found.");
                }

                var collectionNumber = "0000000001";
                var lastCollection = from d in db.TrnCollections.OrderByDescending(d => d.Id)
                                     select d;

                if (lastCollection.Any())
                {
                    var lastCollectionNumber = Convert.ToInt32(lastCollection.FirstOrDefault().CollectionNumber) + 0000000001;
                    collectionNumber = LeadingZeroes(lastCollectionNumber, 10);
                }

                Data.TrnCollection newCollection = new Data.TrnCollection()
                {
                    CollectionNumber = collectionNumber,
                    CollectionDate = Convert.ToDateTime(objCollection.CollectionDate),
                    ManualCollectionNumber = collectionNumber,
                    CustomerId = customer.FirstOrDefault().Id,
                    LoanId = loan.FirstOrDefault().Id,
                    PayTypeId = payType.FirstOrDefault().Id,
                    PaidAmount = objCollection.PaidAmount,
                    PenaltyAmount = objCollection.PenaltyAmount,
                    Remarks = objCollection.Remarks,
                    IsLocked = true,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.TrnCollections.InsertOnSubmit(newCollection);
                db.SubmitChanges();

                UpdateLoanTransaction(loan.FirstOrDefault().Id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPost, Route("api/collection/update/{id}")]
        public HttpResponseMessage UpdateCollection(ApiModels.TrnCollectionModel objCollection, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var customer = from d in db.MstCustomers
                               where d.Id == objCollection.CustomerId
                               select d;

                if (customer.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
                }

                var loan = from d in db.TrnLoans
                           where d.Id == objCollection.LoanId
                           && d.CustomerId == customer.FirstOrDefault().Id
                           select d;

                if (loan.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan transaction not found.");
                }

                var payType = from d in db.MstPayTypes
                              where d.Id == objCollection.PayTypeId
                              select d;

                if (payType.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Pay type not found.");
                }

                var collection = from d in db.TrnCollections
                                 where d.Id == Convert.ToInt32(id)
                                 select d;

                if (collection.Any())
                {
                    var updateCollection = collection.FirstOrDefault();
                    updateCollection.CollectionDate = Convert.ToDateTime(objCollection.CollectionDate);
                    updateCollection.CustomerId = customer.FirstOrDefault().Id;
                    updateCollection.LoanId = loan.FirstOrDefault().Id;
                    updateCollection.PayTypeId = payType.FirstOrDefault().Id;
                    updateCollection.PaidAmount = objCollection.PaidAmount;
                    updateCollection.PenaltyAmount = objCollection.PenaltyAmount;
                    updateCollection.Remarks = objCollection.Remarks;
                    updateCollection.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateCollection.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    UpdateLoanTransaction(loan.FirstOrDefault().Id);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Collection not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpDelete, Route("api/collection/delete/{id}")]
        public HttpResponseMessage DeleteCollection(String id)
        {
            try
            {
                var collection = from d in db.TrnCollections
                                 where d.Id == Convert.ToInt32(id)
                                 select d;

                if (collection.Any())
                {
                    db.TrnCollections.DeleteOnSubmit(collection.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Collection not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
