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
        int idpaciente;
        public CancelarSolicitacao(int idPaciente)
        {
            InitializeComponent();
            txtResponsavel.Text = System.Environment.UserName;
            idpaciente = idPaciente;
        }

        private void BtnConfirmando_Click(object sender, EventArgs e)
        {
            using(DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sa in db.solicitacoes_ambulancias
                             where sa.idSolicitacoesPacientes == idpaciente &&
                             sa.SolicitacaoConcluida == 0
                             select sa).Count();
                if(query >= 1)
                {
                    MessageBox.Show("Paciente está em transporte, necessário entrar em contato com o Controle para cancelar !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Dispose();
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
                using (DAHUEEntities db = new DAHUEEntities())
                {
                    cancelados_pacientes cancelados = new cancelados_pacientes();
                    cancelados.idPaciente = idpaciente;
                    cancelados.idSolicitacaoAM = 0;
                    cancelados.MotivoCancelamento = MotivoCancelar.Text;
                    cancelados.DtHrCancelamento = DtHrCancelamento.Text;
                    cancelados.ResposavelCancelamento = txtResponsavel.Text;
                    cancelados.ObsCancelamento = txtObsCancelamento.Text;

                    db.cancelados_pacientes.Add(cancelados);

                    solicitacoes_paciente sp = db.solicitacoes_paciente.First(s => s.idPaciente_Solicitacoes == idpaciente);
                    sp.AmSolicitada = 1;

                    if(idpaciente != 0){
                        solicitacoes_ambulancias sa = db.solicitacoes_ambulancias.First(s => s.idSolicitacoesPacientes == idpaciente);
                        sa.SolicitacaoConcluida = 1;
                    }

                    db.SaveChanges();

                    MessageBox.Show("Solicitação cancelada com sucesso !!!");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
