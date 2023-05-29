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
    public partial class LobbyForm : Form
    {
        public Client _client { get; set; }
        public LobbyForm(Client client)
        {
            InitializeComponent();
            _client = client;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var game = new Game1(_client);
            MessageBox.Show(_client.serializedBoard.ToString());
            //game.client1.Username = "George";   //textBox1.Text.ToString();
            game.Run();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            _client.DisconnectFromServer(_client.Username);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var roomId = txtIdRoom.Text.ToString();
            var roomName = txtRoomName.Text.ToString();
            // dgwServerStatusLog.Rows.Add(new[] { e.LogMessage.Id, e.LogMessage.Message });
            dgvRooms.Rows.Add(new[] { roomId, roomName, "0/2" });
        }
    }
}
