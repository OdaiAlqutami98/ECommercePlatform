using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain.Enums
{
    public enum Role
    {
        Owner=1,
        Admin=2,
        User=3,
        Merchant=4,
    }
    public enum ProductStatus
    {
        Available ,
        Expired ,
        ComingSoon ,
        OutOfStock 
    }
    public enum OrderStatus
    {
        Pending,      
        Processing,    
        Shipped,       
        Delivered,    
        Cancelled,     
        Returned,      
        Failed        
    }
}
