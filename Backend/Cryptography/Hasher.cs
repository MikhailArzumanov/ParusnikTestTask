using System.Security.Cryptography;
using System.Text;

namespace Backend.Cryptography {
    #region Класс Hasher
    /** Класс Hasher
     * <summary>
     *  Класс представляет функционал генерации контрольной суммы от строк символов.<br/>
     *  Для генерации префикса используется алгоритм MD5.<br/>
     *  Для суффикса -- SHA1.<br/>
     *  Основной алгоритм -- SHA2-512.<br/>
     * </summary>
     */
    public class Hasher {
        #region Функционал генерации префикса
        /** Функция GetLSalt
         * <summary>
         *  Функция осуществляет генерацию динамического префикса, 
         *  зависящего от длины передаваемой строки.<br/>
         *  <br/>
         *  Используемый алгоритм -- MD5.<br/>
         *  <br/>
         *  Требуется для исключения атак, основанных на таблицах контрольных сумм.
         * </summary>
         * <param name="plain">
         *  Исходный текст по которому ведётся генерация.
         * </param>
         * <returns>
         *  Возвращает сгенерирвоанный префикс.
         * </returns>
         */
        private static string GetLSalt(string plain) {
            var l = plain.Length;
            var bytes = BitConverter.GetBytes(l);
            var salt = MD5.HashData(bytes);
            return Convert.ToBase64String(salt);
        }
        #endregion
        #region Функционал генерации суффикса
        /** Функция GetRSalt
         * <summary>
         *  Функция осуществляет генерацию динамического суффикса, 
         *  зависящего от длины передаваемой строки.<br/>
         *  <br/>
         *  Используемый алгоритм -- SHA1.<br/>
         *  <br/>
         *  Требуется для исключения атак, основанных на таблицах контрольных сумм.
         * </summary>
         * <param name="plain">
         *  Исходный текст по которому ведётся генерация.
         * </param>
         * <returns>
         *  Возвращает сгенерирвоанный суффикс.
         * </returns>
         */
        private static string GetRSalt(string plain) {
            var l = plain.Length;
            var bytes = BitConverter.GetBytes(l);
            var salt = SHA1.HashData(bytes);
            return Convert.ToBase64String(salt);
        }
        #endregion
        #region Функционал генерации контрольной суммы
        /** Функция GetRSalt
         * <summary>
         *  Функция осуществляет генерацию контрольной суммы.<br/>
         *  Предварительно генерируются префикс и суффикс. <br/>
         *  (См. <see cref="GetLSalt(string)"/>; <see cref="GetRSalt(string)"/>)<br/>
         *  <br/>
         *  Строки интерпретируются с использованием кодировки UTF8.<br/>
         *  <br/>
         *  Используемый алгоритм -- SHA2-512.
         * </summary>
         * <param name="plain">
         *  Исходный текст по которому ведётся генерация.
         * </param>
         * <returns>
         *  Возвращает сгенерированную контрольную сумму, отображенную в Base64.
         * </returns>
         */
        public static string Hash(string plain) {
            string lSalt = GetLSalt(plain), 
                   rSalt = GetRSalt(plain);
            string salted = $"{lSalt}{plain}{rSalt}";
            var saltedBytes = Encoding.UTF8.GetBytes(salted);
            var hash = SHA512.HashData(saltedBytes);
            return Convert.ToBase64String(hash);
        }
        #endregion
    }
    #endregion
}
