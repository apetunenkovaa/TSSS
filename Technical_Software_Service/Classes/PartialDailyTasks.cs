using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical_Software_Service
{
    public partial class DailyTasks
    {
        Users users = new Users();
        public string CompletionText
        {
            get
            {
                if (users.CompletedCountTickets == TotalCount)
                {
                    return "Задание выполнено!";
                }
                else
                {
                    return string.Format("{0} из {1}", users.CompletedCountTickets, TotalCount);
                }
            }
        }

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

        public string totalCount
        {
            get
            {
                return "Всего : " + TotalCount;
            }
        }
    }
}
