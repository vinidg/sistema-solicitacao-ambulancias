//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace db_transporte_sanitario
{
    using System;
    using System.Collections.Generic;
    
    public partial class solicitacoes_ambulancias
    {
        public int? idSolicitacoes_Ambulancias { get; set; }
        public int? idSolicitacoesPacientes { get; set; }
        public byte[] Cancelamento { get; set; }
        public string DtHrCiencia { get; set; }
        public string DtHrCienciaReg { get; set; }
        public string DtHrChegadaOrigem { get; set; }
        public string DtHrChegadaOrigemReg { get; set; }
        public string DtHrSaidaOrigem { get; set; }
        public string DtHrSaidaOrigemReg { get; set; }
        public string DtHrChegadaDestino { get; set; }
        public string DtHrChegadaDestinoReg { get; set; }
        public string DtHrLiberacaoEquipe { get; set; }
        public string DtHrLiberacaoEquipeReg { get; set; }
        public string DtHrEquipePatio { get; set; }
        public string DtHrEquipePatioReg { get; set; }
        public int idAmbulanciaSol { get; set; }
        public int? SolicitacaoConcluida { get; set; }
        public int? IdOutroPaciente { get; set; }
        public string Status { get; set; }
    
        public virtual ambulancia ambulancia { get; set; }
    }
}
