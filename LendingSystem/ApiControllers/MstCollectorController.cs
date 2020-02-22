using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace LendingSystem.ApiControllers
{
    public class MstCollectorController : ApiController
    {
        public Data.lendingsystemDataContext db = new Data.lendingsystemDataContext();

        [Authorize, HttpGet, Route("api/collector/list")]
        public List<ApiModels.MstCollectorModel> GetCollectorList()
        {
            var collectors = from d in db.MstCollectors
                             select new ApiModels.MstCollectorModel
                             {
                                 Id = d.Id,
                                 Collector = d.Collector
                             };

            return collectors.ToList();
        }

        [Authorize, HttpPost, Route("api/collector/save")]
        public HttpResponseMessage SaveCollector(ApiModels.MstCollectorModel objCollector)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                Data.MstCollector newCollector = new Data.MstCollector()
                {
                    Collector = objCollector.Collector
                };

                db.MstCollectors.InsertOnSubmit(newCollector);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPut, Route("api/collector/update/{id}")]
        public HttpResponseMessage UpdateCollector(ApiModels.MstCollectorModel objCollector, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                var collector = from d in db.MstCollectors
                                where d.Id == Convert.ToInt32(id)
                                select d;

                if (collector.Any())
                {
                    var updateCollector = collector.FirstOrDefault();
                    updateCollector.Collector = objCollector.Collector;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Collector not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpDelete, Route("api/collector/delete/{id}")]
        public HttpResponseMessage DeleteCollector(String id)
        {
            try
            {
                var collector = from d in db.MstCollectors
                                where d.Id == Convert.ToInt32(id)
                                select d;

                if (collector.Any())
                {
                    db.MstCollectors.DeleteOnSubmit(collector.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Collector not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}