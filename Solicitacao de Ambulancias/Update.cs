using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Solicitacao_de_Ambulancias
{
   class Update
    {
        bool yn;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public bool Yn
        {
            get { return yn; }
            set { yn = value; }
        }
        public void up()
        {
            
            string donwloadurl = "";
            Version newVersion = null;

            string xmlURL = @"\\\10.1.0.109\\SAUDE\Mapa_de_Leitos\\Sistemas - Vinicius\\Sistema de Solicitacao de Ambulancias\\update.xml";
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(xmlURL);
                reader.MoveToContent();
                string elemeto = "";

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "coolapp"))
                {
                    while(reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            elemeto = reader.Name;
                        }
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                            {
                                switch(elemeto)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        donwloadurl = reader.Value;
                                        break;

                                }
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o sistema ! Podendo conter erros ao utilizar essa versão antiga");
            }
            finally
            {
                if(reader != null)
                
                    reader.Close();
                
            }
            Version appverion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (appverion.CompareTo(newVersion) < 0)
            {
                    yn = true;
                    Process.Start(donwloadurl);
            }
            else
            {
                yn = false;

            }
        }
    }
}
