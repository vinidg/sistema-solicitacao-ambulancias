using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using db_transporte_sanitario;
using System.Windows;

namespace Solicitacao_de_Ambulancias
{
    public class InsercoesDoBanco
    {

        public void inserirSolicitacaoDoPaciente(string TipoSolicitacao, DateTime DtHrdoInicio, string Agendamento, DateTime DtHrdoAgendamento,
            string NomeSolicitante, string LocalSolicitacao, string Telefone, string Paciente, string Genero, string Idade,string Diagnostico, 
            string Motivo, string SubMotivo, string Prioridade, string Origem, string EnderecoOrigem, string Destino, string EnderecoDestino, 
            string ObsGerais, int AmSolicitada, string usuario, DateTime DtHrRegistro, bool Gestante)
        {

            using (DAHUEEntities1 dahue = new DAHUEEntities1())
            {

                solicitacoes_paciente solicitacoesPaciente = new solicitacoes_paciente();

                solicitacoesPaciente.TipoSolicitacao = TipoSolicitacao;
                solicitacoesPaciente.DtHrdoInicio = DtHrdoInicio;
                solicitacoesPaciente.Agendamento = Agendamento;
                solicitacoesPaciente.DtHrdoAgendamento = DtHrdoAgendamento;
                solicitacoesPaciente.NomeSolicitante = NomeSolicitante;
                solicitacoesPaciente.LocalSolicitacao = LocalSolicitacao;
                solicitacoesPaciente.Telefone = Telefone;
                solicitacoesPaciente.Paciente = Paciente;
                solicitacoesPaciente.Genero = Genero;
                solicitacoesPaciente.Idade = Idade;
                solicitacoesPaciente.Diagnostico = Diagnostico;
                solicitacoesPaciente.Motivo = Motivo;
                solicitacoesPaciente.SubMotivo = SubMotivo;
                solicitacoesPaciente.Prioridade = Prioridade;
                solicitacoesPaciente.Origem = Origem;
                solicitacoesPaciente.EnderecoOrigem = EnderecoOrigem;
                solicitacoesPaciente.Destino = Destino;
                solicitacoesPaciente.EnderecoDestino = EnderecoDestino;
                solicitacoesPaciente.ObsGerais = ObsGerais;
                solicitacoesPaciente.AmSolicitada = AmSolicitada;
                solicitacoesPaciente.Gestante = Gestante;
                if (Agendamento == "Sim")
                {
                    solicitacoesPaciente.Registrado = "Aguardando resposta do controle";
                }
                else
                {
                    solicitacoesPaciente.Registrado = "Sim";
                }
                historico hi = new historico();
                hi.Usuario = usuario;
                hi.DtHrRegistro = DtHrRegistro;

                dahue.solicitacoes_paciente.Add(solicitacoesPaciente);
                dahue.historico.Add(hi);
                dahue.SaveChanges();
            }
            MessageBox.Show("Solicitação salva com sucesso !!!");
        }

        public void cancelarSolicitacao(int idSolicitacaoAmbulancias, int idPaciente, string motivoCancelar, string responsavel, string obs)
        {
            
                using (DAHUEEntities1 db = new DAHUEEntities1())
                {
                    cancelados_pacientes cancelados = new cancelados_pacientes();
                    cancelados.idPaciente = idPaciente;
                    cancelados.idSolicitacaoAM = 0;
                    cancelados.MotivoCancelamento = motivoCancelar;
                    cancelados.DtHrCancelamento = DateTime.Now;
                    cancelados.ResposavelCancelamento = responsavel;
                    cancelados.ObsCancelamento = obs;

                    db.cancelados_pacientes.Add(cancelados);

                    solicitacoes_paciente sp = db.solicitacoes_paciente.First(s => s.idPaciente_Solicitacoes == idPaciente);
                    sp.AmSolicitada = 1;

                    if (idSolicitacaoAmbulancias != 0)
                    {
                        solicitacoes_ambulancias sa = db.solicitacoes_ambulancias.First(s => s.idSolicitacoes_Ambulancias == idSolicitacaoAmbulancias);
                        sa.SolicitacaoConcluida = 1;
                    }

                    db.SaveChanges();

                    MessageBox.Show("Solicitação cancelada com sucesso !!!");
                    
                }
            
        }

    }
}
