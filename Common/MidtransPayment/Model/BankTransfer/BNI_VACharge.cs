using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class BNI_VAModel {
        public string bank { get; set; }
        public string vs_number { get; set; }
    }
    
    public class BNI_VACharge : BasePaymentModel
    {
        public BNI_VAModel bank_transfer { get; set; }
    }
}
