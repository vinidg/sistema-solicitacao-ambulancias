﻿using System;
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
    public partial class Reagedamentos : Form
    {
        public Reagedamentos(int IdPaciente)
        {
            InitializeComponent();

            using (DAHUEEntities1 db = new DAHUEEntities1())
            {
                var reagendamentos = (from sad in db.solicitacoes_agendamentos
                                      join sp in db.solicitacoes_paciente
                                      on sad.idSolicitacao_paciente equals sp.idPaciente_Solicitacoes into sad_join
                                      from sp in sad_join.DefaultIfEmpty()
                                      where sad.idSolicitacao_paciente == IdPaciente
                                      select new
                                      {
                                          ID = sad.idSolicitacaoAgendamento,
                                          sad.DtHrAgendamento
                                      }).ToList();

                ListaReagementos.DataSource = reagendamentos;
                ListaReagementos.Refresh();

                var negativas = (from h in db.historico
                                 where h.Obs != "" && h.Obs != null && h.idPaciente_Solicitacao == IdPaciente
                                 select new
                                 {
                                     h.IdHistorico,
                                     h.DtHrRegistro,
                                     h.Obs
                                 }).ToList();

                Negadas.DataSource = negativas;
                Negadas.Refresh();
            }
        }
    }
}
