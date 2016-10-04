namespace Solicitacao_de_Ambulancias
{
    partial class ReagendamentoUnidade
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.label29 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.Unidade = new System.Windows.Forms.ComboBox();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(252)))), ((int)(((byte)(194)))));
            this.panel4.Controls.Add(this.label29);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.Unidade);
            this.panel4.Location = new System.Drawing.Point(12, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(260, 237);
            this.panel4.TabIndex = 7;
            // 
            // label29
            // 
            this.label29.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(61, 37);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(147, 18);
            this.label29.TabIndex = 0;
            this.label29.Text = "Selecione a Unidade:";
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(205)))), ((int)(((byte)(169)))));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(253)))), ((int)(((byte)(243)))));
            this.button2.Location = new System.Drawing.Point(50, 160);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(167, 38);
            this.button2.TabIndex = 2;
            this.button2.Text = "Entrar";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Unidade
            // 
            this.Unidade.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Unidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Unidade.FormattingEnabled = true;
            this.Unidade.Items.AddRange(new object[] {
            "",
            "HMU",
            "HOSPITAL ANCHIETA(ENSINO)",
            "HOSPITAL DE CLÍNICAS MUNICIPAL SBC",
            "PSC - PRONTO SOCORRO CENTRAL",
            "UPA ALVES DIAS/ASSUNÇÃO",
            "UPA BAETA NEVES",
            "UPA DEMARCHI/BATISTINI",
            "UPA PAULICÉIA/TABOÃO",
            "UPA RIACHO GRANDE",
            "UPA RUDGE RAMOS",
            "UPA SÃO PEDRO",
            "UPA SILVINA/FERRAZÓPOLIS",
            "UPA UNIÃO/ALVARENGA"});
            this.Unidade.Location = new System.Drawing.Point(50, 104);
            this.Unidade.Name = "Unidade";
            this.Unidade.Size = new System.Drawing.Size(167, 21);
            this.Unidade.TabIndex = 5;
            // 
            // ReagendamentoUnidade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(224)))), ((int)(((byte)(173)))));
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.panel4);
            this.Name = "ReagendamentoUnidade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReagendamentoUnidade";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox Unidade;
    }
}