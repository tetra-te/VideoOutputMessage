using System.Reflection;
using System.Runtime.Loader;

namespace VideoOutputMessage
{
    internal class LoadContext : AssemblyLoadContext
    {
        public LoadContext() : base(isCollectible: false)
        {
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
