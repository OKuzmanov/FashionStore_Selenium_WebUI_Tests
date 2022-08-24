using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using Webdriver_Automation_Tests.Pages;
using Webdriver_Automation_Tests.Tests;

namespace Webdriver_Automation_Tests
{
    public class DressPageTests : BaseTests
    {

        [Test, Order(1)]
        public void Test_MoreDetaialsButton_WorksCorrectly()
        {
            const int prodNum = 1;

            DressesPage Page = new DressesPage(Driver);

            Page.Open();

            string ProductName = Page.GetProductName(prodNum);
            string ProductPrice = Page.GetProductPrice(prodNum);

            Page.ClickMoreProductDetailsView(prodNum);

            DetailsPage detailsPage = new DetailsPage(Driver);

            Assert.AreEqual(ProductName, detailsPage.productTitle);
            Assert.AreEqual(ProductPrice, detailsPage.productPrice);
        }

        [Test, Order(2)]
        public void Test_ProductIsAddedToComparisonSection_OneItem()
        {
            const int prodNum = 1;
            const int expectedCountComparedProds = 1;

            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            dressesPage.AddProductToCompare(prodNum);

            string title = dressesPage.GetProductName(prodNum);
            string price = dressesPage.GetProductPrice(prodNum);

            this.wait.Until(ExpectedConditions.ElementToBeClickable(dressesPage.CompareButton));

            dressesPage.CompareButton.Click();

            ComparisonPage comparisonPage = new ComparisonPage(Driver);

            this.wait.Until(driver => comparisonPage.heading.Equals("Product Comparison".ToUpper()));

            int countComparedProds = comparisonPage.products.Count;

            IWebElement productInComparison = comparisonPage.getProductByTitleAndPrice(title, price);

            Assert.AreEqual(expectedCountComparedProds, countComparedProds);
            Assert.NotNull(productInComparison);
        }

        [Test, Order(3)]
        public void Test_ProductIsAddedToComparisonSection_ThreeItems()
        {
            const int firstProdNum = 1;
            const int secondProdNum = 2;
            const int thirdProdNum = 3;
            const int expectedCountComparedProds = 3;

            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            dressesPage.AddProductToCompare(firstProdNum);
            wait.Until(Driver => dressesPage.comapreBttnVal.Equals("1"));
            dressesPage.AddProductToCompare(secondProdNum);
            wait.Until(Driver => dressesPage.comapreBttnVal.Equals("2"));
            dressesPage.AddProductToCompare(thirdProdNum);

            string firstProdTitle = dressesPage.GetProductName(firstProdNum);
            string firstProdPice = dressesPage.GetProductPrice(firstProdNum);

            string secondProdTitle = dressesPage.GetProductName(secondProdNum);
            string secondProdPice = dressesPage.GetProductPrice(secondProdNum);

            string thridProdTitle = dressesPage.GetProductName(thirdProdNum);
            string thridProdPice = dressesPage.GetProductPrice(thirdProdNum);

            this.wait.Until(ExpectedConditions.ElementToBeClickable(dressesPage.CompareButton));

            dressesPage.CompareButton.Click();

            ComparisonPage comparisonPage = new ComparisonPage(Driver);

            this.wait.Until(driver => comparisonPage.heading.Equals("Product Comparison".ToUpper()));

            int countComparedProds = comparisonPage.products.Count;

            IWebElement firstProdInComparison = comparisonPage.getProductByTitleAndPrice(firstProdTitle, firstProdPice);
            IWebElement secondProdInComparison = comparisonPage.getProductByTitleAndPrice(secondProdTitle, secondProdPice);
            IWebElement thirdProdInComparison = comparisonPage.getProductByTitleAndPrice(thridProdTitle, thridProdPice);

            Assert.AreEqual(expectedCountComparedProds, countComparedProds);
            Assert.NotNull(firstProdInComparison);
            Assert.NotNull(secondProdInComparison);
            Assert.NotNull(thirdProdInComparison);
        }

