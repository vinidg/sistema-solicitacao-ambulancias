using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using db_transporte_sanitario;

namespace Solicitacao_de_Ambulancias
{
    public class InsercoesDoBanco
    {

        public void inserirSolicitacaoDoPaciente(string TipoSolicitacao, DateTime DtHrdoInicio, string Agendamento, string DtHrAgendamento,
            string NomeSolicitante, string LocalSolicitacao, string Telefone, string Paciente, string Genero, string Idade,string Diagnostico, 
            string Motivo, string SubMotivo, string Prioridade, string Origem, string EnderecoOrigem, string Destino, string EnderecoDestino, 
            string ObsGerais, int AmSolicitada, string usuario, DateTime DtHrRegistro)
        {

            using (DAHUEEntities dahue = new DAHUEEntities())
            {

                solicitacoes_paciente solicitacoesPaciente = new solicitacoes_paciente();

                solicitacoesPaciente.TipoSolicitacao = TipoSolicitacao;
                solicitacoesPaciente.DtHrdoInicio = DtHrdoInicio;
                solicitacoesPaciente.Agendamento = Agendamento;
                solicitacoesPaciente.DtHrAgendamento = DtHrAgendamento;
                solicitacoesPaciente.DtHrdoAgendamento = Convert.ToDateTime(DtHrAgendamento);
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
        }

    }
}
