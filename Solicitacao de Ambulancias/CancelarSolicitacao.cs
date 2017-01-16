using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using db_transporte_sanitario;

namespace Solicitacao_de_Ambulancias
{
    public partial class CancelarSolicitacao : Form
    {
        int idpaciente, idSolicitacaoAmbulancias;
        public CancelarSolicitacao(int idPaciente, int idSolicitacaoAmbulancia)
        {
            InitializeComponent();
            txtResponsavel.Text = System.Environment.UserName;
            idpaciente = idPaciente;
            idSolicitacaoAmbulancias = idSolicitacaoAmbulancia;
        }

        private void BtnConfirmando_Click(object sender, EventArgs e)
        {
            if (idSolicitacaoAmbulancias != 0)
            {
                using (DAHUEEntities db = new DAHUEEntities())
                {
                    var query = (from sa in db.solicitacoes_ambulancias
                                 where sa.idSolicitacoes_Ambulancias == idSolicitacaoAmbulancias &&
                                 sa.SolicitacaoConcluida == 0
                                 select sa).Count();
                    if (query >= 1)
                    {
                        MessageBox.Show("Paciente está em transporte, necessário entrar em contato com o Controle para cancelar !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Dispose();
                    }
                }

            }
                DialogResult result1 = MessageBox.Show("Deseja cancelar a solicitação do paciente na ambulancia ?",
                "Atenção !",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result1 == DialogResult.Yes)
                {
                    cancelar();
                    this.Dispose();
                }
        }
        private void cancelar()
        {
            try
            {
                InsercoesDoBanco i = new InsercoesDoBanco();
                i.cancelarSolicitacao(idSolicitacaoAmbulancias,idpaciente,MotivoCancelar.Text, txtResponsavel.Text, txtObsCancelamento.Text);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
