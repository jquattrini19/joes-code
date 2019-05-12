using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace PayIt
{
  public class PayItPresenter : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    string filePath = System.Environment.CurrentDirectory + "\\PayIt.xml";
    ObservableCollection<Account> accountList = new ObservableCollection<Account>();
    public ObservableCollection<Account> AccountList { get { return accountList; } set { accountList = value; OnPropertyChanged("AccountList"); } }
    XElement mainElement;
    public int DaysUntilTimeToPay { get; set; } = 18;

    string status = string.Empty;
    public string Status
    {
      get { return status; }
      set
      {
        status = value;
        OnPropertyChanged("Status");
      }
    }

    bool needsSave = false;
    public bool NeedsSave
    {
      get { return needsSave; }
      set { needsSave = value; }
    }

    public PayItPresenter()
    {
    
      // create xml document if it doesnt already exist
      if (!File.Exists(filePath))
        XmlHelper.CreateXMLFile(filePath, "accounts");  

      mainElement = XmlHelper.GetData(filePath);

      foreach (XElement accountElement in mainElement.Elements())
      {
        int day = Convert.ToInt32(accountElement.Attribute("dayOfMonthDue").Value);
        string statementEndDay = string.Empty;
        if(accountElement.Attribute("statementEndDay") != null)
          statementEndDay = accountElement.Attribute("statementEndDay").Value;
        DateTime lastPaid = Convert.ToDateTime(accountElement.Attribute("lastPaidDueDate").Value);
        string link = accountElement.Attribute("link").Value;
        string note = string.Empty;
        XAttribute attribute = accountElement.Attribute("note");



        if (attribute != null)
          note = attribute.Value;
        Account account = new Account(accountElement.Attribute("name").Value, day, lastPaid, link, note, statementEndDay, DaysUntilTimeToPay);

        accountList.Add(account);
      }



    }

    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }

    public void AddAccount(string name, string dayOfMonthDue, string lastPaidDueDate, string link, string note, string statementEndDay)
    {
      XAttribute attribute;
      XElement accountElement = new XElement("account");

      attribute = new XAttribute("name", name);
      accountElement.Add(attribute);
      attribute = new XAttribute("dayOfMonthDue", dayOfMonthDue);
      accountElement.Add(attribute);
      attribute = new XAttribute("lastPaidDueDate", lastPaidDueDate);
      accountElement.Add(attribute);
      attribute = new XAttribute("link", link);
      accountElement.Add(attribute);
      attribute = new XAttribute("statementEndDay", statementEndDay);
      accountElement.Add(attribute);


      mainElement.Add(accountElement);

      XmlHelper.SaveToXML(filePath, mainElement);
      //foo
      try
      {
        DateTime lastPaidDue = DateTime.Parse(lastPaidDueDate);
        Int32 day = Convert.ToInt32(dayOfMonthDue);
        var newAccount = new Account(name, day, lastPaidDue, link, note, statementEndDay, DaysUntilTimeToPay);
        AccountList.Add(newAccount);
      }
      catch (Exception)
      {
        MessageBox.Show("Error invalid day of month or last paid due date or end day");
        return;
      }
    }
    public void AddAccount(Account account)
    {
      XAttribute attribute;
      XElement accountElement = new XElement("account");

      attribute = new XAttribute("name", account.Name);
      accountElement.Add(attribute);
      attribute = new XAttribute("dayOfMonthDue", account.DayOfMonthDue);
      accountElement.Add(attribute);
      attribute = new XAttribute("lastPaidDueDate", account.LastDueDatePaid);
      accountElement.Add(attribute);
      attribute = new XAttribute("statementEndDay", account.StatementEndDay);
      accountElement.Add(attribute);
      attribute = new XAttribute("link", account.Link);
      accountElement.Add(attribute);
      attribute = new XAttribute("note", account.Note);
      accountElement.Add(attribute);

      mainElement.Add(accountElement);
      accountList.Add(account);
      //XmlHelper.SaveToXML(filePath, mainElement);

      //try
      //{
      //  DateTime lastPaidDue = DateTime.Parse(lastPaidDueDate);
      //  Int32 day = Convert.ToInt32(dayOfMonthDue);
      //  var newAccount = new Account(name, day, lastPaidDue, link, note, statementEndDay);
      //  AccountList.Add(newAccount);
      //}
      //catch (Exception)
      //{
      //  MessageBox.Show("Error invalid day of month or last paid due date or end day");
      //  return;
      //}
    }
    public void GoToLink(Uri link)
    {
      Process.Start(link.AbsoluteUri);
    }

    public string Save()
    {
      var tempAccountList = new List<Account>(accountList);
      accountList.Clear();
      // clear the mainElement
      XmlHelper.CreateXMLFile(filePath, "accounts");
      mainElement = XmlHelper.GetData(filePath);

      foreach (var account in tempAccountList)
        AddAccount(account);
      XmlHelper.SaveToXML(filePath, mainElement);
      return string.Empty;
    }
    public void DeleteAccount(Account account)
    {
      accountList.Remove(account);
      needsSave = true;
    }

    public void LastPaidDateChanged(Account account,DateTime newDate)
    {
      //account.SetTimeToPay(DaysUntilTimeToPay);
      //account.LastDueDatePaid = newDate;
      needsSave = true;
    }
  }
}
