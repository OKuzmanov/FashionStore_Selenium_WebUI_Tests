using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Webdriver_Automation_Tests
{
    public class DressesPage
    {
        private const string PageUrl = "http://automationpractice.com/index.php?id_category=8&controller=category";
        
        private IWebDriver Driver;

        public DressesPage(IWebDriver Driver)
        {
            this.Driver = Driver;
        }

        public IWebElement CompareButton => this.Driver.FindElement(By.CssSelector("#center_column > div.content_sortPagiBar.clearfix > div.top-pagination-content.clearfix > form > button"));

        public IWebElement cartButton => this.Driver.FindElement(By.CssSelector("div.shopping_cart > a"));

        public int cartButtonValue => int.Parse(this.Driver.FindElement(By.CssSelector("span.ajax_cart_quantity.unvisible")).Text);
        
        public string emptyCartButtonValue => this.Driver.FindElement(By.CssSelector("span.ajax_cart_no_product")).Text;

        public string comapreBttnVal => this.Driver.FindElement(By.CssSelector("div.top-pagination-content.clearfix > form > button > span > strong.total-compare-val")).Text;

        public List<IWebElement> ProductsList => this.Driver.FindElements(By.CssSelector("#center_column > ul > li")).ToList();

        public IWebElement alertToManyToCompare => this.Driver.FindElement(By.CssSelector("p.fancybox-error"));

        public IWebElement layerCart => this.Driver.FindElement(By.Id("layer_cart"));

        public IWebElement quickViewQuantity => this.Driver.FindElement(By.CssSelector("form#buy_block > div > div > p#quantity_wanted_p > input#quantity_wanted"));

        public List<IWebElement> quickViewColors => this.Driver.FindElements(By.ClassName("color_pick")).ToList();

        public SelectElement qucikViewSizeSelect => new SelectElement(this.Driver.FindElement(By.Id("group_1")));

        public IWebElement qucikViewAddToCartBttn => this.Driver.FindElement(By.CssSelector("#add_to_cart > button"));

        public IWebElement closeAlertBtn => this.Driver.FindElement(By.CssSelector("#layer_cart > div.clearfix > div.layer_cart_product.col-xs-12.col-md-6 > span"));

        public void Open()
        {
            this.Driver.Navigate().GoToUrl(PageUrl);
        }

        public void AddProductToCompare(int prodNum)
        {
            checkProductExists(prodNum);

            IWebElement AddToCompareBttn = this.ProductsList[prodNum - 1].FindElement(By.CssSelector("#center_column > ul > li > div > div.functional-buttons.clearfix > div.compare > a"));

            IJavaScriptExecutor JsExecutor = (IJavaScriptExecutor)Driver;

            JsExecutor.ExecuteScript("arguments[0].click();", AddToCompareBttn);
        }

        public void AddProductToCart(int prodNum)
        {
            checkProductExists(prodNum);

            IWebElement addToCartBttn = this.ProductsList[prodNum - 1].FindElement(By.CssSelector("#center_column > ul > li > div > div.right-block > div.button-container > a.button.ajax_add_to_cart_button.btn.btn-default"));

            IJavaScriptExecutor JsExecutor = (IJavaScriptExecutor)Driver;

            JsExecutor.ExecuteScript("arguments[0].click()", addToCartBttn);

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#layer_cart > div.clearfix > div.layer_cart_product.col-xs-12.col-md-6 > span")));

            IWebElement CloseAlertBtn = this.Driver.FindElement(By.CssSelector("#layer_cart > div.clearfix > div.layer_cart_product.col-xs-12.col-md-6 > span"));

            CloseAlertBtn.Click();

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("#layer_cart > div.clearfix > div.layer_cart_product.col-xs-12.col-md-6 > span")));

        }

        public void ClickMoreProductDetailsView(int prodNum)
        {
            checkProductExists(prodNum);

            IWebElement MoreDetailsView = this.ProductsList[prodNum - 1].FindElement(By.CssSelector("#center_column > ul > li > div > div.right-block > div.button-container > a.button.lnk_view.btn.btn-default"));

            IJavaScriptExecutor JsExecutor = (IJavaScriptExecutor)Driver;

            JsExecutor.ExecuteScript("arguments[0].click()", MoreDetailsView);
        }

        public void ClickQuickView(int prodNum)
        {
            checkProductExists(prodNum);

            IWebElement quickView = this.ProductsList[prodNum - 1].FindElement(By.CssSelector("a.quick-view"));

            IJavaScriptExecutor JsExecutor = (IJavaScriptExecutor)Driver;

            JsExecutor.ExecuteScript("arguments[0].click()", quickView);

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.fancybox-item.fancybox-close")));
        }


        public String GetProductName(int prodNum)
        {
            checkProductExists(prodNum);

            Actions actions = new Actions(this.Driver);

            IWebElement product = this.ProductsList[prodNum - 1];

            actions.MoveToElement(product);

            actions.Build().Perform();

            return this.ProductsList[prodNum - 1].FindElement(By.ClassName("product-name")).Text;

        }

        public String GetProductPrice(int prodNum)
        {
            checkProductExists(prodNum);

            Actions actions = new Actions(this.Driver);

            IWebElement product = this.ProductsList[prodNum - 1];

            actions.MoveToElement(product);

            actions.Build().Perform();

            return this.ProductsList[prodNum - 1].FindElement(By.ClassName("price")).Text;
        }

        public void DeleteAllProductsFromCart()
        {
           Actions actions = new Actions(this.Driver);

           actions.MoveToElement(this.cartButton);

           actions.Build().Perform();

            List<IWebElement> products = this.Driver.FindElements(By.CssSelector("dl > dt")).ToList();

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            int subtractor = 1;
            foreach (var product in products)
            {
                product.FindElement(By.CssSelector(".ajax_cart_block_remove_link")).Click();

                if (subtractor < products.Count)
                {
                    wait.Until(Driver => cartButtonValue.Equals(products.Count - subtractor));
                    subtractor++;
                } else
                {
                    wait.Until(Driver => emptyCartButtonValue);
                }
            }
        }

        private void checkProductExists(int prodNum)
        {
            if (prodNum <= 0 || prodNum > this.ProductsList.Count)
            {
                return;
            }
        }
    }
}
