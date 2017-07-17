using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.CognitiveServices
{
    public class Voice
    {
        public static Voice GetFromCulture(CultureInfo culture)
        {
            var voices = new List<Voice>
            {
                new Voice("ar-EG", "Female", "Microsoft Server Speech Text to Speech Voice (ar-EG, Hoda)"),
                new Voice("ar-SA", "Male", "Microsoft Server Speech Text to Speech Voice (ar-SA, Naayf)"),
                new Voice("ca-ES", "Female", "Microsoft Server Speech Text to Speech Voice (ca-ES, HerenaRUS)"),
                new Voice("cs-CZ", "Male", "Microsoft Server Speech Text to Speech Voice (cs-CZ, Vit)"),
                new Voice("da-DK", "Female", "Microsoft Server Speech Text to Speech Voice (da-DK, HelleRUS)"),
                new Voice("de-AT", "Male", "Microsoft Server Speech Text to Speech Voice (de-AT, Michael)"),
                new Voice("de-CH", "Male", "Microsoft Server Speech Text to Speech Voice (de-CH, Karsten)"),
                new Voice("de-DE", "Female", "Microsoft Server Speech Text to Speech Voice (de-DE, Hedda)"),
                new Voice("de-DE", "Female", "Microsoft Server Speech Text to Speech Voice (de-DE, HeddaRUS)"),
                new Voice("de-DE", "Male", "Microsoft Server Speech Text to Speech Voice (de-DE, Stefan, Apollo)"),
                new Voice("el-GR", "Male", "Microsoft Server Speech Text to Speech Voice (el-GR, Stefanos)"),
                new Voice("en-AU", "Female", "Microsoft Server Speech Text to Speech Voice (en-AU, Catherine)"),
                new Voice("en-AU", "Female", "Microsoft Server Speech Text to Speech Voice (en-AU, HayleyRUS)"),
                new Voice("en-CA", "Female", "Microsoft Server Speech Text to Speech Voice (en-CA, Linda)"),
                new Voice("en-CA", "Female", "Microsoft Server Speech Text to Speech Voice (en-CA, HeatherRUS)"),
                new Voice("en-GB", "Female", "Microsoft Server Speech Text to Speech Voice (en-GB, Susan, Apollo)"),
                new Voice("en-GB", "Female", "Microsoft Server Speech Text to Speech Voice (en-GB, HazelRUS)"),
                new Voice("en-GB", "Male", "Microsoft Server Speech Text to Speech Voice (en-GB, George, Apollo)"),
                new Voice("en-IE", "Male", "Microsoft Server Speech Text to Speech Voice (en-IE, Shaun)"),
                new Voice("en-IN", "Female", "Microsoft Server Speech Text to Speech Voice (en-IN, Heera, Apollo)"),
                new Voice("en-IN", "Female", "Microsoft Server Speech Text to Speech Voice (en-IN, PriyaRUS)"),
                new Voice("en-IN", "Male", "Microsoft Server Speech Text to Speech Voice (en-IN, Ravi, Apollo)"),
                new Voice("en-US", "Female", "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)"),
                new Voice("en-US", "Female", "Microsoft Server Speech Text to Speech Voice (en-US, JessaRUS)"),
                new Voice("en-US", "Male", "Microsoft Server Speech Text to Speech Voice (en-US, BenjaminRUS)"),
                new Voice("es-ES", "Female", "Microsoft Server Speech Text to Speech Voice (es-ES, Laura, Apollo)"),
                new Voice("es-ES", "Female", "Microsoft Server Speech Text to Speech Voice (es-ES, HelenaRUS)"),
                new Voice("es-ES", "Male", "Microsoft Server Speech Text to Speech Voice (es-ES, Pablo, Apollo)"),
                new Voice("es-MX", "Female", "Microsoft Server Speech Text to Speech Voice (es-MX, HildaRUS)"),
                new Voice("es-MX", "Male", "Microsoft Server Speech Text to Speech Voice (es-MX, Raul, Apollo)"),
                new Voice("fi-FI", "Female", "Microsoft Server Speech Text to Speech Voice (fi-FI, HeidiRUS)"),
                new Voice("fr-CA", "Female", "Microsoft Server Speech Text to Speech Voice (fr-CA, Caroline)"),
                new Voice("fr-CA", "Female", "Microsoft Server Speech Text to Speech Voice (fr-CA, HarmonieRUS)"),
                new Voice("fr-CH", "Male", "Microsoft Server Speech Text to Speech Voice (fr-CH, Guillaume)"),
                new Voice("fr-FR", "Female", "Microsoft Server Speech Text to Speech Voice (fr-FR, Julie, Apollo)"),
                new Voice("fr-FR", "Female", "Microsoft Server Speech Text to Speech Voice (fr-FR, HortenseRUS)"),
                new Voice("fr-FR", "Male", "Microsoft Server Speech Text to Speech Voice (fr-FR, Paul, Apollo)"),
                new Voice("he-IL", "Male", "Microsoft Server Speech Text to Speech Voice (he-IL, Asaf)"),
                new Voice("hi-IN", "Female", "Microsoft Server Speech Text to Speech Voice (hi-IN, Kalpana, Apollo)"),
                new Voice("hi-IN", "Female", "Microsoft Server Speech Text to Speech Voice (hi-IN, Kalpana)"),
                new Voice("hi-IN", "Male", "Microsoft Server Speech Text to Speech Voice (hi-IN, Hemant)"),
                new Voice("hu-HU", "Male", "Microsoft Server Speech Text to Speech Voice (hu-HU, Szabolcs)"),
                new Voice("id-ID", "Male", "Microsoft Server Speech Text to Speech Voice (id-ID, Andika)"),
                new Voice("it-IT", "Male", "Microsoft Server Speech Text to Speech Voice (it-IT, Cosimo, Apollo)"),
                new Voice("ja-JP", "Female", "Microsoft Server Speech Text to Speech Voice (ja-JP, Ayumi, Apollo)"),
                new Voice("ja-JP", "Male", "Microsoft Server Speech Text to Speech Voice (ja-JP, Ichiro, Apollo)"),
                new Voice("ko-KR", "Female", "Microsoft Server Speech Text to Speech Voice (ko-KR, HeamiRUS)"),
                new Voice("nb-NO", "Female", "Microsoft Server Speech Text to Speech Voice (nb-NO, HuldaRUS)"),
                new Voice("nl-NL", "Female", "Microsoft Server Speech Text to Speech Voice (nl-NL, HannaRUS)"),
                new Voice("pl-PL", "Female", "Microsoft Server Speech Text to Speech Voice (pl-PL, PaulinaRUS)"),
                new Voice("pt-BR", "Female", "Microsoft Server Speech Text to Speech Voice (pt-BR, HeloisaRUS)"),
                new Voice("pt-BR", "Male", "Microsoft Server Speech Text to Speech Voice (pt-BR, Daniel, Apollo)"),
                new Voice("pt-PT", "Female", "Microsoft Server Speech Text to Speech Voice (pt-PT, HeliaRUS)"),
                new Voice("ro-RO", "Male", "Microsoft Server Speech Text to Speech Voice (ro-RO, Andrei)"),
                new Voice("ru-RU", "Female", "Microsoft Server Speech Text to Speech Voice (ru-RU, Irina, Apollo)"),
                new Voice("ru-RU", "Male", "Microsoft Server Speech Text to Speech Voice (ru-RU, Pavel, Apollo)"),
                new Voice("sk-SK", "Male", "Microsoft Server Speech Text to Speech Voice (sk-SK, Filip)"),
                new Voice("sv-SE", "Female", "Microsoft Server Speech Text to Speech Voice (sv-SE, HedvigRUS)"),
                new Voice("th-TH", "Male", "Microsoft Server Speech Text to Speech Voice (th-TH, Pattara)"),
                new Voice("tr-TR", "Female", "Microsoft Server Speech Text to Speech Voice (tr-TR, SedaRUS)"),
                new Voice("zh-CN", "Female", "Microsoft Server Speech Text to Speech Voice (zh-CN, HuihuiRUS)"),
                new Voice("zh-CN", "Female", "Microsoft Server Speech Text to Speech Voice (zh-CN, Yaoyao, Apollo)"),
                new Voice("zh-CN", "Male", "Microsoft Server Speech Text to Speech Voice (zh-CN, Kangkang, Apollo)"),
                new Voice("zh-HK", "Female", "Microsoft Server Speech Text to Speech Voice (zh-HK, Tracy, Apollo)"),
                new Voice("zh-HK", "Female", "Microsoft Server Speech Text to Speech Voice (zh-HK, TracyRUS)"),
                new Voice("zh-HK", "Male", "Microsoft Server Speech Text to Speech Voice (zh-HK, Danny, Apollo)"),
                new Voice("zh-TW", "Female", "Microsoft Server Speech Text to Speech Voice (zh-TW, Yating, Apollo)"),
                new Voice("zh-TW", "Female", "Microsoft Server Speech Text to Speech Voice (zh-TW, HanHanRUS)"),
                new Voice("zh-TW", "Male", "Microsoft Server Speech Text to Speech Voice (zh-TW, Zhiwei, Apollo)")
            };
            var voice = voices.FirstOrDefault(v => v.Gender == "Female" && v.Locale == culture.ToString());

            return voice;
        }
        
        public string Locale { get; set; }
        public string Gender { get; set; }
        public string ServiceName { get; set; }

        public Voice(string locale, string gender, string serviceName)
        {
            Locale = locale;
            Gender = gender;
            ServiceName = serviceName;
        }
    }
}
