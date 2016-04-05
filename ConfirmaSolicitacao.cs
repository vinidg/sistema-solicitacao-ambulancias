using MySql.Data.MySqlClient;
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

namespace Solicitacao_de_Ambulancias
{
    public partial class ConfirmaSolicitacao : Form
    {
        string TipoAM = null;
        string Agendamento = null;
        DateTime now = DateTime.Now;
        string pegaUnidade;     //para pegar o telefone com o nome da unidade
        string pegaUnidadeEnd;  //para pegar o endereco com o nome da unidade
        string Sexo, pegamotivo, Id;
        string Endereco1, UPAselecionada, destino, origem;
        string DATAop;
        
        public ConfirmaSolicitacao()
        {
            InitializeComponent();
           
            Status();
            countparaSol();
            countparaSolAgendadas();
            txtAtendMarcado.Text = now.ToString();
            StartPosition = FormStartPosition.CenterScreen;
            Endereco();
            label3.Visible = false;
            txtAtendMarcado.Visible = false;
            Limpar();
            update();
            this.Text = "Sistema de Solicitação de Ambulancias. Versão: " + appverion;
            AbasControle.SelectedTab = Aba2;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            
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
            //registrar a solicitacao de vaga
            SqlConnection conexao = ConexaoSqlServer.GetConexao();
            //   MySqlConnection conexao = conexaoMysql.GetConexao();

            if (Agendamento == "Nao")
            {
                txtAtendMarcado.Text = " ";
            }

            string sqlQuery = "insert into solicitacoes_paciente(TipoSolicitacao,DtHrdoInicio,Agendamento,DtHrAgendamento," +
            "NomeSolicitante,LocalSolicitacao,Telefone,Paciente,Genero,Idade,Diagnostico,Motivo,SubMotivo,Prioridade,Origem," +
            "EnderecoOrigem,Destino,EnderecoDestino,ObsGerais,AmSolicitada) VALUES " +
            "('" + TipoAM + "','" + now + "','" + Agendamento + "','" + this.txtAtendMarcado.Text + "','" + this.txtNomeSolicitante.Text + "','" +
            this.CbLocalSolicita.Text + "','" + this.txtTelefone.Text + "','" + this.txtNomePaciente.Text + "','" + Sexo + "','" + this.txtIdade.Text + "','" + this.txtDiagnostico.Text + "','" +
            this.CbMotivoChamado.Text + "','" + this.CbTipoMotivoSelecionado.Text + "','" + this.CbAtendimentoPrioridade.Checked + "','" + this.CbOrigem.Text + "','" +
            this.txtEnderecoOrigem.Text + "','" + this.CbDestino.Text + "','" + this.txtEnderecoDestino.Text + "','" + this.richTextBox1.Text + "','" + 0 + "')";

            try
            {

                SqlCommand objComm = new SqlCommand(sqlQuery, conexao);
                //   MySqlCommand objComm = new MySqlCommand(sqlQuery, conexao);
                // MySqlDataReader MyReader2;

                objComm.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
                MessageBox.Show("Solicitação salva com sucesso !!!");
            }

        }
        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void Endereco()
        {
            //Consultar na tabela de enderecos
            //MySqlConnection conexao = conexaoMysql.GetConexao();
            SqlConnection conexao = ConexaoSqlServer.GetConexao();

            string sqlQuery = "select NomeUnidade from enderecos";

            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, conexao);
            //  MySqlDataAdapter da = new MySqlDataAdapter(sqlQuery, conexao);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                string rowz = string.Format("{0}", row.ItemArray[0]);
                CbLocalSolicita.Items.Add(rowz);
                CbDestino.Items.Add(rowz);
                CbOrigem.Items.Add(rowz);
            }

            // MessageBox.Show("Solicitação salva com sucesso !!!");

