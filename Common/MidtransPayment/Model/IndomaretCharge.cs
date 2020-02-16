using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class CStore
    {
        public string store { get; set; }
        public string messege { get; set; }
    }
    
    public class IndomaretCharge : BasePaymentModel
    {
        public CStore cstore { get; set; }
    }
}
