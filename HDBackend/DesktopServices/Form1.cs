using DesktopServices.VendedorMes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopServices
{
    public partial class Servicios : Form
    {
        public Servicios()
        {
            InitializeComponent();
        }

        private void btnFacturacionMensual_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                var result = LeerFacturacionMensual.ObtenerVendedorDelMesExcel(ofd.FileName);
                GuardarVendedorMes.Guardar(result);
            }
        }
    }
}
