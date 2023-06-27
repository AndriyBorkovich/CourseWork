using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CourseWork
{
    //клас для розширення функціоналу класу Enum
    public static class EnumExtensions
    {
        //метод-розширення, який передбачає отримання атрибуту Name
        public static DisplayAttribute GetDisplayAttributesFrom(this Enum enumValue, Type enumType)
        {
            return enumType.GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>();
        }
    }
}
