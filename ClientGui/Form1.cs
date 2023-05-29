using JocDameMultyplayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGui
{
    public partial class LogInForm : Form
    {
        public Client client1 { get; set; }
        public LogInForm()
        {
            client1 = new Client(); // Se declara un nou client participant la joc
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client1.Username = textBox1.Text.ToString();
            client1.Start();
            MessageBox.Show(client1.serializedBoard.ToString());
            LobbyForm f2 = new LobbyForm(client1);
            f2.Show();
            this.Visible = false;
            
        }
    }
}
