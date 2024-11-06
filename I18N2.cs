/// i18n for 2 languages
using System;

namespace winforms_explorer
{
    /// <summary>
    /// Assume L is a two-item enum.
    /// </summary>
    /// <typeparam name="L"></typeparam>
    public class I18N2<L> where L : Enum
    {
        public I18N2(L e)
        {
            Lang = e;
        }
        private L lang = default;
        private delegate string AorB(string a, string b);
        private AorB aorb;
        private static string aAorB(string a, string _) => a;
        private static string bAorB(string _, string b) => b;
        public L Lang
        {
            get => lang;
            set {
                if (Convert.ToByte(value) == 0)
                    aorb = aAorB;
                else
                    aorb = bAorB;
                lang = value;

            }
        }
        public string T(string a, string b) => aorb(a, b);

    }
}