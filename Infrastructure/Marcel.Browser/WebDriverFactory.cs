using Marcel.Common.Extension;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;

namespace Marcel.Browser
{
    public interface IWebDriverFactory
    {
        IWebDriver CreateInstance(BrowserEnum browser, params string[] browserArgs);
    }

    public class WebDriverFactory : IWebDriverFactory
    {
        public IWebDriver CreateInstance(BrowserEnum browser, params string[] browserArgs)
        {
            switch (browser)
            {
                case BrowserEnum.Chrome:
                    {
                        ChromeOptions options = new ChromeOptions();
                        options.AddArguments(browserArgs);
                        return new ChromeDriver(AppContext.BaseDirectory.ToRuntimeOSPath(), options);
                    }
                case BrowserEnum.InternetExploder:
                    {
                        InternetExplorerOptions options = new InternetExplorerOptions();
                        return new InternetExplorerDriver(AppContext.BaseDirectory.ToRuntimeOSPath(), options);
                    }
                case BrowserEnum.Firefox:
                    {
                        FirefoxOptions options = new FirefoxOptions();
                        options.AddArguments(browserArgs);
                        return new FirefoxDriver(AppContext.BaseDirectory.ToRuntimeOSPath(), options);
                    }
            }
            throw new InvalidOperationException();
        }
    }
}