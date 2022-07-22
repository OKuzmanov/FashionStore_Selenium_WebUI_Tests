using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdriver_Automation_Tests.Pages
{
    public class ShoppingCartPage
    {
        private IWebDriver driver;

        public ShoppingCartPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement title => this.driver.FindElement(By.Id("cart_title"));

        private string itemsInCart => this.driver.FindElement(By.CssSelector("h1 > span.heading-counter > span")).Text;

        public int countItemsinCart => int.Parse(itemsInCart.Substring(0, itemsInCart.IndexOf(" ")));

        public List<IWebElement> products => this.driver.FindElements(By.CssSelector("tr.cart_item")).ToList();

        public IWebElement errorEmptyCart => this.driver.FindElement(By.CssSelector("#center_column > p"));

        public IWebElement GetProductByPriceAndTitle(string title, string price)
        {
            foreach(var product in products)
            {
                string prodTitle = product.FindElement(By.CssSelector("td.cart_description > p > a")).Text;
                string prodPrice = product.FindElement(By.CssSelector("span.price > span.price")).Text;

                if(title == prodTitle && price == prodPrice)
                {
                    return product;
                }
            }

            return null;
        }

        public int GetProductQuantityByPriceAndTitle(string title, string price)
        {
            foreach (var product in products)
            {
                string prodTitle = product.FindElement(By.CssSelector("td.cart_description > p > a")).Text;
                string prodPrice = product.FindElement(By.CssSelector("span.price > span.price")).Text;

                if (title == prodTitle && price == prodPrice)
                {
                    string quntityValue = product.FindElement(By.CssSelector("td.cart_quantity.text-center > input.cart_quantity_input.form-control.grey")).GetAttribute("value");
                    return int.Parse(quntityValue);
                }
            }

            return -1;
        }

        public string GetProductSizeByPriceAndTitle(string title, string price)
        {
            foreach (var product in products)
            {
                string prodTitle = product.FindElement(By.CssSelector("td.cart_description > p > a")).Text;
                string prodPrice = product.FindElement(By.CssSelector("span.price > span.price")).Text;

                if (title == prodTitle && price == prodPrice)
                {
                    string sizeAndColor = product.FindElement(By.CssSelector("td.cart_description > small:nth-child(3) > a")).Text;
                    return sizeAndColor.Substring(sizeAndColor.LastIndexOf(" ") + 1);
                }
            }

            return null;
        }

        public string GetProductColorByPriceAndTitle(string title, string price)
        {
            foreach (var product in products)
            {
                string prodTitle = product.FindElement(By.CssSelector("td.cart_description > p > a")).Text;
                string prodPrice = product.FindElement(By.CssSelector("span.price > span.price")).Text;

                if (title == prodTitle && price == prodPrice)
                {
                    string sizeAndColor = product.FindElement(By.CssSelector("td.cart_description > small:nth-child(3) > a")).Text;
                    string substring = sizeAndColor.Substring("Color : ".Length);
                    return substring.Remove(substring.IndexOf(","));
                }
            }

            return null;
        }

        public bool DeleteProductFromCartByNameAndPrice(string name, string price)
        {
            foreach (var product in products)
            {
                string prodTitle = product.FindElement(By.CssSelector("td.cart_description > p > a")).Text;
                string prodPrice = product.FindElement(By.CssSelector("span.price > span.price")).Text;

                if (name == prodTitle && price == prodPrice)
                {
                    product.FindElement(By.CssSelector("tr > td > div > a.cart_quantity_delete")).Click();
                    return true;
                }
            }

            return false;
        }
    }
}
