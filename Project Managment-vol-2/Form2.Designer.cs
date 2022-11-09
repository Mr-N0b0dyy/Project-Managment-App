namespace Project_Managment_vol_2
{
    partial class frmReg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.regemil = new System.Windows.Forms.TextBox();
            this.regpas = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.regkey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.reg = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bactolog = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.label1.Location = new System.Drawing.Point(28, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Register";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "E-mail";
            // 
            // regemil
            // 
            this.regemil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.regemil.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.regemil.Location = new System.Drawing.Point(28, 130);
            this.regemil.Name = "regemil";
            this.regemil.Size = new System.Drawing.Size(216, 28);
            this.regemil.TabIndex = 2;
            // 
            // regpas
            // 
            this.regpas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.regpas.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.regpas.Location = new System.Drawing.Point(28, 190);
            this.regpas.Name = "regpas";
            this.regpas.Size = new System.Drawing.Size(216, 28);
            this.regpas.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password";
            // 
            // regkey
            // 
            this.regkey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.regkey.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.regkey.Location = new System.Drawing.Point(28, 250);
            this.regkey.Name = "regkey";
            this.regkey.Size = new System.Drawing.Size(216, 28);
            this.regkey.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Registration Key";
            // 
            // reg
            // 
            this.reg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.reg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.reg.FlatAppearance.BorderSize = 0;
            this.reg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reg.ForeColor = System.Drawing.Color.White;
            this.reg.Location = new System.Drawing.Point(28, 310);
            this.reg.Name = "reg";
            this.reg.Size = new System.Drawing.Size(216, 32);
            this.reg.TabIndex = 7;
            this.reg.Text = "Register";
            this.reg.UseVisualStyleBackColor = false;
            this.reg.Click += new System.EventHandler(this.reg_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 370);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Already Have an Account?";
            // 
            // bactolog
            // 
            this.bactolog.AutoSize = true;
            this.bactolog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.bactolog.Location = new System.Drawing.Point(28, 390);
            this.bactolog.Name = "bactolog";
            this.bactolog.Size = new System.Drawing.Size(97, 17);
            this.bactolog.TabIndex = 9;
            this.bactolog.Text = "Back to LOGIN";
            this.bactolog.Click += new System.EventHandler(this.bactolog_Click);
            // 
            // frmReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(285, 455);
            this.Controls.Add(this.bactolog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.reg);
            this.Controls.Add(this.regkey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.regpas);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.regemil);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmReg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.frmReg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox regemil;
        private System.Windows.Forms.TextBox regpas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox regkey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button reg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label bactolog;
    }
}