using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class Permata {
        public string recipient_name { get; set; }
    }

    public class Permata_VAModel {
        public string bank { get; set; }
        public Permata permata { get; set; }
    }
    
    public class Permata_VACharge : BasePaymentModel
    {
        public Permata_VAModel bank_transfer { get; set; }
    }
}
