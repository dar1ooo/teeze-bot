using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Windows;
using teeze_bot.classes;

namespace teeze_bot.Modules
{
    public class DigitecTask
    {
        public TaskInfo taskinfo = new TaskInfo();
        public IWebDriver driver;
        public bool InProgress = false;
        public int delay = 2;

        public void StartTask()
        {
            try
            {
                InProgress = true;
                ChromeOptions options = new ChromeOptions();

                //Proxies
                //Proxy proxy = new Proxy();
                //proxy.Kind = ProxyKind.Manual;
                //proxy.IsAutoDetect = false;
                //proxy.SslProxy = "<HOST:PORT>";
                //options.Proxy = proxy;
                //options.AddArgument("ignore-certificate-errors");

                options.AddArgument("--disable-blink-features=AutomationControlled");
                options.AddArgument("window-size=1280,800");
                options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");

                driver = new ChromeDriver(options);
                driver.Url = taskinfo.ProductLink;
                driver.Url = taskinfo.ProductLink;

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(delay);

                driver.FindElement(By.Id("addToCartButton")).Click();

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(delay + 5);

                if (driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div[3]/div[2]/button")).Displayed)
                {
                    driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div[3]/div[2]/button")).Click();
                }

                if (driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div[3]/div[2]/button")).Displayed)
                {
                    driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div[3]/div[2]/button")).Click();
                }

                driver.FindElement(By.XPath("/html/body/div[1]/div/header/div[4]/div[4]/button")).Click();

                driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div[1]/aside/div[1]/div/div[2]/div/div[4]/a")).Click();
            }
            catch
            {
                MessageBox.Show("An error has occured. I know my code is bad :) sorry" + "\n" + "Please restart and end the Task");
            }
        }

        public void QuitTask()
        {
            if (InProgress)
            {
                InProgress = false;
                if (driver != null)
                {
                    driver.Quit();
                }
            }
        }
    }
}