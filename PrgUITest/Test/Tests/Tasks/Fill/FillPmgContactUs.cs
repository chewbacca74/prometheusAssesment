using PrgUITest.Test.Common;
using OpenQA.Selenium;

namespace PrgUITest.Test.Tests.Tasks.Fill
{

    public class FillPmgConactUs
    {

        private string _firstName = "";
        private string _lastName = "";

        private FillPmgConactUs(string firstName, string lastName)
        {
            this._firstName = firstName;
            this._lastName = lastName;
        }

        public void FillFirstLastAndSubmit()
        {
            IWebDriver driver = Driver.GetInstance();
            IWebElement firstName = driver.FindElement(By.Name("firstname"));
            firstName.SendKeys(this._firstName);
            IWebElement lastName = driver.FindElement(By.Name("lastname"));
            lastName.SendKeys(this._lastName);
            //haha accept cookies or scroll otherwise submit is covered up
            driver.FindElement(By.Id("hs-eu-confirmation-button")).Click();
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        }

        public static FillPmgConactUs With(string firstName, string lastName)
        {
            return new FillPmgConactUs(firstName, lastName);
        }
    }
}