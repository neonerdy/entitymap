﻿
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
    public class EntityManagerFactory
    {
        public static IEntityManager CreateInstance(DataSource dataSource)
        {
            return new EntityManager(dataSource);
        }
    }
}
