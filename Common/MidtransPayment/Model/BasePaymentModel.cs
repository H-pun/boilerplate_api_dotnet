using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class TransactionDetail {
        public string order_id { get; set; }
        public decimal gross_amount { get; set; }
    }
    public class CostumExpiry
    {
        public int expiry_duration { get; set; }
        public string unit { get; set; }
    }

    public class MemberInfo 
    {
        public int idMember { get; set; } 
        public int idMemberDream {get; set; }
        public int idMemberStep { get; set; }
    }

    public class BasePaymentModel {
        public string payment_type { get; set; }
        public TransactionDetail transaction_details { get; set; }
        public CostumExpiry custom_expiry { get; set; }
        public MemberInfo member_info { get; set;}
    }
}
