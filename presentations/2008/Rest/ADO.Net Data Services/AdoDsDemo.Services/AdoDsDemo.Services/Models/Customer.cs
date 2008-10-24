using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Services.Common;
using System.IO;
using System.Reflection;

namespace AdoDsDemo.Services.Models
{
    [DataServiceKey("Name")]
    public class AssemblyInfo
    {       
        private Assembly _assembly;

        public AssemblyInfo(Assembly assembly)
        {
            _assembly = assembly;
        }

        public string Name { get { return _assembly.GetName().Name.Replace(".", "_"); } }
        public string Verison { get { return _assembly.GetName().Version.ToString(); }}        
        public List<TypeInfo> Types { get { return _assembly.GetTypes().Select(type => new TypeInfo(type)).ToList(); } }
    }

    [DataServiceKey("FullName")]
    public class TypeInfo
    {
        private Type _type;

        public TypeInfo(Type type)
        {
            _type = type;
        }

        public string FullName { get { return _type.FullName.Replace(".","_"); }}
        public string Name { get { return _type.Name; }}        
        public string NameSpace { get { return _type.Namespace; }}
        public TypeInfo BaseType { get { return new TypeInfo(_type.BaseType); }}
    }

    public class SystemEntities
    {
        public IQueryable<AssemblyInfo> Assemblies
        {
            get
            {
                return AppDomain.CurrentDomain.GetAssemblies().Select(assembly => new AssemblyInfo(assembly)).AsQueryable();
            }
        }

        public IQueryable<TypeInfo> Types
        {
            get
            {
                return Assemblies.SelectMany(assembly => assembly.Types).AsQueryable();
            }
        }
    }
}
