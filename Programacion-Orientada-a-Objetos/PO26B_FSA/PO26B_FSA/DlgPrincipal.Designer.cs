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
            this.PnlContenedor = new System.Windows.Forms.Panel();
            this.LblAtras = new System.Windows.Forms.Label();
            this.PnlMesas = new System.Windows.Forms.Panel();
            this.PnlLabelMesas = new System.Windows.Forms.Panel();
            this.LblSelectMesas = new System.Windows.Forms.Label();
            this.PnlMenu = new System.Windows.Forms.Panel();
            this.LblMesas = new System.Windows.Forms.Label();
            this.UcMesa2 = new PO26B_FSA.FilaMesa();
            this.UcMesa4 = new PO26B_FSA.FilaMesa();
            this.UcMesa3 = new PO26B_FSA.FilaMesa();
            this.UcMesa1 = new PO26B_FSA.FilaMesa();
            this.PnlBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogoMenu)).BeginInit();
            this.PnlContenedor.SuspendLayout();
            this.PnlMesas.SuspendLayout();
            this.PnlLabelMesas.SuspendLayout();
            this.PnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBackground
            // 
            this.PnlBackground.BackgroundImage = global::PO26B_FSA.Properties.Resources.TerrariaBackground;
            this.PnlBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlBackground.Controls.Add(this.PbxLogoMenu);
            this.PnlBackground.Controls.Add(this.PnlContenedor);
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
            // PnlContenedor
            // 
            this.PnlContenedor.BackColor = System.Drawing.Color.Transparent;
            this.PnlContenedor.Controls.Add(this.LblAtras);
            this.PnlContenedor.Controls.Add(this.PnlMesas);
            this.PnlContenedor.Controls.Add(this.PnlLabelMesas);
            this.PnlContenedor.Controls.Add(this.PnlMenu);
            this.PnlContenedor.Location = new System.Drawing.Point(94, 214);
            this.PnlContenedor.Name = "PnlContenedor";
            this.PnlContenedor.Size = new System.Drawing.Size(619, 468);
            this.PnlContenedor.TabIndex = 2;
            // 
            // LblAtras
            // 
            this.LblAtras.AutoSize = true;
            this.LblAtras.Location = new System.Drawing.Point(57, 436);
            this.LblAtras.Name = "LblAtras";
            this.LblAtras.Size = new System.Drawing.Size(28, 13);
            this.LblAtras.TabIndex = 3;
            this.LblAtras.Text = "Atrás";
            this.LblAtras.Click += new System.EventHandler(this.LblAtras_Click);
            // 
            // PnlMesas
            // 
            this.PnlMesas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.PnlMesas.Controls.Add(this.UcMesa2);
            this.PnlMesas.Controls.Add(this.UcMesa4);
            this.PnlMesas.Controls.Add(this.UcMesa3);
            this.PnlMesas.Controls.Add(this.UcMesa1);
            this.PnlMesas.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PnlMesas.Location = new System.Drawing.Point(60, 64);
            this.PnlMesas.Name = "PnlMesas";
            this.PnlMesas.Size = new System.Drawing.Size(511, 369);
            this.PnlMesas.TabIndex = 1;
            this.PnlMesas.Visible = false;
            // 
            // PnlLabelMesas
            // 
            this.PnlLabelMesas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(94)))), ((int)(((byte)(171)))));
            this.PnlLabelMesas.Controls.Add(this.LblSelectMesas);
            this.PnlLabelMesas.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PnlLabelMesas.Location = new System.Drawing.Point(140, 15);
            this.PnlLabelMesas.Name = "PnlLabelMesas";
            this.PnlLabelMesas.Size = new System.Drawing.Size(372, 55);
            this.PnlLabelMesas.TabIndex = 2;
            this.PnlLabelMesas.Visible = false;
            // 
            // LblSelectMesas
            // 
            this.LblSelectMesas.AutoSize = true;
            this.LblSelectMesas.Font = new System.Drawing.Font("Andy", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSelectMesas.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblSelectMesas.Location = new System.Drawing.Point(16, 6);
            this.LblSelectMesas.Name = "LblSelectMesas";
            this.LblSelectMesas.Size = new System.Drawing.Size(337, 54);
            this.LblSelectMesas.TabIndex = 0;
            this.LblSelectMesas.Text = "Seleccionar Mesa";
            // 
            // PnlMenu
            // 
            this.PnlMenu.BackColor = System.Drawing.Color.Transparent;
            this.PnlMenu.Controls.Add(this.LblMesas);
            this.PnlMenu.Location = new System.Drawing.Point(212, 111);
            this.PnlMenu.Name = "PnlMenu";
            this.PnlMenu.Size = new System.Drawing.Size(175, 200);
            this.PnlMenu.TabIndex = 0;
            // 
            // LblMesas
            // 
            this.LblMesas.Font = new System.Drawing.Font("Andy", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMesas.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblMesas.Location = new System.Drawing.Point(0, 70);
            this.LblMesas.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblMesas.Name = "LblMesas";
            this.LblMesas.Size = new System.Drawing.Size(175, 60);
            this.LblMesas.TabIndex = 4;
            this.LblMesas.Text = "Mesas";
            this.LblMesas.Click += new System.EventHandler(this.LblMesas_Click);
            // 
            // UcMesa2
            // 
            this.UcMesa2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(126)))));
            this.UcMesa2.Location = new System.Drawing.Point(14, 96);
            this.UcMesa2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.UcMesa2.Name = "UcMesa2";
            this.UcMesa2.Size = new System.Drawing.Size(482, 80);
            this.UcMesa2.TabIndex = 4;
            // 
            // UcMesa4
            // 
            this.UcMesa4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(126)))));
            this.UcMesa4.Location = new System.Drawing.Point(14, 272);
            this.UcMesa4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.UcMesa4.Name = "UcMesa4";
            this.UcMesa4.Size = new System.Drawing.Size(482, 80);
            this.UcMesa4.TabIndex = 3;
            // 
            // UcMesa3
            // 
            this.UcMesa3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(126)))));
            this.UcMesa3.Location = new System.Drawing.Point(14, 184);
            this.UcMesa3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.UcMesa3.Name = "UcMesa3";
            this.UcMesa3.Size = new System.Drawing.Size(482, 80);
            this.UcMesa3.TabIndex = 2;
            // 
            // UcMesa1
            // 
            this.UcMesa1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(126)))));
            this.UcMesa1.Location = new System.Drawing.Point(14, 8);
            this.UcMesa1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.UcMesa1.Name = "UcMesa1";
            this.UcMesa1.Size = new System.Drawing.Size(482, 80);
            this.UcMesa1.TabIndex = 1;
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
            this.PnlContenedor.ResumeLayout(false);
            this.PnlContenedor.PerformLayout();
            this.PnlMesas.ResumeLayout(false);
            this.PnlLabelMesas.ResumeLayout(false);
            this.PnlLabelMesas.PerformLayout();
            this.PnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LblMesas;
        private System.Windows.Forms.Panel PnlBackground;
        private System.Windows.Forms.Panel PnlMenu;
        private System.Windows.Forms.PictureBox PbxLogoMenu;
        private System.Windows.Forms.Panel PnlContenedor;
        private System.Windows.Forms.Panel PnlMesas;
        private System.Windows.Forms.Panel PnlLabelMesas;
        private System.Windows.Forms.Label LblSelectMesas;
        private FilaMesa UcMesa2;
        private FilaMesa UcMesa4;
        private FilaMesa UcMesa3;
        private FilaMesa UcMesa1;
        private System.Windows.Forms.Label LblAtras;
    }
}

