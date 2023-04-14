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
    }
}
