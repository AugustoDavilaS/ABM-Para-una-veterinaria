using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MascotasNuevoEditar
{
    public partial class Form1 : Form
    {
        int Machos;
        private AccesoDatos Oacceso;
        private List<Mascota> lMascotas;
        public Form1()
        {
            InitializeComponent();
            Oacceso = new AccesoDatos();
            lMascotas = new List<Mascota>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargarLista();
            cargarCombo();
            Habilitar(false);
            btCancelar.Enabled = false;
        }
        private void cargarLista()
        {
            lMascotas.Clear();
            lstMascotas.Items.Clear();
            DataTable T1 = Oacceso.conectarDB("SELECT * FROM Mascotas ORDER BY nombre desc");
            foreach (DataRow fila in T1.Rows)
            {
                Mascota m = new Mascota();
                m.Codigo = int.Parse(fila["codigo"].ToString());
                m.Nombre = Convert.ToString(fila["nombre"]);
                m.Especie = (int)(fila["especie"]);
                m.Sexo = Convert.ToInt32(fila["sexo"]);
                m.Fecha = Convert.ToDateTime(fila["fechaNacimiento"]);
                lMascotas.Add(m);
                lstMascotas.Items.Add(m.ToString());
            }
        }
        private void cargarCombo()
        {
            DataTable T1 = Oacceso.conectarDB("SELECT * FROM Especies ORDER BY 2");
            cbEspecie.DataSource = T1;
            cbEspecie.ValueMember = "idEspecie";
            cbEspecie.DisplayMember = "nombreEspecie";
            cbEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void Habilitar(bool v)
        {
            txCodigo.Enabled = v;
            txNombre.Enabled = v;
            cbEspecie.Enabled = v;
            rbHembra.Enabled = v;
            rbMacho.Enabled = v;
            dtFecha.Enabled = v;
            btnEditar.Enabled = v;
            btnGrabar.Enabled = v;
            btnNuevo.Enabled = !v;
            btnSalir.Enabled = !v;
            lstMascotas.Enabled = !v;
            btCancelar.Enabled = !v;
        }
        private void limpiar()
        {
            txCodigo.Text = "";
            txNombre.Text = "";
            cbEspecie.SelectedIndex = 1;
            rbHembra.Checked = false;
            rbMacho.Checked = false;
            dtFecha.Value = DateTime.Today;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar(true);
            limpiar();
            txCodigo.Focus();
            btCancelar.Enabled = true;
        }
        private void restaurar(bool v)
        {
            txCodigo.Enabled = v;
            txNombre.Enabled = v;
            cbEspecie.Enabled = v;
            rbHembra.Enabled = v;
            rbMacho.Enabled = v;
            dtFecha.Enabled = v;
            txCodigo.Text = "";
            txNombre.Text = "";
            cbEspecie.SelectedIndex = 1;
            rbHembra.Checked = false;
            rbMacho.Checked = false;
            dtFecha.Value = DateTime.Today;
            btnEditar.Enabled = v;
            btnGrabar.Enabled = v;
            btnNuevo.Enabled = v;
            btnSalir.Enabled = v;
            lstMascotas.Enabled = v;
            btCancelar.Enabled = !v;
        }
        private void btCancelar_Click(object sender, EventArgs e)
        {
            restaurar(true);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de salir?"
                , "Saliendo..."
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
                this.Close();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            //Valida datos
            if (validarDatos())
            {
                //si valida bien, crea un objeto
                Mascota M = new Mascota();
                M.Nombre = txNombre.Text;
                M.Codigo = int.Parse(txCodigo.Text);
                M.Especie = cbEspecie.SelectedIndex;
                if (rbHembra.Checked)
                {
                    M.Sexo = 2;
                }
                else
                    M.Sexo = 1;
                M.Fecha = dtFecha.Value;

                if (!existe(M))
                {
                    string insertSQL = "INSERT INTO Mascotas VALUES (" +
                    M.Codigo + ",'" +
                    M.Nombre + "', " +
                    M.Especie + "," +
                    M.Sexo + ",'" +
                    M.Fecha.ToString("yyyy/MM/dd") + "')";

                    if (Oacceso.actualizarDB(insertSQL) > 0)
                    {
                        MessageBox.Show("se inserto la mascota");
                        cargarLista();
                    }
                }
                else
                {
                    MessageBox.Show("la mascota ya existe");
                }
                    Habilitar(false); 
            }

        }
        private bool existe(Mascota nueva)
        {
            for (int i = 0; i < lMascotas.Count; i++)
            {
                if (lMascotas[i].Codigo == nueva.Codigo)
                    return true;
            }
            return false;
        }
        private bool validarDatos()
        {

            if (txCodigo.Text == "")
            {
                MessageBox.Show("Debe ingresar un codigo...");
                txCodigo.Focus();
                return false;
            }
            else
            {
                try
                {
                    int.Parse(txCodigo.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Debe ingresar valores numéricos...");
                    txCodigo.Focus();
                    return false;
                }
                if (txNombre.Text == "") {
                    MessageBox.Show("Debe ingresar un nombre...");
                    txNombre.Focus();
                    return false;
                }
                if (cbEspecie.SelectedIndex == -1) {
                    MessageBox.Show("Debe seleccionar una especie...");
                    cbEspecie.Focus();
                    return false;
                }
                if (rbHembra.Checked == false && rbMacho.Checked == false) {
                    MessageBox.Show("Debe seleccionar un sex...");
                    rbMacho.Focus();
                    return false;
                }
                if (DateTime.Today.Year - dtFecha.Value.Year > 10) {
                    MessageBox.Show("la mascota es muy mayor...");
                    dtFecha.Focus();
                    return false;
                }
                return true;
            }
        }

        private void btBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show( $"desea eliminar a la mascota" + lMascotas[lstMascotas.SelectedIndex]  +"?" , "borrando",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2
            )== DialogResult.Yes) 
            {
                string deletesql = $"DELETE FROM Mascotas WHERE codigo = {lMascotas[lstMascotas.SelectedIndex].Codigo}";
                Oacceso.actualizarDB(deletesql);
                limpiar();
                cargarLista();
            }


        }

        private void cargarCampos(int posicion)
        {
            Mascota m = new Mascota();
            txCodigo.Text = lMascotas[posicion].Codigo.ToString();
            txNombre.Text = lMascotas[posicion].Nombre.ToString();
            cbEspecie.SelectedValue = lMascotas[posicion].Especie;
            if (lMascotas[posicion].Sexo == 1)
            {
                rbMacho.Checked = true;
            }
            else
            {
                rbHembra.Checked = true;
            }
            dtFecha.Value = lMascotas[posicion].Fecha;
        }
        private void lstMascotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarCampos(lstMascotas.SelectedIndex);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Habilitar(true);
            txCodigo.Enabled = false; //deshabilitar la PK
            txCodigo.Focus();
        }

        private void btBorrarT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"desea borrar a todas las mascotas??", "borrando",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Warning,
           MessageBoxDefaultButton.Button2
           ) == DialogResult.Yes)
            {
                string deletesql = $"DELETE FROM Mascotas";
                Oacceso.actualizarDB(deletesql);
                limpiar();
                cargarLista();
            }
        }

        private void btcargarlista_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"la cantidad de mascotas Macho es : {Machos}");
            
        }

        private void btordenar_Click(object sender, EventArgs e)
        {
            {
                lMascotas.Clear();
                lstMascotas.Items.Clear();
                DataTable T1 = Oacceso.conectarDB("SELECT * FROM Mascotas ORDER BY nombre ");
                foreach (DataRow fila in T1.Rows)
                {
                    Mascota m = new Mascota();
                    m.Codigo = int.Parse(fila["codigo"].ToString());
                    m.Nombre = Convert.ToString(fila["nombre"]);
                    m.Especie = (int)(fila["especie"]);
                    m.Sexo = Convert.ToInt32(fila["sexo"]);
                    m.Fecha = Convert.ToDateTime(fila["fechaNacimiento"]);
                    lMascotas.Add(m);
                    lstMascotas.Items.Add(m.ToString());
                }
            }
        }
    }
}
