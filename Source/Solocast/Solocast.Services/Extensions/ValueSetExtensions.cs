using Newtonsoft.Json;
using Solocast.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Solocast.Services.Extensions
{
    public static class ValueSetExtensions
    {
        public static T ToObject<T>(this ValueSet valueSet)
        {
            var tObject = JsonConvert.DeserializeObject<T>(valueSet[nameof(T)].ToString());

            return tObject;
        }

        public static ValueSet ToValueSet<T>(this T tObject)
        {
            var valueSet = new ValueSet();
            valueSet.Add(nameof(T), JsonConvert.SerializeObject(tObject));

            return valueSet;
        }
    }
}
