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
using System.Threading;
using System.ComponentModel;

namespace AIGeneticProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<IterationDetails> iterationsDetailsList;
        private Genetic genetic;

        public MainWindow()
        {
            InitializeComponent();

            iterationsDetailsList = new List<IterationDetails>();
        }

        private void butCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (txtExpression.Text == "")
            {
                MessageBox.Show("Nie podano ani nie wybrano żadnego wzoru!");

                return;
            }

            if (numMaxRange.Value < numMinRange.Value)
                numMaxRange.Value = numMinRange.Value;

            iterationsDetailsList.Clear();

            genetic = new Genetic(txtExpression.Text, (int)numPopulation.Value, (int)numIterations.Value, numMutation.Value, numCrossover.Value,
                (int)numMinRange.Value, (int)numMaxRange.Value, (int)numResultPrecision.Value, iterationsDetailsList, cbxSelectionMethod.SelectedIndex);

            Mouse.OverrideCursor = Cursors.Wait;
            butCalculate.IsEnabled = false;
            butCalculate.Content = "Trwa obliczanie...";

            BackgroundWorker calculateWork = new BackgroundWorker();
            calculateWork.DoWork += new DoWorkEventHandler(calculateWork_Work);
            calculateWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculateWork_WorkComplete);

            calculateWork.RunWorkerAsync();
        }

        private void calculateWork_Work(object sender, DoWorkEventArgs e)
        {
            genetic.Run();
        }

        private void calculateWork_WorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            butCalculate.IsEnabled = true;
            butCalculate.Content = "Oblicz";

            showResults();
        }

        private void butConfirm_Click(object sender, RoutedEventArgs e)
        {
            foreach (IterationDetails iterationDetails in iterationsDetailsList)
                iterationDetails.Precision = (int)numResultPrecision.Value;

            showResults();
        }

        private void cbxExpressions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtExpression.Text = ((ComboBoxItem)cbxExpressions.SelectedItem).Content.ToString();
        }

        private void showResults()
        {
            dgrResults.ItemsSource = null;

            dgrResults.Items.Clear();

            dgrResults.ItemsSource = iterationsDetailsList;

            dgrResults.Items.Refresh();
        }
    }
}
