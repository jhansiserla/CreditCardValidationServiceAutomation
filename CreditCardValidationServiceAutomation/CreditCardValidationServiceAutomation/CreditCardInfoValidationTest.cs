using CreditCardValidationServiceAutomation.ValidationErrorResponse;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Text;

namespace CreditCardValidationServiceAutomation
{
    public class CreditCardInfoValidationTest
    {
        private string requestURL = "http://localhost:15797/api/CreditCardType";
        private HttpRequestMessage request;
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            client = GetHttpClient(requestURL);
            request = new HttpRequestMessage(HttpMethod.Post, System.Uri.EscapeUriString(client.BaseAddress.ToString()));
        }

        #region Success Validations returns Credit Card Type
        [Test]

        //Method returns Visa Card type after all sucessful validations on input
        public void CreditCardInfoValidation_ReturnsVisaCardType()
        {
            var responseString = "Visa";
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "4111111111111111",
                IssueDate = "02/20",
                CVC = 123
            };         
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var actualResponse = httpResponse.Content.ReadAsStringAsync().Result;
            actualResponse = actualResponse.Replace('"', ' ').Trim();
            Assert.AreEqual(responseString, actualResponse);
        }

        [Test]

        //Method returns MasterCard type after all successfull validations on input
        public void CreditCardInfoValidation_ReturnsVisaMasterCardType()
        {
            var responseString = "MasterCard";
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "5555555555554444",
                IssueDate = "02/19",
                CVC = 108
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var actualResponse = httpResponse.Content.ReadAsStringAsync().Result;
            actualResponse = actualResponse.Replace('"', ' ').Trim();
            Assert.AreEqual(responseString, actualResponse);
        }

        [Test]

        //Method returns American Express Card type after all successfull validations on input
        public void CreditCardInfoValidation_ReturnsVisaAmericanExpressCardType()
        {
            var responseString = "AmericanExpress";
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "378282246310005",
                IssueDate = "02/19",
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var actualResponse = httpResponse.Content.ReadAsStringAsync().Result;
            actualResponse = actualResponse.Replace('"', ' ').Trim();
            Assert.AreEqual(responseString, actualResponse);
        }

        #endregion

        #region Credit Card Info Error Validations

        [Test]

        //Method returns errors in all input fields if they are empty and invalid data
        public void CreditCardInfoValidation_AllEmptyFields_ReturnsErrors()
        {
            var input = new
            {
                //Empty informations in all fields with invalid data in CVC
                CreditCardOwnerName = "",
                CreditCardNumber = "",
                IssueDate = "",
                CVC = 0
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CreditCardOwnerName.Length > 0
                && response.errors.CreditCardNumber.Length > 0
                && response.errors.CVC.Length > 0
                && response.errors.IssueDate.Length >0
                )
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns invalid card owner name error message with invalid owner name in input
        public void CreditCardInfoValidation_InvalidCardOwnerName_ReturnsErrors()
        {
            var input = new
            {
                //Invalid card owner name since only alphabets are allowed
                CreditCardOwnerName = "34234",
                CreditCardNumber = "378282246310005",
                IssueDate = "02/19",
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CreditCardOwnerName.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns invalid credit card number error message with invalid card number in input
        public void CreditCardInfoValidation_InvalidCreditCardNumber_ReturnsErrors()
        {
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                //Invalid Credit Card Number since only Visa,Master and American Express type card numbers are allowed
                CreditCardNumber = "343453",
                IssueDate = "02/19",
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CreditCardNumber.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns invalid credit card issue date error message with invalid issue date in input
        public void CreditCardInfoValidation_InvalidIssueDate_ReturnsErrors()
        {
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "378282246310005",
                //Invalid issue date format where only allowed format is mm/yy
                IssueDate = "02/2019",
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.IssueDate.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns expired card error message with invalid issue date in input
        public void CreditCardInfoValidation_ExpiredCard_ReturnsErrors()
        {
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "378282246310005",
                //Expired card since after adding up 4 years(this is configurable in service) to issue date results date  which is less than current date
                IssueDate = "02/17",
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.IssueDate.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns invalid CVC for American Express Card
        public void CreditCardInfoValidation_InvalidCVCForAmericanExpress_ReturnsErrors()
        {
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "378282246310005",
                IssueDate = "02/19",
                //Number of digits in CVC should be four for American Express Credit Card
                CVC = 108
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CVC.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns invalid CVC for Visa Card
        public void CreditCardInfoValidation_InvalidCVCForVisaCard_ReturnsErrors()
        {
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "4111111111111111",
                IssueDate = "02/19",
                //Number of digits in CVC should be three for Visa Credit Card
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CVC.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns invalid CVC for Master Card
        public void CreditCardInfoValidation_InvalidCVCForMasterCard_ReturnsErrors()
        {
            var input = new
            {
                CreditCardOwnerName = "TestUser",
                CreditCardNumber = "5555555555554444",
                IssueDate = "02/19",
                //Number of digits in CVC should be three for Master Credit Card
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CVC.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns error messages for both invalid card owner and invalid card number
        public void CreditCardInfoValidation_InvalidCardOwnerAndCardNumber_ReturnsErrors()
        {
            var input = new
            {
                //Invalid card owner name since only alphabets are allowed
                CreditCardOwnerName = "123434",
                //Invalid card number since only visa,master and american express card numbers are allowed
                CreditCardNumber = "6535567666",
                IssueDate = "02/19",
                CVC = 1084
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CreditCardNumber.Length > 0 && response.errors.CreditCardOwnerName.Length > 0)
            {
                Assert.Pass("Passed");
            }
        }

        [Test]

        //Method returns error messages for  invalid card owner,invalid card number, invalid issue date and invalid CVC
        public void CreditCardInfoValidation_InvalidCardOwnerAndCardNumberAndIssueDateAndCVC_ReturnsErrors()
        {
            var input = new
            {
                //Invalid card owner name since only alphabets are allowed
                CreditCardOwnerName = "123434",
                //Invalid card number since only visa,master and american express card numbers are allowed
                CreditCardNumber = "6535567666",
                //Invalid Issue date as only format is mm/yy
                IssueDate = "02/2019",
                //Invalid CVC 
                CVC = 1
            };
            var inputInJsonFormat = JsonConvert.SerializeObject(input);
            request.Content = new StringContent(inputInJsonFormat, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(System.Uri.EscapeUriString(client.BaseAddress.ToString()), request.Content).Result;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
            if (response.status == 400 && response.errors.CreditCardNumber.Length > 0
                && response.errors.CreditCardOwnerName.Length > 0
                && response.errors.IssueDate.Length > 0
                && response.errors.CVC.Length > 0
                )
            {
                Assert.Pass("Passed");
            }
        }

        #endregion

        #region private methods
        private HttpClient GetHttpClient(string postRequestURL)
        {
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = false };
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new System.Uri(postRequestURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        #endregion

    }
}