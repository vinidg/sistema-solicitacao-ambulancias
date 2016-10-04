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
    public partial class ReagendamentoUnidade : Form
    {
        public ReagendamentoUnidade()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RespostaDeAmbulancias ra = new RespostaDeAmbulancias(Unidade.Text);
            this.Dispose();
            ra.ShowDialog();
        }
    }
}
