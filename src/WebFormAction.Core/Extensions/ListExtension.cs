using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebFormAction.Core.Extensions
{
    public static class ListExtension
    {
        public static List<T> Clone<T>(this List<T> list)
        {
            var newList = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(list));
            return newList;
        }
    }
}
