using System.Reflection;

using Newtonsoft.Json.Serialization;

namespace Paddi.DemoUsages.ApiDemo.Serializations
{
    public class CancellationTokenIgnoreContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (typeof(CancellationToken).IsAssignableFrom(property.PropertyType))
            {
                property.ShouldSerialize = _ => false;
            }

            return property;
        }
    }
}
