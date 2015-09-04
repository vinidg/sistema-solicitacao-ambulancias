using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solicitacao_de_Ambulancias
{

    public partial class Opcoes : Form
    {

        private string DATAop;

        public string DATAop1
        {
            get { return DATAop; }
            set { DATAop = value; }
        }

        public Opcoes()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void hoje_Click(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio=" + sdsd;
        }

        private void Ontem_Click(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 1 AND" + sdsd;
        }

        private void dias2_Click(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 2 AND" + sdsd;
        }

        private void dias5_Click(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 5 AND" + sdsd;
        }

        private void semana1_Click(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 7 AND" + sdsd;
        }

        private void semana2_Click(object sender, EventArgs e)
        {
            DateTime dsds = DateTime.Now;
            string sdsd = dsds.ToString("dd/MM/yyyy hh:MM:ss");
            DATAop = "AND DtHrdoInicio BETWEEN GETDATE() - 14 AND" + sdsd;
        }

        private void mes1_Click(object sender, EventArgs e)
        {
            DateTime mes1 = DateTime.Now;
            string MES = mes1.ToString("MM");
            DATAop = "AND month(DtHrdoInicio)=" + MES;
        }

        private void ano1_Click(object sender, EventArgs e)
        {
            DateTime ano = DateTime.Now;
            string anos = ano.ToString("yyyy");
            DATAop = "AND year(DtHrdoInicio)=" + anos; 
        }


    }
}
