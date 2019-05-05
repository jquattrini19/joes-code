using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace mastermind
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    static string[] masterCode = new string[4];
    List<string> masterCode_copy = new List<string>();

    List<List<TextBox>> guessTextBoxes = new List<List<TextBox>>();
    List<List<TextBox>> answerTextBoxes = new List<List<TextBox>>();


    public MainWindow()
    {
      InitializeComponent();
      guessTextBoxes.Add(new List<TextBox>() { row1_box1, row1_box2, row1_box3, row1_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row2_box1, row2_box2, row2_box3, row2_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row3_box1, row3_box2, row3_box3, row3_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row4_box1, row4_box2, row4_box3, row4_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row5_box1, row5_box2, row5_box3, row5_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row6_box1, row6_box2, row6_box3, row6_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row7_box1, row7_box2, row7_box3, row7_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row8_box1, row8_box2, row8_box3, row8_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row9_box1, row9_box2, row9_box3, row9_box4 });
      guessTextBoxes.Add(new List<TextBox>() { row10_box1, row10_box2, row10_box3, row10_box4 });

      answerTextBoxes.Add(new List<TextBox>() { row1_answerBox1, row1_answerBox2, row1_answerBox3, row1_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row2_answerBox1, row2_answerBox2, row2_answerBox3, row2_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row3_answerBox1, row3_answerBox2, row3_answerBox3, row3_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row4_answerBox1, row4_answerBox2, row4_answerBox3, row4_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row5_answerBox1, row5_answerBox2, row5_answerBox3, row5_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row6_answerBox1, row6_answerBox2, row6_answerBox3, row6_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row7_answerBox1, row7_answerBox2, row7_answerBox3, row7_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row8_answerBox1, row8_answerBox2, row8_answerBox3, row8_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row9_answerBox1, row9_answerBox2, row9_answerBox3, row9_answerBox4 });
      answerTextBoxes.Add(new List<TextBox>() { row10_answerBox1, row10_answerBox2, row10_answerBox3, row10_answerBox4 });
    }

    private void newGameButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: submit buttons

      masterCode = generateMasterCode();
      HideMasterTextBoxes();

      SetColors(masterCode[0], master_box1);
      SetColors(masterCode[1], master_box2);
      SetColors(masterCode[2], master_box3);
      SetColors(masterCode[3], master_box4);

      ClearTextBoxes();
    }

    private void SetColors(string color, TextBox textBox)
    {
      if (color == "red")
        textBox.Background = Brushes.Red;
      if (color == "blue")
        textBox.Background = Brushes.Blue;
      if (color == "green")
        textBox.Background = Brushes.Green;
      if (color == "yellow")
        textBox.Background = Brushes.Yellow;
      if (color == "orange")
        textBox.Background = Brushes.Orange;
      if (color == "purple")
        textBox.Background = Brushes.Purple;
    }

    private void ClearTextBoxes()
    {
      foreach (var child in stackPanel_main.Children)
      {
        var stackPanel = child as StackPanel;
        if (stackPanel != null)
        {
          foreach (var child2 in stackPanel.Children)
          {
            var textBox = child2 as TextBox;
            if (textBox != null)
            {
              textBox.Text = string.Empty;
              if (textBox.Tag != null && textBox.Tag.ToString() == "answerBox")
              {
                textBox.Background = Brushes.Transparent;
                textBox.Visibility = Visibility.Visible;
              }
            }
          }
        }
      }
    }

    private void showButton_Click(object sender, RoutedEventArgs e)
    {
      master_box1.Visibility = Visibility.Visible;
      master_box2.Visibility = Visibility.Visible;
      master_box3.Visibility = Visibility.Visible;
      master_box4.Visibility = Visibility.Visible;
    }

    private void hideButton_Click(object sender, RoutedEventArgs e)
    {
      HideMasterTextBoxes();
    }

    private void HideMasterTextBoxes()
    {
      master_box1.Visibility = Visibility.Collapsed;
      master_box2.Visibility = Visibility.Collapsed;
      master_box3.Visibility = Visibility.Collapsed;
      master_box4.Visibility = Visibility.Collapsed;
    }

    private void SubmitButton(object sender, RoutedEventArgs e)
    {

      var submitButton = sender as Button;
      if (submitButton == null || submitButton.Tag == null)
        return;

      int row = Convert.ToInt32(submitButton.Tag)-1;

      masterCode_copy = new List<string>(masterCode);
      List<string> pegs = new List<string>();
      List<string> pegsFinal = new List<string>();
      string box1 = guessTextBoxes[row][0].Text;
      string box2 = guessTextBoxes[row][1].Text;
      string box3 = guessTextBoxes[row][2].Text;
      string box4 = guessTextBoxes[row][3].Text;


      // gets the answer for each box
      string peg1 = getAnswer(box1, 1);
      pegs.Add(peg1);
      string peg2 = getAnswer(box2, 2);
      pegs.Add(peg2);
      string peg3 = getAnswer(box3, 3);
      pegs.Add(peg3);
      string peg4 = getAnswer(box4, 4);
      pegs.Add(peg4);

      // puts pegs in order from black to white to none
      for (int count = 0; count < 4; count++)
      {
        if (pegs[count] == "black")
          pegsFinal.Add(pegs[count]);
      }
      for (int count = 0; count < 4; count++)
      {
        if (pegs[count] == "white")
          pegsFinal.Add(pegs[count]);
      }
      for (int count = 0; count < 4; count++)
      {
        if (pegs[count] == "NOPE!")
          pegsFinal.Add(pegs[count]);
      }

      // assigns the answer box's to the correct color
      // answer box 1
      if (pegsFinal[0] == "black")
        answerTextBoxes[row][0].Background = Brushes.Black;
      if (pegsFinal[0] == "white")
      {
        answerTextBoxes[row][0].Text = "";
        answerTextBoxes[row][0].Background = Brushes.White;
      }
      if (pegsFinal[0] == "NOPE!")
        answerTextBoxes[row][0].Text = "NOPE!";

      // answer box 2
      if (pegsFinal[1] == "black")
        answerTextBoxes[row][1].Background = Brushes.Black;
      if (pegsFinal[1] == "white")
      {
        answerTextBoxes[row][1].Text = "";
        answerTextBoxes[row][1].Background = Brushes.White;
      }
      if (pegsFinal[1] == "NOPE!")
        answerTextBoxes[row][1].Text = "NOPE!";

      // answer box 3
      if (pegsFinal[2] == "black")
        answerTextBoxes[row][2].Background = Brushes.Black;
      if (pegsFinal[2] == "white")
      {
        answerTextBoxes[row][2].Text = "";
        answerTextBoxes[row][2].Background = Brushes.White;
      }
      if (pegsFinal[2] == "NOPE!")
        answerTextBoxes[row][2].Text = "NOPE!";

      // answer box 4
      if (pegsFinal[3] == "black")
        answerTextBoxes[row][3].Background = Brushes.Black;
      if (pegsFinal[3] == "white")
      {
        answerTextBoxes[row][3].Text = "";
        answerTextBoxes[row][3].Background = Brushes.White;
      }
      if (pegsFinal[3] == "NOPE!")
        answerTextBoxes[row][3].Text = "NOPE!";

      answerTextBoxes[row][0].Visibility = Visibility.Visible;
      answerTextBoxes[row][1].Visibility = Visibility.Visible;
      answerTextBoxes[row][2].Visibility = Visibility.Visible;
      answerTextBoxes[row][3].Visibility = Visibility.Visible;

      if (guessTextBoxes[row][0].Text == masterCode[0] &&
        guessTextBoxes[row][1].Text == masterCode[1] &&
        guessTextBoxes[row][2].Text == masterCode[2] &&
        guessTextBoxes[row][3].Text == masterCode[3])
      {
        congrats_window congrats_window1 = new congrats_window();
        System.Diagnostics.Process.Start("files\\02 - We Are The Champions.mp3");
        congrats_window1.Show();
      }

    }


    public static string[] generateMasterCode()
    {
      //string[] masterCode = new string[4];
      List<string> masterCodeList = new List<string>();
      Random random = new Random();

      // get 4 random numbers
      for (int i = 0; i < 4; i++)
      {
        int randomNum = random.Next(1, 7);

        // convert number to color
        string randomColor = string.Empty;
        switch (randomNum)
        {
          case 1:
            randomColor = "red";
            break;
          case 2:
            randomColor = "blue";
            break;
          case 3:
            randomColor = "green";
            break;
          case 4:
            randomColor = "yellow";
            break;
          case 5:
            randomColor = "orange";
            break;
          case 6:
            randomColor = "purple";
            break;
        }
        // add color to list
        masterCodeList.Add(randomColor);
      }
      // set the masterCode to the list
      masterCode = masterCodeList.ToArray();

      return masterCode;
    }

    public string getAnswer(string boxColor, int boxNum)
    {
      string answer = "NOPE!";
      if (masterCode_copy.Contains(boxColor))
      {
        if (masterCode_copy[boxNum - 1] == boxColor)
        {
          answer = "black";
          masterCode_copy[boxNum - 1] = "exact";
        }
        else
          answer = "white";
      }
      return answer;
    }
  }
}
