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
                return "Опыт: " + XP;
            }
        }
        public string score
        {
            get
            {
                return "Очки: " + Score;
            }
        }

        public string totalCount
        {
            get
            {
                return "Всего: " + TotalCount;
            }
        }
    }
}
