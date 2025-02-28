using System.Text.RegularExpressions;

namespace Backend.Constants {
    public class RegularExpressions {
        public static Regex NAME_REGEXP = 
            new Regex(
                @"^[a-zA-Zа-яА-ЯёЁ_]{1}" +
                @"[a-zA-Zа-яА-ЯёЁ0-9 ]{0,70}$"
            );

        public static Regex DESCRIPTION_REGEXP =
            new Regex(
                @"^[a-zA-Zа-яА-ЯёЁ0-9 " +
                @",\.!?\{\}\[\]\(\)" +
                @"<>\^|&%$#@\-\+\*\\\=\~_" +
                "\\`\\'\"]{0,313}$"
            );

        public static Regex TEXT_REGEXP =
            new Regex(
                @"^[a-zA-Zа-яА-ЯёЁ0-9 " +
                @",\.!?\{\}\[\]\(\)" +
                @"<>\^|&%$#@\-\+\*\\\=\~_" +
                "\\`\\'\"]{0,1380}$"
            );

        public static Regex PERSON_NAME_REGEXP =
            new Regex(
                @"^[A-ZА-ЯЁ]" +
                @"[a-zА-ЯёЁ]{0,39}$"
            );
        public static Regex EMAIL_REGEXP =
            new Regex(
                @"^[a-zA-Z]([a-zA-Z0-9\.\-]{1,38}[a-zA-Z])?"+
                @"\@[a-z0-9]([a-z0-9\-]{0,125}[a-z0-9])?"+
                @"(\.[a-z]{1,7}){1,2}$"
            );
        public static Regex LOGIN_REGEXP =
            new Regex("^[a-zA-Z_][a-zA-Z0-9_]{0,39}$");
        public static Regex PASSWORD_REGEXP =
            new Regex(
                "^[a-zA-Z0-9`~!?@#$%^&*()\\[\\]{}<>+\\-*/=:;'\\\\_.|/ ]{1,40}$"
            );
    }
}
