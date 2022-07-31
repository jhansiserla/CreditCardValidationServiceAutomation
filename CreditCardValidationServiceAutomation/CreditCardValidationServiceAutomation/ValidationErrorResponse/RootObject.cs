
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardValidationServiceAutomation.ValidationErrorResponse
{
    public class Rootobject
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string traceId { get; set; }
        public Errors errors { get; set; }
    }

   

    
    
}
