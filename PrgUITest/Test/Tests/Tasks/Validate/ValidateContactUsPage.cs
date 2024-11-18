using PrgUITest.Test.Common;
using OpenQA.Selenium;
using Newtonsoft.Json;
using OpenQA.Selenium.DevTools.V128.HeapProfiler;
using System.Collections.ObjectModel;

namespace PrgUITest.Test.Tests.Tasks.Validate
{

    public class ValidateContactUsPage
    {

        private ValidateContactUsPage()
        {
        }

        public ValidateContactUsPage RequiredFields()
        {

            //For this excercise we are assuming first and last are filled out, irl much more felxible 
            //if we have a map for which fields to check (much more reuse)

            IWebDriver driver = Driver.GetInstance();

            //Start by checking that there are 4 fields with messages still there
            //Again irl this would be a parameter for the static factory method and constructor
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.CssSelector(".hs-error-msg.hs-main-font-element"));
            Assert.Equal(4, elements.Count());

            //looking at multiple classes to be more reliable
            IWebElement email = driver.FindElement(By.CssSelector(".hs_email.hs-email.hs-fieldtype-text.field.hs-form-field"));
            ValidateMarkedRequired(email);
            ValidateRequiredErrorMessage(email);

            IWebElement company = driver.FindElement(By.CssSelector(".hs_company.hs-company.hs-fieldtype-text.field.hs-form-field"));
            ValidateMarkedRequired(company);
            ValidateRequiredErrorMessage(company);

            IWebElement region = driver.FindElement(By.CssSelector(".hs_global_region.hs-global_region.hs-fieldtype-select.field.hs-form-field"));
            ValidateMarkedRequired(region);
            ValidateRequiredErrorMessage(region);

            IWebElement product = driver.FindElement(By.CssSelector(".hs_product.hs-product.hs-fieldtype-select.field.hs-form-field"));
            ValidateMarkedRequired(product);
            ValidateRequiredErrorMessage(product);

            //in the future might call another method for validation
            return this;
        }


        //DRY helper method since the code for validating the error message 
        private void ValidateMarkedRequired(IWebElement container)
        {
            //looking for error messahe within element
            IWebElement asterisk = container.FindElement(By.CssSelector(".hs-form-required"));
            Assert.Equal("*", asterisk.Text);
        }

        //DRY helper method for validating the error message
        private void ValidateRequiredErrorMessage(IWebElement container)
        {
            //looking for error message within element
            IWebElement errMsg = container.FindElement(By.CssSelector(".hs-error-msg.hs-main-font-element"));
            Assert.Equal("Please complete this required field.", errMsg.Text);
        }

        public static ValidateContactUsPage For()
        {
            return new ValidateContactUsPage();
        }
    }
}