using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace memoryGame
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    bool isflipped = false;
    Button firstButton = null;
    string player1 = string.Empty;
    string player2 = string.Empty;
    string whosTurn = string.Empty;
    int sleepTime = 2000;
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Button button = sender as Button;
      if (button == null)
        return;

      Image image = button.Content as Image;

      if (image.Visibility == Visibility.Visible) // if user clicks already visible card
        return;

      image.Visibility = Visibility.Visible;
      if (image == null)
      {
        MessageBox.Show("Error: no image");
        image.Visibility = Visibility.Hidden;
        return;
      }

      if (!isflipped) // if this is the first flipped card
      {
        firstButton = button;
        isflipped = true;
        return;
      }

      // this is the second card flipped
      Image firstImage = firstButton.Content as Image;


      if (image.Name.TrimEnd('2') == firstImage.Name.TrimEnd('2')) // if the cards match
      {
        // TODO: award player 1 point

      }
      else
      {
        // this will wait untill UI is drawn (DispatcherPriority.Render) before continuing
        Dispatcher.Invoke(new Action(() => { Thread.Sleep(100); }), System.Windows.Threading.DispatcherPriority.Render, null);
        Thread.Sleep(sleepTime);
        firstImage.Visibility = Visibility.Hidden;
        image.Visibility = Visibility.Hidden;

          firstButton = null;

      }

      isflipped = false;
    }
  }
}
