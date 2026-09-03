namespace PO26B_FSA
{
    partial class DlgPrincipal
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgPrincipal));
            this.PnlBackground = new System.Windows.Forms.Panel();
            this.PbxLogoMenu = new System.Windows.Forms.PictureBox();
            this.PnlMenu = new System.Windows.Forms.Panel();
            this.LblMesa1 = new System.Windows.Forms.Label();
            this.LblMesa2 = new System.Windows.Forms.Label();
            this.LblMesa3 = new System.Windows.Forms.Label();
            this.LblMesa4 = new System.Windows.Forms.Label();
            this.PnlBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogoMenu)).BeginInit();
            this.PnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBackground
            // 
            this.PnlBackground.BackgroundImage = global::PO26B_FSA.Properties.Resources.TerrariaBackground;
            this.PnlBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlBackground.Controls.Add(this.PbxLogoMenu);
            this.PnlBackground.Controls.Add(this.PnlMenu);
            this.PnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlBackground.Location = new System.Drawing.Point(0, 0);
            this.PnlBackground.MaximumSize = new System.Drawing.Size(783, 718);
            this.PnlBackground.MinimumSize = new System.Drawing.Size(783, 718);
            this.PnlBackground.Name = "PnlBackground";
            this.PnlBackground.Size = new System.Drawing.Size(783, 718);
            this.PnlBackground.TabIndex = 8;
            // 
            // PbxLogoMenu
            // 
            this.PbxLogoMenu.BackColor = System.Drawing.Color.Transparent;
            this.PbxLogoMenu.Image = global::PO26B_FSA.Properties.Resources.LogoPOO1;
            this.PbxLogoMenu.Location = new System.Drawing.Point(50, 26);
            this.PbxLogoMenu.Name = "PbxLogoMenu";
            this.PbxLogoMenu.Size = new System.Drawing.Size(690, 182);
            this.PbxLogoMenu.TabIndex = 1;
            this.PbxLogoMenu.TabStop = false;
            // 
            // PnlMenu
            // 
            this.PnlMenu.BackColor = System.Drawing.Color.Transparent;
            this.PnlMenu.Controls.Add(this.LblMesa1);
            this.PnlMenu.Controls.Add(this.LblMesa2);
            this.PnlMenu.Controls.Add(this.LblMesa3);
            this.PnlMenu.Controls.Add(this.LblMesa4);
            this.PnlMenu.Location = new System.Drawing.Point(301, 270);
            this.PnlMenu.Name = "PnlMenu";
            this.PnlMenu.Size = new System.Drawing.Size(175, 200);
            this.PnlMenu.TabIndex = 0;
            // 
            // LblMesa1
            // 
            this.LblMesa1.Font = new System.Drawing.Font("Andy", 25F, System.Drawing.FontStyle.Bold);
            this.LblMesa1.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblMesa1.Location = new System.Drawing.Point(18, 13);
            this.LblMesa1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblMesa1.Name = "LblMesa1";
            this.LblMesa1.Size = new System.Drawing.Size(167, 50);
            this.LblMesa1.TabIndex = 4;
            this.LblMesa1.Text = "Mesa 1";
            this.LblMesa1.Click += new System.EventHandler(this.LblMesa1_Click);
            // 
            // LblMesa2
            // 
            this.LblMesa2.Font = new System.Drawing.Font("Andy", 25F, System.Drawing.FontStyle.Bold);
            this.LblMesa2.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblMesa2.Location = new System.Drawing.Point(18, 63);
            this.LblMesa2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblMesa2.Name = "LblMesa2";
            this.LblMesa2.Size = new System.Drawing.Size(167, 50);
            this.LblMesa2.TabIndex = 5;
            this.LblMesa2.Text = "Mesa 2";
            this.LblMesa2.Click += new System.EventHandler(this.LblMesa2_Click);
            // 
            // LblMesa3
            // 
            this.LblMesa3.Font = new System.Drawing.Font("Andy", 25F, System.Drawing.FontStyle.Bold);
            this.LblMesa3.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblMesa3.Location = new System.Drawing.Point(18, 113);
            this.LblMesa3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblMesa3.Name = "LblMesa3";
            this.LblMesa3.Size = new System.Drawing.Size(167, 50);
            this.LblMesa3.TabIndex = 6;
            this.LblMesa3.Text = "Mesa 3";
            this.LblMesa3.Click += new System.EventHandler(this.LblMesa3_Click);
            // 
            // LblMesa4
            // 
            this.LblMesa4.Font = new System.Drawing.Font("Andy", 25F, System.Drawing.FontStyle.Bold);
            this.LblMesa4.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblMesa4.Location = new System.Drawing.Point(18, 163);
            this.LblMesa4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblMesa4.Name = "LblMesa4";
            this.LblMesa4.Size = new System.Drawing.Size(167, 50);
            this.LblMesa4.TabIndex = 7;
            this.LblMesa4.Text = "Mesa 4";
            this.LblMesa4.Click += new System.EventHandler(this.LblMesa4_Click);
            // 
            // DlgPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 718);
            this.Controls.Add(this.PnlBackground);
            this.Font = new System.Drawing.Font("Andy", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "DlgPrincipal";
            this.Text = "POO26B_FSA Programación Orientada a Objetos";
            this.Load += new System.EventHandler(this.DlgPrincipal_Load);
            this.PnlBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogoMenu)).EndInit();
            this.PnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LblMesa1;
        private System.Windows.Forms.Label LblMesa2;
        private System.Windows.Forms.Label LblMesa3;
        private System.Windows.Forms.Label LblMesa4;
        private System.Windows.Forms.Panel PnlBackground;
        private System.Windows.Forms.Panel PnlMenu;
        private System.Windows.Forms.PictureBox PbxLogoMenu;
    }
}

