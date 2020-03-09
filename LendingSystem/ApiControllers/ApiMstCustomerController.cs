using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LendingSystem.ApiControllers
{
    public class ApiMstCustomerController : ApiController
    {
        public Data.lendingsystemDataContext db = new Data.lendingsystemDataContext();

        [Authorize, HttpGet, Route("api/customer/list")]
        public List<ApiModels.MstCustomerModel> GetCustomerList()
        {
            var customers = from d in db.MstCustomers
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
                                UserName = d.MstUser2.Username,
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                ImageURL = d.ImageURL.ToArray()
                            };

            return customers.ToList();
        }

        [Authorize, HttpPost, Route("api/customer/save")]
        public HttpResponseMessage SaveCustomer(ApiModels.MstCustomerModel objCustomer)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                Data.MstCustomer newCustomer = new Data.MstCustomer()
                {
                    FullName = objCustomer.FullName,
                    BirthDate = Convert.ToDateTime(objCustomer.BirthDate),
                    Gender = objCustomer.Gender,
                    Address = objCustomer.Address,
                    ContactNumber = objCustomer.ContactNumber,
                    Photo = "NA",
                    UserId = null,
                    IsLocked = true,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.MstCustomers.InsertOnSubmit(newCustomer);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPut, Route("api/customer/update/{id}")]
        public HttpResponseMessage UpdateCustomer(ApiModels.MstCustomerModel objCustomer, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var customer = from d in db.MstCustomers
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (customer.Any())
                {
                    var updateCustomer = customer.FirstOrDefault();
                    updateCustomer.FullName = objCustomer.FullName;
                    updateCustomer.BirthDate = Convert.ToDateTime(objCustomer.BirthDate);
                    updateCustomer.Gender = objCustomer.Gender;
                    updateCustomer.Address = objCustomer.Address;
                    updateCustomer.ContactNumber = objCustomer.ContactNumber;
                    updateCustomer.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateCustomer.UpdatedDateTime = DateTime.Now;
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

        [Authorize, HttpDelete, Route("api/customer/delete/{id}")]
        public HttpResponseMessage DeleteCustomer(String id)
        {
            try
            {
                var customer = from d in db.MstCustomers
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (customer.Any())
                {
                    if (customer.FirstOrDefault().UserId == null)
                    {
                        db.MstCustomers.DeleteOnSubmit(customer.FirstOrDefault());
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Cannot delete customer with user accounts.");
                    }
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

        [Authorize, HttpPut, Route("api/customer/uploadImage/{id}")]
        public HttpResponseMessage UploadImageCustomer(ApiModels.MstCustomerModel objCustomer, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var customer = from d in db.MstCustomers
                               where d.Id == Convert.ToInt32(id)
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
    }
}
