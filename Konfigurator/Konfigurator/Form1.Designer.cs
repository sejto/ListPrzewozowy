namespace Konfigurator
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
            this.connBoxremote = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Zapisz = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.connBox = new System.Windows.Forms.TextBox();
            this.passBox = new System.Windows.Forms.TextBox();
            this.userBox = new System.Windows.Forms.TextBox();
            this.connBoxList = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connBoxremote
            // 
            this.connBoxremote.Location = new System.Drawing.Point(236, 107);
            this.connBoxremote.Name = "connBoxremote";
            this.connBoxremote.Size = new System.Drawing.Size(311, 20);
            this.connBoxremote.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Parametry połączenia zdalnej bazy SQL:";
            // 
            // Zapisz
            // 
            this.Zapisz.Location = new System.Drawing.Point(472, 168);
            this.Zapisz.Name = "Zapisz";
            this.Zapisz.Size = new System.Drawing.Size(75, 23);
            this.Zapisz.TabIndex = 24;
            this.Zapisz.Text = "Zapisz";
            this.Zapisz.UseVisualStyleBackColor = true;
            this.Zapisz.Click += new System.EventHandler(this.Zapisz_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Parametry połączenia sql:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Hasło:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(157, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Użytkownik:";
            // 
            // connBox
            // 
            this.connBox.Location = new System.Drawing.Point(236, 75);
            this.connBox.Name = "connBox";
            this.connBox.Size = new System.Drawing.Size(311, 20);
            this.connBox.TabIndex = 20;
            // 
            // passBox
            // 
            this.passBox.Location = new System.Drawing.Point(236, 49);
            this.passBox.Name = "passBox";
            this.passBox.Size = new System.Drawing.Size(106, 20);
            this.passBox.TabIndex = 19;
            // 
            // userBox
            // 
            this.userBox.Location = new System.Drawing.Point(236, 22);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(106, 20);
            this.userBox.TabIndex = 18;
            // 
            // connBoxList
            // 
            this.connBoxList.Location = new System.Drawing.Point(236, 139);
            this.connBoxList.Name = "connBoxList";
            this.connBoxList.Size = new System.Drawing.Size(311, 20);
            this.connBoxList.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(210, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Parametry połączenia ListuPrzewozowego:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 224);
            this.Controls.Add(this.connBoxList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.connBoxremote);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Zapisz);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connBox);
            this.Controls.Add(this.passBox);
            this.Controls.Add(this.userBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox connBoxremote;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Zapisz;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox connBox;
        private System.Windows.Forms.TextBox passBox;
        private System.Windows.Forms.TextBox userBox;
        private System.Windows.Forms.TextBox connBoxList;
        private System.Windows.Forms.Label label5;
    }
}

