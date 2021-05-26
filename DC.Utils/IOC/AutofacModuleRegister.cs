using Autofac;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DC.Utils.IOC
{
    /// <summary>
    /// AutoFac自动注入
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {

        /// <summary>
        /// dll文件名称列表
        /// </summary>
        public List<DllFile> DllFiles { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dllFiles"></param>
        public AutofacModuleRegister(List<DllFile> dllFiles)
        {
            DllFiles = dllFiles;
        }

        /// <summary>
        /// 加载DLL并且注入
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            var dls = DllFiles.Select(m => Assembly.LoadFile(Path.Combine(m.Path, m.Name))).ToArray();

            builder.RegisterAssemblyTypes(dls)
               .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Controller") || t.Name.EndsWith("Attribute"))
               .AsImplementedInterfaces()
               .PropertiesAutowired()
               .AsSelf()
               .InstancePerDependency();
        }
    }
}
