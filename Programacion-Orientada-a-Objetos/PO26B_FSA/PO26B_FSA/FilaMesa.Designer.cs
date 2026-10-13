namespace PO26B_FSA
{
    partial class FilaMesa
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.PnlRetrato = new System.Windows.Forms.Panel();
            this.PbxRetrato = new System.Windows.Forms.PictureBox();
            this.LblNombre = new System.Windows.Forms.Label();
            this.LblMana = new System.Windows.Forms.Label();
            this.LblVida = new System.Windows.Forms.Label();
            this.LblTiempo = new System.Windows.Forms.Label();
            this.PnlStats = new System.Windows.Forms.Panel();
            this.PnlDificultad = new System.Windows.Forms.Panel();
            this.LblDificultad = new System.Windows.Forms.Label();
            this.PnlTiempo = new System.Windows.Forms.Panel();
            this.PnlRetrato.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbxRetrato)).BeginInit();
            this.PnlStats.SuspendLayout();
            this.PnlDificultad.SuspendLayout();
            this.PnlTiempo.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlRetrato
            // 
            this.PnlRetrato.Controls.Add(this.PbxRetrato);
            this.PnlRetrato.Location = new System.Drawing.Point(11, 10);
            this.PnlRetrato.Name = "PnlRetrato";
            this.PnlRetrato.Size = new System.Drawing.Size(61, 60);
            this.PnlRetrato.TabIndex = 0;
            // 
            // PbxRetrato
            // 
            this.PbxRetrato.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PbxRetrato.Location = new System.Drawing.Point(0, 0);
            this.PbxRetrato.Name = "PbxRetrato";
            this.PbxRetrato.Size = new System.Drawing.Size(61, 60);
            this.PbxRetrato.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PbxRetrato.TabIndex = 0;
            this.PbxRetrato.TabStop = false;
            // 
            // LblNombre
            // 
            this.LblNombre.AutoSize = true;
            this.LblNombre.Font = new System.Drawing.Font("Andy", 12.25F, System.Drawing.FontStyle.Bold);
            this.LblNombre.Location = new System.Drawing.Point(80, 6);
            this.LblNombre.Name = "LblNombre";
            this.LblNombre.Size = new System.Drawing.Size(137, 19);
            this.LblNombre.TabIndex = 1;
            this.LblNombre.Text = "Mesa De Prácticas 1";
            // 
            // LblMana
            // 
            this.LblMana.AutoSize = true;
            this.LblMana.Font = new System.Drawing.Font("Andy", 10.25F, System.Drawing.FontStyle.Bold);
            this.LblMana.Location = new System.Drawing.Point(88, 6);
            this.LblMana.Name = "LblMana";
            this.LblMana.Size = new System.Drawing.Size(41, 16);
            this.LblMana.TabIndex = 2;
            this.LblMana.Text = "20 mp";
            // 
            // LblVida
            // 
            this.LblVida.AutoSize = true;
            this.LblVida.Font = new System.Drawing.Font("Andy", 10.25F, System.Drawing.FontStyle.Bold);
            this.LblVida.Location = new System.Drawing.Point(15, 6);
            this.LblVida.Name = "LblVida";
            this.LblVida.Size = new System.Drawing.Size(45, 16);
            this.LblVida.TabIndex = 3;
            this.LblVida.Text = "120 HP";
            // 
            // LblTiempo
            // 
            this.LblTiempo.AutoSize = true;
            this.LblTiempo.Font = new System.Drawing.Font("Andy", 10.25F, System.Drawing.FontStyle.Bold);
            this.LblTiempo.Location = new System.Drawing.Point(15, 6);
            this.LblTiempo.Name = "LblTiempo";
            this.LblTiempo.Size = new System.Drawing.Size(43, 16);
            this.LblTiempo.TabIndex = 4;
            this.LblTiempo.Text = "1:24:47";
            // 
            // PnlStats
            // 
            this.PnlStats.Controls.Add(this.LblVida);
            this.PnlStats.Controls.Add(this.LblMana);
            this.PnlStats.Location = new System.Drawing.Point(81, 26);
            this.PnlStats.Name = "PnlStats";
            this.PnlStats.Size = new System.Drawing.Size(157, 28);
            this.PnlStats.TabIndex = 6;
            // 
            // PnlDificultad
            // 
            this.PnlDificultad.Controls.Add(this.LblDificultad);
            this.PnlDificultad.Location = new System.Drawing.Point(244, 26);
            this.PnlDificultad.Name = "PnlDificultad";
            this.PnlDificultad.Size = new System.Drawing.Size(115, 28);
            this.PnlDificultad.TabIndex = 7;
            // 
            // LblDificultad
            // 
            this.LblDificultad.AutoSize = true;
            this.LblDificultad.Font = new System.Drawing.Font("Andy", 10.25F, System.Drawing.FontStyle.Bold);
            this.LblDificultad.Location = new System.Drawing.Point(20, 6);
            this.LblDificultad.Name = "LblDificultad";
            this.LblDificultad.Size = new System.Drawing.Size(46, 16);
            this.LblDificultad.TabIndex = 3;
            this.LblDificultad.Text = "Normal";
            this.LblDificultad.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PnlTiempo
            // 
            this.PnlTiempo.Controls.Add(this.LblTiempo);
            this.PnlTiempo.Location = new System.Drawing.Point(365, 26);
            this.PnlTiempo.Name = "PnlTiempo";
            this.PnlTiempo.Size = new System.Drawing.Size(93, 28);
            this.PnlTiempo.TabIndex = 8;
            // 
            // FilaMesa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PnlTiempo);
            this.Controls.Add(this.PnlDificultad);
            this.Controls.Add(this.PnlStats);
            this.Controls.Add(this.LblNombre);
            this.Controls.Add(this.PnlRetrato);
            this.Name = "FilaMesa";
            this.Size = new System.Drawing.Size(482, 80);
            this.Click += new System.EventHandler(this.FilaMesa_Click);
            this.PnlRetrato.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbxRetrato)).EndInit();
            this.PnlStats.ResumeLayout(false);
            this.PnlStats.PerformLayout();
            this.PnlDificultad.ResumeLayout(false);
            this.PnlDificultad.PerformLayout();
            this.PnlTiempo.ResumeLayout(false);
            this.PnlTiempo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PnlRetrato;
        private System.Windows.Forms.PictureBox PbxRetrato;
        private System.Windows.Forms.Label LblNombre;
        private System.Windows.Forms.Label LblMana;
        private System.Windows.Forms.Label LblVida;
        private System.Windows.Forms.Label LblTiempo;
        private System.Windows.Forms.Panel PnlStats;
        private System.Windows.Forms.Panel PnlDificultad;
        private System.Windows.Forms.Label LblDificultad;
        private System.Windows.Forms.Panel PnlTiempo;
    }
}
