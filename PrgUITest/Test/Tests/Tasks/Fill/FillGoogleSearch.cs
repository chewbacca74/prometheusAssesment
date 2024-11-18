using PrgUITest.Test.Common;
using OpenQA.Selenium;

namespace PrgUITest.Test.Tests.Tasks.Fill
{

    public class FillGoogleSearch
    {

        private string _query = "";

        private FillGoogleSearch(string query)
        {
            _query = query;
        }

        public void AndSubmit()
        {
            IWebDriver driver = Driver.GetInstance();
            IWebElement searchBox = driver.FindElement(By.Name("q"));
            searchBox.SendKeys(_query);
            searchBox.Submit();
        }

        public static FillGoogleSearch With(string query)
        {
            return new FillGoogleSearch(query);
        }
    }
}