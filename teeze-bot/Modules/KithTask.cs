using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Windows;

namespace teeze_bot.classes
{
    public class KithTask
    {
        private readonly TaskInfo taskinfo = new TaskInfo();
        private IWebDriver driver;
        private bool InProgress = false;

        public void StartTask()
        {
            try
            {
                InProgress = true;
                driver = new ChromeDriver();
                driver.Url = taskinfo.ProductLink;
                driver.Url = taskinfo.ProductLink;

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