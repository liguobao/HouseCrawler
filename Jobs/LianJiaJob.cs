using Pomelo.AspNetCore.TimedJob;

namespace HouseCrawler.Jobs
{
    public class LianJiaJob : Job
    {
       
        public LianJiaJob()
        {
           
        }
        
        [Invoke(Begin = "2018-05-20 00:00", Interval = 1000 * 3600, SkipWhileExecuting = true)]
        public void Run()
        {

        }
    }
}
