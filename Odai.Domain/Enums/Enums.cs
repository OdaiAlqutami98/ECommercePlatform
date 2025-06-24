
using System.ComponentModel;

namespace Odai.Domain.Enums
{

    public enum ProductSortOption
    {
        Popular = 1,
        PriceHighToLow = 2,
        PriceLowToHigh = 3,
        Newest = 4,
        //TopRated = 5
    }
    public enum ProductStatus
    {
        [Description("متوفر")]
        Available=1 ,

        [Description("قريبًا")]
        ComingSoon=2 ,

        [Description("نفدت الكمية")]
        OutOfStock=3 
    }
    public enum OrderStatus
    {
        Pending,      
        Confirmed,    
        Cancelled,    
        Returned         
    }
    public enum Payment
    {
        [Description("الدفع عند الاستلام")]
        CashOnDelivery = 1,

        [Description("كليك CliQ")]
        CliQ = 2
    }
    public enum City
    {
        [Description("عمّان")]
        Amman = 1,

        [Description("الزرقاء")]
        Zarqa = 2,

        [Description("إربد")]
        Irbid = 3,

        [Description("مادبا")]
        Madaba = 4,

        [Description("السلط")]
        Balqa = 5
    }
    
    public enum ReportFormat
    {
        PDF,
        WORD,
        EXCEL
    }
}
