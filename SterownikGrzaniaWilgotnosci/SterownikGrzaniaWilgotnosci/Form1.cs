using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Fuzzy;

namespace SterownikGrzaniaWilgotnosci
{
    public partial class Form1 : Form
    {
        InferenceSystem tempIS, humiIS;
        bool simulationRunning = false;
        double heatCoolTemperature, fanSpeed;
        public Form1()
        {
            InitializeComponent();

            numTZ.Increment = 0.5M;
            numTW.Increment = 0.5M;

            InitFuzzyEngine();
            GetHeatCoolTemperature();
            GetFanSpeed();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numTZ_ValueChanged(object sender, EventArgs e)
        {
            GetHeatCoolTemperature();
        }

        private void numTW_ValueChanged(object sender, EventArgs e)
        {
            GetHeatCoolTemperature();
        }

        private void GetHeatCoolTemperature()
        {
            tempIS.SetInput("TZ", (float)numTZ.Value);
            tempIS.SetInput("TW", (float)numTW.Value);

            heatCoolTemperature = tempIS.Evaluate("TG");

            lblHeatCoolTemperature.Text = (heatCoolTemperature.ToString("N1") + " stopni");
        }

        private void GetFanSpeed()
        {
            humiIS.SetInput("WZ", (float)numWZ.Value);
            humiIS.SetInput("WW", (float)numWW.Value);

            fanSpeed = humiIS.Evaluate("OW");
            lblFanSpeed.Text = (fanSpeed.ToString("N0") + "%");
        }

        private void numWZ_ValueChanged(object sender, EventArgs e)
        {
            GetFanSpeed();
        }

        private void numWW_ValueChanged(object sender, EventArgs e)
        {
            GetFanSpeed();
        }

        private void butSimulate_Click(object sender, EventArgs e)
        {
            //Stop simulation if running
            if (simulationRunning)
            {
                simulationRunning = false;
                tmrSimulation.Stop();
                butSimulate.Text = "Symulacja";
                return;
            }

            GetHeatCoolTemperature();
            GetFanSpeed();

            butSimulate.Text = "Zatrzymaj symulację";

            simulationRunning = true;

            tmrSimulation.Interval = 200;
            tmrSimulation.Start();
        }

        private void tmrSimulation_Tick(object sender, EventArgs e)
        {
            //Stop simulation when temperature is optimal
            if (numTW.Value >= 23 && numTW.Value <= 25)
                butSimulate.PerformClick();

            //If heatercooler temperature is higher than outside then it should make inside temperature higher
            if (heatCoolTemperature > (float)numTZ.Value)
                numTW.Value += (decimal)(heatCoolTemperature/1000);
            else
                numTW.Value -= (decimal)(heatCoolTemperature / 1000);

            //If outside humidity is higher than inside then fan will raise inside humidity
            if (numWZ.Value > numWW.Value)
                numWW.Value += (decimal)(fanSpeed/1000);
            else
                numWW.Value -= (decimal)(fanSpeed / 1000);

            GetHeatCoolTemperature();
            GetFanSpeed();
        }

        private void InitFuzzyEngine()
        {
            //Outside temperature
            FuzzySet outTempVeryCold = new FuzzySet("Bardzo_zimno", new TrapezoidalFunction(-20, -5, TrapezoidalFunction.EdgeType.Right));
            FuzzySet outTempCold = new FuzzySet("Zimno", new TrapezoidalFunction(-10, 0, 10, 15));
            FuzzySet outTempWarm = new FuzzySet("Cieplo", new TrapezoidalFunction(10, 20, 25, 30));
            FuzzySet outTempHot = new FuzzySet("Bardzo_cieplo", new TrapezoidalFunction(25, 30, TrapezoidalFunction.EdgeType.Left));

            //Inside temperature
            FuzzySet inTempCold = new FuzzySet("Zimno", new TrapezoidalFunction(5, 10, TrapezoidalFunction.EdgeType.Right));
            FuzzySet inTempOptimal = new FuzzySet("Optymalna", new TrapezoidalFunction(5, 15, 20, 25));
            FuzzySet inTempHot = new FuzzySet("Cieplo", new TrapezoidalFunction(20, 30, TrapezoidalFunction.EdgeType.Left));

            //Heater and cooler temperature
            FuzzySet outHeatCoolVeryCold = new FuzzySet("Bardzo_niska", new TrapezoidalFunction(5, 15, TrapezoidalFunction.EdgeType.Right));
            FuzzySet outHeatCoolCold = new FuzzySet("Niska", new TrapezoidalFunction(10, 15, 20, 25));
            FuzzySet outHeatCoolDisabled = new FuzzySet("Zerowa", new TrapezoidalFunction(21, 22, 23, 24));
            FuzzySet outHeatCoolWarm = new FuzzySet("Wysoka", new TrapezoidalFunction(20, 23, 25, 27));
            FuzzySet outHeatCoolHot = new FuzzySet("Bardzo_wysoka", new TrapezoidalFunction(25, 30, TrapezoidalFunction.EdgeType.Left));

            //Outside humidity
            FuzzySet outHumiLow = new FuzzySet("Niska", new TrapezoidalFunction(30, 35, TrapezoidalFunction.EdgeType.Right));
            FuzzySet outHumiOptimal = new FuzzySet("Optymalna", new TrapezoidalFunction(30, 35, 40, 45));
            FuzzySet outHumiHigh = new FuzzySet("Wysoka", new TrapezoidalFunction(40, 45, TrapezoidalFunction.EdgeType.Left));

            //Inside humidity
            FuzzySet inHumiLow = new FuzzySet("Niska", new TrapezoidalFunction(25, 30, TrapezoidalFunction.EdgeType.Right));
            FuzzySet inHumiOptimal = new FuzzySet("Optymalna", new TrapezoidalFunction(25, 30, 35, 40));
            FuzzySet inHumiHigh = new FuzzySet("Wysoka", new TrapezoidalFunction(35, 40, TrapezoidalFunction.EdgeType.Left));

            //Fan speed (percent)
            FuzzySet outFanSpeedZero = new FuzzySet("Zerowe", new TrapezoidalFunction(1, 5, TrapezoidalFunction.EdgeType.Right));
            FuzzySet outFanSpeedLow = new FuzzySet("Niskie", new TrapezoidalFunction(5, 10, 40, 50));
            FuzzySet outFanSpeedHigh = new FuzzySet("Wysokie", new TrapezoidalFunction(45, 55, TrapezoidalFunction.EdgeType.Left));

            //Outside temperature (input)
            LinguisticVariable outTemp = new LinguisticVariable("TZ", -50, 50);
            outTemp.AddLabel(outTempVeryCold);
            outTemp.AddLabel(outTempCold);
            outTemp.AddLabel(outTempWarm);
            outTemp.AddLabel(outTempHot);

            //Inside temperature (input)
            LinguisticVariable inTemp = new LinguisticVariable("TW", -50, 50);
            inTemp.AddLabel(inTempCold);
            inTemp.AddLabel(inTempOptimal);
            inTemp.AddLabel(inTempHot);

            //Heater and cooler temperature (output)
            LinguisticVariable heatCoolTemperature = new LinguisticVariable("TG", 0, 35);
            heatCoolTemperature.AddLabel(outHeatCoolVeryCold);
            heatCoolTemperature.AddLabel(outHeatCoolCold);
            heatCoolTemperature.AddLabel(outHeatCoolDisabled);
            heatCoolTemperature.AddLabel(outHeatCoolWarm);
            heatCoolTemperature.AddLabel(outHeatCoolHot);


            //Outside humidity (input)
            LinguisticVariable outHumidity = new LinguisticVariable("WZ", 0, 100);
            outHumidity.AddLabel(outHumiLow);
            outHumidity.AddLabel(outHumiOptimal);
            outHumidity.AddLabel(outHumiHigh);

            //Inside humidity (input)
            LinguisticVariable inHumidity = new LinguisticVariable("WW", 0, 100);
            inHumidity.AddLabel(inHumiLow);
            inHumidity.AddLabel(inHumiOptimal);
            inHumidity.AddLabel(inHumiHigh);

            //Fan speed (output)
            LinguisticVariable fanSpeed = new LinguisticVariable("OW", 0, 100);
            fanSpeed.AddLabel(outFanSpeedZero);
            fanSpeed.AddLabel(outFanSpeedLow);
            fanSpeed.AddLabel(outFanSpeedHigh);

            //Temperature database
            Database tempFuzzyDB = new Database();
            tempFuzzyDB.AddVariable(outTemp);
            tempFuzzyDB.AddVariable(inTemp);
            tempFuzzyDB.AddVariable(heatCoolTemperature);

            //Humidity database
            Database humiFuzzyDB = new Database();
            humiFuzzyDB.AddVariable(outHumidity);
            humiFuzzyDB.AddVariable(inHumidity);
            humiFuzzyDB.AddVariable(fanSpeed);
 
            //Temperature inference system
            tempIS = new InferenceSystem(tempFuzzyDB, new CentroidDefuzzifier(1000));

            //Humidity inference system
            humiIS = new InferenceSystem(humiFuzzyDB, new CentroidDefuzzifier(1000));

            //Temperature rules
            tempIS.NewRule("Rule 1", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 2", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 3", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 4", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 5", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 6", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 7", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 8", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 9", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 10", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 11", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 12", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 13", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 14", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 15", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 16", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 17", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 18", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 19", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 20", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 21", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 22", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 23", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 24", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 25", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 26", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 27", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 28", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 29", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 30", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 31", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 32", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 33", "IF TZ IS Bardzo_zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 34", "IF TZ IS Zimno AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 35", "IF TZ IS Cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 36", "IF TZ IS Bardzo_cieplo AND TW IS Zimno THEN TG IS Bardzo_wysoka");
            tempIS.NewRule("Rule 37", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 38", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 39", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 40", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 41", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 42", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 43", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 44", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 45", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 46", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 47", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 48", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 49", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 50", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 51", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 52", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 53", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 54", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 55", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 56", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 57", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 58", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 59", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 60", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 61", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 62", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 63", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 64", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 65", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 66", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 67", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 68", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 69", "IF TZ IS Bardzo_zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 70", "IF TZ IS Zimno AND TW IS Optymalna THEN TG IS Wysoka");
            tempIS.NewRule("Rule 71", "IF TZ IS Cieplo AND TW IS Optymalna THEN TG IS Zerowa");
            tempIS.NewRule("Rule 72", "IF TZ IS Bardzo_cieplo AND TW IS Optymalna THEN TG IS Niska");
            tempIS.NewRule("Rule 73", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 74", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 75", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 76", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 77", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 78", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 79", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 80", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 81", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 82", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 83", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 84", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 85", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 86", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 87", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 88", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 89", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 90", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 91", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 92", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 93", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 94", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 95", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 96", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 97", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 98", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 99", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 100", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 101", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 102", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 103", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 104", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 105", "IF TZ IS Bardzo_zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 106", "IF TZ IS Zimno AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 107", "IF TZ IS Cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");
            tempIS.NewRule("Rule 108", "IF TZ IS Bardzo_cieplo AND TW IS Cieplo THEN TG IS Bardzo_niska");

            //Humidity rules
            humiIS.NewRule("Rule 1", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 2", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 3", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 4", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 5", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 6", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 7", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 8", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 9", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 10", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 11", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 12", "IF WZ IS Niska AND WW IS Niska THEN OW IS Zerowe");
            humiIS.NewRule("Rule 13", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 14", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 15", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 16", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 17", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 18", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 19", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 20", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 21", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 22", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 23", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 24", "IF WZ IS Optymalna AND WW IS Niska THEN OW IS Niskie");
            humiIS.NewRule("Rule 25", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 26", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 27", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 28", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 29", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 30", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 31", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 32", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 33", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 34", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 35", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 36", "IF WZ IS Wysoka AND WW IS Niska THEN OW IS Wysokie");
            humiIS.NewRule("Rule 37", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 38", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 39", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 40", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 41", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 42", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 43", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 44", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 45", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 46", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 47", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 48", "IF WZ IS Niska AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 49", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 50", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 51", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 52", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 53", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 54", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 55", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 56", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 57", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 58", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 59", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 60", "IF WZ IS Optymalna AND WW IS Optymalna THEN OW IS Niskie");
            humiIS.NewRule("Rule 61", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 62", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 63", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 64", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 65", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 66", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 67", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 68", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 69", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 70", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 71", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 72", "IF WZ IS Wysoka AND WW IS Optymalna THEN OW IS Zerowe");
            humiIS.NewRule("Rule 73", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 74", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 75", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 76", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 77", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 78", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 79", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 80", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 81", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 82", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 83", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 84", "IF WZ IS Niska AND WW IS Wysoka THEN OW IS Wysokie");
            humiIS.NewRule("Rule 85", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 86", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 87", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 88", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 89", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 90", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 91", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 92", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 93", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 94", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 95", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 96", "IF WZ IS Optymalna AND WW IS Wysoka THEN OW IS Niskie");
            humiIS.NewRule("Rule 97", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 98", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 99", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 100", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 101", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 102", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 103", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 104", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 105", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 106", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 107", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
            humiIS.NewRule("Rule 108", "IF WZ IS Wysoka AND WW IS Wysoka THEN OW IS Zerowe");
        }
    }
}
