using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using teeze_bot.classes;

namespace teeze_bot.Modules
{
    public class DigitecTask
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

                driver.FindElement(By.Id("addToCartButton")).Click();
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
