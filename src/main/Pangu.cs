using System;
using System.Text.RegularExpressions;

namespace Pangu
{
    /* 自动在文本中所有的中日韩文字与半角的字母、数字、符号之间插入空白，让文字变得美观好看。 */

    /*
     * 汉学家称这个空格符为「盘古之白」，因为它劈开了全角字和半角字之间的混沌。另有研究显示，打字的时候
     * 不喜欢在中文和英文之间加空格的人，感情路都走得很辛苦，有七成的比例会在 34 岁的时候跟自己不爱的人结婚，
     * 而其余三成的人最后只能把遗产留给自己的猫。毕竟爱情跟书写都需要适时地留白。
     *
     * 与大家共勉之。
     */
    public class Pangu
    {
        /*
         * 为方便起见，以下是一些捕获组模式。
         *
         * CJK: Chinese, Japanese and Korean（中日韩文字）
         * ANS: Alphabet, Number, Symbol（字母、数字、符号）
         *
         * CJK 包含以下 Unicode 区段：
         * \u2e80-\u2eff 中日韩汉字部首补充
         * \u2f00-\u2fdf 偏旁
         * \u3040-\u309f 平假名
         * \u30a0-\u30ff 片假名
         * \u3100-\u312f 注音符号
         * \u3200-\u32ff 中日韩带圈字符及月份
         * \u3400-\u4dbf 中日韩统一表意文字扩展区 A
         * \u4e00-\u9fff 中日韩统一表意文字
         * \uf900-\ufaff 中日韩兼容表意文字
         */

        private static readonly Regex CJK_quote = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "([\"'])"
        );

        private static readonly Regex quote_CJK = new Regex(
            "([\"'])" +
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])"
        );

        private static readonly Regex fix_quote = new Regex("([\"']+)(\\s*)(.+?)(\\s*)([\"']+)");

        private static readonly Regex fix_single_quote = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "()" +
            "(')" +
            "([a-z])",
            RegexOptions.IgnoreCase
        );

        private static readonly Regex hash_ANS_CJK_hash = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "(#)" +
            "([a-z0-9\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff]+)" +
            "(#)" +
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])",
            RegexOptions.IgnoreCase
        );

        private static readonly Regex CJK_hash = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "(#([^ ]))"
        );

        private static readonly Regex hash_CJK = new Regex(
            "(([^ ])#)" +
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])"
        );

        private static readonly Regex CJK_operator_ANS = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "([+\\-*/=&|<>])" +
            "([a-z0-9])",
            RegexOptions.IgnoreCase
        );

        private static readonly Regex ANS_operator_CJK = new Regex(
            "([a-z0-9])" +
            "([+\\-*/=&|<>])" +
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])",
            RegexOptions.IgnoreCase
        );

        private static readonly Regex CJK_bracket_CJK = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "([([{<\u201c] + (.*?)[)]}>\u201d]+)" +
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])"
        );

        private static readonly Regex CJK_bracket = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "([([{<\u201c>])"
        );

        private static readonly Regex bracket_CJK = new Regex(
            "([)]}>\u201d<])" +
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])"
        );

        private static readonly Regex fix_bracket = new Regex(@"([\(\[\{<\u201c] +)(\\s *)(.+?)(\\s *)([\)\]\}>\u201d]+)");

        private static readonly Regex fix_symbol = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "([~!;:,.?\u2026])" +
            "([a-z0-9])",
            RegexOptions.IgnoreCase
        );

        private static readonly Regex CJK_ANS = new Regex(
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])" +
            "([a-z0-9`$%^&*-=+\\|/@\u00a1-\u00ff\u2022\u2027\u2150-\u218f])",
            RegexOptions.IgnoreCase
        );

        private static readonly Regex ANS_CJK = new Regex(
            "([a-z0-9`~$%^&*-=+\\|/!;:,.?\u00a1-\u00ff\u2022\u2026\u2027\u2150-\u218f])" +
            "([\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff])",
            RegexOptions.IgnoreCase
        );

        /* 在文本中加上盘古空白 */
        public static string SpacingText(string text)
        {
            var newText = text;

            newText = CJK_quote.Replace(newText, "$1 $2");
            newText = quote_CJK.Replace(newText, "$1 $2");
            newText = fix_quote.Replace(newText, "$1$3$5");
            newText = fix_single_quote.Replace(newText, "$1$3$4");

            newText = hash_ANS_CJK_hash.Replace(newText, "$1 $2$3$4 $5");
            newText = CJK_hash.Replace(newText, "$1 $2");
            newText = hash_CJK.Replace(newText, "$1 $3");

            newText = CJK_operator_ANS.Replace(newText, "$1 $2 $3");
            newText = ANS_operator_CJK.Replace(newText, "$1 $2 $3");

            var oldText = newText;
            var tmpText = CJK_bracket_CJK.Replace(newText, "$1 $2 $4");
            newText = tmpText;
            if (oldText.Equals(tmpText))
            {
                newText = CJK_bracket.Replace(newText, "$1 $2");
                newText = bracket_CJK.Replace(newText, "$1 $2");
            }
            newText = fix_bracket.Replace(newText, "$1$3$5");

            newText = fix_symbol.Replace(newText, "$1$2 $3");

            newText = CJK_ANS.Replace(newText, "$1 $2");
            newText = ANS_CJK.Replace(newText, "$1 $2");

            return newText;
        }

        /* 在字节流中加上盘古空白 */
        public static byte[] SpacingByteStream(byte[] bytes)
        {
            if (!EncodingManager.IsTextFile(bytes)) // 产生该字节流的文件不是文本文件
                throw new FormatException("此文件不是文本文件，不可转换。");
            var encoding = EncodingManager.GetEncodingType(bytes); // 获取字节流的编码
            var originalText = encoding.GetString(bytes); // 将字节流转化为字符串
            var targetText = SpacingText(originalText); // 在字符串中加入盘古空白
            return encoding.GetBytes(targetText); // 将转换后的字符串重新转化为字节流
        }
    }
}