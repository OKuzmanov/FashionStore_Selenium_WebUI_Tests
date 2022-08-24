using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdriver_Automation_Tests.Tests
{
    public class BaseTests
    {
        protected IWebDriver Driver;
        protected WebDriverWait wait;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            this.Driver = new ChromeDriver();
            this.wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            this.Driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.Driver.Quit();
        }

        [SetUp]
        public void deleteAllCookiesSetup()
        {
            this.Driver.Manage().Cookies.DeleteAllCookies();
        }
    }
}
