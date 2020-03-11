using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LendingSystem.ApiControllers
{
    public class ApiTrnLoanChecklistController : ApiController
    {
        public Data.lendingsystemDataContext db = new Data.lendingsystemDataContext();

        [Authorize, HttpGet, Route("api/loanChecklist/list/{loanId}")]
        public List<ApiModels.TrnLoanChecklistModel> ListLoanCheckList(String loanId)
        {
            var loanCheckLists = from d in db.TrnLoanChecklists
                                 where d.LoanId == Convert.ToInt32(loanId)
                                 select new ApiModels.TrnLoanChecklistModel
                                 {
                                     Id = d.Id,
                                     LoanId = d.LoanId,
                                     Remarks = d.Remarks,
                                     CheckListId = d.CheckListId,
                                     CheckList = d.MstChecklist.CheckList,
                                     ImageAttachment = d.ImageAttachment != null ? d.ImageAttachment.ToArray() : null
                                 };

            return loanCheckLists.ToList();
        }

        [Authorize, HttpPut, Route("api/loanChecklist/update/{id}")]
        public HttpResponseMessage UpdateLoanCheckList(ApiModels.TrnLoanChecklistModel objLoanChecklist, String id)
        {
            try
            {
                var loanChecklist = from d in db.TrnLoanChecklists
                                    where d.Id == Convert.ToInt32(id)
                                    select d;

                if (loanChecklist.Any())
                {
                    if (loanChecklist.FirstOrDefault().TrnLoan.IsCancelled == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot change cancelled loan application.");
                    }

                    if (loanChecklist.FirstOrDefault().TrnLoan.IsApproved == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot change approved loan application.");
                    }

                    if (loanChecklist.FirstOrDefault().TrnLoan.IsDeclined == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot change declined loan application.");
                    }

                    if (loanChecklist.FirstOrDefault().TrnLoan.IsClosed == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is closed.");
                    }

                    if (loanChecklist.FirstOrDefault().TrnLoan.IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "This transaction is locked.");
                    }

                    byte[] imgarr = objLoanChecklist.ImageAttachment;

                    var updateLoanChecklist = loanChecklist.FirstOrDefault();
                    updateLoanChecklist.Remarks = objLoanChecklist.Remarks;
                    updateLoanChecklist.ImageAttachment = imgarr;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Checklist not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
