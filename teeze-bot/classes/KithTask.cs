using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace teeze_bot.classes
{
    public class KithTask
    {
        public TaskInfo taskinfo = new TaskInfo();
        public IWebDriver driver = new ChromeDriver();
        public bool InProgress = false;

        public void StartTask()
        {
            InProgress = true;

            driver.Url = taskinfo.ProductLink;
        }

        public void QuitTask()
        {
            if (InProgress)
            {
                InProgress = false;
            }
        }
    }
}