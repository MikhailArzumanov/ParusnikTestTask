using Backend.Models;
using Backend.Serialization;

namespace Backend.Tests.Serialization {
    #region Класс SerializationTests
    /** Класс SerializationTests
     * <summary>
     *  Класс предоставляет возможность тестирования модуля сериализации.
     * </summary>
     */
    public class SerializationTests {
        #region Поля
        /** Поле rng
         * <summary>
         *  Предоставляет возможность генерации случайных значений.
         * </summary>
         */
        Random rng = new Random();
        #endregion
        #region Конструктор
        public SerializationTests() { }
        #endregion
        #region Функция ThrowException
        /** Функция ThrowException
         * <summary>
         *  Осуществляет проброс исключения проваленного тестирования.
         * </summary>
         */
        private void ThrowException() {
            throw new Exception("Serialization test have failed.");
        }
        #endregion
        #region Функция AreEqual
        /** Функция AreEqual
         * <summary>
         *  Функция осуществляет сравнение двух токенов.
         * </summary>
         * <param name="x">Первый объект токена</param>
         * <param name="y">Второй объект токена</param>
         * <returns>Возвращает результат сравнения.</returns>
         */
        private bool AreEqual(Token x, Token y) {
            bool loginFieldEqual = x.UserLogin          == y.UserLogin          ;
            bool passwFieldEqual = x.UserPassword       == y.UserPassword       ;
            bool dateFieldEqual  = x.Expires.ToString() == y.Expires.ToString() ;
            return loginFieldEqual && passwFieldEqual && dateFieldEqual;
        }
        #endregion
        #region Тестирование сериализации токенов
        /** Функция Test
         * <summary>
         *  Функция осуществляет тестирование сериализации для заданного токена.
         * </summary>
         * <param name="subject">Токен тестирования</param>
         */
        private void Test(Token subject) {
            var jsonStr = JsonTokenSerializer.ToJson(subject);
            var rebuilded = JsonTokenSerializer.FromJson(jsonStr);
            if(!AreEqual(subject, rebuilded)) {
                ThrowException();
            }
        }
        /** Функция TokenSingleTest
         * <summary>
         *  Функция осуществляет тестирование сериализации для заданных параметров токена.
         * </summary>
         * <param name="loginLength">Число генерируемых байт логина</param>
         * <param name="passwLength">Число генерируемых байт пароля</param>
         */
        private void TokenSingleTest(int loginLength, int passwLength) {
            var loginBytes = new byte[loginLength];
            var passwBytes = new byte[passwLength];
            rng.NextBytes(loginBytes); rng.NextBytes(passwBytes);
            var login = Convert.ToBase64String(loginBytes);
            var passw = Convert.ToBase64String(passwBytes);
            var token = new Token {
                UserLogin = login,
                UserPassword = passw,
                Expires = DateTime.UtcNow,
            };
            Test(token);
        }
        /** Функция TestTokenSerialization
         * <summary>
         *  Функция осуществляет тестирование сериализации токенов в рамках ряда случаев.
         * </summary>
         */
        private void TestTokenSerialization() {
            for(int loginLength = 12; loginLength < 40; loginLength++) {
                for (int passwLength = 12; passwLength < 40; passwLength++) {
                    TokenSingleTest(loginLength, passwLength);
                }
            }
        }
        #endregion
        #region Внешний интерфейс
        /** Функция Test
         * <summary>
         *  Функция осуществляет тестирование модуля сериализации.
         * </summary>
         */
        public static void Test() {
            var instance = new SerializationTests();
            instance.TestTokenSerialization();
        }
        #endregion
    }
    #endregion
}
