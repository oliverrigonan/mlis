using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingSystem.ApiModels
{
    public class MstCustomerModel
    {
        public Int32 Id { get; set; }
        public String FullName { get; set; }
        public String BirthDate { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String ContactNumber { get; set; }
        public String Photo { get; set; }
        public Int32? UserId { get; set; }
        public String UserName { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedDateTime { get; set; }
        public byte[] ImageURL { get; set; }
    }
}