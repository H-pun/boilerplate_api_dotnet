using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class Gopay
    {
        public bool enable_callback { get; set; }
        public string callback_url { get; set; }
    }
    
    public class GopayCharge : BasePaymentModel
    {
        public Gopay gopay { get; set; }
    }
}
