//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrivatesquaresWebApiNew.Persistance.Data
{
    using System;
    
    public partial class GetReview_Result
    {
        public Nullable<long> ProductId { get; set; }
        public Nullable<long> UserId { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> TotalRating { get; set; }
        public Nullable<decimal> GivenRating { get; set; }
        public string Review { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string AppStatus { get; set; }
    }
}
