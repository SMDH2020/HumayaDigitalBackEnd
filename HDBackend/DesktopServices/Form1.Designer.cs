namespace DesktopServices
{
    partial class Servicios
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
            this.btnFacturacionMensual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFacturacionMensual
            // 
            this.btnFacturacionMensual.Location = new System.Drawing.Point(37, 12);
            this.btnFacturacionMensual.Name = "btnFacturacionMensual";
            this.btnFacturacionMensual.Size = new System.Drawing.Size(161, 23);
            this.btnFacturacionMensual.TabIndex = 0;
            this.btnFacturacionMensual.Text = "Facturacion Mensual";
            this.btnFacturacionMensual.UseVisualStyleBackColor = true;
            this.btnFacturacionMensual.Click += new System.EventHandler(this.btnFacturacionMensual_Click);
            // 
            // Servicios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnFacturacionMensual);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Servicios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFacturacionMensual;
    }
}

