using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace ListenPortG4
{
    public static class ComandosBD
    {
        public enum RetornoBD
        {
            NonQuery, Scalar
        }

        public static DataTable Select(string query)
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter(query, Properties.Settings.Default.ConexaoMDF);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //adaptation for database .sdf - update at 03 april 2014 - IT WORKS!
        public static DataTable SelectSDF(string query)
        {
            try
            {
                SqlCeDataAdapter conn = new SqlCeDataAdapter(query, Properties.Settings.Default.ConexaoSDF);
                DataTable dt = new DataTable();
                conn.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }          
           
        }

        public static DataTable Select(string query, SqlParameter[] parametros)
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter(query, Properties.Settings.Default.ConexaoMDF);
                adp.SelectCommand.Parameters.AddRange(parametros);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable Select(string query, int timeOut)
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter(query, Properties.Settings.Default.ConexaoMDF);
                adp.SelectCommand.CommandTimeout = timeOut;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable Select(string query, int timeOut, SqlParameter[] parametros)
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter(query, Properties.Settings.Default.ConexaoMDF);
                adp.SelectCommand.CommandTimeout = timeOut;
                adp.SelectCommand.Parameters.AddRange(parametros);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object UpdateDeleteInsert(string query, RetornoBD retornoBD)
        {
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(Properties.Settings.Default.ConexaoMDF));
            try
            {
                cmd.Connection.Open();
                return (retornoBD == RetornoBD.NonQuery ? cmd.ExecuteNonQuery() : cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        //for database .SDF without parameters
        public static object UpdateDeleteInsertSDF(string query, RetornoBD retornoBD)
        {
            SqlCeCommand cmd = new SqlCeCommand(query, new SqlCeConnection(Properties.Settings.Default.ConexaoSDF));
            try
            {
                cmd.Connection.Open();
                return (retornoBD == RetornoBD.NonQuery ? cmd.ExecuteNonQuery() : cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
         }

        public static object UpdateDeleteInsert(string query, RetornoBD retornoBD, SqlParameter[] parametros)
        {
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(Properties.Settings.Default.ConexaoMDF));
            try
            {
                cmd.Connection.Open();
                cmd.Parameters.AddRange(parametros);
                return (retornoBD == RetornoBD.NonQuery ? cmd.ExecuteNonQuery() : cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
}
