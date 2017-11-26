
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

namespace EntityMap
{
    public class DataSource
    {
        private string provider;
        private string connectionString;
       
        public DataSource()
        {

        }

        public DataSource(string provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }


    }
}
