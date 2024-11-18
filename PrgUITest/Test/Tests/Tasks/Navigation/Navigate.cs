using PrgUITest.Test.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PrgUITest.Test.Tests.Tasks.Navigation
{

    public class Navigate
    {

        public void Google()
        {
            IWebDriver driver = Driver.GetInstance();
            driver.Navigate().GoToUrl("https://www.google.com");
        }

        private Navigate() { }

        public static Navigate To()
        {
            return new Navigate();
        }
    }

}