using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace teeze_bot.classes
{
    public class TitoloTask
    {
        public IWebDriver driver;
        public TaskInfo taskinfo;

        public void OpenChrome()
        {
            driver = new ChromeDriver();
            driver.Url = taskinfo.Product;
        }

        public void CreateTitoloTask(TaskInfo taskInfo)
        {
            taskinfo = taskInfo;
        }
    }
}