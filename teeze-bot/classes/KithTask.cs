using teeze_bot.Sites;

namespace teeze_bot.classes
{
    public class KithTask
    {
        public TaskInfo taskinfo = new TaskInfo();
        public KithSite kithsite = new KithSite();
        public bool InProgress = false;

        public void StartTask()
        {
            InProgress = true;

            kithsite.LoadPage(taskinfo.ProductLink);
        }

        public void QuitTask()
        {
            if (InProgress)
            {
                InProgress = false;

                kithsite.QuitTask();
            }
        }
    }
}