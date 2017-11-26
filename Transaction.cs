
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
    public class Transaction : IDisposable
    {
        private DbConnection conn;
        private DbTransaction tx;
     
        public Transaction(DbConnection conn)
        {
            try
            {
                this.conn = conn;
                tx = conn.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbConnection GetConnection()
        {
            return conn;
        }
       

        public DbTransaction GetTransaction()
        {
            return tx; 
        }

        public void Commit()
        {
            try
            {
                tx.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Rollback()
        {
            try
            {
                tx.Rollback();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        #region IDisposable Members

        public void Dispose()
        {
            if (this.tx != null)
                tx.Dispose();
            if (this.conn != null)
                conn.Dispose();
           
       }

        #endregion
    }
}
