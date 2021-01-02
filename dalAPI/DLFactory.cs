using System;
using System.Collections.Generic;
using System.Reflection;

namespace DalApi
{
    public static class DLFactory
    {
        public static IDal GetDL()
        {
            // get dal implementation name from config.xml according to <data> element
            string dlType = DLConfig.DLName;
            // bring package name (dll file name) for the dal name (above) from the list of packages in config.xml
            DLConfig.DLPackage dlPackage;
            try // get dal package info according to <dal> element value in config file
            {
                dlPackage = DLConfig.DLPackages[dlType];
            }
            catch (KeyNotFoundException ex)
            {
                // if package name is not found in the list - there is a problem in config.xml
                throw new DLConfigException($"Wrong DL type: {dlType}", ex);
            }
            string dlPackageName = dlPackage.PkgName;
            string dlNameSpace = dlPackage.NameSpace;
            string dlClass = dlPackage.ClassName;

            try // Load into CLR the dal implementation assembly according to dll file name (taken above)
            {
                Assembly.Load(dlPackageName);
            }
            catch (Exception ex)
            {
                throw new DLConfigException($"Failed loading {dlPackageName}.dll", ex);
            }

            Type type;
            try
            {
                type = Type.GetType($"{dlNameSpace}.{dlClass}, {dlPackageName}", true);
            }
            catch (Exception ex)
            { // If the type is not found - the implementation is not correct - it looks like the class name is wrong...
                throw new DLConfigException($"Class not found due to a wrong namespace or/and class name: {dlPackageName}:{dlNameSpace}.{dlClass}", ex);
            }

            try
            {
                IDal dal = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) as IDal;
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                if (dal == null)
                    throw new DLConfigException($"Class {dlNameSpace}.{dlClass} instance is not initialized");
                // now it looks like we have appropriate dal implementation instance :-)
                return dal;
            }
            catch (NullReferenceException ex)
            {
                throw new DLConfigException($"Class {dlNameSpace}.{dlClass} is not a singleton", ex);
            }

        }
    }
}