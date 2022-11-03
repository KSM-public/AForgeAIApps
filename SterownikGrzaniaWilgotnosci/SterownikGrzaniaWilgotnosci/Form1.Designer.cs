namespace SterownikGrzaniaWilgotnosci
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.numTZ = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numTW = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numWZ = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numWW = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblFanSpeed = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblHeatCoolTemperature = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.butSimulate = new System.Windows.Forms.Button();
            this.tmrSimulation = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numTZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWW)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // numTZ
            // 
            this.numTZ.DecimalPlaces = 1;
            this.numTZ.Location = new System.Drawing.Point(9, 32);
            this.numTZ.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numTZ.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numTZ.Name = "numTZ";
            this.numTZ.Size = new System.Drawing.Size(120, 20);
            this.numTZ.TabIndex = 0;
            this.numTZ.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numTZ.ValueChanged += new System.EventHandler(this.numTZ_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Temperatura zewnętrzna:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Temperatura wewnętrzna:";
            // 
            // numTW
            // 
            this.numTW.DecimalPlaces = 1;
            this.numTW.Location = new System.Drawing.Point(9, 85);
            this.numTW.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numTW.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numTW.Name = "numTW";
            this.numTW.Size = new System.Drawing.Size(120, 20);
            this.numTW.TabIndex = 2;
            this.numTW.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numTW.ValueChanged += new System.EventHandler(this.numTW_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Wilgotność zewnętrzna:";
            // 
            // numWZ
            // 
            this.numWZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numWZ.Location = new System.Drawing.Point(9, 179);
            this.numWZ.Name = "numWZ";
            this.numWZ.Size = new System.Drawing.Size(120, 20);
            this.numWZ.TabIndex = 4;
            this.numWZ.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.numWZ.ValueChanged += new System.EventHandler(this.numWZ_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Wilgotność wewnętrzna:";
            // 
            // numWW
            // 
            this.numWW.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numWW.Location = new System.Drawing.Point(9, 234);
            this.numWW.Name = "numWW";
            this.numWW.Size = new System.Drawing.Size(120, 20);
            this.numWW.TabIndex = 6;
            this.numWW.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numWW.ValueChanged += new System.EventHandler(this.numWW_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numTZ);
            this.groupBox1.Controls.Add(this.numWW);
            this.groupBox1.Controls.Add(this.numTW);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numWZ);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 264);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wartości wejściowe";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblFanSpeed);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblHeatCoolTemperature);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(273, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 259);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wartości wyjściowe";
            // 
            // lblFanSpeed
            // 
            this.lblFanSpeed.AutoSize = true;
            this.lblFanSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblFanSpeed.Location = new System.Drawing.Point(6, 217);
            this.lblFanSpeed.Name = "lblFanSpeed";
            this.lblFanSpeed.Size = new System.Drawing.Size(27, 13);
            this.lblFanSpeed.TabIndex = 3;
            this.lblFanSpeed.Text = "0 %";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Prędkość wentylatora:";
            // 
            // lblHeatCoolTemperature
            // 
            this.lblHeatCoolTemperature.AutoSize = true;
            this.lblHeatCoolTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblHeatCoolTemperature.Location = new System.Drawing.Point(6, 62);
            this.lblHeatCoolTemperature.Name = "lblHeatCoolTemperature";
            this.lblHeatCoolTemperature.Size = new System.Drawing.Size(52, 13);
            this.lblHeatCoolTemperature.TabIndex = 1;
            this.lblHeatCoolTemperature.Text = "0 stopni";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Temperatura grzania/chłodzenia:";
            // 
            // butSimulate
            // 
            this.butSimulate.Location = new System.Drawing.Point(12, 282);
            this.butSimulate.Name = "butSimulate";
            this.butSimulate.Size = new System.Drawing.Size(461, 23);
            this.butSimulate.TabIndex = 10;
            this.butSimulate.Text = "Symulacja";
            this.butSimulate.UseVisualStyleBackColor = true;
            this.butSimulate.Click += new System.EventHandler(this.butSimulate_Click);
            // 
            // tmrSimulation
            // 
            this.tmrSimulation.Tick += new System.EventHandler(this.tmrSimulation_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 312);
            this.Controls.Add(this.butSimulate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Sterownik temperatury/wilgotności";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numTZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWW)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numTZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numTW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numWZ;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numWW;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblHeatCoolTemperature;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblFanSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button butSimulate;
        private System.Windows.Forms.Timer tmrSimulation;
    }
}

