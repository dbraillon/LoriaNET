using System.Globalization;

namespace Loria.Core
{
    public class Configuration
    {
        public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("en-US");
        public CultureInfo Culture { get; set; }

        public Configuration()
        {
            Culture = CultureInfo.CurrentCulture;
        }
    }
}
