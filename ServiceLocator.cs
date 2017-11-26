
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
using System.Reflection;
using System.Collections.Generic;

namespace EntityMap
{
    public class ServiceLocator
    {
        private static IDictionary<object, Type> repositories = new Dictionary<object, Type>();
        private static IDictionary<object, object> propertyDependencyList = new Dictionary<object, object>();
        private static IDictionary<object,object> ctorDepedencyList=new Dictionary<object,object>();
     
        private static T GetObjectFromRepository<T>(T instance)
        {
            if (ctorDepedencyList.Count > 0)
            {
                if (ctorDepedencyList.ContainsKey(typeof(T)))
                {
                    object[] ctorValues = (object[])ctorDepedencyList[typeof(T)];
                    instance = (T)Activator.CreateInstance(repositories[typeof(T)], ctorValues);
                }
               
                 InjectProperty<T>(instance);
            }
            else
            {
                instance = (T)Activator.CreateInstance(repositories[typeof(T)]);
                InjectProperty<T>(instance);
            }
            return instance;
        }



        private static object GetObjectFromRepository(string objectId,object instance)
        {
            if (ctorDepedencyList.Count >0)
            {
                if (ctorDepedencyList.ContainsKey(objectId))
                {
                    object[] ctorValues = (object[])ctorDepedencyList[objectId];

                    instance = Activator.CreateInstance(repositories[objectId], ctorValues);
                }

                InjectProperty(objectId,instance);
            }
            else
            {
                instance = Activator.CreateInstance(repositories[objectId]);
                InjectProperty(objectId,instance);
            }
            return instance;
        }


        
        private static void InjectProperty<T>(T instance)
        {
            if (propertyDependencyList.Count>0)
            {
                if (propertyDependencyList.ContainsKey(typeof(T)))
                {
                    IDictionary<string, object> props = (IDictionary<string, object>)propertyDependencyList[typeof(T)];

                    foreach (KeyValuePair<string, object> p in props)
                    {
                        PropertyInjector.SetValue(instance, p.Key, p.Value);
                    }
                }
            }
        }

        private static void InjectProperty(string objectId,object instance)
        {
            if (propertyDependencyList.Count>0)
            {
                if (propertyDependencyList.ContainsKey(objectId))
                {
                    IDictionary<string, object> props = (IDictionary<string, object>)propertyDependencyList[objectId];

                    foreach (KeyValuePair<string, object> p in props)
                    {
                        PropertyInjector.SetValue(instance, p.Key, p.Value);
                    }
                }
            }
        }

       
        public static void RegisterObject(string objectId, Type type)
        {
            if (!repositories.ContainsKey(objectId))
            {
                repositories.Add(objectId, type);
            }
        }

        public static void RegisterObject(string objectId, Type type, object[] ctorDependency)
        {
            if (!repositories.ContainsKey(objectId))
            {
                repositories.Add(objectId, type);
                ctorDepedencyList.Add(objectId, ctorDependency);
            }
        }


        public static void RegisterObject(string objectId, Type type,
            IDictionary<string, object> propertyDependency)
        {
            if (!repositories.ContainsKey(objectId))
            {
                repositories.Add(objectId, type);
                propertyDependencyList.Add(objectId,propertyDependency);
             }
        }


        public static void RegisterObject(string objectId, Type type,
            object[] ctorDependency, IDictionary<string, object> propertyDependency)
        {
            if (!repositories.ContainsKey(objectId))
            {
                repositories.Add(objectId, type);
                ctorDepedencyList.Add(objectId, ctorDependency);
                propertyDependencyList.Add(objectId,propertyDependency);
            }
        }


        public static void RegisterObject<T, V>()
        {
            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), typeof(V));
            }
        }


        public static void RegisterObject<T, V>(object[] ctorDependency)
        {
            if (ctorDependency == null)
                throw new Exception("Constructor depedency is null");

            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), typeof(V));
                ctorDepedencyList.Add(typeof(T), ctorDependency);
            }
        }


        public static void RegisterObject<T, V>(IDictionary<string, object> propertyDependency)
        {
            if (propertyDependency == null)
                throw new Exception("Property depedency is null");

            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), typeof(V));
                propertyDependencyList.Add(typeof(T),propertyDependency);
            }
        }


        public static void RegisterObject<T, V>(object[] ctorDependency,
            IDictionary<string, object> propertyDependency)
        {
            if (ctorDependency == null)
                throw new Exception("Constructor depedency is null");

            if (propertyDependency == null)
                throw new Exception("Property depedency is null");

            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), typeof(V));
                ctorDepedencyList.Add(typeof(T), ctorDependency);
                propertyDependencyList.Add(typeof(T),propertyDependency);
            }
        }




        public static T GetObject<T>()
        {
            T instance = default(T);
            try
            {
                instance = GetObjectFromRepository<T>(instance);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return instance;
        }



        public static object GetObject(string objectId)
        {
            object instance = null;
            try
            {              
                if (repositories.ContainsKey(objectId))
                {
                    instance = GetObjectFromRepository(objectId,instance);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return instance;
        }

       
        public static void AddRegistry(IRegistry registry)
        {
            registry.Configure();
        }

     

    }
}
