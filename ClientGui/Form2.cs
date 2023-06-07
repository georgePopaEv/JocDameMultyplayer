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
        string roomIdSelected = "";
        string sizeRoomSelected = "";
        public LobbyForm(Client client)
        {
            InitializeComponent();
            _client = client;
            for(int n = 0; n < _client._rooms.Count; n++)
            {
                var roomId = _client._rooms[n].id.ToString();
                var roomName = _client._rooms[n].Name.ToString();
                dgvRooms.Rows.Add(new[] { roomId, roomName, string.Format("{0}/2", _client._rooms[n]._players.Count) });
            }
            // dgwServerStatusLog.Rows.Add(new[] { e.LogMessage.Id, e.LogMessage.Message });
            
        }
        private void dgvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificăm dacă a fost selectată o celulă întreagă (nu antetul sau coloanele)
            
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            { 
                DataGridView dataGridView = (DataGridView)sender;
                //Obținem valoarea celulei din prima coloană a rândului selectat
                if (dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() is null)
                {
                    MessageBox.Show("No rows selected");
                }
                else
                {
                    roomIdSelected = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    sizeRoomSelected = dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                }

            }
        }

        //Start Button, trebuie sa se connecteze la o Camera
        private void button1_Click(object sender, EventArgs e)
        {

            
            if (roomIdSelected == "")
            {
                MessageBox.Show("Te rog selecteaza o camera altfel nu pot sa te connectez la joc");
                
                return;
            }
            else
            {
                //_client.connectToRoom();
                if (sizeRoomSelected == "2/2")
                {
                    MessageBox.Show("Te rog selecteaza o camera care nu este deja plina");
                }
                else
                {
                    // _client.connectToRoom(roomIdSelected);
                    var game = new Game1(_client);
                    MessageBox.Show("Room selected : " + roomIdSelected);
                    //MessageBox.Show(_client.serializedBoard.ToString());
                    // game.client1.Username = "George";   //textBox1.Text.ToString();
                    game.Run();
                    this.Visible = false;
                }
                
            }
            
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
            _client.sendRoomCreated(roomId, roomName);
        }

        

    }
}