        [Test, Order(4)]
        public void Test_ProductIsRemovedFromComparisonSection_OneItem()
        {
            const string expectedErrMsg = "There are no products selected for comparison.";
            const int prodNum = 1; 
            const int expectedCountBeforeDel = 1; 

            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            dressesPage.AddProductToCompare(prodNum);

            string title = dressesPage.GetProductName(prodNum);
            string price = dressesPage.GetProductPrice(prodNum);

            this.wait.Until(ExpectedConditions.ElementToBeClickable(dressesPage.CompareButton));

            dressesPage.CompareButton.Click();

            ComparisonPage comparisonPage = new ComparisonPage(Driver);

            this.wait.Until(driver => comparisonPage.heading.Equals("Product Comparison".ToUpper()));

            int countBeforeDel = comparisonPage.products.Count;

            Assert.AreEqual(expectedCountBeforeDel, countBeforeDel);

            bool isDeleted = comparisonPage.DeleteProductByTitleAndPrice(title, price);

            if (!isDeleted)
            {
                Assert.Fail();
            }

            this.Driver.Navigate().Back();

            dressesPage.CompareButton.Click();

            Assert.AreEqual(expectedErrMsg, comparisonPage.errorMsg);
        }

        [Test, Order(5)]
        public void Test_AlertAddingMoreThanThreeItemsToCompare()
        {
            const string expectedAlert = "You cannot add more than 3 product(s) to the product comparison";
            const int firstProdNum = 1;
            const int secondProdNum = 2;
            const int thirdProdNum = 3;
            const int fourthProdNum = 4;

            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            dressesPage.AddProductToCompare(firstProdNum);
            wait.Until(Driver => dressesPage.comapreBttnVal.Equals("1"));
            dressesPage.AddProductToCompare(secondProdNum);
            wait.Until(Driver => dressesPage.comapreBttnVal.Equals("2"));
            dressesPage.AddProductToCompare(thirdProdNum);
            wait.Until(Driver => dressesPage.comapreBttnVal.Equals("3"));
            dressesPage.AddProductToCompare(fourthProdNum);

            this.wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("p.fancybox-error")));

            string actualAlert = dressesPage.alertToManyToCompare.Text;

