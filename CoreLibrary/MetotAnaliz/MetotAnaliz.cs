using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.MetotAnaliz
{

    public static class MetotAnaliz
    {
        public static void AnalizEt()
        {
            var db = new BaseRepository();

            Assembly entryAssembly = Assembly.GetEntryAssembly()!;

            int projectId = Helpers.Helpers.SelectOrInsert(new Project() { ProjectName = entryAssembly.GetName().Name! });

            Type[] types = entryAssembly.GetTypes();
            foreach (Type type in types)
            {
                object[] classAttributes = type.GetCustomAttributes(false);
                foreach (var attr in classAttributes)
                {
                    var attrName = attr.GetType().Name;
                    if (attrName == "ApiControllerAttribute")
                    {

                        int controllerId = Helpers.Helpers.SelectOrInsert(new Controller() { ProjectId = projectId, ControllerName = type.Name });

                        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        foreach (MethodInfo method in methods)
                        {

                            Helpers.Helpers.SelectOrInsert(new Method() { ControllerId = controllerId, MethodName = method.Name });
                        }
                    }
                }
            }
        }
    }
}
