using Marcel.Browser;
using Marcel.Browser.Extension;
using Marcel.DbModels.Model;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Marcel.Pure.Co.UK
{
    public interface IDishPageService
    {
        Dish Sniff(By clickSelector);

        Dish Sniff(By clickSelector, IWebDriver browser);

        Dish Sniff(string url);

        Dish Sniff(string url, IWebDriver browser);
    }

    public class DishPageService : IDishPageService
    {
        private readonly ILogger<DishPageService> logger;
        private readonly IWebDriverFactory webDriverFactory;

        public DishPageService(IWebDriverFactory webDriverFactory,
            ILogger<DishPageService> logger)
        {
            this.logger = logger;
            this.webDriverFactory = webDriverFactory ?? throw new ArgumentNullException(nameof(webDriverFactory));
        }

        public Dish Sniff(By clickSelector)
        {
            using (var browser = webDriverFactory.CreateInstance(BrowserEnum.Chrome, "headless"))
            {
                return Sniff(clickSelector, browser);
            }
        }

        public Dish Sniff(By clickSelector, IWebDriver browser)
        {
            //string url = browser.Url;
            browser.FindElement(clickSelector).Click();
            Dish dish = Sniff(browser);
            // browser.Navigate().GoToUrl(url);
            return dish;
        }

        public Dish Sniff(string url)
        {
            using (var browser = webDriverFactory.CreateInstance(BrowserEnum.Chrome, "headless"))
            {
                return Sniff(url, browser);
            }
        }

        public Dish Sniff(string url, IWebDriver browser)
        {
            browser.Navigate().GoToUrl(url);
            return Sniff(browser);
        }

        private Dish Sniff(IWebDriver browser)
        {
            Dish dish = new Dish();
            dish.Url = browser.Url;
            dish.DishName = browser.WaitUntilElementVisible(By.CssSelector("h2"))?.Text;
            dish.DishDescription = browser.FindElements(By.CssSelector(".menu-item-details div p"))
                    .FirstOrDefault()
                    ?.Text;
            logger.LogInformation($"Processing done for: {dish.Url}");
            return dish;
        }
    }
}