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

namespace AIGeneticProject
{
    /// <summary>
    /// Interaction logic for NumberUpDown.xaml
    /// </summary>
    public partial class NumberUpDown : UserControl
    {
        private double value, minValue, maxValue, stepValue;
        private int precision;

        public NumberUpDown()
        {
            InitializeComponent();

            value = 0;
            minValue = 0;
            maxValue = 100;
            stepValue = 1;
            precision = 0;

            txtNum.Text = value.ToString();
        }

        public double Value 
        { 
            get => value; 
            
            set
            {
                this.value = setValue(value);
                txtNum.Text = value.ToString("N" + precision);
            }
        }

        public double MinValue { get => minValue; set => minValue = value; }
        
        public double MaxValue { get => maxValue; set => maxValue = value; }

        public double StepValue { get => stepValue; set => stepValue = value; }

        public int Precision { get => precision; set => precision = setPrecison(value); }

        //Return correct value in range
        private double setValue (double value)
        {
            if (value > maxValue)
                return maxValue;

            if (value < minValue)
                return minValue;

            return value;
        }

        //Return correct precision
        private int setPrecison(int precision)
        {
            if (precision < 0)
                return precision;

            if (precision > 10)
                return 10;

            return precision;
        }

        private void butUp_Click(object sender, RoutedEventArgs e)
        {
            value = setValue(value + stepValue);

            txtNum.Text = value.ToString("N" + precision);
        }

        private void butDown_Click(object sender, RoutedEventArgs e)
        {
            value = setValue(value - stepValue);

            txtNum.Text = value.ToString("N" + precision);
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!double.TryParse(txtNum.Text, out value))
                txtNum.Text = value.ToString("N"+precision);
        }
    }
}
