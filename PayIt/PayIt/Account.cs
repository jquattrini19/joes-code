using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIt
{
    public class Account
    {
    int daysUntilTimeToPay;

    string name = string.Empty;
    public string Name { get { return name; } set { name = value; } }
    //foo

    int dayOfMonthDue = 0;
    public int DayOfMonthDue { get { return dayOfMonthDue; } set { dayOfMonthDue = value; } }

    // let this be a string so it can be empty
    public string StatementEndDay { get; set; }
    
    DateTime nextDue = DateTime.MaxValue;
    public DateTime NextDue { get { return nextDue; } set { nextDue = value; SetTimeToPay(daysUntilTimeToPay); } }

    DateTime lastDueDatePaid = DateTime.MaxValue;
    public DateTime LastDueDatePaid { get { return lastDueDatePaid; }
      set { lastDueDatePaid = value; NextDue = lastDueDatePaid.AddMonths(1); } }

    public bool TimeToPay { get; set; } = false;

    public string Link { get; set; }
    public string Note { get; set; }
    public bool Check { get; set; }

    public Account(string accountName, int monthlyDueDate, DateTime lastDueDatePaid,string link,string note, string statementEndDay, int daysUntilTimeToPay)
    {
      this.daysUntilTimeToPay = daysUntilTimeToPay;
      name = accountName;
      dayOfMonthDue = monthlyDueDate;
      StatementEndDay = statementEndDay;
      LastDueDatePaid = lastDueDatePaid;
      //nextDue = lastDueDatePaid.AddMonths(1);
      Link = link;
      Note = note;
      SetTimeToPay(daysUntilTimeToPay);
      Check = false;
    }

    public void SetTimeToPay(int daysUntilTimeToPay)
    {
      int daysLeft = (nextDue - DateTime.Now).Days;
      Debug.WriteLine(name + ", " + daysLeft + " days left to pay.");

      if (daysLeft <= daysUntilTimeToPay)
        TimeToPay = true;
      else
        TimeToPay = false;
    }

  }
}
