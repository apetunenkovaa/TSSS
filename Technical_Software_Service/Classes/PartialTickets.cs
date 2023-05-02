using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical_Software_Service
{
    public partial class Tickets
    {
        public string Requesters
        {
            get
            {
                return "Заказчик: " + Requester;
            }
        }
        public string Date
        {
            get
            {
                return "Дата открытия: " + string.Format("{0:dd MMMM yyyy}", OpeningDate);
            }
        }
        public string Category
        {
            get
            {
                return "Категория: " + Categories.Kind;
            }
        }
        public string States
        {
            get
            {
                return "Состояние: " + TicketStates.Kind;
            }
        }
        public string Importance
        {
            get
            {
                return "Важность: " + ImportanceTypes.Kind;
            }
        }

        public string Score
            {
                get
                {
                    int score = 0;
                    if (ImportanceTypes != null)
                    {
                        switch (ImportanceTypes.Id)
                        {
                            case 1:
                                score = 10;
                                break;
                            case 2:
                                score = 15;
                                break;
                            case 3:
                                score = 5;
                                break;
                            case 4:
                                score = 1;
                                break;
                        }
                    }

                    return "Очков: " + score;
                }
            }
        }
}
