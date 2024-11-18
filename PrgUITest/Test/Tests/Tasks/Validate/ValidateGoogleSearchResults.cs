using PrgUITest.Test.Common;
using OpenQA.Selenium;

namespace PrgUITest.Test.Tests.Tasks.Validate
{

    public class ValidateGoogleSearchResults
    {

        private ValidateGoogleSearchResults()
        {
        }

        public ValidateGoogleSearchResults SearchResult()
        {

            IWebDriver driver = Driver.GetInstance();
            //Check that the first search result is for Prometheus Group
            //Note that specific selectors should be externalized
            IWebElement result = driver.FindElement(By.CssSelector("H3"));
            Assert.Equal("Prometheus Group", result.Text);
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

        public static ValidateGoogleSearchResults For()
        {
            return new ValidateGoogleSearchResults();
        }
    }
}