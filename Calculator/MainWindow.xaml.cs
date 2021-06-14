using System;
using System.Collections.Generic;
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

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double _leftOperand;
        private char _selectOperator;
        private bool _considerAgain;
        private bool _clearAll;

        public string OpText { get; set; }
        public string Member { get; set; }

        public MainWindow()
        {
            _clearAll = false;
            OpText = "";
            _considerAgain = false;
            _leftOperand = 0;
            _selectOperator = ' ';
            InitializeComponent();
            LabelNums.Content = "";
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            LabelNums.Content = "";
            if (_clearAll)
            {
                Clean();

                LabelMemberVal.Content = "";
                LabelMemberVal.Visibility = Visibility.Hidden;
                LabelMember.Visibility = Visibility.Hidden;

                _clearAll = false;
                Member = null;
            }
            _clearAll = true;
        }

        private void ButtonNum_Click(object sender, RoutedEventArgs e)
        {
            _clearAll = false;
            Button btn = sender as Button;
            string strButton = "";
            if (btn.Content != null)
                strButton += btn.Content;

            OpText = LabelNums.Content.ToString();

            if (OpText.Length < 10)
            {

                if (_considerAgain)
                {
                    Clean();
                }
                if (OpText.Length == 1 && OpText[0] == '0' && btn.Content != "0" && btn.Content != ",")
                {
                    LabelNums.Content = "";
                    LabelNums.Content = strButton;
                }
                else if (strButton == ",")
                {
                    if (OpText.IndexOf(',') == -1 && OpText.Length > 0)
                    {
                        LabelNums.Content += strButton;
                    }
                }
                else if (btn.Content .ToString() == "0")
                {
                    if (OpText.Length == 0 || (OpText[0] != '0'))
                    {
                        LabelNums.Content += strButton;
                    }
                }
                else
                {
                    LabelNums.Content += strButton;
                }
            }
            OpText = "";
        }


        private void ButtonMember_Click(object sender, EventArgs e)
        {
            _clearAll = false;
            OpText = LabelNums.Content.ToString();
            if (OpText != "")
            {
                if (OpText == Member)
                    Member = "";
                else
                    Member = OpText;
            }
            else if (Member != null && OpText == "")
            {
                if (OpText == Member)
                    Member = "";
                else
                    LabelNums.Content = Member;
            }

            if (Member != "0" && Member != "")
            {
                LabelMemberVal.Content = Member;
                LabelMemberVal.Visibility = Visibility.Visible;
                LabelMember.Visibility = Visibility.Visible;
            }
            else
            {
                LabelMemberVal.Content = "";
                LabelMemberVal.Visibility = Visibility.Hidden;
                LabelMember.Visibility = Visibility.Hidden;
            }
            OpText = "";
        }

        private void ButtonMemberPlusMinus_Click(object sender, EventArgs e)
        {
            _clearAll = false;
            double tmp = 0;
            bool isNegative = false;
            OpText = LabelNums.Content as string;

            if (Member != null && OpText != "")
            {
                if (Member[0] == '-')
                {
                    isNegative = true;
                    Member = Member.Substring(1);
                }
                tmp = Double.Parse(Member);
                if (isNegative)
                    tmp *= -1;

                var operandminus = Double.Parse(OpText);

                Member = "";
                switch (((Button)sender).Content.ToString())
                {
                    case "M+":
                        Member += tmp + operandminus;
                        break;
                    case "M-":
                        Member += tmp - operandminus;
                        break;
                }

                if (Member != "0" && Member != "")
                {
                    LabelMemberVal.Content = Member;
                    LabelMemberVal.Visibility = Visibility.Visible;
                    LabelMember.Visibility = Visibility.Visible;
                }
                else
                {
                    LabelMemberVal.Content = "";
                    LabelMemberVal.Visibility = Visibility.Hidden;
                    LabelMember.Visibility = Visibility.Hidden;
                }
            }
            OpText = "";
        }



        private void Clean()
        {
            _leftOperand = 0;
            _considerAgain = false;
            _selectOperator = ' ';
            LabelMember.ContentStringFormat = "";

            LabelResult.Visibility = Visibility.Hidden;
            LabelResultVal.Visibility = Visibility.Hidden;

        }

        private void ButtonOperator_Click(object sender, RoutedEventArgs e)
        {
            _clearAll = false;
            OpText = LabelNums.Content.ToString();
            if (OpText != "")
            {
                if (_considerAgain)
                {
                    double tmp = _leftOperand;
                    Clean();
                    LabelResultVal.Content = tmp;
                    _leftOperand = tmp;
                    ButtonGetResult.IsEnabled = true;
                    ButtonGetResult.Visibility = Visibility.Visible;
                }

                if (OpText[OpText.Length - 1] == ',')
                    OpText += '0';
                if (_leftOperand == 0)
                    _leftOperand = Double.Parse(LabelNums.Content.ToString());

                else if (_selectOperator != ' ')
                {
                    Сonsider(OpText);
                }

                switch (((Button)sender).Content.ToString())
                {
                    case "+":
                        _selectOperator = '+';
                        break;
                    case "-":
                        _selectOperator = '-';
                        break;
                    case "x":
                        _selectOperator = '*';
                        break;
                    case "÷":
                        _selectOperator = '/';
                        break;
                }

                LabelResultVal.Content = _leftOperand;
                LabelResultVal.Visibility = Visibility.Visible;
                ButtonGetResult.Visibility = Visibility.Visible;
                LabelNums.Content = "";
            }
            OpText = "";
        }

        private bool Сonsider(string text)
        {
            bool alright = false;

            switch (_selectOperator)
            {
                case '+':
                    _leftOperand += Double.Parse(text);
                    alright = true;
                    break;
                case '-':
                    _leftOperand -= Double.Parse(text);
                    alright = true;
                    break;
                case '*':
                    _leftOperand *= Double.Parse(text);
                    alright = true;
                    break;
                case '/':
                    if (Double.Parse(text) != 0)
                    {
                        _leftOperand /= Double.Parse(text);
                        alright = true;
                    }
                    break;
            }
            return alright;
        }

        private void ButtonEquels_Click(object sender, EventArgs e)
        {
            _clearAll = false;
            OpText = LabelNums.Content.ToString();

            if (OpText != "" && _selectOperator != ' ')
            {
                if (OpText[OpText.Length - 1] == ',')
                    OpText += '0';
                if (Сonsider(OpText))
                {
                    LabelResultVal.Content = _leftOperand;
                    LabelResultVal.Visibility = Visibility.Visible;
                    LabelResult.Visibility = Visibility.Visible;

                    ButtonGetResult.Visibility = Visibility.Visible;
                    _considerAgain = true;
                }
            }
        }

        private void ButtonGetResult_Click(object sender, EventArgs e)
        {
            LabelNums.Content = LabelResultVal.Content;
        }

        private void ButtonChangeSing_Click(object sender, EventArgs e)
        {
            string content = LabelNums.Content as string;
            if (content[0]=='-')
            {
                LabelNums.Content = content.Substring(1);
            }
            else
            {
                LabelNums.Content = '-' + content;
            }

        }
    }
}
