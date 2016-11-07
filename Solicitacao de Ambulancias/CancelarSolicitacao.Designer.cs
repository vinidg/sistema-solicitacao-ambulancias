namespace Solicitacao_de_Ambulancias
{
    partial class CancelarSolicitacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CancelarSolicitacao));
            this.painelCancelar = new System.Windows.Forms.Panel();
            this.BtnConfirmando = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtObsCancelamento = new System.Windows.Forms.TextBox();
            this.txtResponsavel = new System.Windows.Forms.TextBox();
            this.DtHrCancelamento = new System.Windows.Forms.TextBox();
            this.MotivoCancelar = new System.Windows.Forms.ComboBox();
            this.painelCancelar.SuspendLayout();
            this.SuspendLayout();
            // 
            // painelCancelar
            // 
            this.painelCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.painelCancelar.Controls.Add(this.BtnConfirmando);
            this.painelCancelar.Controls.Add(this.label27);
            this.painelCancelar.Controls.Add(this.label26);
            this.painelCancelar.Controls.Add(this.label25);
            this.painelCancelar.Controls.Add(this.label24);
            this.painelCancelar.Controls.Add(this.txtObsCancelamento);
            this.painelCancelar.Controls.Add(this.txtResponsavel);
            this.painelCancelar.Controls.Add(this.DtHrCancelamento);
            this.painelCancelar.Controls.Add(this.MotivoCancelar);
            this.painelCancelar.Location = new System.Drawing.Point(12, 12);
            this.painelCancelar.Name = "painelCancelar";
            this.painelCancelar.Size = new System.Drawing.Size(558, 187);
            this.painelCancelar.TabIndex = 17;
            // 
            // BtnConfirmando
            // 
            this.BtnConfirmando.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnConfirmando.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.BtnConfirmando.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.BtnConfirmando.ForeColor = System.Drawing.Color.Snow;
            this.BtnConfirmando.Location = new System.Drawing.Point(199, 141);
            this.BtnConfirmando.Name = "BtnConfirmando";
            this.BtnConfirmando.Size = new System.Drawing.Size(135, 43);
            this.BtnConfirmando.TabIndex = 56;
            this.BtnConfirmando.Text = "Cancelar";
            this.BtnConfirmando.UseVisualStyleBackColor = false;
            this.BtnConfirmando.Click += new System.EventHandler(this.BtnConfirmando_Click);
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(308, 71);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(93, 16);
            this.label27.TabIndex = 55;
            this.label27.Text = "Observações:";
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(19, 71);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(85, 16);
            this.label26.TabIndex = 54;
            this.label26.Text = "Reponsavel:";
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(308, 16);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(183, 16);
            this.label25.TabIndex = 53;
            this.label25.Text = "Data/Hora do Cancelamento:";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(19, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(160, 16);
            this.label24.TabIndex = 50;
            this.label24.Text = "Motivo do Cancelamento:";
            // 
            // txtObsCancelamento
            // 
            this.txtObsCancelamento.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObsCancelamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObsCancelamento.Location = new System.Drawing.Point(311, 90);
            this.txtObsCancelamento.Name = "txtObsCancelamento";
            this.txtObsCancelamento.Size = new System.Drawing.Size(229, 21);
            this.txtObsCancelamento.TabIndex = 51;
            // 
            // txtResponsavel
            // 
            this.txtResponsavel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResponsavel.Enabled = false;
            this.txtResponsavel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResponsavel.Location = new System.Drawing.Point(19, 90);
            this.txtResponsavel.Name = "txtResponsavel";
            this.txtResponsavel.Size = new System.Drawing.Size(165, 21);
            this.txtResponsavel.TabIndex = 50;
            // 
            // DtHrCancelamento
            // 
            this.DtHrCancelamento.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DtHrCancelamento.Enabled = false;
            this.DtHrCancelamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtHrCancelamento.Location = new System.Drawing.Point(311, 35);
            this.DtHrCancelamento.Name = "DtHrCancelamento";
            this.DtHrCancelamento.Size = new System.Drawing.Size(229, 21);
            this.DtHrCancelamento.TabIndex = 50;
            // 
            // MotivoCancelar
            // 
            this.MotivoCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MotivoCancelar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MotivoCancelar.FormattingEnabled = true;
            this.MotivoCancelar.Items.AddRange(new object[] {
            "Alteração na solicitação",
            "Duplicidade de ocorrência",
            "Erro de digitação",
            "Familiares recusa remoção",
            "Indisponibilidade de VTR",
            "Meios próprios",
            "Paciente de alta",
            "Paciente desistiu da ambulancia",
            "Paciente evadiu",
            "Recusado pelo enfermeiro do controle",
            "Recusado por parte da equipe de ambulacia",
            "Unidade cancelou",
            "Outro motivo"});
            this.MotivoCancelar.Location = new System.Drawing.Point(19, 35);
            this.MotivoCancelar.Name = "MotivoCancelar";
            this.MotivoCancelar.Size = new System.Drawing.Size(273, 21);
            this.MotivoCancelar.TabIndex = 50;
            // 
            // CancelarSolicitacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(224)))), ((int)(((byte)(173)))));
            this.ClientSize = new System.Drawing.Size(588, 216);
            this.Controls.Add(this.painelCancelar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CancelarSolicitacao";
            this.Text = "CancelarSolicitacao";
            this.painelCancelar.ResumeLayout(false);
            this.painelCancelar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel painelCancelar;
        private System.Windows.Forms.Button BtnConfirmando;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtObsCancelamento;
        private System.Windows.Forms.TextBox txtResponsavel;
        private System.Windows.Forms.TextBox DtHrCancelamento;
        private System.Windows.Forms.ComboBox MotivoCancelar;
    }
}