            Assert.AreEqual(expectedAlert, actualAlert);  
        }

        [Test, Order(6)]
        public void Test_ShoppingCartPageLoadsCorrectly()
        {
            const string expectedTitle = "SHOPPING-CART SUMMARY";
            const string expectedErrMsg = "Your shopping cart is empty.";
            const int expectedCountElems = 0; 

            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            wait.Until(ExpectedConditions.ElementToBeClickable(dressesPage.cartButton));

            dressesPage.cartButton.Click();

            ShoppingCartPage cartPage = new ShoppingCartPage(Driver);

            Assert.AreEqual(expectedTitle, cartPage.title.Text);
            Assert.AreEqual(expectedErrMsg, cartPage.errorEmptyCart.Text);
            Assert.That(expectedCountElems == cartPage.products.Count);
        }

        [Test, Order(7)]
        public void Test_AddProductToCartSuccessfully()
        {
            const int prodNum = 3;
            const int expectedCountItems = 1;

            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            dressesPage.AddProductToCart(prodNum);

            string prodTitle = dressesPage.GetProductName(prodNum);
            string prodPrice = dressesPage.GetProductPrice(prodNum);

            dressesPage.cartButton.Click();

            ShoppingCartPage cartPage = new ShoppingCartPage(Driver);

            System.Collections.Generic.List<IWebElement> products = cartPage.products;

           IWebElement addedToCart = cartPage.GetProductByPriceAndTitle(prodTitle, prodPrice);

            if (addedToCart == null)
            {
                Assert.Fail();
            }
            int countItemsinCart = cartPage.countItemsinCart;
            Assert.That(cartPage.countItemsinCart == expectedCountItems);
        }

        [Test, Order(8)]
        public void Test_DeleteProductFromCartSuccessfully()
        {
            const int prodNum = 1;
            const string cartPageTitle = "SHOPPING-CART SUMMARY\r\nYour shopping cart contains: {0} Product";
            const string errMsg = "Your shopping cart is empty.";

            DressesPage dressesPage = new DressesPage(Driver);
            ShoppingCartPage cartPage = new ShoppingCartPage(Driver);

            dressesPage.Open();

            dressesPage.AddProductToCart(prodNum);

            string prodName = dressesPage.GetProductName(prodNum);
            string prodPrice = dressesPage.GetProductPrice(prodNum);

            dressesPage.cartButton.Click();
            string text = cartPage.title.Text;
            wait.Until(Driver => cartPage.title.Text.Equals(String.Format(cartPageTitle, cartPage.products.Count)));

            bool isDeleted = cartPage.DeleteProductFromCartByNameAndPrice(prodName, prodPrice);

            if (!isDeleted)
            {
                Assert.Fail("The product was not deleted successfully.");
            }

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("#cart_title > span")));

            Assert.AreEqual(errMsg, cartPage.errorEmptyCart.Text);
        }

        [Test, Order(9)]
        public void Test_EmptyShoppingCartFromDressesPage()
        {
            const int firstProdNum = 1;
            const int secondProdNum = 2;
            const int thirdProdNum = 3;

            const string errMsg = "Your shopping cart is empty.";

            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            dressesPage.AddProductToCart(firstProdNum);
            dressesPage.AddProductToCart(secondProdNum);
            dressesPage.AddProductToCart(thirdProdNum);

            dressesPage.DeleteAllProductsFromCart();

            dressesPage.cartButton.Click();

            ShoppingCartPage cartPage = new ShoppingCartPage(Driver);

            Assert.AreEqual(errMsg, cartPage.errorEmptyCart.Text);
        }

        [Test, Order(10)]
        public void Test_AddProductToCartFromQuickView_ChangeQuantitySizeColor()
        {
            DressesPage dressesPage = new DressesPage(Driver);

            dressesPage.Open();

            int randProdNum = new Random().Next(0, dressesPage.ProductsList.Count) + 1;

            string prodName = dressesPage.GetProductName(randProdNum);
            string prodPrice = dressesPage.GetProductPrice(randProdNum);

            dressesPage.ClickQuickView(randProdNum);

            IWebElement divFrame = this.Driver.FindElement(By.CssSelector("#category > div.fancybox-overlay.fancybox-overlay-fixed > div > div > div.fancybox-outer > div > iframe"));
            this.Driver.SwitchTo().Frame(divFrame);

            int randQuantity = new Random().Next(1, 20);
            dressesPage.quickViewQuantity.Clear();
            dressesPage.quickViewQuantity.SendKeys(randQuantity.ToString());

            int randSizeValue = new Random().Next(1, 4);
            dressesPage.qucikViewSizeSelect.SelectByValue(randSizeValue.ToString());
            string sizeTitle = dressesPage.qucikViewSizeSelect.SelectedOption.GetAttribute("title");

            int randColorOption = new Random().Next(0, dressesPage.quickViewColors.Count);
            dressesPage.quickViewColors[randColorOption].Click();
            string colorTitle = dressesPage.quickViewColors[randColorOption].GetAttribute("title");

            dressesPage.qucikViewAddToCartBttn.Click();

            this.Driver.SwitchTo().ParentFrame();

            wait.Until(ExpectedConditions.ElementToBeClickable(dressesPage.closeAlertBtn));

            dressesPage.closeAlertBtn.Click();

            dressesPage.cartButton.Click();

            ShoppingCartPage cartPage = new ShoppingCartPage(Driver);

            wait.Until(driver => cartPage.title);

            int quantityInCart = cartPage.GetProductQuantityByPriceAndTitle(prodName, prodPrice);
            string sizeInCart = cartPage.GetProductSizeByPriceAndTitle(prodName, prodPrice);
            string colorInCart = cartPage.GetProductColorByPriceAndTitle(prodName, prodPrice);

            if (quantityInCart == -1 || sizeInCart == null || colorInCart == null)
            {
                Assert.Fail("Product was not added to cart successfully.");
            }

            Assert.AreEqual(randQuantity, quantityInCart);
            Assert.AreEqual(sizeTitle, sizeInCart);
            Assert.AreEqual(colorTitle, colorInCart);
        }
    }
}