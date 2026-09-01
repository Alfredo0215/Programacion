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
            this.BtnMesaPractica1 = new System.Windows.Forms.Button();
            this.BtnMesaPracticas4 = new System.Windows.Forms.Button();
            this.BtnMesaPracticas3 = new System.Windows.Forms.Button();
            this.BtnMesaPracticas2 = new System.Windows.Forms.Button();
            this.LblMesa1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnMesaPractica1
            // 
            this.BtnMesaPractica1.Location = new System.Drawing.Point(26, 29);
            this.BtnMesaPractica1.Name = "BtnMesaPractica1";
            this.BtnMesaPractica1.Size = new System.Drawing.Size(122, 57);
            this.BtnMesaPractica1.TabIndex = 0;
            this.BtnMesaPractica1.Text = "Mesa de Prácticas 1";
            this.BtnMesaPractica1.UseVisualStyleBackColor = true;
            this.BtnMesaPractica1.Click += new System.EventHandler(this.BtnMesaPractica1_Click);
            // 
            // BtnMesaPracticas4
            // 
            this.BtnMesaPracticas4.Location = new System.Drawing.Point(26, 267);
            this.BtnMesaPracticas4.Name = "BtnMesaPracticas4";
            this.BtnMesaPracticas4.Size = new System.Drawing.Size(122, 57);
            this.BtnMesaPracticas4.TabIndex = 1;
            this.BtnMesaPracticas4.Text = "Mesa de Prácticas 4";
            this.BtnMesaPracticas4.UseVisualStyleBackColor = true;
            this.BtnMesaPracticas4.Click += new System.EventHandler(this.BtnMesaPracticas4_Click);
            // 
            // BtnMesaPracticas3
            // 
            this.BtnMesaPracticas3.Location = new System.Drawing.Point(26, 184);
            this.BtnMesaPracticas3.Name = "BtnMesaPracticas3";
            this.BtnMesaPracticas3.Size = new System.Drawing.Size(122, 57);
            this.BtnMesaPracticas3.TabIndex = 2;
            this.BtnMesaPracticas3.Text = "Mesa de Prácticas 3";
            this.BtnMesaPracticas3.UseVisualStyleBackColor = true;
            this.BtnMesaPracticas3.Click += new System.EventHandler(this.BtnMesaPracticas3_Click);
            // 
            // BtnMesaPracticas2
            // 
            this.BtnMesaPracticas2.Location = new System.Drawing.Point(26, 104);
            this.BtnMesaPracticas2.Name = "BtnMesaPracticas2";
            this.BtnMesaPracticas2.Size = new System.Drawing.Size(122, 57);
            this.BtnMesaPracticas2.TabIndex = 3;
            this.BtnMesaPracticas2.Text = "Mesa de Prácticas 2";
            this.BtnMesaPracticas2.UseVisualStyleBackColor = true;
            this.BtnMesaPracticas2.Click += new System.EventHandler(this.BtnMesaPracticas2_Click);
            // 
            // LblMesa1
            // 
            this.LblMesa1.AutoSize = true;
            this.LblMesa1.Location = new System.Drawing.Point(254, 38);
            this.LblMesa1.Name = "LblMesa1";
            this.LblMesa1.Size = new System.Drawing.Size(42, 13);
            this.LblMesa1.TabIndex = 4;
            this.LblMesa1.Text = "Mesa 1";
            this.LblMesa1.MouseHover += new System.EventHandler(this.LblMesa1_MouseHover);
            // 
            // DlgPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LblMesa1);
            this.Controls.Add(this.BtnMesaPracticas2);
            this.Controls.Add(this.BtnMesaPracticas3);
            this.Controls.Add(this.BtnMesaPracticas4);
            this.Controls.Add(this.BtnMesaPractica1);
            this.Name = "DlgPrincipal";
            this.Text = "POO26B_FSA";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnMesaPractica1;
        private System.Windows.Forms.Button BtnMesaPracticas4;
        private System.Windows.Forms.Button BtnMesaPracticas3;
        private System.Windows.Forms.Button BtnMesaPracticas2;
        private System.Windows.Forms.Label LblMesa1;
    }
}

