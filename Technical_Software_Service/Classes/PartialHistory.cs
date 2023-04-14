using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Technical_Software_Service
{
    public partial class HistoryEntries
    {
        public SolidColorBrush Color
        {
            get
            {
                Tickets tickets = DataBase.Base.Tickets.FirstOrDefault(x=>x.Id == TicketId);
                TicketStates ticketStates = DataBase.Base.TicketStates.FirstOrDefault(z => z.Id == tickets.TicketStateId);
                var brush = new BrushConverter();                
                if (ticketStates.Id == 1)
                {                    
                    return (SolidColorBrush)(Brush)brush.ConvertFrom("#E7835F");
                }
                else if (ticketStates.Id == 2)
                {                   
                    return (SolidColorBrush)(Brush)brush.ConvertFrom("#C1B4DF");
                }
                else
                {                    
                    return (SolidColorBrush)(Brush)brush.ConvertFrom("#BED7E8");
                }
            }
        }
        public string States
        {
            get
            {
                Tickets tickets = DataBase.Base.Tickets.FirstOrDefault(x => x.Id == TicketId);
                TicketStates ticketStates = DataBase.Base.TicketStates.FirstOrDefault(z => z.Id == tickets.TicketStateId);
                string str = "";
                if (ticketStates.Id == 1)
                {
                    return str = "Данная заявка в ожидании принятия";
                }
                else if (ticketStates.Id == 2)
                {
                    return str = "Данная заявка закрыта";
                }
                else
                {
                    return str = "Данная заявка в впроцессе выполнения";
                }
            }
        }
        public string Indetificatory
        {
            get
            {
                return "Индентификатор:" + Id;
            }
        }
        public string UsersName
        {
            get
            {
                List<HistoryEntries> history = DataBase.Base.HistoryEntries.Where(x => x.UserId == Users.Id).ToList();
                string str = "";
                foreach (HistoryEntries historyEntry in history)
                {
                    str += Users.LastName + " " + Users.FirstName + " " + Users.MiddleName;
                }                
                return "Пользователь: " + str;
            }
        }
        public string TicketTitle
        {
            get
            {
                List<HistoryEntries> history = DataBase.Base.HistoryEntries.Where(x => x.TicketId == Tickets.Id).ToList();
                string str = "";
                foreach (HistoryEntries historyEntry in history)
                {
                    str += Tickets.Title;
                }
                return "Название: " + str;
            }
        }
        public string Description
        {
            get
            {
                return "Описание: " + UpdateDescription;
            }
        }
    }
}
