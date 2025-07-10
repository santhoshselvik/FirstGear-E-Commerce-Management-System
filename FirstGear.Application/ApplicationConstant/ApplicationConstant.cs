using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstGear.Application.ApplicationConstant
{
    public class CommonMessage
    {
        public static string RecordCreated = "Record Created Successfully";
        public static string RecordUpdated = "Record Updated Successfully";
        public static string RecordDeleted = "Record Deleted Successfully";
    }

    public static class CustomRole
    {
           
        public const string MasterAdmin = "MASTERADMIN";
        public const string Admin = "ADMIN";
        public const string Customer = " CUSTOMER";

    }
}
