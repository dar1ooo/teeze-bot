using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace teeze_bot.classes
{
    public class TaskCommand
    {
        public IWebDriver driver;

        public void OpenChrome()
        {
            driver = new ChromeDriver();
            driver.Url = "https://www.zalando.ch/herrenschuhe-sneaker/";
        }
    }
}