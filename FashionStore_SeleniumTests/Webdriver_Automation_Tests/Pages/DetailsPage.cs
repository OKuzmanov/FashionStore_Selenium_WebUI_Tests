using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdriver_Automation_Tests.Pages
{
    public class DetailsPage
    {
        private IWebDriver Driver;

        public DetailsPage(IWebDriver Driver)
        {
            this.Driver = Driver;
        }
        public string productTitle => this.Driver.FindElement(By.CssSelector("#center_column > div > div > div.pb-center-column.col-xs-12.col-sm-4 > h1")).Text;
        public string productPrice => this.Driver.FindElement(By.Id("our_price_display")).Text;
    }
}
