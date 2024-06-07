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
            var pRepo = new BaseRepository<Project>();
            var cRepo = new BaseRepository<Controller>();
            var mRepo = new BaseRepository<Method>();

            Assembly entryAssembly = Assembly.GetEntryAssembly()!;
            //proje kaydedilir id döner.
            var projectId = pRepo.Insert(new Project() { ProjectName = entryAssembly.GetName().Name! });
            Type[] types = entryAssembly.GetTypes();
            foreach (Type type in types)
            {
                object[] classAttributes = type.GetCustomAttributes(false);
                foreach (var attr in classAttributes)
                {
                    var attrName = attr.GetType().Name;
                    if (attrName == "ApiControllerAttribute")
                    {
                        //controller kaydeder id döner
                        var controllerId = cRepo.Insert(new Controller() { ProjectId = projectId, ControllerName = type.Name });
                        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        foreach (MethodInfo method in methods)
                        {
                            //metotları kaydeder.
                            mRepo.Insert(new Method() { ControllerId = controllerId, MethodName = method.Name });
                        }
                    }
                }
            }
        }
    }
}
