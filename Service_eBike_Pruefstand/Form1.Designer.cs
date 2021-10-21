
namespace Service_eBike_Pruefstand
{
    partial class eBIKE
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Temperatur = new System.Windows.Forms.GroupBox();
            this.label_Temp = new System.Windows.Forms.Label();
            this.Gewicht = new System.Windows.Forms.GroupBox();
            this.label_Gewicht = new System.Windows.Forms.Label();
            this.Anemometer = new System.Windows.Forms.GroupBox();
            this.label_Anemo = new System.Windows.Forms.Label();
            this.Lufter = new System.Windows.Forms.GroupBox();
            this.label_Luef = new System.Windows.Forms.Label();
            this.MessagesText = new System.Windows.Forms.TextBox();
            this.Temperatur.SuspendLayout();
            this.Gewicht.SuspendLayout();
            this.Anemometer.SuspendLayout();
            this.Lufter.SuspendLayout();
            this.SuspendLayout();
            // 
            // Temperatur
            // 
            this.Temperatur.Controls.Add(this.label_Temp);
            this.Temperatur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Temperatur.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.Temperatur.ForeColor = System.Drawing.SystemColors.Control;
            this.Temperatur.Location = new System.Drawing.Point(31, 60);
            this.Temperatur.Name = "Temperatur";
            this.Temperatur.Size = new System.Drawing.Size(746, 422);
            this.Temperatur.TabIndex = 0;
            this.Temperatur.TabStop = false;
            this.Temperatur.Text = "Temperatur";
            // 
            // label_Temp
            // 
            this.label_Temp.AutoSize = true;
            this.label_Temp.Font = new System.Drawing.Font("Microsoft Sans Serif", 100F, System.Drawing.FontStyle.Bold);
            this.label_Temp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.label_Temp.Location = new System.Drawing.Point(50, 146);
            this.label_Temp.Name = "label_Temp";
            this.label_Temp.Size = new System.Drawing.Size(560, 153);
            this.label_Temp.TabIndex = 0;
            this.label_Temp.Text = "00.00°C";
            this.label_Temp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Gewicht
            // 
            this.Gewicht.Controls.Add(this.label_Gewicht);
            this.Gewicht.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Gewicht.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.Gewicht.ForeColor = System.Drawing.SystemColors.Control;
            this.Gewicht.Location = new System.Drawing.Point(871, 60);
            this.Gewicht.Name = "Gewicht";
            this.Gewicht.Size = new System.Drawing.Size(746, 422);
            this.Gewicht.TabIndex = 1;
            this.Gewicht.TabStop = false;
            this.Gewicht.Text = "Gewicht";
            // 
            // label_Gewicht
            // 
            this.label_Gewicht.AutoSize = true;
            this.label_Gewicht.Font = new System.Drawing.Font("Microsoft Sans Serif", 100F, System.Drawing.FontStyle.Bold);
            this.label_Gewicht.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.label_Gewicht.Location = new System.Drawing.Point(69, 146);
            this.label_Gewicht.Name = "label_Gewicht";
            this.label_Gewicht.Size = new System.Drawing.Size(573, 153);
            this.label_Gewicht.TabIndex = 1;
            this.label_Gewicht.Text = "00.00Kg";
            this.label_Gewicht.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Anemometer
            // 
            this.Anemometer.Controls.Add(this.label_Anemo);
            this.Anemometer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Anemometer.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.Anemometer.ForeColor = System.Drawing.SystemColors.Control;
            this.Anemometer.Location = new System.Drawing.Point(31, 571);
            this.Anemometer.Name = "Anemometer";
            this.Anemometer.Size = new System.Drawing.Size(746, 422);
            this.Anemometer.TabIndex = 1;
            this.Anemometer.TabStop = false;
            this.Anemometer.Text = "Anemometer";
            // 
            // label_Anemo
            // 
            this.label_Anemo.AutoSize = true;
            this.label_Anemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 100F, System.Drawing.FontStyle.Bold);
            this.label_Anemo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.label_Anemo.Location = new System.Drawing.Point(50, 163);
            this.label_Anemo.Name = "label_Anemo";
            this.label_Anemo.Size = new System.Drawing.Size(626, 153);
            this.label_Anemo.TabIndex = 2;
            this.label_Anemo.Text = "00.00m/s";
            this.label_Anemo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lufter
            // 
            this.Lufter.Controls.Add(this.label_Luef);
            this.Lufter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Lufter.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.Lufter.ForeColor = System.Drawing.SystemColors.Control;
            this.Lufter.Location = new System.Drawing.Point(871, 571);
            this.Lufter.Name = "Lufter";
            this.Lufter.Size = new System.Drawing.Size(746, 422);
            this.Lufter.TabIndex = 1;
            this.Lufter.TabStop = false;
            this.Lufter.Text = "Lüfter";
            // 
            // label_Luef
            // 
            this.label_Luef.AutoSize = true;
            this.label_Luef.Font = new System.Drawing.Font("Microsoft Sans Serif", 100F, System.Drawing.FontStyle.Bold);
            this.label_Luef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.label_Luef.Location = new System.Drawing.Point(69, 163);
            this.label_Luef.Name = "label_Luef";
            this.label_Luef.Size = new System.Drawing.Size(527, 153);
            this.label_Luef.TabIndex = 3;
            this.label_Luef.Text = "00.00%";
            this.label_Luef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MessagesText
            // 
            this.MessagesText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            this.MessagesText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MessagesText.Enabled = false;
            this.MessagesText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessagesText.ForeColor = System.Drawing.Color.Red;
            this.MessagesText.Location = new System.Drawing.Point(12, 1029);
            this.MessagesText.Name = "MessagesText";
            this.MessagesText.ReadOnly = true;
            this.MessagesText.Size = new System.Drawing.Size(1635, 25);
            this.MessagesText.TabIndex = 1;
            this.MessagesText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // eBIKE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(1659, 1061);
            this.Controls.Add(this.MessagesText);
            this.Controls.Add(this.Lufter);
            this.Controls.Add(this.Anemometer);
            this.Controls.Add(this.Gewicht);
            this.Controls.Add(this.Temperatur);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1659, 1061);
            this.Name = "eBIKE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "e-BIKE Prüfstand";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Temperatur.ResumeLayout(false);
            this.Temperatur.PerformLayout();
            this.Gewicht.ResumeLayout(false);
            this.Gewicht.PerformLayout();
            this.Anemometer.ResumeLayout(false);
            this.Anemometer.PerformLayout();
            this.Lufter.ResumeLayout(false);
            this.Lufter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Temperatur;
        private System.Windows.Forms.GroupBox Gewicht;
        private System.Windows.Forms.GroupBox Anemometer;
        private System.Windows.Forms.GroupBox Lufter;
        private System.Windows.Forms.Label label_Temp;
        private System.Windows.Forms.Label label_Gewicht;
        private System.Windows.Forms.Label label_Anemo;
        private System.Windows.Forms.Label label_Luef;
        private System.Windows.Forms.TextBox MessagesText;
    }
}

