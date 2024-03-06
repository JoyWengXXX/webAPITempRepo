using Autofac;
using System.Reflection;

namespace CommonLibrary.AutofacHelper
{
    public class AutofacModuleRegister : Autofac.Module
    {
        private readonly string _nameSpace;
        private readonly string _endWith;
        private readonly Assembly _assembly;

        /// <summary>
        /// 設定Autofac需讀取的參數
        /// </summary>
        /// <param name="nameSpace">需註冊類別檔的命名空間</param>
        /// <param name="endWith">需註冊類別檔的結尾名詞</param>
        /// <param name="assembly"></param>
        public AutofacModuleRegister(string nameSpace, string endWith, Assembly assembly)
        {
            _nameSpace = nameSpace;
            _assembly = assembly;
            _endWith = endWith;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_assembly)
                   .Where(t => t.Namespace == _nameSpace && t.Name.EndsWith(_endWith))
                   .AsImplementedInterfaces();
        }
    }
}
