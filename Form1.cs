using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppLoL_v1._0_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Requisicoes requisicoes = new Requisicoes();
        ConectarLCU lcu = new ConectarLCU();

        private async void button1_Click(object sender, EventArgs e)
        {
            //requisicoes.AceitarPartida();
            //var teste = await requisicoes.RequisicaoSearch_State();
            //MessageBox.Show(await requisicoes.RequisicaoSearch_State());
            //MessageBox.Show(lcu.LCUPassPort().Split(':')[1]);
            //MessageBox.Show(await requisicoes.RequisicaoGameFlow_Phase());
            //if (await requisicoes.RequisicaoGameFlow_Phase() == "Lobby") MessageBox.Show("Voce esta no lobby");
            MessageBox.Show(lcu.LCUPassPort());
        }

        
        //esta funcionando
        private void Aceitar_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            Thread aa = new Thread(Main);
            aa.Start();
            //Naosei();
        }
        //esta funcionando
        private async void Main()
        {
            while (true)
            {
                if(await requisicoes.RequisicaoSearch_State() == "Found")
                {
                    Thread.Sleep(1000);
                    requisicoes.AceitarPartida();
                    if (label1.InvokeRequired) label1.Invoke(new Action(() => { label1.Text = "Partida Aceita"; }));
                    else
                    {
                        label1.Text = "Partida Aceita";
                        Thread.Sleep(5000);
                    }
                    //MessageBox.Show("Partida Aceita");
                    //Vou usar esse if para ver se esta na champselect ou nao vou ter que fazer outra requisicao
                }
                if(await requisicoes.RequisicaoSearch_State() == "Searching")
                {
                    if (label1.InvokeRequired) label1.Invoke(new Action(() => { label1.Text = "Na Fila"; }));
                    else label1.Text = "Na Fila";
                    //MessageBox.Show("Na fila");                 
                }

                if(await requisicoes.RequisicaoGameFlow_Phase() == "Lobby")
                {
                    if (label1.InvokeRequired) label1.Invoke(new Action(() => { label1.Text = "Lobby"; }));
                    else label1.Text = "Lobby";
                }

                if (await requisicoes.RequisicaoGameFlow_Phase() == "InProgress")
                {
                    if (label1.InvokeRequired) label1.Invoke(new Action(() => { label1.Text = "Partida Iniciou"; }));
                    else label1.Text = "Partida Iniciou";
                    break;
                }
                Thread.Sleep(3000);
            }

        }
    }
}
