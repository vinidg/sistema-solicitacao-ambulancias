using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using db_transporte_sanitario;
using System.Data.Entity.SqlServer;

namespace Solicitacao_de_Ambulancias
{
    public partial class ConfirmaSolicitacao : Form
    {
        string TipoAM = null;
        string Agendamento = null;

        string pegaUnidade;     //para pegar o telefone com o nome da unidade
        string pegaUnidadeEnd;  //para pegar o endereco com o nome da unidade
        string Sexo, pegamotivo, Id;
        string Endereco1, UnidadeSelecionada, destino, origem;
        int IdSolicitacaoAmbulancia;
        int idPaciente;
        string UnidadeReagendamento;
        public ConfirmaSolicitacao()
        {
            InitializeComponent();

            pegarDadosDasAmbulancias();
            countparaSol();
            countparaSolAgendadas();
            countparaSolAgendadasPendentes();
            StartPosition = FormStartPosition.CenterScreen;
            Endereco();
            label3.Visible = false;
            dataAgendamento.Visible = false;
            this.Text = "Sistema de Solicitação de Ambulancias. Versão: " + appversion;
            AbasControle.SelectedTab = NovaSolicitacao;
            Detalhes.Text = "";
            AutoCompletar();
        }

        Version appversion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public void Limpar()
        {
            RbFemenino.Checked = false;
            RbMasculino.Checked = false;
            TipoAM = "";
            Agendamento = "";
            Obs.Text = "";
            label3.Visible = false;
            dataAgendamento.Visible = false;

            Btnagendasim.BackColor = Color.FromArgb(69, 173, 168);
            Btnagendasim.ForeColor = Color.FromArgb(229, 252, 194);
            Btnagendanao.BackColor = Color.FromArgb(69, 173, 168);
            Btnagendanao.ForeColor = Color.FromArgb(229, 252, 194);

            BtnAvancada.BackColor = Color.FromArgb(69, 173, 168);
            BtnAvancada.ForeColor = Color.FromArgb(229, 252, 194);
            BtnBasica.BackColor = Color.FromArgb(69, 173, 168);
            BtnBasica.ForeColor = Color.FromArgb(229, 252, 194);

            Btnagendanao.Enabled = true;
            Btnagendasim.Enabled = true;
            BtnAvancada.Enabled = true;
            BtnBasica.Enabled = true;
        }
        private void AbasControle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AbasControle.SelectedTab == AbasControle.TabPages["ReagendamentosTab"])
            {
                if (RespostasNegadas.Checked == true)
                {
                    puxarAgendadasNegadas();
                    RespostasNegadas.Checked = true;
                }
                else
                {
                    puxarAgendadasPendentes();
                    RespostaDoControle.Checked = true;
                }
            }
        }

        #region Incluir_solicitacao_confirma_solicitacao

        private void RegistrarSolicitacao()
        {
            InsercoesDoBanco IB = new InsercoesDoBanco();

            VerificarPontos(this);

            try
            {
                IB.inserirSolicitacaoDoPaciente(TipoAM, DateTime.Now, Agendamento, this.dataAgendamento.Value, this.txtNomeSolicitante.Text, this.CbLocalSolicita.Text, this.txtTelefone.Text,
                this.txtNomePaciente.Text, Sexo, this.txtIdade.Text, this.txtDiagnostico.Text, this.CbMotivoChamado.Text, this.CbTipoMotivoSelecionado.Text,
                this.Prioridade.Text, this.CbOrigem.Text, this.txtEnderecoOrigem.Text, this.CbDestino.Text, this.txtEnderecoDestino.Text, this.Obs.Text,
                0, System.Environment.UserName, DateTime.Now);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void VerificarPontos(Control container)
        {
            foreach (Control objeto in container.Controls)
            {
                if (objeto is TextBox)
                {
                    if (objeto.Text.IndexOf("'") == -1)
                        continue;
                    else
                        throw new Exception("Caracter ' é inválido!");

                }
                else if (objeto.Controls.Count > 0)
                {
                    VerificarPontos(objeto);
                }
            }
        }
        public void Endereco()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                CbLocalSolicita.DataSource = db.enderecos.OrderBy(x => x.NomeUnidade).ToList();
                CbLocalSolicita.ValueMember = "NomeUnidade";
                CbLocalSolicita.DisplayMember = "NomeUnidade";
                CbDestino.DataSource = db.enderecos.OrderBy(x => x.NomeUnidade).ToList();
                CbDestino.ValueMember = "NomeUnidade";
                CbDestino.DisplayMember = "NomeUnidade";
                CbOrigem.DataSource = db.enderecos.OrderBy(x => x.NomeUnidade).ToList();
                CbOrigem.ValueMember = "NomeUnidade";
                CbOrigem.DisplayMember = "NomeUnidade";
            }
        }
        public void unidade_telefone()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var telefoneDoEndereco = db.enderecos
                    .Where(e => e.NomeUnidade == pegaUnidade)
                    .Select(e => e.Telefone);

                txtTelefone.Text = telefoneDoEndereco.FirstOrDefault();

            }
        }
        private void unidade_Endereco()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var enderecoDoEnderecos = db.enderecos
                    .Where(e => e.NomeUnidade == pegaUnidadeEnd)
                    .Select(e => e.Endereco);

                Endereco1 = enderecoDoEnderecos.FirstOrDefault();
            }
        }
        private void Motivo()
        {
            //descobrir o que foi selecionado e criar uma variavel para ela
            if (CbMotivoChamado.Text == "ALTA HOSPITALAR")
            {
                pegamotivo = "ALTA_HOSPITALAR";
            }
            else if (CbMotivoChamado.Text == "AVALIAÇÃO DE MÉDICO ESPECIALISTA")
            {
                pegamotivo = "AVALIACAO_DE_MEDICO_ESPECIALISTA";
            }
            else if (CbMotivoChamado.Text == "AVALIAÇÃO DE PROFISSIONAL NÃO MÉDICO")
            {
                pegamotivo = "AVALIACAO_DE_PROFISSIONAL_NAO_MEDICO";
            }
            else if (CbMotivoChamado.Text == "CONSULTA AGENDADA")
            {
                pegamotivo = "CONSULTA_AGENDADA";
            }
            else if (CbMotivoChamado.Text == "DEMANDAS JUDICIAIS")
            {
                pegamotivo = "DEMANDA_JUDICIAL";
            }
            else if (CbMotivoChamado.Text == "EVENTO COMEMORATIVO")
            {
                pegamotivo = "EVENTO_COMEMORATIVO_DO_MUNICIPIO";
            }
            else if (CbMotivoChamado.Text == "EVENTO DE CULTURA, LAZER OU RELIGIÃO")
            {
                pegamotivo = "EVENTO_DE_CULTURA_LAZER_OU_RELIGIAO";
            }
            else if (CbMotivoChamado.Text == "EVENTO ESPORTIVO")
            {
                pegamotivo = "EVENTO_ESPORTIVO";
            }
            else if (CbMotivoChamado.Text == "EXAME AGENDADO")
            {
                pegamotivo = "EXAME_AGENDADO";
            }
            else if (CbMotivoChamado.Text == "EXAME DE URGÊNCIA")
            {
                pegamotivo = "EXAME_DE_URGENCIA";
            }
            else if (CbMotivoChamado.Text == "INTERNAÇÃO EM ENFERMARIA")
            {
                pegamotivo = "INTERNACAO_EM_ENFERMARIA";
            }
            else if (CbMotivoChamado.Text == "INTERNAÇÃO EM UTI")
            {
                pegamotivo = "INTERNACAO_EM_UTI";
            }
            else if (CbMotivoChamado.Text == "PROCEDIMENTO")
            {
                pegamotivo = "PROCEDIMENTO";
            }
            else if (CbMotivoChamado.Text == "RETORNO")
            {
                pegamotivo = "RETORNO";
            }
            else if (CbMotivoChamado.Text == "SALA VERMELHA/EMERGÊNCIA")
            {
                pegamotivo = "SALA_VERMELHA_EMERGENCIA";
            }
            else if (CbMotivoChamado.Text == "TRANSPORTE DE INSUMOS/PRODUTOS/MATERIAIS")
            {
                pegamotivo = "TRANSPORTE_DE_INSUMOS_PRODUTOS_MATERIAIS";
            }
            else if (CbMotivoChamado.Text == "TRANSPORTE DE PROFISSIONAIS")
            {
                pegamotivo = "TRANSPORTE_DE_PROFISSIONAIS";
            }
            else if (CbMotivoChamado.Text == "TRANSFERENCIA")
            {
                pegamotivo = "TRANSFERENCIA";
            }
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from r in db.referencias
                             orderby pegamotivo ascending
                             select r).ToList();
                CbTipoMotivoSelecionado.DataSource = query;
                CbTipoMotivoSelecionado.ValueMember = pegamotivo;
                CbTipoMotivoSelecionado.DisplayMember = pegamotivo;
            }

        }
        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            ClearComboBox();
            ClearTextBoxes();
            Limpar();
        }
        private void CbLocalSolicita_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            pegaUnidade = CbLocalSolicita.Text;
            unidade_telefone();
        }
        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (RbFemenino.Checked)
            {
                Sexo = "F";
            }
            else if (RbMasculino.Checked)
            {
                Sexo = "M";
            }

            if (String.IsNullOrEmpty(Agendamento) || String.IsNullOrEmpty(TipoAM))
            {

                MessageBox.Show("Marque a opção do tipo de ambulancia ou se é agendado !", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (String.IsNullOrEmpty(txtNomeSolicitante.Text) ||
            String.IsNullOrEmpty(CbLocalSolicita.Text) ||
            String.IsNullOrEmpty(txtTelefone.Text) ||
            String.IsNullOrEmpty(txtNomePaciente.Text) ||
            String.IsNullOrEmpty(txtIdade.Text) ||
            String.IsNullOrEmpty(txtDiagnostico.Text) ||
            String.IsNullOrEmpty(CbMotivoChamado.Text) ||
            String.IsNullOrEmpty(Sexo) ||
            String.IsNullOrEmpty(CbTipoMotivoSelecionado.Text) ||
            String.IsNullOrEmpty(CbOrigem.Text) ||
            String.IsNullOrEmpty(CbDestino.Text) ||
            String.IsNullOrEmpty(txtEnderecoOrigem.Text) ||
            String.IsNullOrEmpty(txtEnderecoDestino.Text))
            {

                MessageBox.Show("Verifique se algum campo esta vazio ou desmarcado !", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (dataAgendamento.Value <= DateTime.Now && Agendamento == "Sim")
            {
                MessageBox.Show("A data de agendamento não deve ser menor que a data atual", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (Agendamento == "Sim")
                {
                    if (CbLocalSolicita.Text.Contains("UPA"))
                    {
                        MessageBox.Show("Upas não devem agendar uma solicitação, entre em contato com o Transporte Sanitario", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    dataAgendamento.Value = DateTime.Now;
                }

                RegistrarSolicitacao();

                if (Agendamento == "Sim")
                {
                    DialogResult result1 = MessageBox.Show("Deseja usar as mesmas informações para solicitar outro agendamento ?",
                    "Atenção !",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 == DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        ClearTextBoxes();
                        ClearComboBox();
                        Limpar();
                    }
                }
                else
                {
                    ClearTextBoxes();
                    ClearComboBox();
                    Limpar();
                }
            }
        }
        private void BtnBasica_Click_1(object sender, EventArgs e)
        {
            label2.Visible = true;
            Btnagendanao.Visible = true;
            Btnagendasim.Visible = true;
            TipoAM = "Basica";

            if (BtnAvancada.BackColor == Color.FromArgb(69, 173, 168))
            {
                BtnBasica.BackColor = Color.FromArgb(229, 252, 194);
                BtnBasica.ForeColor = Color.FromArgb(69, 173, 168);
                BtnAvancada.ForeColor = Color.FromArgb(69, 173, 168);
                BtnAvancada.BackColor = Color.FromArgb(229, 252, 194);
            }
            BtnAvancada.BackColor = Color.FromArgb(229, 252, 194);
            BtnBasica.BackColor = Color.FromArgb(69, 173, 168);
            BtnBasica.ForeColor = Color.FromArgb(229, 252, 194);
            BtnAvancada.ForeColor = Color.FromArgb(69, 173, 168);
        }
        private void BtnAvancada_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            Btnagendanao.Visible = true;
            Btnagendasim.Visible = true;
            TipoAM = "Avancada";

            if (BtnBasica.BackColor == Color.FromArgb(69, 173, 168))
            {
                BtnAvancada.BackColor = Color.FromArgb(229, 252, 194);
                BtnAvancada.ForeColor = Color.FromArgb(69, 173, 168);
                BtnBasica.ForeColor = Color.FromArgb(69, 173, 168);
                BtnBasica.BackColor = Color.FromArgb(229, 252, 194);
            }
            BtnBasica.BackColor = Color.FromArgb(229, 252, 194);
            BtnAvancada.BackColor = Color.FromArgb(69, 173, 168);
            BtnAvancada.ForeColor = Color.FromArgb(229, 252, 194);
            BtnBasica.ForeColor = Color.FromArgb(69, 173, 168);
        }
        private void Btnagendanao_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            dataAgendamento.Visible = false;
            label4.Visible = true;
            label5.Visible = true;
            txtNomeSolicitante.Visible = true;
            label6.Visible = true;
            CbLocalSolicita.Visible = true;
            label7.Visible = true;
            txtTelefone.Visible = true;
            Agendamento = "Nao";
            if (Btnagendasim.BackColor == Color.FromArgb(69, 173, 168))
            {
                Btnagendasim.BackColor = Color.FromArgb(229, 252, 194);
                Btnagendasim.ForeColor = Color.FromArgb(69, 173, 168);
                Btnagendanao.BackColor = Color.FromArgb(69, 173, 168);
                Btnagendanao.ForeColor = Color.FromArgb(229, 252, 194);

            }

            Btnagendasim.BackColor = Color.FromArgb(229, 252, 194);
            Btnagendanao.BackColor = Color.FromArgb(69, 173, 168);
            Btnagendasim.ForeColor = Color.FromArgb(69, 173, 168);
            Btnagendanao.ForeColor = Color.FromArgb(229, 252, 194);
        }
        private void Btnagendasim_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            dataAgendamento.Visible = true;
            dataAgendamento.Focus();
            Agendamento = "Sim";

            if (Btnagendanao.BackColor == Color.FromArgb(69, 173, 168))
            {
                Btnagendanao.BackColor = Color.FromArgb(229, 252, 194);
                Btnagendanao.ForeColor = Color.FromArgb(69, 173, 168);
                Btnagendasim.ForeColor = Color.FromArgb(69, 173, 168);
                Btnagendasim.BackColor = Color.FromArgb(229, 252, 194);

            }
            Btnagendanao.BackColor = Color.FromArgb(229, 252, 194);
            Btnagendasim.BackColor = Color.FromArgb(69, 173, 168);
            Btnagendasim.ForeColor = Color.FromArgb(229, 252, 194);
            Btnagendanao.ForeColor = Color.FromArgb(69, 173, 168);
        }
        private void CbOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            pegaUnidadeEnd = CbOrigem.Text;
            unidade_Endereco();
            txtEnderecoOrigem.Text = Endereco1;
        }
        private void CbDestino_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            pegaUnidadeEnd = CbDestino.Text;
            unidade_Endereco();
            txtEnderecoDestino.Text = Endereco1;
        }
        private void CbMotivoChamado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CbTipoMotivoSelecionado.DataSource = null;
            CbTipoMotivoSelecionado.ValueMember = "";
            CbTipoMotivoSelecionado.DisplayMember = "";

            if (CbMotivoChamado.Text == "INTERNAÇÃO EM UTI" || CbMotivoChamado.Text == "SALA VERMELHA/EMERGÊNCIA" || String.IsNullOrEmpty(CbMotivoChamado.Text))
            {
                BtnAvancada.PerformClick();
                BtnAvancada.Enabled = false;
                BtnBasica.Enabled = false;
            }
            else
            {
                label2.Visible = true;
                Btnagendanao.Visible = true;
                Btnagendasim.Visible = true;
                TipoAM = "";
                BtnAvancada.Enabled = true;
                BtnBasica.Enabled = true;
                BtnAvancada.BackColor = Color.FromArgb(69, 173, 168);
                BtnAvancada.ForeColor = Color.FromArgb(229, 252, 194);
                BtnBasica.ForeColor = Color.FromArgb(229, 252, 194);
                BtnBasica.BackColor = Color.FromArgb(69, 173, 168);
            }
            Motivo();
        }
        private void txtIdade_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void txtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void ClearComboBox()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is ComboBox)
                        (control as ComboBox).SelectedIndex = -1;
                    else
                        func(control.Controls);
            };

            func(Controls);
        }
        private void Obs_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }
        private void txtNomePaciente_KeyUp(object sender, KeyEventArgs e)
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var autoCompletarDadosPaciente = db.solicitacoes_paciente
                .Where(a => a.Paciente == txtNomePaciente.Text)
                .Select(a => new { a.Genero, a.Idade }).FirstOrDefault();

                if (autoCompletarDadosPaciente != null)
                {

                    if (autoCompletarDadosPaciente.Genero == "M")
                    {
                        RbFemenino.Checked = false;
                        RbMasculino.Checked = true;
                    }
                    else
                    {
                        RbMasculino.Checked = false;
                        RbFemenino.Checked = true;
                    }


                    txtIdade.Text = autoCompletarDadosPaciente.Idade;
                }
            }
        }
        private void AutoCompletar()
        {
            RbFemenino.Checked = false;
            RbMasculino.Checked = false;
            txtIdade.Text = "";

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var autoCompletar = db.solicitacoes_paciente
                    .Select(a => a.Paciente).Distinct().ToArray();
                AutoCompleteStringCollection source = new AutoCompleteStringCollection();
                source.AddRange(autoCompletar);
                txtNomePaciente.AutoCompleteCustomSource = source;

            }
        }

        #endregion

        #region Todas_solicitacoes
        private void button2_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Selecione a unidade !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PainelSolicitacoes.Visible = true;
            UnidadeSelecionada = comboBox1.Text;
            SelectPacientes();
        }
        private void Horarios()
        {

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sa in db.solicitacoes_ambulancias
                             where sa.idSolicitacoes_Ambulancias == IdSolicitacaoAmbulancia
                             select new
                             {
                                 sa.DtHrCiencia,
                                 sa.DtHrCienciaReg,
                                 sa.DtHrChegadaOrigem,
                                 sa.DtHrChegadaOrigemReg,
                                 sa.DtHrSaidaOrigem,
                                 sa.DtHrSaidaOrigemReg,
                                 sa.DtHrChegadaDestino,
                                 sa.DtHrChegadaDestinoReg,
                                 sa.DtHrLiberacaoEquipe,
                                 sa.DtHrLiberacaoEquipeReg,
                                 sa.DtHrEquipePatio,
                                 sa.DtHrEquipePatioReg
                             }).DefaultIfEmpty().FirstOrDefault();

                if (query != null)
                {

                    if (query.DtHrCiencia != null)
                    {
                        txtHora.Text = query.DtHrCiencia;
                    }
                    if (query.DtHrChegadaOrigem != null)
                    {
                        txtHora2.Text = query.DtHrChegadaOrigem;
                    }
                    if (query.DtHrSaidaOrigem != null)
                    {
                        txtHora3.Text = query.DtHrSaidaOrigem;
                    }
                    if (query.DtHrChegadaDestino != null)
                    {
                        txtHora4.Text = query.DtHrChegadaDestino;
                    }
                    if (query.DtHrLiberacaoEquipe != null)
                    {
                        txtHora5.Text = query.DtHrLiberacaoEquipe;
                    }
                    if (query.DtHrEquipePatio != null)
                    {
                        txtHora6.Text = query.DtHrEquipePatio;
                    }
                    /*if (query.DtHrCienciaReg != null)
                    {
                        txtAlterador.Text = query.DtHrCienciaReg;
                    }
                    if (query.DtHrChegadaOrigemReg != null)
                    {
                        txtAlterador2.Text = query.DtHrChegadaOrigemReg;
                    }
                    if (query.DtHrSaidaOrigemReg != null)
                    {
                        txtAlterador3.Text = query.DtHrSaidaOrigemReg;
                    }
                    if (query.DtHrChegadaDestinoReg != null)
                    {
                        txtAlterador4.Text = query.DtHrChegadaDestinoReg;
                    }
                    if (query.DtHrLiberacaoEquipeReg != null)
                    {
                        txtAlterador5.Text = query.DtHrLiberacaoEquipeReg;
                    }
                    if (query.DtHrEquipePatioReg != null)
                    {
                        txtAlterador6.Text = query.DtHrEquipePatioReg;
                    }*/
                }
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            SelectPacientes();
        }
        private void label59_Click(object sender, EventArgs e)
        {
            PainelSolicitacoes.Visible = false;
        }
        private void SelectPacientes()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on sp.idPaciente_Solicitacoes
                             equals sa.idSolicitacoesPacientes into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 sp.DtHrdoAgendamento,
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty().ToList();

                Lista.DataSource = query;
                Lista.Refresh();
                Lista.ClearSelection();
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;

            }
        }

        #region Filtro_todas_solicitacoes
        private void hoje_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                             sp.LocalSolicitacao == UnidadeSelecionada
                             && (sp.DtHrdoInicio.Value.Day == DateTime.Now.Day && sp.DtHrdoInicio.Value.Month == DateTime.Now.Month && sp.DtHrdoInicio.Value.Year == DateTime.Now.Year)
                             || (sp.DtHrdoAgendamento.Value.Day == DateTime.Now.Day && sp.DtHrdoAgendamento.Value.Month == DateTime.Now.Month && sp.DtHrdoAgendamento.Value.Year == DateTime.Now.Year)
                             || (sag.DtHrAgendamento.Value.Day == DateTime.Now.Day && sag.DtHrAgendamento.Value.Month == DateTime.Now.Month && sag.DtHrAgendamento.Value.Year == DateTime.Now.Year)
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }

        }
        
        private void Ontem_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            DateTime data = DateTime.Now;
            DateTime ontem = DateTime.Now.AddDays(-1);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && (sp.DtHrdoInicio >= ontem && sp.DtHrdoInicio <= data) || (sp.DtHrdoAgendamento >= ontem && sp.DtHrdoAgendamento <= data) || (sag.DtHrAgendamento >= ontem && sag.DtHrAgendamento <= data)

                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }
        }
        private void dias2_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            DateTime data = DateTime.Now;
            DateTime dias2 = DateTime.Now.AddDays(-2);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                              && (sp.DtHrdoInicio >= dias2 && sp.DtHrdoInicio <= data) || (sp.DtHrdoAgendamento >= dias2 && sp.DtHrdoAgendamento <= data) || (sag.DtHrAgendamento >= dias2 && sag.DtHrAgendamento <= data)
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }

        }
        private void dias5_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            DateTime data = DateTime.Now;
            DateTime dias5 = DateTime.Now.AddDays(-5);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && (sp.DtHrdoInicio >= dias5 && sp.DtHrdoInicio <= data) || (sp.DtHrdoAgendamento >= dias5 && sp.DtHrdoAgendamento <= data) || (sag.DtHrAgendamento >= dias5 && sag.DtHrAgendamento <= data)
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }
        }
        private void semana1_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            DateTime data = DateTime.Now;
            DateTime dias7 = DateTime.Now.AddDays(-7);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && (sp.DtHrdoInicio >= dias7 && sp.DtHrdoInicio <= data) || (sp.DtHrdoAgendamento >= dias7 && sp.DtHrdoAgendamento <= data) || (sag.DtHrAgendamento >= dias7 && sag.DtHrAgendamento <= data)
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }
        }
        private void semana2_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            DateTime data = DateTime.Now;
            DateTime dias14 = DateTime.Now.AddDays(-14);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && (sp.DtHrdoInicio >= dias14 && sp.DtHrdoInicio <= data) || (sp.DtHrdoAgendamento >= dias14 && sp.DtHrdoAgendamento <= data) || (sag.DtHrAgendamento >= dias14 && sag.DtHrAgendamento <= data)
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }
        }
        private void mes1_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                             sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                             &&
                             (SqlFunctions.DatePart("month", sp.DtHrdoInicio) == mes && SqlFunctions.DatePart("year", sp.DtHrdoInicio) == ano) ||
                             (SqlFunctions.DatePart("month", sp.DtHrdoAgendamento) == mes && SqlFunctions.DatePart("year", sp.DtHrdoAgendamento) == ano) ||
                             (SqlFunctions.DatePart("month", sag.DtHrAgendamento) == mes && SqlFunctions.DatePart("year", sag.DtHrAgendamento) == ano)
                             orderby
                                sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }

        }
        private void ano1_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            this.Cursor = Cursors.WaitCursor;
            int ano = DateTime.Now.Year;
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             join sag in db.solicitacoes_agendamentos
                             on sp.idPaciente_Solicitacoes
                             equals sag.idSolicitacao_paciente into spsag_join
                             from sag in spsag_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && (SqlFunctions.DatePart("year", sp.DtHrdoInicio) == ano) ||
                               (SqlFunctions.DatePart("year", sp.DtHrdoAgendamento) == ano) ||
                               (SqlFunctions.DatePart("year", sag.DtHrAgendamento) == ano)
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 DtHrdoAgendamento = (sag.DtHrAgendamento == null ? (sp.Agendamento == "Sim" ? sp.DtHrdoAgendamento : default(DateTime?)) : sag.DtHrAgendamento),
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
                this.Cursor = Cursors.Default;
            }
        }
        private void agendadas_ValueChanged(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into c_join
                             from ca in c_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada
                               && sp.Agendamento == "Sim"
                               && SqlFunctions.DateDiff("day", agendadas.Value, sp.DtHrdoAgendamento) == 0
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 sp.DtHrdoAgendamento,
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;

            }
        }
        private void reagendadas_ValueChanged(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into spsa_join
                             from sa in spsa_join.DefaultIfEmpty()
                             join ca in db.cancelados_pacientes
                             on sp.idPaciente_Solicitacoes
                             equals ca.idPaciente into spca_join
                             from ca in spca_join.DefaultIfEmpty()
                             join saa in db.solicitacoes_agendamentos
                             on sp.idReagendamento equals saa.idSolicitacaoAgendamento into spsaaa_join
                             from saa in spsaaa_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && SqlFunctions.DateDiff("day", reagendadas.Value, saa.DtHrAgendamento) == 0 && sp.Agendamento == "Sim"
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
                                 Status = (sp.AmSolicitada == 0 ? "AGUARDANDO TRANSPORTE" : (ca.idPaciente == null ? (sa.SolicitacaoConcluida == 1 ? "TRANSPORTE REALIZADO" : (sa.SolicitacaoConcluida == null ? "TRANSPORTE EM ANDAMENTO" : "ERRO")) : "TRANSPORTE CANCELADO")),
                                 saa.DtHrAgendamento,
                                 sp.Paciente,
                                 sp.LocalSolicitacao,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 Tipo = sp.TipoSolicitacao,
                                 Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.NomeSolicitante,
                                 sp.Telefone,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico
                             }).DefaultIfEmpty();

                Lista.DataSource = query.OrderBy(s => s.DtHrdoInicio).ToList();
                Lista.Refresh();
                Lista.ClearSelection();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;

            }
        }

        #endregion

        private void Lista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                txtHora.Text = "";
                txtHora2.Text = "";
                txtHora3.Text = "";
                txtHora4.Text = "";
                txtHora5.Text = "";
                txtHora6.Text = "";

                if (Lista.Rows[e.RowIndex].Cells["idSolicitacoes_Ambulancias"].Equals(0) || Lista.Rows[e.RowIndex].Cells["idSolicitacoes_Ambulancias"].Equals(null) ||
                    String.IsNullOrEmpty(Lista.Rows[e.RowIndex].Cells["idSolicitacoes_Ambulancias"].Value as String) && Lista.Rows[e.RowIndex].Cells["Status"].Value.ToString() == "AGUARDANDO TRANSPORTE")
                {
                    Status.Text = "Aguardando vaga";
                    IdSolicitacaoAmbulancia = 0;
                }
                else if (Lista.Rows[e.RowIndex].Cells["Status"].Value.ToString() == "TRANSPORTE REALIZADO" || Lista.Rows[e.RowIndex].Cells["Status"].Value.ToString() == "TRANSPORTE CANCELADO")
                {
                    Status.Text = "Solicitação encerrada";
                    IdSolicitacaoAmbulancia = Convert.ToInt32(Lista.Rows[e.RowIndex].Cells["idSolicitacoes_Ambulancias"].Value);
                }
                else
                {
                    Status.Text = "Solicitação à caminho";
                    IdSolicitacaoAmbulancia = Convert.ToInt32(Lista.Rows[e.RowIndex].Cells["idSolicitacoes_Ambulancias"].Value);
                }
                if (e.RowIndex > -1)
                {

                    Id = Lista.Rows[e.RowIndex].Cells["Id"].Value.ToString();

                    if (IdSolicitacaoAmbulancia != 0)
                    {
                        Horarios();
                    }

                    origem = Lista.Rows[e.RowIndex].Cells["Origem"].Value.ToString();
                    destino = Lista.Rows[e.RowIndex].Cells["Destino"].Value.ToString();

                    lbDestino.Text = destino;
                    lbOrigem.Text = origem;
                    IdPacienteLabel.Text = Id;
                    NomePacienteLabel.Text = Lista.Rows[e.RowIndex].Cells["Paciente"].Value.ToString();
                }
            }
        }
        private void Lista_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && e.Value.Equals("AGUARDANDO TRANSPORTE"))
            {
                Lista.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            if (e.Value != null && e.Value.Equals("TRANSPORTE EM ANDAMENTO"))
            {
                Lista.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
        }
        private void AtualizarBtn_Click(object sender, EventArgs e)
        {
            pegarDadosDasAmbulancias();
        }

        #endregion

        #region Status_ambulancia

        string AvisoReagendamento = "Número de solicitações que o Transporte Sanitario irá Aceitar ou Negar !";
        public void pegarDadosDasAmbulancias()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var queryUsb = (from am in db.ambulancia
                                join sa in db.solicitacoes_ambulancias
                                on new { idAmbulanciaSol = am.idAmbulancia, SolicitacaoConcluida = 0 }
                                equals new { sa.idAmbulanciaSol, SolicitacaoConcluida = (int)sa.SolicitacaoConcluida } into sa_join
                                from sa in sa_join.DefaultIfEmpty()
                                join sp in db.solicitacoes_paciente
                                on new { idSolicitacoesPacientes = (int)sa.idSolicitacoesPacientes }
                                equals new { idSolicitacoesPacientes = sp.idPaciente_Solicitacoes } into sp_join
                                from sp in sp_join.DefaultIfEmpty()
                                where am.TipoAM == "BASICO" && am.Desativado == 0
                                orderby am.NomeAmbulancia ascending
                                select new
                                {
                                    am.idAmbulancia,
                                    Ambulancia = am.NomeAmbulancia,
                                    Status = sa.Status,
                                    StatusE = am.StatusAmbulancia,
                                    idPaciente = sa.idSolicitacoesPacientes,
                                    Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                    Idade = sp.Idade,
                                    Origem = sp.Origem,
                                    Destino = sp.Destino
                                }).ToList();

                listaUsb.DataSource = queryUsb;
                listaUsb.ClearSelection();

                var queryUsa = (from am in db.ambulancia
                                join sa in db.solicitacoes_ambulancias
                                on new { idAmbulanciaSol = am.idAmbulancia, SolicitacaoConcluida = 0 }
                                equals new { sa.idAmbulanciaSol, SolicitacaoConcluida = (int)sa.SolicitacaoConcluida } into sa_join
                                from sa in sa_join.DefaultIfEmpty()
                                join sp in db.solicitacoes_paciente on new { idSolicitacoesPacientes = (int)sa.idSolicitacoesPacientes } equals new { idSolicitacoesPacientes = sp.idPaciente_Solicitacoes } into sp_join
                                from sp in sp_join.DefaultIfEmpty()
                                where am.TipoAM == "AVANCADO" && am.Desativado == 0
                                orderby am.NomeAmbulancia ascending
                                select new
                                {
                                    am.idAmbulancia,
                                    Ambulancia = am.NomeAmbulancia,
                                    Status = sa.Status,
                                    StatusE = am.StatusAmbulancia,
                                    idPaciente = sa.idSolicitacoesPacientes,
                                    Prioridade = (sp.Prioridade.Contains("P0") ? "P0" : (sp.Prioridade.Contains("P1") ? "P1" : (sp.Prioridade.Contains("P2") ? "P2" : (sp.Prioridade.Contains("P3") ? "P3" : "SP")))),
                                    Idade = sp.Idade,
                                    Origem = sp.Origem,
                                    Destino = sp.Destino
                                }).ToList();

                listaUsa.DataSource = queryUsa;
                listaUsa.ClearSelection();

            }
            listaUsa.Columns[0].Visible = false;
            listaUsb.Columns[0].Visible = false;
            listaUsa.Columns["idPaciente"].Visible = false;
            listaUsb.Columns["idPaciente"].Visible = false;
            listaUsa.Columns["StatusE"].Width = 0;
            listaUsb.Columns["StatusE"].Width = 0;

            this.listaUsa.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsa.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsa.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsa.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsa.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.listaUsa.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.listaUsb.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsb.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsb.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsb.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsb.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.listaUsb.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            label28.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        private void countparaSol()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.AmSolicitada == 0 && sp.Agendamento == "Nao" && sp.Registrado == "Sim"
                             select sp.idPaciente_Solicitacoes).Count();

                solPendentes.Text = query.ToString();
            }
        }
        private void countparaSolAgendadas()
        {
            //CONTA AS solicitacoes agendadas
            using (DAHUEEntities db = new DAHUEEntities())
            {

                var query = (from sp in db.solicitacoes_paciente
                             join saa in db.solicitacoes_agendamentos
                             on sp.idReagendamento equals saa.idSolicitacaoAgendamento
                             where sp.Agendamento == "Sim" &&
                             sp.AmSolicitada == 0 &&
                             sp.Registrado == "Sim" &&
                             SqlFunctions.DateDiff("day", DateTime.Now, saa.DtHrAgendamento) == 0
                             select sp.idPaciente_Solicitacoes).Count();

                solAgendadasHoje.Text = query.ToString();
            }
        }
        private void countparaSolAgendadasPendentes()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.AmSolicitada == 0 && sp.Agendamento == "Sim"
                             && sp.Registrado == "Aguardando resposta do controle"
                             select sp.idPaciente_Solicitacoes).Count();

                solAgendadasPendentes.Text = query.ToString();
            }

        }

        private void listaUsb_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && e.Value.Equals("BLOQUEADA"))
            {
                listaUsb.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(0, 122, 181);
                listaUsb.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
            else if (e.Value != null && e.Value.Equals("OCUPADA"))
            {
                listaUsb.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(224, 62, 54);
                listaUsb.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
            else if (e.Value != null && e.Value.Equals("DISPONIVEL"))
            {
                listaUsb.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(46, 172, 109);
                listaUsb.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
        }
        private void listaUsa_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && e.Value.Equals("BLOQUEADA"))
            {
                listaUsa.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(0, 122, 181);
                listaUsa.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
            else if (e.Value != null && e.Value.Equals("OCUPADA"))
            {
                listaUsa.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(224, 62, 54);
                listaUsa.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
            else if (e.Value != null && e.Value.Equals("DISPONIVEL"))
            {
                listaUsa.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(46, 172, 109);
                listaUsa.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
        }
        private void panel7_MouseEnter(object sender, EventArgs e)
        {
            Detalhes.Text = AvisoReagendamento;
        }
        private void label33_MouseEnter(object sender, EventArgs e)
        {
            Detalhes.Text = AvisoReagendamento;
        }
        private void solAgendadasPendentes_MouseEnter(object sender, EventArgs e)
        {
            Detalhes.Text = AvisoReagendamento;
        }
        private void panel7_MouseLeave(object sender, EventArgs e)
        {
            Detalhes.Text = "";
        }
        private void solAgendadasPendentes_MouseLeave(object sender, EventArgs e)
        {
            Detalhes.Text = "";
        }
        private void label33_MouseLeave(object sender, EventArgs e)
        {
            Detalhes.Text = "";
        }

        #endregion

        #region Reagendamentos

        public void puxarAgendadasNegadas()
        {
            int zero = 0;
            var final = Calendario.SelectionRange.End;
            var comeco = Calendario.SelectionRange.Start;
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = from sp in db.solicitacoes_paciente
                            join sa in db.solicitacoes_ambulancias
                            on sp.idPaciente_Solicitacoes
                            equals sa.idSolicitacoesPacientes into b_join
                            from sa in b_join.DefaultIfEmpty()
                            where sp.AmSolicitada == zero &&
                            sp.Agendamento == "Sim" &&
                            SqlFunctions.DateDiff("day", final, sp.DtHrdoAgendamento) == 0 &&
                            sp.Registrado == "Aguardando resposta do solicitante" &&
                            sp.LocalSolicitacao == UnidadeReagendamento
                            select new
                            {
                                ID = sp.idPaciente_Solicitacoes,
                                idAm = sa.idSolicitacoes_Ambulancias,
                                sp.Paciente,
                                Tipo = sp.TipoSolicitacao,
                                sp.DtHrdoInicio,
                                sp.LocalSolicitacao,
                                sp.Agendamento,
                                sp.DtHrdoAgendamento,
                                sp.Prioridade,
                                sp.Motivo,
                                sp.Origem,
                                sp.Destino
                            };
                var quertCont = query.Count();
                var queryAmbu = query.ToList();
                ListaAgendados.Columns["idAm"].Visible = false;
                RespostasNegadas.Text = "Respostas negadas (" + quertCont + ")";
                ListaAgendados.DataSource = queryAmbu;
                ListaAgendados.ClearSelection();

            }
        }
        public void puxarAgendadasPendentes()
        {

            int zero = 0;
            var data = Calendario.SelectionRange.End;
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = from sp in db.solicitacoes_paciente
                            join sa in db.solicitacoes_ambulancias
                            on sp.idPaciente_Solicitacoes
                            equals sa.idSolicitacoesPacientes into b_join
                            from sa in b_join.DefaultIfEmpty()
                            where sp.AmSolicitada == zero &&
                            sp.Agendamento == "Sim" &&
                            SqlFunctions.DateDiff("day", data, sp.DtHrdoAgendamento) == 0 &&
                            sp.Registrado == "Aguardando resposta do controle" &&
                            sp.LocalSolicitacao == UnidadeReagendamento
                            select new
                            {
                                ID = sp.idPaciente_Solicitacoes,
                                idAm = sa.idSolicitacoes_Ambulancias,
                                sp.Paciente,
                                Tipo = sp.TipoSolicitacao,
                                sp.DtHrdoInicio,
                                sp.LocalSolicitacao,
                                sp.Agendamento,
                                sp.DtHrdoAgendamento,
                                sp.Prioridade,
                                sp.Motivo,
                                sp.Origem,
                                sp.Destino
                            };

                var queryAmbu = query.ToList();
                var querycont = query.Count();
                RespostaDoControle.Text = "Solicitações agendadas (" + querycont + ")";
                ListaAgendados.DataSource = queryAmbu;
                ListaAgendados.Columns["idAm"].Visible = false;
                ListaAgendados.ClearSelection();

            }
        }
        private void ListaAgendados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                idPaciente = Convert.ToInt32(ListaAgendados.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                IdSolicitacaoAmbulancia = Convert.ToInt32(ListaAgendados.Rows[e.RowIndex].Cells["idAm"].Value.ToString());
                using (DAHUEEntities db = new DAHUEEntities())
                {
                    var query = (from sp in db.solicitacoes_paciente
                                 join saa in db.solicitacoes_agendamentos
                                 on sp.idReagendamento equals saa.idSolicitacaoAgendamento into saa_sp_join
                                 from saa in saa_sp_join.DefaultIfEmpty()
                                 where sp.idPaciente_Solicitacoes == idPaciente
                                 select new { sp, saa }).FirstOrDefault();

                    CodigoPacienteReagendamento.Text = query.sp.idPaciente_Solicitacoes.ToString();
                    TipoAmbulanciaReagendamentos.Text = query.sp.TipoSolicitacao;
                    dataAberturaReagendamentos.Text = query.sp.DtHrdoInicio.ToString();
                    dataAgendamentoReagendamentos.Text = query.sp.DtHrdoAgendamento.ToString();
                    solicitanteReagendamentos.Text = query.sp.NomeSolicitante;
                    localReagendamentos.Text = query.sp.LocalSolicitacao;
                    telefoneReagendamentos.Text = query.sp.Telefone;
                    pacienteReagendamentos.Text = query.sp.Paciente;
                    if (query.sp.Genero == "F")
                    {
                        femininoReagendamentos.Checked = true;
                    }
                    else
                    {
                        masculinoReagendamentos.Checked = true;
                    }
                    idadeReagendamentos.Text = query.sp.Idade;
                    diagnosticosReagendamentos.Text = query.sp.Diagnostico;
                    motivoReagendamentos.Text = query.sp.Motivo;
                    tipoMotivoReagendamentos.Text = query.sp.SubMotivo;
                    prioridadeReagendamentos.Text = query.sp.Prioridade;
                    origemReagendamentos.Text = query.sp.Origem;
                    EnderecoOrigemReagendamentos.Text = query.sp.EnderecoOrigem;
                    destinoReagendamentos.Text = query.sp.Destino;
                    EnderecoDestinoReagendamentos.Text = query.sp.EnderecoDestino;
                    ObsReagendamentos.Text = query.sp.ObsGerais;
                    if (query.saa != null)
                    {
                        dtHrReagendamento.Text = query.saa.DtHrAgendamento.ToString();
                    }

                }

            }
        }
        private void EntrarReagendamentos_Click(object sender, EventArgs e)
        {
            if (RespostasNegadas.Checked == true)
            {
                puxarAgendadasNegadas();
                RespostasNegadas.Checked = true;
            }
            else
            {
                puxarAgendadasPendentes();
                RespostaDoControle.Checked = true;
            }
            UnidadeReagendamento = unidadesReagenda.Text;
            painelReagendamento.Visible = true;
        }
        private void label62_Click(object sender, EventArgs e)
        {
            painelReagendamento.Visible = false;
        }
        private void Calendario_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (RespostasNegadas.Checked == true)
            {
                puxarAgendadasNegadas();
            }
            else if (RespostaDoControle.Checked == true)
            {
                puxarAgendadasPendentes();
            }
        }
        private void Encaminhados_Click(object sender, EventArgs e)
        {
            puxarAgendadasPendentes();
            BtnReagendar.Visible = false;
        }
        private void Respondidos_Click(object sender, EventArgs e)
        {
            puxarAgendadasNegadas();
            BtnReagendar.Visible = true;
        }
        private void BtnReagendar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CodigoPacienteReagendamento.Text).Equals(false))
            {
                this.ClearTextBoxes();
                this.ClearComboBox();
                this.CodigoPacienteReagendamento.Text = "";
                this.dtHrReagendamento.Text = "";
                this.RbFemenino.Checked = false;
                this.RbMasculino.Checked = false;
                this.Obs.Text = "";
                Reagendar re = new Reagendar(DataHrAgendamento.Text, idPaciente);
                re.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione a solicitação que deseja reagendar !", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TodosReagendamentos_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CodigoPacienteReagendamento.Text).Equals(false))
            {
                Reagedamentos re = new Reagedamentos(idPaciente);
                re.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione a solicitação que deseja ver o histórico de reagendamentos !", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CancelarReagendamento_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CodigoPacienteReagendamento.Text).Equals(false) || CodigoPacienteReagendamento.Text != "ID")
            {
                CancelarSolicitacao cas = new CancelarSolicitacao(Convert.ToInt32(CodigoPacienteReagendamento.Text),IdSolicitacaoAmbulancia);
                cas.ShowDialog();
                SelectPacientesParaCancelar();
                ClearTextBoxes();
                ClearComboBox();
            }
            else
            {
                MessageBox.Show("Não há nenhuma solicitação selecionada !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion

        #region Cancelar_solicitacoes
        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            if (idPaciente != 0)
            {
                CancelarSolicitacao cas = new CancelarSolicitacao(idPaciente, IdSolicitacaoAmbulancia);
                cas.ShowDialog();
                SelectPacientesParaCancelar();
                ClearTextBoxes();
                ClearComboBox();
            }
            else
            {
                MessageBox.Show("Não há nenhuma solicitação selecionada !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void ListaCancelar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                idPaciente = Convert.ToInt32(ListaCancelar.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                if (ListaCancelar.Rows[e.RowIndex].Cells["idAm"].Equals(0) || ListaCancelar.Rows[e.RowIndex].Cells["idAm"].Equals(null))
                {
                    IdSolicitacaoAmbulancia = Convert.ToInt32(ListaCancelar.Rows[e.RowIndex].Cells["idAm"].Value.ToString());
                }
                else
                {
                    IdSolicitacaoAmbulancia = 0;
                }
                using (DAHUEEntities db = new DAHUEEntities())
                {
                    var query = (from sp in db.solicitacoes_paciente
                                 where sp.idPaciente_Solicitacoes == idPaciente
                                 select sp).FirstOrDefault();

                    CodigoId.Text = query.idPaciente_Solicitacoes.ToString();
                    Tipo.Text = query.TipoSolicitacao;
                    DataInicio.Text = query.DtHrdoInicio.ToString();
                    DataHrAgendamento.Text = query.DtHrdoAgendamento.ToString();
                    NomeSolicitante.Text = query.NomeSolicitante;
                    LocalSolicitacao.Text = query.LocalSolicitacao;
                    Telefone.Text = query.Telefone;
                    NomePaciente.Text = query.Paciente;
                    if (query.Genero == "F")
                    {
                        RbFemenino.Checked = true;
                    }
                    else
                    {
                        RbMasculino.Checked = true;
                    }
                    Idade.Text = query.Idade;
                    Diagnostico.Text = query.Diagnostico;
                    MotivoChamado.Text = query.Motivo;
                    TipoMotivoSelecionado.Text = query.SubMotivo;
                    PrioridadeCancelar.Text = query.Prioridade;
                    COrigem.Text = query.Origem;
                    CEnderecoOrigem.Text = query.EnderecoOrigem;
                    CDestino.Text = query.Destino;
                    CEnderecoDestino.Text = query.EnderecoDestino;
                    CObs.Text = query.ObsGerais;

                }

            }
        }
        private void SelectPacientesParaCancelar()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on sp.idPaciente_Solicitacoes
                             equals sa.idSolicitacoesPacientes into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where sp.LocalSolicitacao == UnidadeSelecionada &&
                             sp.AmSolicitada == 0
                             orderby sp.DtHrdoInicio descending
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idAm = sa.idSolicitacoes_Ambulancias,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrdoAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Prioridade,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).DefaultIfEmpty().ToList();

                ListaCancelar.DataSource = query;
                ListaCancelar.Columns["idAm"].Visible = false;
                ListaCancelar.Refresh();

            }
        }
        private void EntrarCancelar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(SelecionarUnidade.Text))
            {
                MessageBox.Show("Selecione a unidade !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PainelCancelar.Visible = true;
            UnidadeSelecionada = SelecionarUnidade.Text;
            SelectPacientesParaCancelar();
        }
        private void AtualizarCancelar_Click(object sender, EventArgs e)
        {
            ListaCancelar.DataSource = "";
            SelectPacientesParaCancelar();
        }
        private void label37_Click(object sender, EventArgs e)
        {
            PainelCancelar.Visible = false;
        }

        #endregion

    }
}

