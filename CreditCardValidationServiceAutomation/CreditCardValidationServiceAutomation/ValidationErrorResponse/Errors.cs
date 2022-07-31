using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardValidationServiceAutomation.ValidationErrorResponse
{
    public class Errors
    {
        public string[] CVC { get; set; }
        public string[] CreditCardNumber { get; set; }
        public string[] CreditCardOwnerName { get; set; }
        public string[] IssueDate { get; set; }
    }
}
