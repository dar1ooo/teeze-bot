using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Windows;

namespace teeze_bot.classes
{
    public class KithTask
    {
        public TaskInfo taskinfo = new TaskInfo();
        public IWebDriver driver;
        public bool InProgress = false;

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

                options.AddExcludedArgument("enable-automation");
                driver = new ChromeDriver(options);
                driver.Url = taskinfo.ProductLink;
                driver.Url = taskinfo.ProductLink;

                driver.FindElement(By.XPath("//*[@id='usercentrics - root']//div/div[2]/div/footer/div/div/div[2]/div/button[2]")).Click();

                var sizes = driver.FindElement(By.Id("SingleOptionSelector-0"));
                //create select element object
                var selectElement = new SelectElement(sizes);

                //select by value
                selectElement.SelectByValue(taskinfo.ShoeSizes.ToString());
                driver.FindElement(By.XPath("//*[@id='shopify-section-product']/section/div[2]/form/button")).Click();
            }
            catch
            {
                MessageBox.Show("An error has occured. Please end the Task");
            }
        }

        public void QuitTask()
        {
            if (InProgress)
            {
                InProgress = false;

                driver.Quit();
            }
        }
    }
}