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
            this.PnlBackground = new System.Windows.Forms.Panel();
            this.PnlMensaje = new System.Windows.Forms.Panel();
            this.BtnEnviar = new System.Windows.Forms.Button();
            this.TbxMensaje = new System.Windows.Forms.TextBox();
            this.BtnChip4 = new System.Windows.Forms.Button();
            this.BtnChip3 = new System.Windows.Forms.Button();
            this.BtnChip2 = new System.Windows.Forms.Button();
            this.BtnChip1 = new System.Windows.Forms.Button();
            this.LblSaludo = new System.Windows.Forms.Label();
            this.LblLogo = new System.Windows.Forms.Label();
            this.PnlBarraLateral = new System.Windows.Forms.Panel();
            this.BtnMesa4 = new System.Windows.Forms.Button();
            this.BtnMesa3 = new System.Windows.Forms.Button();
            this.BtnMesa2 = new System.Windows.Forms.Button();
            this.BtnMesa1 = new System.Windows.Forms.Button();
            this.LblTituloMesas = new System.Windows.Forms.Label();
            this.PnlLineaPie = new System.Windows.Forms.Panel();
            this.LblPie = new System.Windows.Forms.Label();
            this.TbxConversacion = new System.Windows.Forms.TextBox();
            this.PnlBackground.SuspendLayout();
            this.PnlMensaje.SuspendLayout();
            this.PnlBarraLateral.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBackground
            // 
            this.PnlBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.PnlBackground.Controls.Add(this.TbxConversacion);
            this.PnlBackground.Controls.Add(this.PnlMensaje);
            this.PnlBackground.Controls.Add(this.BtnChip4);
            this.PnlBackground.Controls.Add(this.BtnChip3);
            this.PnlBackground.Controls.Add(this.BtnChip2);
            this.PnlBackground.Controls.Add(this.BtnChip1);
            this.PnlBackground.Controls.Add(this.LblSaludo);
            this.PnlBackground.Controls.Add(this.LblLogo);
            this.PnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlBackground.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.PnlBackground.Location = new System.Drawing.Point(288, 0);
            this.PnlBackground.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlBackground.Name = "PnlBackground";
            this.PnlBackground.Size = new System.Drawing.Size(920, 679);
            this.PnlBackground.TabIndex = 8;
            // 
            // PnlMensaje
            // 
            this.PnlMensaje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.PnlMensaje.Controls.Add(this.BtnEnviar);
            this.PnlMensaje.Controls.Add(this.TbxMensaje);
            this.PnlMensaje.Location = new System.Drawing.Point(172, 524);
            this.PnlMensaje.Name = "PnlMensaje";
            this.PnlMensaje.Padding = new System.Windows.Forms.Padding(14, 8, 52, 8);
            this.PnlMensaje.Size = new System.Drawing.Size(600, 50);
            this.PnlMensaje.TabIndex = 7;
            // 
            // BtnEnviar
            // 
            this.BtnEnviar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(100)))), ((int)(((byte)(66)))));
            this.BtnEnviar.FlatAppearance.BorderSize = 0;
            this.BtnEnviar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(100)))), ((int)(((byte)(66)))));
            this.BtnEnviar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(100)))), ((int)(((byte)(66)))));
            this.BtnEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEnviar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.BtnEnviar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.BtnEnviar.Location = new System.Drawing.Point(556, 8);
            this.BtnEnviar.Name = "BtnEnviar";
            this.BtnEnviar.Size = new System.Drawing.Size(34, 34);
            this.BtnEnviar.TabIndex = 3;
            this.BtnEnviar.TabStop = false;
            this.BtnEnviar.Text = "↑";
            this.BtnEnviar.UseVisualStyleBackColor = false;
            this.BtnEnviar.Click += new System.EventHandler(this.BtnEnviar_Click);
            // 
            // TbxMensaje
            // 
            this.TbxMensaje.AcceptsReturn = true;
            this.TbxMensaje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.TbxMensaje.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TbxMensaje.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbxMensaje.Location = new System.Drawing.Point(14, 8);
            this.TbxMensaje.Multiline = true;
            this.TbxMensaje.Name = "TbxMensaje";
            this.TbxMensaje.Size = new System.Drawing.Size(534, 34);
            this.TbxMensaje.TabIndex = 2;
            // 
            // BtnChip4
            // 
            this.BtnChip4.AutoSize = true;
            this.BtnChip4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.BtnChip4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnChip4.FlatAppearance.BorderSize = 0;
            this.BtnChip4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.BtnChip4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnChip4.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.BtnChip4.Location = new System.Drawing.Point(585, 590);
            this.BtnChip4.Name = "BtnChip4";
            this.BtnChip4.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.BtnChip4.Size = new System.Drawing.Size(90, 37);
            this.BtnChip4.TabIndex = 6;
            this.BtnChip4.Text = "Mesa 4";
            this.BtnChip4.UseVisualStyleBackColor = false;
            this.BtnChip4.Click += new System.EventHandler(this.BtnMesa4_Click);
            // 
            // BtnChip3
            // 
            this.BtnChip3.AutoSize = true;
            this.BtnChip3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.BtnChip3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnChip3.FlatAppearance.BorderSize = 0;
            this.BtnChip3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.BtnChip3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnChip3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.BtnChip3.Location = new System.Drawing.Point(468, 590);
            this.BtnChip3.Name = "BtnChip3";
            this.BtnChip3.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.BtnChip3.Size = new System.Drawing.Size(90, 37);
            this.BtnChip3.TabIndex = 5;
            this.BtnChip3.Text = "Mesa 3";
            this.BtnChip3.UseVisualStyleBackColor = false;
            this.BtnChip3.Click += new System.EventHandler(this.BtnMesa3_Click);
            // 
            // BtnChip2
            // 
            this.BtnChip2.AutoSize = true;
            this.BtnChip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.BtnChip2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnChip2.FlatAppearance.BorderSize = 0;
            this.BtnChip2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.BtnChip2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnChip2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.BtnChip2.Location = new System.Drawing.Point(351, 590);
            this.BtnChip2.Name = "BtnChip2";
            this.BtnChip2.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.BtnChip2.Size = new System.Drawing.Size(90, 37);
            this.BtnChip2.TabIndex = 4;
            this.BtnChip2.Text = "Mesa 2";
            this.BtnChip2.UseVisualStyleBackColor = false;
            this.BtnChip2.Click += new System.EventHandler(this.BtnMesa2_Click);
            // 
            // BtnChip1
            // 
            this.BtnChip1.AutoSize = true;
            this.BtnChip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.BtnChip1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnChip1.FlatAppearance.BorderSize = 0;
            this.BtnChip1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.BtnChip1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnChip1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.BtnChip1.Location = new System.Drawing.Point(230, 590);
            this.BtnChip1.Name = "BtnChip1";
            this.BtnChip1.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.BtnChip1.Size = new System.Drawing.Size(90, 37);
            this.BtnChip1.TabIndex = 3;
            this.BtnChip1.Text = "Mesa 1";
            this.BtnChip1.UseVisualStyleBackColor = false;
            this.BtnChip1.Click += new System.EventHandler(this.BtnMesa1_Click);
            // 
            // LblSaludo
            // 
            this.LblSaludo.AutoSize = true;
            this.LblSaludo.Font = new System.Drawing.Font("Georgia", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSaludo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.LblSaludo.Location = new System.Drawing.Point(206, 76);
            this.LblSaludo.Name = "LblSaludo";
            this.LblSaludo.Size = new System.Drawing.Size(518, 56);
            this.LblSaludo.TabIndex = 1;
            this.LblSaludo.Text = "¿Qué practicamos hoy?";
            // 
            // LblLogo
            // 
            this.LblLogo.AutoSize = true;
            this.LblLogo.Font = new System.Drawing.Font("Georgia", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(100)))), ((int)(((byte)(66)))));
            this.LblLogo.Location = new System.Drawing.Point(392, 30);
            this.LblLogo.Name = "LblLogo";
            this.LblLogo.Size = new System.Drawing.Size(156, 38);
            this.LblLogo.TabIndex = 0;
            this.LblLogo.Text = "POO26B";
            // 
            // PnlBarraLateral
            // 
            this.PnlBarraLateral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(227)))));
            this.PnlBarraLateral.Controls.Add(this.BtnMesa4);
            this.PnlBarraLateral.Controls.Add(this.BtnMesa3);
            this.PnlBarraLateral.Controls.Add(this.BtnMesa2);
            this.PnlBarraLateral.Controls.Add(this.BtnMesa1);
            this.PnlBarraLateral.Controls.Add(this.LblTituloMesas);
            this.PnlBarraLateral.Controls.Add(this.PnlLineaPie);
            this.PnlBarraLateral.Controls.Add(this.LblPie);
            this.PnlBarraLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.PnlBarraLateral.Location = new System.Drawing.Point(0, 0);
            this.PnlBarraLateral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlBarraLateral.Name = "PnlBarraLateral";
            this.PnlBarraLateral.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.PnlBarraLateral.Size = new System.Drawing.Size(288, 679);
            this.PnlBarraLateral.TabIndex = 2;
            // 
            // BtnMesa4
            // 
            this.BtnMesa4.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa4.FlatAppearance.BorderSize = 0;
            this.BtnMesa4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa4.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.BtnMesa4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa4.Location = new System.Drawing.Point(10, 208);
            this.BtnMesa4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa4.Name = "BtnMesa4";
            this.BtnMesa4.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.BtnMesa4.Size = new System.Drawing.Size(268, 48);
            this.BtnMesa4.TabIndex = 5;
            this.BtnMesa4.Text = "Mesa de prácticas 4";
            this.BtnMesa4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa4.UseVisualStyleBackColor = true;
            this.BtnMesa4.Click += new System.EventHandler(this.BtnMesa4_Click);
            // 
            // BtnMesa3
            // 
            this.BtnMesa3.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa3.FlatAppearance.BorderSize = 0;
            this.BtnMesa3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa3.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.BtnMesa3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa3.Location = new System.Drawing.Point(10, 160);
            this.BtnMesa3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa3.Name = "BtnMesa3";
            this.BtnMesa3.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.BtnMesa3.Size = new System.Drawing.Size(268, 48);
            this.BtnMesa3.TabIndex = 4;
            this.BtnMesa3.Text = "Mesa de prácticas 3";
            this.BtnMesa3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa3.UseVisualStyleBackColor = true;
            this.BtnMesa3.Click += new System.EventHandler(this.BtnMesa3_Click);
            // 
            // BtnMesa2
            // 
            this.BtnMesa2.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa2.FlatAppearance.BorderSize = 0;
            this.BtnMesa2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa2.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.BtnMesa2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa2.Location = new System.Drawing.Point(10, 112);
            this.BtnMesa2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa2.Name = "BtnMesa2";
            this.BtnMesa2.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.BtnMesa2.Size = new System.Drawing.Size(268, 48);
            this.BtnMesa2.TabIndex = 3;
            this.BtnMesa2.Text = "Mesa de prácticas 2";
            this.BtnMesa2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa2.UseVisualStyleBackColor = true;
            this.BtnMesa2.Click += new System.EventHandler(this.BtnMesa2_Click);
            // 
            // BtnMesa1
            // 
            this.BtnMesa1.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnMesa1.FlatAppearance.BorderSize = 0;
            this.BtnMesa1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.BtnMesa1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMesa1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.BtnMesa1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.BtnMesa1.Location = new System.Drawing.Point(10, 64);
            this.BtnMesa1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnMesa1.Name = "BtnMesa1";
            this.BtnMesa1.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.BtnMesa1.Size = new System.Drawing.Size(268, 48);
            this.BtnMesa1.TabIndex = 2;
            this.BtnMesa1.Text = "Mesa de prácticas 1";
            this.BtnMesa1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMesa1.UseVisualStyleBackColor = true;
            this.BtnMesa1.Click += new System.EventHandler(this.BtnMesa1_Click);
            // 
            // LblTituloMesas
            // 
            this.LblTituloMesas.Dock = System.Windows.Forms.DockStyle.Top;
            this.LblTituloMesas.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.LblTituloMesas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.LblTituloMesas.Location = new System.Drawing.Point(10, 8);
            this.LblTituloMesas.Name = "LblTituloMesas";
            this.LblTituloMesas.Size = new System.Drawing.Size(268, 56);
            this.LblTituloMesas.TabIndex = 6;
            this.LblTituloMesas.Text = "Mesas";
            this.LblTituloMesas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlLineaPie
            // 
            this.PnlLineaPie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.PnlLineaPie.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlLineaPie.Location = new System.Drawing.Point(10, 622);
            this.PnlLineaPie.Name = "PnlLineaPie";
            this.PnlLineaPie.Size = new System.Drawing.Size(268, 1);
            this.PnlLineaPie.TabIndex = 8;
            // 
            // LblPie
            // 
            this.LblPie.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LblPie.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblPie.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(119)))), ((int)(((byte)(110)))));
            this.LblPie.Location = new System.Drawing.Point(10, 623);
            this.LblPie.Name = "LblPie";
            this.LblPie.Size = new System.Drawing.Size(268, 48);
            this.LblPie.TabIndex = 7;
            this.LblPie.Text = "Programación Orientada a Objetos\r\n26B\r\n";
            this.LblPie.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TbxConversacion
            // 
            this.TbxConversacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.TbxConversacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TbxConversacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(42)))), ((int)(((byte)(39)))));
            this.TbxConversacion.Location = new System.Drawing.Point(172, 148);
            this.TbxConversacion.Multiline = true;
            this.TbxConversacion.Name = "TbxConversacion";
            this.TbxConversacion.ReadOnly = true;
            this.TbxConversacion.Size = new System.Drawing.Size(600, 360);
            this.TbxConversacion.TabIndex = 8;
            this.TbxConversacion.TabStop = false;
            // 
            // DlgPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 679);
            this.Controls.Add(this.PnlBackground);
            this.Controls.Add(this.PnlBarraLateral);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "DlgPrincipal";
            this.Text = "POO26B_FSA Programación Orientada a Objetos";
            this.PnlBackground.ResumeLayout(false);
            this.PnlBackground.PerformLayout();
            this.PnlMensaje.ResumeLayout(false);
            this.PnlMensaje.PerformLayout();
            this.PnlBarraLateral.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PnlBackground;
        private System.Windows.Forms.Panel PnlBarraLateral;
        private System.Windows.Forms.Button BtnMesa1;
        private System.Windows.Forms.Button BtnMesa4;
        private System.Windows.Forms.Button BtnMesa3;
        private System.Windows.Forms.Button BtnMesa2;
        private System.Windows.Forms.TextBox TbxMensaje;
        private System.Windows.Forms.Label LblSaludo;
        private System.Windows.Forms.Label LblLogo;
        private System.Windows.Forms.Button BtnChip4;
        private System.Windows.Forms.Button BtnChip3;
        private System.Windows.Forms.Button BtnChip2;
        private System.Windows.Forms.Button BtnChip1;
        private System.Windows.Forms.Panel PnlMensaje;
        private System.Windows.Forms.Button BtnEnviar;
        private System.Windows.Forms.Label LblTituloMesas;
        private System.Windows.Forms.Panel PnlLineaPie;
        private System.Windows.Forms.Label LblPie;
        private System.Windows.Forms.TextBox TbxConversacion;
    }
}

