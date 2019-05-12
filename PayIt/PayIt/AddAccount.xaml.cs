using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PayIt
{
  /// <summary>
  /// Interaction logic for AddAccount.xaml
  /// </summary>
  public partial class AddAccount : Window
  {
    public PayItPresenter Presenter { get; set; }

    public AddAccount()
    {
      InitializeComponent();
    }

    public bool Initialize()
    {
      if (DataContext == null)
        return false;

      Presenter = DataContext as PayItPresenter;
      if (Presenter == null)
        return false;

      return true;
    }

    private void LinkClicked(object sender, RoutedEventArgs e)
    {
      //Process.Start(new ProcessStartInfo("Http://msn.com"));

    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      if (textBoxName.Text.Length == 0)
      {
        Presenter.Status = "You must have a name";
        return;
      }

      if (textBoxLastPaidDueDate.Text.Length == 0)
      {
        Presenter.Status = "You must have a last paid due date";
        return;
      }

      int dueDay = -1;
      try
      {
        dueDay = Convert.ToInt32(textBoxDayOfMonthDue.Text);
      }
      catch(Exception)
      {
        Presenter.Status = "Invalid day of month due";
        return;
      }

      string[] split = textBoxLastPaidDueDate.Text.Split('/');
      if (split.Count() < 2 || split.Count() > 3)
      {
        Presenter.Status = "Error, Last paid due date must be in the form mm/dd/yyyy";
        return;
      }

        int month;
        if (int.TryParse(split[0], out month) == false)
        {
          Presenter.Status = "Error on month: Last paid due date must be in the form mm/dd/yyyy";
          return;
        }
        if(month <1 || month >12)
        {
          Presenter.Status = "Error invalid month";
          return;
        }

        int day;
        if (int.TryParse(split[1], out day) == false)
        {
          Presenter.Status = "Error on day: Last paid due date must be in the form mm/dd/yyyy";
          return;
        }
        if (day < 1 || day > 31)
        {
          Presenter.Status = "Error invalid day";
          return;
        }

      int year = DateTime.Now.Year; // if they didn't specify a year, use this year
      if (split.Count() == 3)
      {
        if (int.TryParse(split[2], out year) == false)
        {
          Presenter.Status = "Error on year: Last paid due date must be in the form mm/dd/yyyy";
          return;
        }
        if (year < 1900)
        {
          Presenter.Status = "Error invalid year";
          return;
        }
      }

      DateTime lastPaidDueDate = new DateTime(year, month, day);

      //Presenter.AddAccount(textBoxName.Text, textBoxDayOfMonthDue.Text, textBoxLastPaidDueDate.Text, textBoxLink.Text,textBoxNote.Text, textBoxEndDay.Text);

      Account account = new Account(textBoxName.Text, dueDay, lastPaidDueDate, textBoxLink.Text, textBoxNote.Text, textBoxEndDay.Text,Presenter.DaysUntilTimeToPay);
      Presenter.AddAccount(account);
      Presenter.NeedsSave = true;
      this.Close();
    }
  }
}
