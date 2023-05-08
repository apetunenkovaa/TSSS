using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical_Software_Service
{
    public partial class DailyTasks
    {
        public string xp
        {
            get
            {
                return "XP: " + XP;
            }
        }
        public string score
        {
            get
            {
                return "Очки: " + Score;
            }
        }

        public string CompletionStatus
        {
            get
            {
                int completedCount = UserDailyTasks.Where(udt => udt.UserId == Id && udt.IsCompleted).Count();
                return "Завершенные задачи: " + completedCount + " из " + TotalCount;
            }
        }


    }
}
