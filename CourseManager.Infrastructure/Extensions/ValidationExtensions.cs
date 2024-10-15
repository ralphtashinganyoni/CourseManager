using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Infrastructure.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsPrimitive(this Type t)
        {
            return t.IsPrimitive || t == typeof(decimal) || t == typeof(string) || t == typeof(DateTime) || t == typeof(Guid);
        }
    }
}
