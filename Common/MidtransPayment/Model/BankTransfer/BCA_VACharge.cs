using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class BCA_VAModel {
        public string bank { get; set; }
        public string va_number { get; set; }
    }
    
    public class BCA_VACharge : BasePaymentModel
    {
        public BCA_VAModel bank_transfer { get; set; }
    }
}
