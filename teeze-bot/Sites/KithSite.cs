using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace teeze_bot.Sites
{
    public class KithSite
    {
        public IWebDriver driver;

        public void LoadPage(string url)
        {
            driver = new ChromeDriver();
            driver.Url = url;
        }

        public void QuitTask()
        {
            driver.Quit();
        }
    }
}