            conexao.Close();
        }
        private void unidade_telefone()
        {
            //consulta para mostrar o telefone quando clicar no enderenco

            SqlConnection conexao = ConexaoSqlServer.GetConexao();

            string sqlQuery2 = "select Telefone from enderecos where NomeUnidade = '" + pegaUnidade + "'";
            try
            {
                SqlCommand objComm = new SqlCommand(sqlQuery2, conexao);
                SqlDataReader MyReader2;

                MyReader2 = objComm.ExecuteReader();

                //MessageBox.Show("Alterado com sucesso !!!");
                while (MyReader2.Read())
                {
                    txtTelefone.Text = MyReader2.GetString(0);
                }
                conexao.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
        private void unidade_Endereco()
        {
            //consulta para mostrar o telefone quando clicar no enderenco
            SqlConnection conexao = ConexaoSqlServer.GetConexao();


            string sqlQuery2 = "select Endereco from enderecos where NomeUnidade = '" + pegaUnidadeEnd + "'";
            try
            {
                SqlCommand objComm = new SqlCommand(sqlQuery2, conexao);
                SqlDataReader MyReader2;

                MyReader2 = objComm.ExecuteReader();

                //MessageBox.Show("Alterado com sucesso !!!");
                while (MyReader2.Read())
                {
                    Endereco1 = MyReader2.GetString(0);

                }

                conexao.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                pegamotivo = "AVALIAÇÃO_DE_MÉDICO_ESPECIALISTA";
            }
            else if (CbMotivoChamado.Text == "AVALIAÇÃO DE PROFISSIONAL NÃO MÉDICO")
            {
                pegamotivo = "[AVALIAÇÃO_DE_PROFISSIONAL_NÃO_MÉDICO]";
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
                pegamotivo = "[EVENTO_COMEMORATIVO_DO_MUNICÍPIO]";
            }
            else if (CbMotivoChamado.Text == "EVENTO DE CULTURA, LAZER OU RELIGIÃO")
            {
                pegamotivo = "[EVENTO_DE_CULTURA,_LAZER_OU_RELIGIÃO]";
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
                pegamotivo = "[EXAME_DE_URGÊNCIA]";
            }
            else if (CbMotivoChamado.Text == "INTERNAÇÃO EM ENFERMARIA")
            {
                pegamotivo = "[INTERNAÇÃO_EM_ENFERMARIA]";
            }
            else if (CbMotivoChamado.Text == "INTERNAÇÃO EM UTI")
            {
                pegamotivo = "[INTERNAÇÃO_EM_UTI]";
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
                pegamotivo = "[SALA_VERMELHA/EMERGÊNCIA]";
            }
            else if (CbMotivoChamado.Text == "TRANSPORTE DE INSUMOS/PRODUTOS/MATERIAIS")
            {
                pegamotivo = "[TRANSPORTE_DE_INSUMOS/PRODUTOS/MATERIAIS]";
            }
            else if (CbMotivoChamado.Text == "TRANSPORTE DE PROFISSIONAIS")
            {
                pegamotivo = "[TRANSPORTE_DE_PROFISSIONAIS]";
            }


            //Consultar na tabela de enderecos
            SqlConnection conexao = ConexaoSqlServer.GetConexao();


            string sqlQuery = "select " + pegamotivo + " from referencias";
            try
            {

                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, conexao);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    string rowz = string.Format("{0}", row.ItemArray[0]);
                    CbTipoMotivoSelecionado.Items.Add(rowz);

                }

                // MessageBox.Show("Solicitação salva com sucesso !!!");
                conexao.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void CbMotivoChamado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CbTipoMotivoSelecionado.Items.Clear();
            if (pegamotivo == "[INTERNAÇÃO_EM_UTI]" || pegamotivo == "[SALA_VERMELHA/EMERGÊNCIA]")
            {
                BtnAvancada.PerformClick();
            }
        }

        private void CbTipoMotivoSelecionado_Click(object sender, EventArgs e)
        {
            Motivo();

        }

        /////////////////////////////////////////////////////////////////STATUS AM////////////////////////////////////////////////////////////////////



        public void Status()
        {
            StatusBD d = new StatusBD();
            d.puxarStatus();

            if (d.AM011 == "BLOQUEADA")
            {
                BtnAM01.BackColor = Color.RoyalBlue;

            }
            if (d.AM011 == "DISPONIVEL")
            {
                BtnAM01.BackColor = Color.LimeGreen;
            }
            if (d.AM011 == "OCUPADA")
            {
                BtnAM01.BackColor = Color.Firebrick;
            }

            if (d.AM021 == "BLOQUEADA")
            {
                BtnAM02.BackColor = Color.RoyalBlue;
            }
            if (d.AM021 == "OCUPADA")
            {
                BtnAM02.BackColor = Color.Firebrick;
            }
            if (d.AM021 == "DISPONIVEL")
            {
                BtnAM02.BackColor = Color.LimeGreen;
            }

            if (d.AMRC1 == "DISPONIVEL")
            {
                BtnAMRC.BackColor = Color.LimeGreen;
            }
            if (d.AMRC1 == "OCUPADA")
            {
                BtnAMRC.BackColor = Color.Firebrick;
            }
            if (d.AMRC1 == "BLOQUEADA")
            {
                BtnAMRC.BackColor = Color.RoyalBlue;
            }

            if (d.AM031 == "BLOQUEADA")
            {
                BtnAM03.BackColor = Color.RoyalBlue;
            }
            if (d.AM031 == "DISPONIVEL")
            {
                BtnAM03.BackColor = Color.LimeGreen;
            }
            if (d.AM031 == "OCUPADA")
            {
                BtnAM03.BackColor = Color.Firebrick;
            }

            if (d.AM041 == "BLOQUEADA")
            {
                BtnAM04.BackColor = Color.RoyalBlue;
            }
            if (d.AM041 == "OCUPADA")
            {
                BtnAM04.BackColor = Color.Firebrick;
            }
            if (d.AM041 == "DISPONIVEL")
            {
                BtnAM04.BackColor = Color.LimeGreen;
            }

            if (d.AM051 == "BLOQUEADA")
            {
                BtnAM05.BackColor = Color.RoyalBlue;
            }
            if (d.AM051 == "OCUPADA")
            {
                BtnAM05.BackColor = Color.Firebrick;
            }
            if (d.AM051 == "DISPONIVEL")
            {
                BtnAM05.BackColor = Color.LimeGreen;
            }

            if (d.AM061 == "BLOQUEADA")
            {
                BtnAM06.BackColor = Color.RoyalBlue;
            }

            if (d.AM061 == "DISPONIVEL")
            {
                BtnAM06.BackColor = Color.LimeGreen;
            }

            if (d.AM061 == "OCUPADA")
            {
                BtnAM06.BackColor = Color.Firebrick;
            }

            if (d.AM071 == "BLOQUEADA")
            {
                BtnAM07.BackColor = Color.RoyalBlue;
            }
            if (d.AM071 == "OCUPADA")
            {
                BtnAM07.BackColor = Color.Firebrick;
            }
            if (d.AM071 == "DISPONIVEL")
            {
                BtnAM07.BackColor = Color.LimeGreen;
            }

            if (d.AM081 == "BLOQUEADA")
            {
                BtnAM08.BackColor = Color.RoyalBlue;
            }
            if (d.AM081 == "DISPONIVEL")
            {
                BtnAM08.BackColor = Color.LimeGreen;
            }
            if (d.AM081 == "OCUPADA")
            {
                BtnAM08.BackColor = Color.Firebrick;
            }

            if (d.AM091 == "BLOQUEADA")
            {
                BtnAM09.BackColor = Color.RoyalBlue;
            }
            if (d.AM091 == "DISPONIVEL")
            {
                BtnAM09.BackColor = Color.LimeGreen;
            }
            if (d.AM091 == "OCUPADA")
            {
                BtnAM09.BackColor = Color.Firebrick;
            }

            if (d.AM101 == "BLOQUEADA")
            {
                BtnAM10.BackColor = Color.RoyalBlue;
            }
            if (d.AM101 == "DISPONIVEL")
            {
                BtnAM10.BackColor = Color.LimeGreen;
            }
            if (d.AM101 == "OCUPADA")
            {
                BtnAM10.BackColor = Color.Firebrick;
            }

            if (d.AM111 == "BLOQUEADA")
            {
                BtnAM11.BackColor = Color.RoyalBlue;
            }
            if (d.AM111 == "DISPONIVEL")
            {
                BtnAM11.BackColor = Color.LimeGreen;
            }
            if (d.AM111 == "OCUPADA")
            {
                BtnAM11.BackColor = Color.Firebrick;
            }

            if (d.AM121 == "BLOQUEADA")
            {
                BtnAM12.BackColor = Color.RoyalBlue;
            }
            if (d.AM121 == "DISPONIVEL")
            {
                BtnAM12.BackColor = Color.LimeGreen;
            }
            if (d.AM121 == "OCUPADA")
            {
                BtnAM12.BackColor = Color.Firebrick;
            }

            if (d.AM461 == "DISPONIVEL")
            {
                BtnAM46.BackColor = Color.LimeGreen;
            }
            if (d.AM461 == "OCUPADA")
            {
                BtnAM46.BackColor = Color.Firebrick;
            }
            if (d.AM461 == "BLOQUEADA")
            {
                BtnAM46.BackColor = Color.RoyalBlue;
            }

            if (d.AM471 == "OCUPADA")
            {
                BtnAM47.BackColor = Color.Firebrick;
            }
            if (d.AM471 == "DISPONIVEL")
            {
                BtnAM47.BackColor = Color.LimeGreen;
            }
            if (d.AM471 == "BLOQUEADA")
            {
                BtnAM47.BackColor = Color.RoyalBlue;
            }

            if (d.AM521 == "DISPONIVEL")
            {
                BtnAM52.BackColor = Color.LimeGreen;
            }
            if (d.AM521 == "OCUPADA")
            {
                BtnAM52.BackColor = Color.Firebrick;
            }
            if (d.AM521 == "BLOQUEADA")
            {
                BtnAM52.BackColor = Color.RoyalBlue;
            }
            DateTime time = DateTime.Now;
            string format = "dd/MM/yyyy HH:mm";
            label28.Text = time.ToString(format) ;
        }

        private void countparaSol()
        {
            //CONTA AS solicitacoes
            SqlConnection conexao = ConexaoSqlServer.GetConexao();


            string sqlQuery = "SELECT COUNT(idPaciente_Solicitacoes) FROM [dbo].[solicitacoes_paciente] WHERE AmSolicitada = '0' and Agendamento = 'Nao'";
            try
            {

                using (SqlCommand objComm = new SqlCommand(sqlQuery, conexao))
                {
                    int count = (int)objComm.ExecuteScalar();
                    txtSolicitacoes.Text = count.ToString();
                }
            }
            finally
            {
                conexao.Close();

            }
        }


        private void countparaSolAgendadas()
        {
            DateTime Data = DateTime.Now;
            int i = 0;
            string str = "";
            string data = "";
            int contagem = 0;
            string dataHoje = Data.ToString("dd/MM/yyyy");
            //CONTA AS solicitacoes agendadas
            SqlConnection conexao = ConexaoSqlServer.GetConexao();


            string sqlQuery = "SELECT DtHrAgendamento FROM [dbo].[solicitacoes_paciente] WHERE Agendamento = 'Sim' and AmSolicitada = '0'";

            try
            {

                SqlDataAdapter objComm = new SqlDataAdapter(sqlQuery, conexao);


                DataSet CD = new DataSet();
                objComm.Fill(CD);

                //verificar se é igual a data de 'hoje'
                while (i < CD.Tables[0].Rows.Count)
                {

                    str = CD.Tables[0].Rows[i][0].ToString();
                    data = str.Substring(0, 10);

                    if (data == dataHoje)
                    {
                        contagem++;
                    }
                    i++;
                }

                txtAgendadasHoje.Text = contagem.ToString();
            }
            finally
            {
                conexao.Close();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Status();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            /*ALVES DIAS alvesdias01
            BAETA NEVES baetanever02
            DEMARCHI demarchi03
            PAULICÉIA pauliceia04
            RIACHO GRANDE riachogrande05 
            RUDGE RAMOS rudgeramos06
            SÃO PEDRO saopedro07
            SILVINA silvina08
            UNIÃO uniao09*/

            if (txtSenha.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox1.BackColor = Color.Red;
                txtSenha.BackColor = Color.Red;
                return;
            }

            if(comboBox1.Text == "ALVES DIAS")
            {
                if(txtSenha.Text == "alvesdias01")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();
                }else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if(comboBox1.Text == "BAETA NEVES")
            {
                if (txtSenha.Text == "baetaneves02")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if (comboBox1.Text == "DEMARCHI")
            {
                if (txtSenha.Text == "demarchi03")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if (comboBox1.Text == "PAULICÉIA")
            {
                if (txtSenha.Text == "pauliceia04")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if (comboBox1.Text == "RIACHO GRANDE")
            {
                if (txtSenha.Text == "riachogrande05")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if (comboBox1.Text == "RUDGE RAMOS")
            {
                if (txtSenha.Text == "rudgeramos06")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if (comboBox1.Text == "SÃO PEDRO")
            {
                if (txtSenha.Text == "saopedro07")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if (comboBox1.Text == "SILVINA")
            {
                if (txtSenha.Text == "silvina08")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }
            else if (comboBox1.Text == "UNIÃO")
            {
                if (txtSenha.Text == "uniao09")
                {
                    PainelSolicitacoes.Visible = true;
                    UPAselecionada = comboBox1.Text;
                    SelectPacientes();

                }
                else
                {
                    MessageBox.Show("Verifique se a senha esta correta e a unidade esta selecionada !", "Atenção !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.BackColor = Color.Red;
                    txtSenha.BackColor = Color.Red;
                    return;
                }
            }


        }


        private void ConfirmaSolicitacao_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pacientes.solicitacoes_paciente' table. You can move, or remove it, as needed.

        }

        private void SelectPacientes()
        {
            SqlConnection conexao = ConexaoSqlServer.GetConexao();
            string sqlQuery = "SELECT * FROM solicitacoes_paciente WHERE Origem LIKE 'UPA " + UPAselecionada + "%' "+DATAop+"";
            //DtHrdoInicio BETWEEN '" + dataOp + "' AND '" + now + "'";
            try
            {
                SqlCommand objComm = new SqlCommand(sqlQuery, conexao);
                SqlDataReader MyReader2;

                MyReader2 = objComm.ExecuteReader();

                while (MyReader2.Read())
                {


                    ListViewItem IT = new ListViewItem(MyReader2["idPaciente_Solicitacoes"].ToString());
                    IT.SubItems.Add(MyReader2["TipoSolicitacao"].ToString());
                    IT.SubItems.Add(MyReader2["DtHrdoInicio"].ToString());
                    IT.SubItems.Add(MyReader2["Agendamento"].ToString());
                    IT.SubItems.Add(MyReader2["DtHrAgendamento"].ToString());
                    IT.SubItems.Add(MyReader2["NomeSolicitante"].ToString());
                    IT.SubItems.Add(MyReader2["LocalSolicitacao"].ToString());
                    IT.SubItems.Add(MyReader2["Telefone"].ToString());
                    IT.SubItems.Add(MyReader2["Paciente"].ToString());
                    IT.SubItems.Add(MyReader2["Genero"].ToString());
                    IT.SubItems.Add(MyReader2["Idade"].ToString());
                    IT.SubItems.Add(MyReader2["Diagnostico"].ToString());
                    IT.SubItems.Add(MyReader2["Motivo"].ToString());
                    IT.SubItems.Add(MyReader2["SubMotivo"].ToString());
                    IT.SubItems.Add(MyReader2["Origem"].ToString());
                    IT.SubItems.Add(MyReader2["Destino"].ToString());
                    IT.SubItems.Add(MyReader2["ObsGerais"].ToString());
                    listView1.Items.Add(IT);
                    Console.WriteLine(DATAop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();

            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Id = listView1.SelectedItems[0].Text;
                Horarios();
                if (listView1.SelectedItems[0].Selected)
                {
                    origem = listView1.FocusedItem.SubItems[14].Text;
                    destino = listView1.FocusedItem.SubItems[15].Text;
  
                    lbDestino.Text = destino;
                    lbOrigem.Text = origem;
                }

            }
        }

        private void Horarios()
        {

            SqlConnection conexao = ConexaoSqlServer.GetConexao();

            string sqlQuery = "SELECT * FROM solicitacoes_ambulancias WHERE idSolicitacoesPacientes = '" + Id + "'";


            try
            {

                SqlCommand objComm = new SqlCommand(sqlQuery, conexao);
                SqlDataReader MyReader2;

                MyReader2 = objComm.ExecuteReader();
                while (MyReader2.Read())
                {
                    txtHora.Text = MyReader2["DtHrCiencia"].ToString();
                    txtHora2.Text = MyReader2["DtHrChegadaOrigem"].ToString();
                    txtHora3.Text = MyReader2["DtHrSaidaOrigem"].ToString();
                    txtHora4.Text = MyReader2["DtHrChegadaDestino"].ToString();
                    txtHora5.Text = MyReader2["DtHrLiberacaoEquipe"].ToString();
                    txtHora6.Text = MyReader2["DtHrEquipePatio"].ToString();

                }
            }
            finally
            {
                conexao.Close();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Horarios();
            SelectPacientes();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Opcoes.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Opcoes.Visible = false;
        }


        private void hoje_Click_1(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio = '" + sdsd + "'";
        }

        private void Ontem_Click_1(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 1 AND '" + sdsd + "'";
        }

        private void dias2_Click_1(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 2 AND '" + sdsd + "'";
        }

        private void dias5_Click_1(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 5 AND '" + sdsd + "'";
        }

        private void semana1_Click_1(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 7 AND '" + sdsd + "'";
        }

        private void semana2_Click_1(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 14 AND '" + sdsd + "'";
        }

        private void mes1_Click_1(object sender, EventArgs e)
        {
            DateTime mes1 = DateTime.Now;
            string MES = mes1.ToString("MM");
            DATAop = "AND month(DtHrdoInicio)='" + MES + "'";
        }

        private void ano1_Click_1(object sender, EventArgs e)
        {
            DateTime ano = DateTime.Now;
            string anos = ano.ToString("yyyy");
            DATAop = "AND year(DtHrdoInicio)='" + anos + "'";
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

    }
}
