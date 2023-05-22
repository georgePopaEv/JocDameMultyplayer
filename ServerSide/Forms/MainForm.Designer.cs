
namespace ServerSide.Forms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.grpServerOperations = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.grpServerStatusLog = new System.Windows.Forms.GroupBox();
            this.dgwServerStatusLog = new System.Windows.Forms.DataGridView();
            this.clmId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpPlayers = new System.Windows.Forms.GroupBox();
            this.lstPLayers = new System.Windows.Forms.ListBox();
            this.cmnuPlayers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnPLayersKick = new System.Windows.Forms.ToolStripMenuItem();
            this.grpServerOperations.SuspendLayout();
            this.grpServerStatusLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwServerStatusLog)).BeginInit();
            this.grpPlayers.SuspendLayout();
            this.cmnuPlayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpServerOperations
            // 
            this.grpServerOperations.Controls.Add(this.btnStop);
            this.grpServerOperations.Controls.Add(this.btnStart);
            this.grpServerOperations.Location = new System.Drawing.Point(12, 12);
            this.grpServerOperations.Name = "grpServerOperations";
            this.grpServerOperations.Size = new System.Drawing.Size(347, 65);
            this.grpServerOperations.TabIndex = 1;
            this.grpServerOperations.TabStop = false;
            this.grpServerOperations.Text = "Server Operations";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(169, 21);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(142, 27);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop Server";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(142, 27);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // grpServerStatusLog
            // 
            this.grpServerStatusLog.Controls.Add(this.dgwServerStatusLog);
            this.grpServerStatusLog.Location = new System.Drawing.Point(236, 83);
            this.grpServerStatusLog.Name = "grpServerStatusLog";
            this.grpServerStatusLog.Size = new System.Drawing.Size(552, 443);
            this.grpServerStatusLog.TabIndex = 2;
            this.grpServerStatusLog.TabStop = false;
            this.grpServerStatusLog.Text = "Server Status Logs";
            // 
            // dgwServerStatusLog
            // 
            this.dgwServerStatusLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwServerStatusLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmId,
            this.clmMessage});
            this.dgwServerStatusLog.Location = new System.Drawing.Point(6, 21);
            this.dgwServerStatusLog.Name = "dgwServerStatusLog";
            this.dgwServerStatusLog.RowHeadersWidth = 51;
            this.dgwServerStatusLog.RowTemplate.Height = 24;
            this.dgwServerStatusLog.Size = new System.Drawing.Size(540, 416);
            this.dgwServerStatusLog.TabIndex = 0;
            // 
            // clmId
            // 
            this.clmId.HeaderText = "ID";
            this.clmId.MinimumWidth = 6;
            this.clmId.Name = "clmId";
            this.clmId.Width = 125;
            // 
            // clmMessage
            // 
            this.clmMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmMessage.HeaderText = "Message";
            this.clmMessage.MinimumWidth = 6;
            this.clmMessage.Name = "clmMessage";
            // 
            // grpPlayers
            // 
            this.grpPlayers.Controls.Add(this.lstPLayers);
            this.grpPlayers.Location = new System.Drawing.Point(12, 83);
            this.grpPlayers.Name = "grpPlayers";
            this.grpPlayers.Size = new System.Drawing.Size(218, 443);
            this.grpPlayers.TabIndex = 3;
            this.grpPlayers.TabStop = false;
            this.grpPlayers.Text = "PLayers";
            // 
            // lstPLayers
            // 
            this.lstPLayers.ContextMenuStrip = this.cmnuPlayers;
            this.lstPLayers.FormattingEnabled = true;
            this.lstPLayers.ItemHeight = 16;
            this.lstPLayers.Location = new System.Drawing.Point(6, 21);
            this.lstPLayers.Name = "lstPLayers";
            this.lstPLayers.Size = new System.Drawing.Size(206, 420);
            this.lstPLayers.TabIndex = 0;
            // 
            // cmnuPlayers
            // 
            this.cmnuPlayers.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmnuPlayers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnPLayersKick});
            this.cmnuPlayers.Name = "cmnuPlayers";
            this.cmnuPlayers.Size = new System.Drawing.Size(106, 28);
            // 
            // cmnPLayersKick
            // 
            this.cmnPLayersKick.Name = "cmnPLayersKick";
            this.cmnPLayersKick.Size = new System.Drawing.Size(210, 24);
            this.cmnPLayersKick.Text = "Kick";
            this.cmnPLayersKick.Click += new System.EventHandler(this.cmnPLayersKick_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 538);
            this.Controls.Add(this.grpPlayers);
            this.Controls.Add(this.grpServerStatusLog);
            this.Controls.Add(this.grpServerOperations);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.grpServerOperations.ResumeLayout(false);
            this.grpServerStatusLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwServerStatusLog)).EndInit();
            this.grpPlayers.ResumeLayout(false);
            this.cmnuPlayers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpServerOperations;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox grpServerStatusLog;
        private System.Windows.Forms.DataGridView dgwServerStatusLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmId;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMessage;
        private System.Windows.Forms.GroupBox grpPlayers;
        private System.Windows.Forms.ListBox lstPLayers;
        private System.Windows.Forms.ContextMenuStrip cmnuPlayers;
        private System.Windows.Forms.ToolStripMenuItem cmnPLayersKick;
    }
}