using PrgUITest.Test.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PrgUITest.Test.Tests.Tasks.Navigation
{
    public class NavigateFromGoogleSearchResults
    {

        private string _title;

        public NavigateFromGoogleSearchResults(string title)
        {
            _title = title;
        }

        public void ViaContactLink()
        {
            IWebDriver driver = Driver.GetInstance();
            // IWebElement mainLink = driver.FindElement(By.CssSelector("h3"));            
            driver.FindElement(By.LinkText(_title)).Click();

        }

        public static NavigateFromGoogleSearchResults ToPrometheusGroup(string linkTitle)
        {
            return new NavigateFromGoogleSearchResults(linkTitle);
        }

    }
}