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
    public interface IMenuPageService
    {
        List<Dish> Sniff(By clickSelector);

        List<Dish> Sniff(By clickSelector, IWebDriver browser);

        List<Dish> Sniff(string url);

        List<Dish> Sniff(string url, IWebDriver browser);
    }

    public class MenuPageService : IMenuPageService
    {
        private readonly ILogger<MenuPageService> logger;
        private readonly IWebDriverFactory webDriverFactory;
        private readonly IDishPageService dishPageService;

        public MenuPageService(IWebDriverFactory webDriverFactory,
            ILogger<MenuPageService> logger,
            IDishPageService dishPageService)
        {
            this.logger = logger;
            this.webDriverFactory = webDriverFactory ?? throw new ArgumentNullException(nameof(webDriverFactory));
            this.dishPageService = dishPageService ?? throw new ArgumentNullException(nameof(dishPageService));
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
            using (var browser = webDriverFactory.CreateInstance(BrowserEnum.Chrome, "headless"))
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
            List<Dish> menu = new List<Dish>();
            string menuTitle = browser.WaitUntilElementVisible(By.CssSelector("ul.submenu a.active"))?.Text;
            // in headless no visiblity... 
            if (string.IsNullOrEmpty(menuTitle))
            {
                menuTitle = browser.Title.Replace(" - Pure", "");
            }
            string menuDescription = browser.FindElement(By.CssSelector("header.menu-header p"))?.Text;
            // menu sections
            var sections = browser.FindElements(By.CssSelector("section.container h4.menu-title a")).Select(x => new { Href = x.GetAttribute("href") ?? "", x.Text }).ToList();

            var sectionsSelection = sections.Select(selection =>
            {
                string expanderId = selection.Href.Contains("#") ? selection.Href.Split("#").LastOrDefault() : null;
                return new
                {
                    Text = selection.Text,
                    CardsUrls = browser.FindElements(By.CssSelector($"div#{expanderId} a")).Select(x => x.GetAttribute("href")).Where(x => !string.IsNullOrEmpty(x)).ToList()
                };
            }).ToList();

            foreach (var card in sectionsSelection.Where(x => x.CardsUrls.Count > 0))
            {
                foreach (var dishUrl in card.CardsUrls)
                {
                    // navigation;
                    var dish = dishPageService.Sniff(dishUrl, browser);
                    dish.MenuSectionTitle = card.Text;
                    dish.MenuTitle = menuTitle;
                    dish.MenuDescription = menuDescription;
                    menu.Add(dish);
                }
            }
            return menu;
        }
    }
}