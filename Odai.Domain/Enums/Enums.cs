using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain.Enums
{
    public enum UserType
    {
        Owner = 1,
        Admin =2,
        User=3,
        Merchant=4,
    }
    public enum Role
    {
        Owner=1,
        Admin=2,
        User=3,
        Merchant=4,
    }
    public enum Status
    {
        Available = 1,
        Expired=2,
    }
    public enum OrderStatus
    {
        Pending,       // الطلب قيد الانتظار
        Processing,    // الطلب قيد المعالجة
        Shipped,       // الطلب تم شحنه
        Delivered,     // الطلب تم تسليمه
        Cancelled,     // الطلب تم إلغاؤه
        Returned,      // الطلب تم إرجاعه
        Failed         // الطلب فشل
    }
}
