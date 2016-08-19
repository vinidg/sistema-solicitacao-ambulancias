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
        
        public ConfirmaSolicitacao()
        {
            InitializeComponent();

            pegarDadosDasAmbulancias();
            countparaSol();
            countparaSolAgendadas();
            txtAtendMarcado.Text = DateTime.Now.ToString();
            StartPosition = FormStartPosition.CenterScreen;
            Endereco();
            label3.Visible = false;
            txtAtendMarcado.Visible = false;
            Limpar();
            update();
            this.Text = "Sistema de Solicitação de Ambulancias. Versão: " + appverion;
            AbasControle.SelectedTab = Aba2;
            
        }
       
        public void update()
        {
            Update updatando = new Update();
            updatando.up();
            
            if (updatando.Yn == true)
            {
                Environment.Exit(1);
            }
            
        }
        Version appverion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        private void BtnBasica_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            Btnagendanao.Visible = true;
            Btnagendasim.Visible = true;
            TipoAM = "Basica";

            if (BtnAvancada.BackColor == Color.PaleTurquoise)
            {
                BtnBasica.BackColor = Color.PaleTurquoise;
                BtnBasica.ForeColor = Color.Teal;
                BtnAvancada.ForeColor = Color.Teal;
                BtnAvancada.BackColor = Color.PaleTurquoise;
            }
            BtnAvancada.BackColor = Color.PaleTurquoise;
            BtnBasica.BackColor = Color.Teal;
            BtnBasica.ForeColor = Color.PaleTurquoise;
            BtnAvancada.ForeColor = Color.Teal;
        }
        private void BtnAvancada_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            Btnagendanao.Visible = true;
            Btnagendasim.Visible = true;
            TipoAM = "Avancada";

            if (BtnBasica.BackColor == Color.PaleTurquoise)
            {
                BtnAvancada.BackColor = Color.PaleTurquoise;
                BtnAvancada.ForeColor = Color.Teal;
                BtnBasica.ForeColor = Color.Teal;
                BtnBasica.BackColor = Color.PaleTurquoise;
            }
            BtnBasica.BackColor = Color.PaleTurquoise;
            BtnAvancada.BackColor = Color.Teal;
            BtnAvancada.ForeColor = Color.PaleTurquoise;
            BtnBasica.ForeColor = Color.Teal;
        }


        public void Limpar()
        {
     
            txtAtendMarcado.Text = "";
            txtNomeSolicitante.Text = "";
            CbLocalSolicita.Text = "";
            txtTelefone.Text = "";
            txtNomePaciente.Text = "";
            RbFemenino.Checked = false;
            RbMasculino.Checked = false;
            txtIdade.Text = "";
            txtDiagnostico.Text = "";
            CbMotivoChamado.Text = "";
            CbTipoMotivoSelecionado.Text = "";
            CbAtendimentoPrioridade.Checked = false;
            CbOrigem.Text = "";
            CbDestino.Text = "";
            txtEnderecoOrigem.Text = "";
            txtEnderecoDestino.Text = "";
            richTextBox1.Text = "";

        }

        private void Btnagendanao_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            txtAtendMarcado.Visible = false;
            label4.Visible = true;
            label5.Visible = true;
            txtNomeSolicitante.Visible = true;
            label6.Visible = true;
            CbLocalSolicita.Visible = true;
            label7.Visible = true;
            txtTelefone.Visible = true;
            Agendamento = "Nao";
            if (Btnagendasim.BackColor == Color.PaleTurquoise)
            {
                Btnagendasim.BackColor = Color.PaleTurquoise;
                Btnagendasim.ForeColor = Color.Teal;
                Btnagendanao.ForeColor = Color.Teal;
                Btnagendanao.BackColor = Color.PaleTurquoise;

            }

            Btnagendasim.BackColor = Color.PaleTurquoise;
            Btnagendanao.BackColor = Color.Teal;
            Btnagendanao.ForeColor = Color.PaleTurquoise;
            Btnagendasim.ForeColor = Color.Teal;
        }

        private void Btnagendasim_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            txtAtendMarcado.Visible = true;
            txtAtendMarcado.Focus();
            txtAtendMarcado.Text = DateTime.Now.ToString();
            Agendamento = "Sim";

            if (Btnagendanao.BackColor == Color.PaleTurquoise)
            {
                Btnagendanao.BackColor = Color.PaleTurquoise;
                Btnagendanao.ForeColor = Color.Teal;
                Btnagendasim.ForeColor = Color.Teal;
                Btnagendasim.BackColor = Color.PaleTurquoise;

            }
            Btnagendanao.BackColor = Color.PaleTurquoise;
            Btnagendasim.BackColor = Color.Teal;
            Btnagendasim.ForeColor = Color.PaleTurquoise;
            Btnagendanao.ForeColor = Color.Teal;
        }
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
                                select new
                                {
                                    am.idAmbulancia,
                                    Ambulancia = am.NomeAmbulancia,
                                    Status = am.StatusAmbulancia,
                                    idPaciente = sa.idSolicitacoesPacientes,
                                    Paciente = sp.Paciente,
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
                                where
                                am.TipoAM == "AVANCADO" && am.Desativado == 0
                                select new
                                {
                                    am.idAmbulancia,
                                    Ambulancia = am.NomeAmbulancia,
                                    Status = am.StatusAmbulancia,
                                    idPaciente = sa.idSolicitacoesPacientes,
                                    Paciente = sp.Paciente,
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

            this.listaUsa.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsa.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsa.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsa.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsa.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.listaUsa.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.listaUsb.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsb.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsb.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.listaUsb.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.listaUsb.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.listaUsb.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void CbLocalSolicita_SelectedIndexChanged(object sender, EventArgs e)
        {
            pegaUnidade = CbLocalSolicita.Text;
            unidade_telefone();
        }


        private void CbTipoMotivoSelecionado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Motivo();
        }

        private void CbTipoMotivoSelecionado_TextChanged(object sender, EventArgs e)
        {
            Motivo();

            if (Agendamento == "Sim")
            {
                CbAtendimentoPrioridade.Visible = false;
            }
            else
            {
                CbAtendimentoPrioridade.Visible = true;
            }

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

            if (Agendamento == "" || TipoAM == "" || Agendamento == null || TipoAM == null)
            {

                MessageBox.Show("Marque a opção do tipo de ambulancia ou se é agendado !", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (txtNomeSolicitante.Text == "" ||
            CbLocalSolicita.Text == "" ||
            txtTelefone.Text == "" ||
            txtNomePaciente.Text == "" ||
            txtIdade.Text == "" ||
            txtDiagnostico.Text == "" ||
            CbMotivoChamado.Text == "" ||
            Sexo == "" ||
            CbTipoMotivoSelecionado.Text == "" ||
            CbOrigem.Text == "" ||
            CbDestino.Text == "" ||
            txtEnderecoOrigem.Text == "" ||
            txtEnderecoDestino.Text == "")
            {

                MessageBox.Show("Verifique se algum campo esta vazio ou desmarcado !", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                RegistrarSolicitacao();
                DialogResult ms = MessageBox.Show("Deseja criar uma nova solicitação ?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ms == DialogResult.Yes)
                {
                    Limpar();
                }
                else
                {
                    this.Close();
                }

            }
        }
        private void RegistrarSolicitacao()
        {
            InsercoesDoBanco IB = new InsercoesDoBanco();

            if (Agendamento == "Nao")
            {
                txtAtendMarcado.Text = " ";
            }
            VerificarPontos(this);

            try
            {
                IB.inserirSolicitacaoDoPaciente(TipoAM, DateTime.Now.ToString(), Agendamento, this.txtAtendMarcado.Text, this.txtNomeSolicitante.Text, this.CbLocalSolicita.Text, this.txtTelefone.Text,
                this.txtNomePaciente.Text, Sexo, this.txtIdade.Text, this.txtDiagnostico.Text, this.CbMotivoChamado.Text, this.CbTipoMotivoSelecionado.Text,
                this.CbAtendimentoPrioridade.Checked.ToString(), this.CbOrigem.Text, this.txtEnderecoOrigem.Text, this.CbDestino.Text, this.txtEnderecoDestino.Text, this.richTextBox1.Text,
                0, this.PacienteNaoAcompanhante.Checked.ToString(), System.Environment.UserName, DateTime.Now);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                MessageBox.Show("Solicitação salva com sucesso !!!");
                this.Dispose();
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

        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }
        public void Endereco()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                CbLocalSolicita.DataSource = db.enderecos.ToList();
                CbLocalSolicita.ValueMember = "NomeUnidade";
                CbLocalSolicita.DisplayMember = "NomeUnidade";
                CbDestino.DataSource = db.enderecos.ToList();
                CbDestino.ValueMember = "NomeUnidade";
                CbDestino.DisplayMember = "NomeUnidade";
                CbOrigem.DataSource = db.enderecos.ToList();
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
        private void CbOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            pegaUnidadeEnd = CbOrigem.Text;
            unidade_Endereco();
            txtEnderecoOrigem.Text = Endereco1;

        }

        private void CbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            pegaUnidadeEnd = CbDestino.Text;
            unidade_Endereco();
            txtEnderecoDestino.Text = Endereco1;

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

            using (DAHUEEntities db = new DAHUEEntities())
            {
                CbTipoMotivoSelecionado.DataSource = db.referencias.ToList();
                CbTipoMotivoSelecionado.ValueMember = pegamotivo;
                CbTipoMotivoSelecionado.DisplayMember = pegamotivo;
            }

        }
        private void CbMotivoChamado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CbTipoMotivoSelecionado.DataSource = null;
            CbTipoMotivoSelecionado.ValueMember = "";
            CbTipoMotivoSelecionado.DisplayMember = "";

            if (CbMotivoChamado.Text == "INTERNAÇÃO EM UTI" || CbMotivoChamado.Text == "SALA VERMELHA/EMERGÊNCIA" || CbMotivoChamado.Text == "")
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
                BtnAvancada.BackColor = Color.PaleTurquoise;
                BtnAvancada.ForeColor = Color.Teal;
                BtnBasica.ForeColor = Color.Teal;
                BtnBasica.BackColor = Color.PaleTurquoise;
            }

        }
        private void CbTipoMotivoSelecionado_Click(object sender, EventArgs e)
        {
            Motivo();
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

        /////////////////////////////////////////////////////////////////STATUS AM////////////////////////////////////////////////////////////////////

        private void countparaSol()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = from sp in db.solicitacoes_paciente
                            where sp.AmSolicitada == 0 && sp.Agendamento == "Nao"
                            select sp.idPaciente_Solicitacoes;

                var countQuery = query.Count();
                txtSolicitacoes.Text = countQuery.ToString();
            }
        }


        private void countparaSolAgendadas()
        {
            DateTime Data = DateTime.Now;
            int i = 0;
            int contagem = 0;
            string dataHoje = Data.ToString("dd/MM/yyyy");

            //CONTA AS solicitacoes agendadas
            using (DAHUEEntities db = new DAHUEEntities())
            {

                var query = from solicitacoes_paciente in db.solicitacoes_paciente
                            where solicitacoes_paciente.Agendamento == "Sim" &&
                            solicitacoes_paciente.AmSolicitada == 0
                            select solicitacoes_paciente.DtHrAgendamento;

                var queryPaciente = query.ToList();
                foreach (var item in queryPaciente)
                {
                    string data = item.Substring(0, 10);
                    if (data == dataHoje)
                    {
                        contagem++;
                    }
                    i++;
                }
            }
            txtAgendadasHoje.Text = contagem.ToString();    
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text == "")
            {
                MessageBox.Show("Selecione a unidade !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(comboBox1.Text == "ALVES DIAS")
            {
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
            }
            else if(comboBox1.Text == "BAETA NEVES")
            {
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
            }
            else if (comboBox1.Text == "DEMARCHI")
            {
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
            }
            else if (comboBox1.Text == "PAULICÉIA")
            {
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
                
            }
            else if (comboBox1.Text == "RIACHO GRANDE")
            {
                PainelSolicitacoes.Visible = true;
                UnidadeSelecionada = comboBox1.Text;
                SelectPacientes();
            }
            else if (comboBox1.Text == "RUDGE RAMOS")
            {            
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
                                
            }
            else if (comboBox1.Text == "SÃO PEDRO")
            {
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
            }
            else if (comboBox1.Text == "SILVINA")
            {
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
            }
            else if (comboBox1.Text == "UNIÃO")
            {
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
            }

        }
        private void SelectPacientes()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join b in db.solicitacoes_ambulancias 
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes } equals new { idPaciente_Solicitacoes = (int)b.idSolicitacoesPacientes } into b_join
                             from b in b_join.DefaultIfEmpty()
                             where sp.Origem == UnidadeSelecionada
                             orderby sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = b.idSolicitacoes_Ambulancias,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
   
                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }
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
                             }).FirstOrDefault();

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

        private void button3_Click(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            Horarios();
            SelectPacientes();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Opcoes.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            Horarios();
            SelectPacientes();
            Opcoes.Visible = false;
        }

        private void hoje_Click_1(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;

            using(DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                            where sp.Origem == UnidadeSelecionada &&
                            sp.DtHrdoInicio == data
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }
        }

        private void Ontem_Click_1(object sender, EventArgs e)
        {

            DateTime data = DateTime.Now;
            DateTime ontem = DateTime.Now.AddDays(-1);

            using(DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.Origem == UnidadeSelecionada
                             && sp.DtHrdoInicio >= ontem && sp.DtHrdoInicio <= data
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }
        }

        private void dias2_Click_1(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;
            DateTime ontem = DateTime.Now.AddDays(-2);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.Origem == UnidadeSelecionada
                             && sp.DtHrdoInicio >= ontem && sp.DtHrdoInicio <= data
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }

        }

        private void dias5_Click_1(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;
            DateTime ontem = DateTime.Now.AddDays(-5);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.Origem == UnidadeSelecionada
                             && sp.DtHrdoInicio >= ontem && sp.DtHrdoInicio <= data
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }
        }

        private void semana1_Click_1(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;
            DateTime ontem = DateTime.Now.AddDays(-7);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.Origem == UnidadeSelecionada
                             && sp.DtHrdoInicio >= ontem && sp.DtHrdoInicio <= data
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }
        }

        private void semana2_Click_1(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;
            DateTime ontem = DateTime.Now.AddDays(-14);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.Origem == UnidadeSelecionada
                             && sp.DtHrdoInicio >= ontem && sp.DtHrdoInicio <= data
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }
        }

        private void mes1_Click_1(object sender, EventArgs e)
        {
            string mes = DateTime.Now.Month.ToString("MM");
            string ano = DateTime.Now.Year.ToString("yyyy");
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.Origem == UnidadeSelecionada &&
                             SqlFunctions.DatePart("month", sp.DtHrdoInicio) == Convert.ToInt32(mes) &&
                             SqlFunctions.DatePart("year", sp.DtHrdoInicio) == Convert.ToInt32(ano)
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }

        }

        private void ano1_Click_1(object sender, EventArgs e)
        {
            string ano = DateTime.Now.Year.ToString("yyyy");
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.Origem == UnidadeSelecionada &&
                             SqlFunctions.DatePart("year", sp.DtHrdoInicio) == Convert.ToInt32(ano)
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 Tipo = sp.TipoSolicitacao,
                                 sp.DtHrdoInicio,
                                 sp.Agendamento,
                                 sp.DtHrAgendamento,
                                 sp.NomeSolicitante,
                                 sp.LocalSolicitacao,
                                 sp.Telefone,
                                 sp.Paciente,
                                 sp.Genero,
                                 sp.Idade,
                                 sp.Diagnostico,
                                 sp.Motivo,
                                 sp.SubMotivo,
                                 sp.Origem,
                                 sp.Destino,
                                 sp.ObsGerais
                             }).ToList();
            }
        }

        private void CbMotivoChamado_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (CbMotivoChamado.Text == "INTERNAÇÃO EM UTI" || CbMotivoChamado.Text == "SALA VERMELHA/EMERGÊNCIA")
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
                BtnAvancada.BackColor = Color.PaleTurquoise;
                BtnAvancada.ForeColor = Color.Teal;
                BtnBasica.ForeColor = Color.Teal;
                BtnBasica.BackColor = Color.PaleTurquoise;
            }
        }

        private void AtualizarBtn_Click(object sender, EventArgs e)
        {
            pegarDadosDasAmbulancias();
        }

        private void Lista_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
                Id = Lista.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                IdSolicitacaoAmbulancia = Convert.ToInt32(Lista.Rows[e.RowIndex].Cells["idSolicitacoes_Ambulancias"].Value.ToString());

                Horarios();

                origem = Lista.Rows[e.RowIndex].Cells["Origem"].Value.ToString();
                destino = Lista.Rows[e.RowIndex].Cells["Destino"].Value.ToString();
  
                lbDestino.Text = destino;
                lbOrigem.Text = origem;
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

    }
}
