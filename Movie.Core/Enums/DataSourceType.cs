using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Enums
{
    public enum DataSourceType
    {
        [Description("integrated")]
        INTEGRATED = 1,
        [Description("youtube")]
        YOUTUBE = 2,
        [Description("tmdb")]
        TMDB = 3,
        [Description("cache")]
        CACHE = 4,
        [Description("none")]
        NONE = 0
    }

    public static class AttributesHelperExtension
    {
        public static string Description(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }

    public class EnumConverter
    {
        public static DataSourceType Parse(string value)
        {
            try
            {
                DataSourceType src = (DataSourceType)Enum.Parse(typeof(DataSourceType), value, true);
                return src;
            }
            catch
            {
                return DataSourceType.NONE;
            }
        }
    }
}
