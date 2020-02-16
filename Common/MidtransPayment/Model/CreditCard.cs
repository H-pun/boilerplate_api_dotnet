using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class CreditCard
    {   
        public string client_key { get; set; }
        public string card_number { get; set; }
        public string card_exp_month { get; set; }
        public string card_exp_year { get; set; }
        public string card_cvv { get; set; }
    }
}
