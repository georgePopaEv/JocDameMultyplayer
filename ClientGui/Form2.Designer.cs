
namespace ClientGui
{
    partial class LobbyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startGame = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.grbRooms = new System.Windows.Forms.GroupBox();
            this.dgvRooms = new System.Windows.Forms.DataGridView();
            this.txtRoomName = new System.Windows.Forms.TextBox();
            this.txtIdRoom = new System.Windows.Forms.TextBox();
            this.lblIDRoom = new System.Windows.Forms.Label();
            this.lblNameRoom = new System.Windows.Forms.Label();
            this.idroom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.players = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grbRooms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
            this.SuspendLayout();
            // 
            // startGame
            // 
            this.startGame.Location = new System.Drawing.Point(562, 40);
            this.startGame.Name = "startGame";
            this.startGame.Size = new System.Drawing.Size(226, 56);
            this.startGame.TabIndex = 0;
            this.startGame.Text = "Start";
            this.startGame.UseVisualStyleBackColor = true;
            this.startGame.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(562, 102);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(226, 56);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close Game";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(564, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(226, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Create Room";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // grbRooms
            // 
            this.grbRooms.Controls.Add(this.dgvRooms);
            this.grbRooms.Location = new System.Drawing.Point(13, 13);
            this.grbRooms.Name = "grbRooms";
            this.grbRooms.Size = new System.Drawing.Size(543, 425);
            this.grbRooms.TabIndex = 3;
            this.grbRooms.TabStop = false;
            this.grbRooms.Text = "Rooms";
            // 
            // dgvRooms
            // 
            this.dgvRooms.AllowUserToDeleteRows = false;
            this.dgvRooms.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRooms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idroom,
            this.nameRoom,
            this.players});
            this.dgvRooms.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRooms.Location = new System.Drawing.Point(6, 26);
            this.dgvRooms.MultiSelect = false;
            this.dgvRooms.Name = "dgvRooms";
            this.dgvRooms.RowHeadersWidth = 51;
            this.dgvRooms.RowTemplate.Height = 29;
            this.dgvRooms.Size = new System.Drawing.Size(531, 393);
            this.dgvRooms.TabIndex = 0;
            // 
            // txtRoomName
            // 
            this.txtRoomName.Location = new System.Drawing.Point(564, 285);
            this.txtRoomName.Name = "txtRoomName";
            this.txtRoomName.Size = new System.Drawing.Size(226, 27);
            this.txtRoomName.TabIndex = 4;
            // 
            // txtIdRoom
            // 
            this.txtIdRoom.Location = new System.Drawing.Point(564, 210);
            this.txtIdRoom.Name = "txtIdRoom";
            this.txtIdRoom.Size = new System.Drawing.Size(226, 27);
            this.txtIdRoom.TabIndex = 5;
            // 
            // lblIDRoom
            // 
            this.lblIDRoom.AutoSize = true;
            this.lblIDRoom.Location = new System.Drawing.Point(640, 187);
            this.lblIDRoom.Name = "lblIDRoom";
            this.lblIDRoom.Size = new System.Drawing.Size(68, 20);
            this.lblIDRoom.TabIndex = 6;
            this.lblIDRoom.Text = "ID Room";
            // 
            // lblNameRoom
            // 
            this.lblNameRoom.AutoSize = true;
            this.lblNameRoom.Location = new System.Drawing.Point(627, 262);
            this.lblNameRoom.Name = "lblNameRoom";
            this.lblNameRoom.Size = new System.Drawing.Size(93, 20);
            this.lblNameRoom.TabIndex = 7;
            this.lblNameRoom.Text = "Name Room";
            // 
            // idroom
            // 
            this.idroom.HeaderText = "Id Room";
            this.idroom.MinimumWidth = 6;
            this.idroom.Name = "idroom";
            this.idroom.ReadOnly = true;
            // 
            // nameRoom
            // 
            this.nameRoom.HeaderText = "Name Room";
            this.nameRoom.MinimumWidth = 6;
            this.nameRoom.Name = "nameRoom";
            this.nameRoom.ReadOnly = true;
            // 
            // players
            // 
            this.players.HeaderText = "Players";
            this.players.MinimumWidth = 6;
            this.players.Name = "players";
            this.players.ReadOnly = true;
            // 
            // LobbyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblNameRoom);
            this.Controls.Add(this.lblIDRoom);
            this.Controls.Add(this.txtIdRoom);
            this.Controls.Add(this.txtRoomName);
            this.Controls.Add(this.grbRooms);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.startGame);
            this.Name = "LobbyForm";
            this.Text = "Form2";
            this.grbRooms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startGame;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox grbRooms;
        private System.Windows.Forms.DataGridView dgvRooms;
        private System.Windows.Forms.TextBox txtRoomName;
        private System.Windows.Forms.TextBox txtIdRoom;
        private System.Windows.Forms.Label lblIDRoom;
        private System.Windows.Forms.Label lblNameRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn idroom;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn players;
    }
}