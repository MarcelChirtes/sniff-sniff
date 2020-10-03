using Marcel.Browser;
using Marcel.Browser.Extension;
using Marcel.DbModels.Model;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marcel.Pure.Co.UK
{
    public interface INavigationService
    {
        List<Dish> Sniff(By clickSelector);

        List<Dish> Sniff(By clickSelector, IWebDriver browser);

        List<Dish> Sniff(string url);

        List<Dish> Sniff(string url, IWebDriver browser);
    }

    public class NavigationService : INavigationService
    {
        private readonly ILogger<NavigationService> logger;
        private readonly IWebDriverFactory webDriverFactory;
        private readonly IMenuPageService menuPageService;

        public NavigationService(IWebDriverFactory webDriverFactory,
            ILogger<NavigationService> logger,
            IMenuPageService menuPageService)
        {
            this.logger = logger;
            this.webDriverFactory = webDriverFactory ?? throw new ArgumentNullException(nameof(webDriverFactory));
            this.menuPageService = menuPageService ?? throw new ArgumentNullException(nameof(menuPageService));
        }

        public List<Dish> Sniff(By clickSelector)
        {
            using (var browser = webDriverFactory.CreateInstance(BrowserEnum.Chrome, "headless"))
            {
                return Sniff(clickSelector, browser);
            }
        }

        public List<Dish> Sniff(By clickSelector, IWebDriver browser)
        {
            browser.FindElement(clickSelector).Click();
            List<Dish> dishes = Sniff(browser);
            return dishes;
        }

        public List<Dish> Sniff(string url)
        {
            // "headless"
            using (var browser = webDriverFactory.CreateInstance(BrowserEnum.Chrome))
            {
                return Sniff(url, browser);
            }
        }

        public List<Dish> Sniff(string url, IWebDriver browser)
        {
            browser.Navigate().GoToUrl(url);
            return Sniff(browser);
        }

        private List<Dish> Sniff(IWebDriver browser)
        {
            List<Dish> results = new List<Dish>();
            browser.WaitUntilElementVisible(By.CssSelector("ul.submenu"));
            // menus
            var menus = browser.FindElements(By.CssSelector("ul.submenu a")).Select(x => new { Href = x.GetAttribute("href") ?? "", x.Text }).ToList();
            foreach (var menu in menus.Where(x => !string.IsNullOrEmpty(x.Href)))
            {
                var items = menuPageService.Sniff(menu.Href, browser);
                results.AddRange(items);
            }
            return results;
        }
    }
}