using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.web.Models
{
    public class TicketStatus
    {
        public List<TicketDetails> TicketsDetailsList;

        public TicketStatus()
        {
            TicketsDetailsList = new List<TicketDetails>();
        }
    }
}