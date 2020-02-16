using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class CreditToken
    {
        public string token_id { get; set; }
        public bool authentication { get; set; }
    }
    
    public class CreditCharge : BasePaymentModel
    {
        public CreditToken credit_card { get; set; }
    }
}
