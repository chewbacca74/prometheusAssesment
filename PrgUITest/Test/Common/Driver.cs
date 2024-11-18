using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace PrgUITest.Test.Common
{

    public class Driver
    {
        private static IWebDriver? _driver;
        private static readonly object _lock = new object();

        public static IWebDriver GetInstance()
        {
            if (_driver == null)
            {
                lock (_lock)
                {
                    if (_driver == null)
                    {
                        _driver = CreateWebDriver("chrome");
                        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    }
                }
            }
            return _driver;
        }

        private static IWebDriver CreateWebDriver(string browser)
        {
            return new ChromeDriver();
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}