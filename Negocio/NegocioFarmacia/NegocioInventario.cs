﻿using Capa_Conexion;
using Capa_DTO.Farmacia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.NegocioFarmacia
{
    public class NegocioInventario
    {
        //objeto clase Conexion
        private Conexion conectar;
        //secuencia id
        private string _seq_inventario = "seq_inventario.nextval";

        public string Seq_inventario
        {
            get
            {
                return _seq_inventario;
            }

            set
            {
                _seq_inventario = value;
            }
        }

        public Conexion Conectar
        {
            get
            {
                return conectar;
            }

            set
            {
                conectar = value;
            }
        }

        public void configuraConexion()
        {
            Conectar = new Conexion();
            Conectar.NombreBaseDeDatos = "FUNDACION";
            Conectar.NombreTabla = "INVENTARIO";
            Conectar.CadenaConexion = "DATA SOURCE=localhost;USER ID=FUNDACION ; password =123";

        }

        public int ingresarInventario(Inventario inventario)
        {

            configuraConexion();
            conectar.CadenaSQL = String.Format("INSERT INTO {0}(id_inventario, observaciones, cantidad_productos, fecha_inventario) VALUES({1},'{2}',{3},TO_DATE('{4}','dd/mm/yyyy hh24:mi:ss'))",
                                                   conectar.NombreTabla, Seq_inventario,inventario.Observaciones,inventario.Cantidad_productos,inventario.Fecha_inventario);


            conectar.EsSelect = false;
            return conectar.conecta();

        }


        public int eliminarInventario(int id)
        {

            this.configuraConexion();
            this.conectar.CadenaSQL = string.Format("DELETE FROM {0} WHERE id_inventario = {1}",
                                                  this.conectar.NombreTabla, id);
            this.conectar.EsSelect = false;
            return this.conectar.conecta();
        }





        public int actualizarInventario(Inventario inv)
        {
            this.configuraConexion();
            this.conectar.CadenaSQL = String.Format("UPDATE inventario SET cantidad_productos={0},"
                                                        + " observaciones ='{1}',"
                                                        + " fecha_inventario=to_date('{2}','dd/mm/yyyy hh24:mi:ss')"                                                        
                                                        + " WHERE id_inventario ={3}"
                                                        ,inv.Cantidad_productos,inv.Observaciones,inv.Fecha_inventario,inv.Id_inventario);
            this.conectar.EsSelect = false;
            return this.conectar.conecta();
        }


        public int consultarInventario(String observaciones, DateTime fecha , int cantidad)
        {
            configuraConexion();
            conectar.CadenaSQL = String.Format("SELECT id_inventario FROM {0} WHERE observaciones = '{1}' AND fecha_inventario = to_date('{2}','dd/mm/yyyy hh24:mi:ss') and cantidad_productos={3}",
                                     conectar.NombreTabla, observaciones,fecha,cantidad);
            conectar.EsSelect = true;

            int id_inventario=0;
            if (conectar.conecta()!=0)
	        {       
		        System.Data.DataTable dt = new System.Data.DataTable();
                dt = conectar.DbDataSet.Tables[0];                
                id_inventario = int.Parse(dt.Rows[0]["id_inventario"].ToString());

	        }       
                       
            return id_inventario;
        }
               

        public System.Data.DataSet inventarioModificado(int id)
        {

            configuraConexion();
            conectar.CadenaSQL = String.Format("SELECT * FROM {0} WHERE id_inventario = '{1}'",
                                    conectar.NombreTabla, id);
            conectar.EsSelect = true;
            conectar.conecta();

            return conectar.DbDataSet;
        }
//        select med.NOM_COMERCIAL,bm.stock, inv.CANTIDAD_PRODUCTOS ,
//inv.FECHA_INVENTARIO ,
//inv.OBSERVACIONES  from inventario inv 
//inner join bodega_med bm 
//on inv.id_inventario=bm.id_inventario
//join medicina med 
//on bm.ID_MEDICINA=med.ID_MEDICINA;
        public System.Data.DataSet listarInventarios()
        {

            configuraConexion();
            conectar.CadenaSQL = "select med.NOM_COMERCIAL as NOMBRE,bm.stock, inv.CANTIDAD_PRODUCTOS AS CANTIDAD,"
                                    + "TO_DATE(inv.FECHA_INVENTARIO,'dd/mm/yyyy') AS FECHA ,"
                                    + "inv.OBSERVACIONES  from inventario inv "
                                    + "inner join bodega_med bm "
                                    + "on inv.id_inventario=bm.id_inventario "
                                    + "join medicina med "
                                    + "on bm.ID_MEDICINA=med.ID_MEDICINA";
            conectar.EsSelect = true;
            conectar.conecta();

            return conectar.DbDataSet;
        }
        public Inventario consultarPorId(int id)
        {

            configuraConexion();

            Inventario auxInventario = new Inventario();

            conectar.CadenaSQL = String.Format("SELECT * FROM {0} WHERE id_inventario = {1} order by id_inventario",
                                      conectar.NombreTabla, id);
            conectar.EsSelect = true;
            conectar.conecta();

            if (conectar.conecta() != 0)
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                if (conectar.DbDataSet.Tables[0].Rows.Count >= 1)
                {
                    dt = conectar.DbDataSet.Tables[0];

                    try
                    {
                        auxInventario.Id_inventario = int.Parse(dt.Rows[0]["id_inventario"].ToString());
                        auxInventario.Cantidad_productos = int.Parse(dt.Rows[0]["cantidad_productos"].ToString());
                        auxInventario.Fecha_inventario = DateTime.Parse(dt.Rows[0]["fecha_inventario"].ToString());
                        auxInventario.Observaciones = dt.Rows[0]["observaciones"].ToString();                        

                    }
                    catch (Exception)
                    {
                        auxInventario.Id_inventario = 0;
                        auxInventario.Cantidad_productos = 0;
                        auxInventario.Fecha_inventario = DateTime.Now;
                        auxInventario.Observaciones = "";
                    }
                }
            }
            else
            {
                auxInventario = null;
            }
            return auxInventario;
        }
    }
}
