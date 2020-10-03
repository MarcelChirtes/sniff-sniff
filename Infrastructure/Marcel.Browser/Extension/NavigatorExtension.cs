using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Marcel.Browser.Extension
{
    public static class NavigatorExtension
    {
        public static IWebElement WaitUntilElementExists(this IWebDriver driver, By elementLocator, bool throwUp = false, int timeout = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                return wait.Until(c => c.FindElement(elementLocator));
            }
            catch (Exception ex)
            {
                if (throwUp)
                {
                    throw ex;
                }
                return null;
            }
        }

        public static IWebElement WaitUntilElementVisible(this IWebDriver driver, By elementLocator, bool throwUp = false, int timeout = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                return wait.Until(c =>
                {
                    var x = c.FindElement(elementLocator);
                    return x != null && x.Displayed ? x : null;
                });
            }
            catch (Exception ex)
            {
                if (throwUp)
                {
                    throw ex;
                }
                return null;
            }
        }

        public static IWebElement WaitUntilElementHasValue(this IWebDriver driver, By elementLocator, string value, bool throwUp = false, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                return wait.Until(c =>
                {
                    var x = c.FindElement(elementLocator);
                    return x != null && x.Displayed && x.Text == value ? x : null;
                });
            }
            catch (Exception ex)
            {
                if (throwUp)
                {
                    throw ex;
                }
                return null;
            }
        }

        public static IWebElement WaitUntilElementClickable(this IWebDriver driver, By elementLocator, bool throwUp = false, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                return wait.Until(c =>
                {
                    var x = c.FindElement(elementLocator);
                    return x != null && x.Displayed && x.Enabled ? x : null;
                });
            }
            catch (Exception ex)
            {
                if (throwUp)
                {
                    throw ex;
                }
                return null;
            }
        }

        public static bool ClickAndWaitForPageToLoad(this IWebDriver driver, By elementLocator, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(c =>
            {
                var x = c.FindElement(elementLocator);
                x.Click();
                try
                {
                    // Calling any method forces a staleness check
                    return x == null || !x.Enabled;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }
    }
}