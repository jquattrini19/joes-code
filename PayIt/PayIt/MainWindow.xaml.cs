using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

// TODO
// right click link to edit it
// if due date is less than 10 days make it red
// if due date is less than 10 days send text or email
// backup file once before changing it

namespace PayIt
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public PayItPresenter Presenter { get; set; }

    public MainWindow()
    {
      InitializeComponent();
      Presenter = new PayItPresenter();
      DataContext = Presenter;
    }

    private void AddAccountClick(object sender, RoutedEventArgs e)
    {
      AddAccount addAccountWindow = new AddAccount();
      addAccountWindow.DataContext = Presenter;
      addAccountWindow.Initialize();
      addAccountWindow.ShowDialog();
    }

    private void LinkClicked(object sender, RequestNavigateEventArgs e)
    {
      Presenter.GoToLink(e.Uri);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      mainListview.Items.SortDescriptions.Add(new SortDescription("NextDue", ListSortDirection.Ascending));
    }

    private void LastPaidDueDateChanged(object sender, SelectionChangedEventArgs e)
    {
      //var account = mainListview.SelectedItem as Account;
      var picker = sender as DatePicker;
      if (picker == null)
        return;
      var item = picker.Tag as ListViewItem;
      if (item == null)
        return;
      var account = item.DataContext as Account;
      if (account == null)
        return;
      Presenter.LastPaidDateChanged(account, picker.SelectedDate.GetValueOrDefault());
      refreshListView();
    }
    //foo
    void refreshListView()
    {
      mainListview.ItemsSource = null;
      mainListview.ItemsSource = Presenter.AccountList;
    }
    private void OnClosing(object sender, CancelEventArgs e)
    {
      if (Presenter.NeedsSave)
      {
        AddButton.Focus(); // this is needed if the user edited a textbox and didnt leave the textbox, the changed text is not automatically saved until the focus leaves the textbox
        string error = Presenter.Save();
        if (error.Length > 0)
        {
          var result = MessageBox.Show("Error, do you want to close anyway?" + Environment.NewLine + Environment.NewLine + error, "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No);
          if (result == MessageBoxResult.No)
            e.Cancel = true;
        }
      }
    }

    private void RemoveItemClick(object sender, RoutedEventArgs e)
    {
      Account account = mainListview.SelectedItem as Account;
      if (account == null)
      {
        MessageBox.Show("Error getting the account information");
        return;
      }
      var answer = MessageBox.Show("Are you sure you want do delete " + account.Name, "Are you sure", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
      if (answer == MessageBoxResult.No)
        return;
      Presenter.DeleteAccount(account);
    }

    private void TextBoxChanged(object sender, TextChangedEventArgs e)
    {
      Presenter.NeedsSave = true;
    }
  }
}