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
            this.PnlBarraLateral = new System.Windows.Forms.Panel();
            this.BtnMesa1 = new System.Windows.Forms.Button();
            this.BtnMesa2 = new System.Windows.Forms.Button();
            this.BtnMesa3 = new System.Windows.Forms.Button();
            this.BtnMesa4 = new System.Windows.Forms.Button();
            this.PnlBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogoMenu)).BeginInit();
            this.PnlBarraLateral.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBackground
            // 
            this.PnlBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.PnlBackground.Controls.Add(this.PbxLogoMenu);
            this.PnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlBackground.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PnlBackground.Location = new System.Drawing.Point(288, 0);
            this.PnlBackground.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlBackground.Name = "PnlBackground";
            this.PnlBackground.Size = new System.Drawing.Size(920, 679);
            this.PnlBackground.TabIndex = 8;
            // 
            // PbxLogoMenu
            // 
            this.PbxLogoMenu.BackColor = System.Drawing.Color.Transparent;
            this.PbxLogoMenu.Image = global::PO26B_FSA.Properties.Resources.LogoPOO1;
            this.PbxLogoMenu.Location = new System.Drawing.Point(60, 26);
            this.PbxLogoMenu.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PbxLogoMenu.Name = "PbxLogoMenu";
            this.PbxLogoMenu.Size = new System.Drawing.Size(828, 182);
            this.PbxLogoMenu.TabIndex = 1;
            this.PbxLogoMenu.TabStop = false;
            // 
            // PnlBarraLateral
            // 
            this.PnlBarraLateral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.PnlBarraLateral.Controls.Add(this.BtnMesa4);
            this.PnlBarraLateral.Controls.Add(this.BtnMesa3);
            this.PnlBarraLateral.Controls.Add(this.BtnMesa2);
            this.PnlBarraLateral.Controls.Add(this.BtnMesa1);
            this.PnlBarraLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.PnlBarraLateral.Location = new System.Drawing.Point(0, 0);
            this.PnlBarraLateral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlBarraLateral.Name = "PnlBarraLateral";
            this.PnlBarraLateral.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.PnlBarraLateral.Size = new System.Drawing.Size(288, 679);
            this.PnlBarraLateral.TabIndex = 2;
            // 
            // BtnMesa1
            // 
            this.BtnMesa1.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa1.FlatAppearance.BorderSize = 0;
            this.BtnMesa1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMesa1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa1.Location = new System.Drawing.Point(10, 8);
            this.BtnMesa1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa1.Name = "BtnMesa1";
            this.BtnMesa1.Size = new System.Drawing.Size(268, 40);
            this.BtnMesa1.TabIndex = 2;
            this.BtnMesa1.Text = "Mesa de prácticas 1";
            this.BtnMesa1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa1.UseVisualStyleBackColor = true;
            this.BtnMesa1.Click += new System.EventHandler(this.BtnMesa1_Click);
            // 
            // BtnMesa2
            // 
            this.BtnMesa2.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa2.FlatAppearance.BorderSize = 0;
            this.BtnMesa2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMesa2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa2.Location = new System.Drawing.Point(10, 48);
            this.BtnMesa2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa2.Name = "BtnMesa2";
            this.BtnMesa2.Size = new System.Drawing.Size(268, 40);
            this.BtnMesa2.TabIndex = 3;
            this.BtnMesa2.Text = "Mesa de prácticas 2";
            this.BtnMesa2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa2.UseVisualStyleBackColor = true;
            this.BtnMesa2.Click += new System.EventHandler(this.BtnMesa2_Click);
            // 
            // BtnMesa3
            // 
            this.BtnMesa3.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa3.FlatAppearance.BorderSize = 0;
            this.BtnMesa3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMesa3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa3.Location = new System.Drawing.Point(10, 88);
            this.BtnMesa3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa3.Name = "BtnMesa3";
            this.BtnMesa3.Size = new System.Drawing.Size(268, 40);
            this.BtnMesa3.TabIndex = 4;
            this.BtnMesa3.Text = "Mesa de prácticas 3";
            this.BtnMesa3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa3.UseVisualStyleBackColor = true;
            this.BtnMesa3.Click += new System.EventHandler(this.BtnMesa3_Click);
            // 
            // BtnMesa4
            // 
            this.BtnMesa4.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa4.FlatAppearance.BorderSize = 0;
            this.BtnMesa4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMesa4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa4.Location = new System.Drawing.Point(10, 128);
            this.BtnMesa4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa4.Name = "BtnMesa4";
            this.BtnMesa4.Size = new System.Drawing.Size(268, 40);
            this.BtnMesa4.TabIndex = 5;
            this.BtnMesa4.Text = "Mesa de prácticas 4";
            this.BtnMesa4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa4.UseVisualStyleBackColor = true;
            this.BtnMesa4.Click += new System.EventHandler(this.BtnMesa4_Click);
            // 
            // DlgPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 679);
            this.Controls.Add(this.PnlBackground);
            this.Controls.Add(this.PnlBarraLateral);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "DlgPrincipal";
            this.Text = "POO26B_FSA Programación Orientada a Objetos";
            this.Load += new System.EventHandler(this.DlgPrincipal_Load);
            this.PnlBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogoMenu)).EndInit();
            this.PnlBarraLateral.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PnlBackground;
        private System.Windows.Forms.PictureBox PbxLogoMenu;
        private System.Windows.Forms.Panel PnlBarraLateral;
        private System.Windows.Forms.Button BtnMesa1;
        private System.Windows.Forms.Button BtnMesa4;
        private System.Windows.Forms.Button BtnMesa3;
        private System.Windows.Forms.Button BtnMesa2;
    }
}

