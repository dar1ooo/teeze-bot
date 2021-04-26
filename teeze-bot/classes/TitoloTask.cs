using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace teeze_bot.classes
{
    public class TitoloTask
    {
        public IWebDriver driver;
        public TaskInfo taskinfo;
        public bool InProgress = false;

        public void StartTask()
        {
            driver = new ChromeDriver();
            driver.Url = taskinfo.Product;
            InProgress = true;
        }

        public void QuitTask()
        {
            if (InProgress)
            {
                driver.Quit();
                InProgress = false;
            }
        }
    }
}