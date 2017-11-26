
/*===========================================================
 * 
 * EntitMap 
 * 
 * Lightweight Data Access Layer Framework
 *
 * Version      : 2.0
 * Author       : Ariyanto
 * E-Mail       : neonerdy@gmail.com
 * Last Updated : 06/28/2011
 *  
 * 
 * © 2009, Under Apache Licence 
 * 
 *==========================================================
 */


using System;
using System.Data.Common;

namespace EntityMap
{
    public class ConnectionFactory
    {
        public static DbConnection CreateConnection(DataSource dataSource)
        {
            DbConnection conn=null;
            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(dataSource.Provider);
                conn = factory.CreateConnection();
                conn.ConnectionString = dataSource.ConnectionString;
                conn.Open();
            }
            catch(DbException ex)
            {
                throw ex;
            }
            return conn;

        }


    }
}
