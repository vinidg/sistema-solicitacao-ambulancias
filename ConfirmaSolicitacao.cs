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
            countparaSolAgendadasPendentes();
            StartPosition = FormStartPosition.CenterScreen;
            Endereco();
            label3.Visible = false;
            dataAgendamento.Visible = false;
            Limpar();
            this.Text = "Sistema de Solicitação de Ambulancias. Versão: " + appverion;
            AbasControle.SelectedTab = Aba2;
            Detalhes.Text = "";

        }
       
        Version appverion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

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
            label28.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

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
                return ;
            }

                MessageBox.Show("Solicitação salva com sucesso !!!");
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

            using (DAHUEEntities db = new DAHUEEntities())
            {
                CbTipoMotivoSelecionado.DataSource = db.referencias.ToList();
                CbTipoMotivoSelecionado.ValueMember = pegamotivo;
                CbTipoMotivoSelecionado.DisplayMember = pegamotivo;
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

        /////////////////////////////////////////////////////////////////STATUS AM////////////////////////////////////////////////////////////////////

        private void countparaSol()
        {
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             where sp.AmSolicitada == 0 && sp.Agendamento == "Nao"
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
                             && sp.Registrado == "Aguardando resposta do solicitante"
                             select sp.idPaciente_Solicitacoes).Count();

                solAgendadasPendentes.Text = query.ToString();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text == "")
            {
                MessageBox.Show("Selecione a unidade !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                    PainelSolicitacoes.Visible = true;
                    UnidadeSelecionada = comboBox1.Text;
                    SelectPacientes();
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
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                
   
                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;

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
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                ListaCancelar.Refresh();

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
            SelectPacientes();
        }

        private void hoje_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";

            using(DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                              sp.LocalSolicitacao == UnidadeSelecionada &&
                              SqlFunctions.DateDiff("day", DateTime.Now, sp.DtHrdoAgendamento) == 0 
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();
            
                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }
        }

        private void Ontem_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            DateTime data = DateTime.Now;
            DateTime ontem = DateTime.Now.AddDays(-1);

            using(DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null 
                               && sp.DtHrdoInicio >= ontem && sp.DtHrdoInicio <= data
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();

                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }
        }

        private void dias2_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            DateTime data = DateTime.Now;
            DateTime dias2 = DateTime.Now.AddDays(-2);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && sp.DtHrdoInicio >= dias2 && sp.DtHrdoInicio <= data
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();

                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }

        }

        private void dias5_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            DateTime data = DateTime.Now;
            DateTime dias5 = DateTime.Now.AddDays(-5);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && sp.DtHrdoInicio >= dias5 && sp.DtHrdoInicio <= data
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();

                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }
        }

        private void semana1_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            DateTime data = DateTime.Now;
            DateTime dias7 = DateTime.Now.AddDays(-7);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && sp.DtHrdoInicio >= dias7 && sp.DtHrdoInicio <= data
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();

                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }
        }

        private void semana2_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            DateTime data = DateTime.Now;
            DateTime dias14 = DateTime.Now.AddDays(-14);

            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && sp.DtHrdoInicio >= dias14 && sp.DtHrdoInicio <= data
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();

                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }
        }

        private void mes1_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               &&
                             SqlFunctions.DatePart("month", sp.DtHrdoInicio) == mes &&
                             SqlFunctions.DatePart("year", sp.DtHrdoInicio) == ano
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();

                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
            }

        }

        private void ano1_Click_1(object sender, EventArgs e)
        {
            Lista.DataSource = "";
            int ano = DateTime.Now.Year;
            using (DAHUEEntities db = new DAHUEEntities())
            {
                var query = (from sp in db.solicitacoes_paciente
                             join sa in db.solicitacoes_ambulancias
                             on new { idPaciente_Solicitacoes = sp.idPaciente_Solicitacoes }
                             equals new { idPaciente_Solicitacoes = (int)sa.idSolicitacoesPacientes } into b_join
                             from sa in b_join.DefaultIfEmpty()
                             where
                               sp.LocalSolicitacao == UnidadeSelecionada && sa.idSolicitacoes_Ambulancias != null
                               && SqlFunctions.DatePart("year", sp.DtHrdoInicio) == ano
                             orderby
                               sp.idPaciente_Solicitacoes
                             select new
                             {
                                 Id = sp.idPaciente_Solicitacoes,
                                 idSolicitacoes_Ambulancias = sa.idSolicitacoes_Ambulancias,
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
                             }).ToList();

                Lista.DataSource = query;
                Lista.Refresh();

                Lista.Columns["Id"].Visible = false;
                Lista.Columns["idSolicitacoes_Ambulancias"].Visible = false;
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
        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
            ClearComboBox();
            ClearTextBoxes();
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
                Limpar();
                ClearTextBoxes();
                ClearComboBox();
                

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

        private void Reagendamentos_Click(object sender, EventArgs e)
        {
            ReagendamentoUnidade rr = new ReagendamentoUnidade();
            rr.ShowDialog();
        }
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

        string AvisoReagendamento = "Número de solicitações que o Transporte Sanitario irá Aceitar ou Reagendar !";

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

        private void Obs_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }


    }
}
