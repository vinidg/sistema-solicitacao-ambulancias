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
    
    public partial class cancelados_pacientes
    {
        public int? idcancelados_pacientes { get; set; }
        public int? idPaciente { get; set; }
        public int? idSolicitacaoAM { get; set; }
        public string MotivoCancelamento { get; set; }
        public DateTime? DtHrCancelamento { get; set; }
        public string ResposavelCancelamento { get; set; }
        public string ObsCancelamento { get; set; }
    }
}
