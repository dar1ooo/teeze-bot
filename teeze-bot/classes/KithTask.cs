﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace teeze_bot.classes
{
    public class KithTask
    {
        public TaskInfo taskinfo = new TaskInfo();
        public IWebDriver driver;
        public bool InProgress = false;

        public void StartTask()
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