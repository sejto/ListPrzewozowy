namespace WindowsFormsApp1
{
    partial class Form2
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
            this.userBox = new System.Windows.Forms.TextBox();
            this.passBox = new System.Windows.Forms.TextBox();
            this.connBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.connBoxremote = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // userBox
            // 
            this.userBox.Location = new System.Drawing.Point(206, 29);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(106, 20);
            this.userBox.TabIndex = 0;
            // 
            // passBox
            // 
            this.passBox.Location = new System.Drawing.Point(206, 56);
            this.passBox.Name = "passBox";
            this.passBox.Size = new System.Drawing.Size(106, 20);
            this.passBox.TabIndex = 1;
            // 
            // connBox
            // 
            this.connBox.Location = new System.Drawing.Point(206, 82);
            this.connBox.Name = "connBox";
            this.connBox.Size = new System.Drawing.Size(311, 20);
            this.connBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Użytkownik:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hasło:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Parametry połączenia sql:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(442, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Zapisz";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Zapisz_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Parametry połączenia zdalnej bazy SQL:";
            // 
            // connBoxremote
            // 
            this.connBoxremote.Location = new System.Drawing.Point(206, 114);
            this.connBoxremote.Name = "connBoxremote";
            this.connBoxremote.Size = new System.Drawing.Size(311, 20);
            this.connBoxremote.TabIndex = 8;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 206);
            this.Controls.Add(this.connBoxremote);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connBox);
            this.Controls.Add(this.passBox);
            this.Controls.Add(this.userBox);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox userBox;
        private System.Windows.Forms.TextBox passBox;
        private System.Windows.Forms.TextBox connBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox connBoxremote;
    }
}