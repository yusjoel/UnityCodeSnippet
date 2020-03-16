using System.Text.RegularExpressions;

namespace Extensions
{
    public static class StringExtension
    {
        /// <summary>
        ///     将Camel风格和全小写的下划线风格的合法命名改成Pascal风格
        ///     aaa_bbb_ccc => AaaBbbCcc
        ///     _aaa => Aaa
        ///     aaaBbbCcc => AaaBbbCcc
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string self)
        {
            string pascalCaseName = string.Empty;

            var substrings = self.Split('_');
            foreach (string substring in substrings)
            {
                var chars = substring.ToCharArray();

                for (int i = 0; i < chars.Length; i++)
                    if (i == 0) pascalCaseName += chars[0].ToString().ToUpper();
                    else pascalCaseName += chars[i];
            }

            return pascalCaseName;
        }

        /// <summary>
        ///     将Pascal风格和全小写的下划线风格的合法命名改成Pascal风格
        ///     aaa_bbb_ccc => aaaBbbCcc
        ///     _Aaa_Bbb => aaaBbb
        ///     AaaBbbCcc => aaaBbbCcc
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string self)
        {
            string pascalCaseName = string.Empty;

            var substrings = self.Split('_');
            for (int i = 0; i < substrings.Length; i++)
            {
                string substring = substrings[i];
                var chars = substring.ToCharArray();

                for (int j = 0; j < chars.Length; j++)
                    if (j == 0)
                        if (i == 0)
                            pascalCaseName += chars[0].ToString().ToLower();
                        else
                            pascalCaseName += chars[0].ToString().ToUpper();
                    else
                        pascalCaseName += chars[j];
            }

            return pascalCaseName;
        }

        /// <summary>
        ///     将合法的文件名转成合法的命名
        ///     1xxxx => Name1xxxx
        ///     idle@hero => idle_hero
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToLegalSymbolName(this string self)
        {
            string legalSymbolName = "";
            char first = self[0];
            string name = self;
            if (char.IsDigit(first))
                name = "Name" + self;

            var chars = name.ToCharArray();
            foreach (char c in chars)
                if (char.IsLetterOrDigit(c))
                    legalSymbolName += c;
                else
                    legalSymbolName += "_";

            return legalSymbolName;
        }

        /// <summary>
        ///     去掉字符串中的数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNumber(this string key)
        {
            return Regex.Replace(key, @"\d", "");
        }
    }
}
