using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdriver_Automation_Tests.Pages
{
    public class ComparisonPage
    {
        public IWebDriver driver;

        public ComparisonPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string heading => this.driver.FindElement(By.ClassName("page-heading")).Text;

        public string errorMsg => this.driver.FindElement(By.CssSelector("#center_column > p")).Text;

        public List<IWebElement> products => this.driver.FindElements(By.CssSelector("td.ajax_block_product.comparison_infos.product-block")).ToList();

        public IWebElement getProductByTitleAndPrice(string title, string price)
        {
            foreach(var product in products)
            {
                string prodTitle = product.FindElement(By.ClassName("product-name")).Text;
                string prodPrice = product.FindElement(By.ClassName("price")).Text;

                if (prodTitle == title && prodPrice == price)
                {
                    return product;
                }
            }

            return null;
        }

        public bool DeleteProductByTitleAndPrice(string title, string price)
        {
            foreach(var product in products)
            {
                string prodTitle = product.FindElement(By.ClassName("product-name")).Text;
                string prodPrice = product.FindElement(By.ClassName("price")).Text;

                if (prodTitle == title && prodPrice == price)
                {
                    product.FindElement(By.ClassName("icon-trash")).Click();
                    return true;
                }
            }

            return false;
        }
    }
}
