using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MascotasNuevoEditar
{
    internal class AccesoDatos
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader Lector;
        String cadenaConexion;

        public AccesoDatos()
        {
            cadenaConexion = @"Data Source=DESKTOP-9IP8KIP\SQLEXPRESS01;Initial Catalog=Veterinaria;Integrated Security=True";
            conexion = new SqlConnection(cadenaConexion);
            comando = new SqlCommand();
        }
        private void conectar()
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
        }
        public void desconectar()
        {
            conexion.Close();
        }
        public DataTable conectarDB(string consultaSql)
        {
            DataTable t = new DataTable();
            conectar();
            comando.CommandText = consultaSql;
            t.Load(comando.ExecuteReader());
            desconectar();
            return t;
        }
        public int actualizarDB(string consultaSQl)
        {
            int filasAfectadas;
            conectar();
            comando.CommandText = consultaSQl;
            filasAfectadas = comando.ExecuteNonQuery();
            desconectar();
            return filasAfectadas;
        }

    }
}
