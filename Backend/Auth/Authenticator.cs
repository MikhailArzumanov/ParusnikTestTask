using Backend.Cryptography;
using Backend.Data;
using Backend.Models;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Backend.Authentication {
    #region Класс Authenticator
    /** Класс Authenticator
     * <summary>
     *  Класс несёт своей целью инкапсуляцию логики аутентификации.<br/>
     *  <br/>
     *  Класс предоставляет интерфейс аутентификации 
     *  через функцию <see cref="Authenticate(HttpRequest)"/>
     * </summary>
     */
    public class Authenticator {
        #region Поля
        /** Поле db
         * <summary>
         *  Поле содержит результат инъекции контекста приложения.
         * </summary>
         */
        private ApplicationContext db    ;
        /** Поле aesKey
         * <summary>
         *  Поле содержит ключ симметричного шифрования.
         * </summary>
         */
        private byte[]             aesKey;
        #endregion
        #region Конструктор
        /** Конструктор Authenticator(ApplicationContext, IConfiguration)
         * <summary>
         *  Конструктор осуществляет инъекцию контекста приложения <br/>
         *  и выведение симметричного ключа.
         * </summary>
         * <param name="db">Контекст приложения</param>
         * <param name="configuration">Интерфейс конфигурации</param>
         */
        public Authenticator(
            ApplicationContext db           ,
            IConfiguration     configuration
        ) {
            this.db = db;
            var keyText = configuration["Tokens:Key"] ?? String.Empty;
            aesKey = Encoding.UTF8.GetBytes(keyText);
        }
        #endregion
        #region Функция пользователя по-умолчанию
        /** Константа DEFAULT_USER_ID 
         * <summary>
         *  Константа содержит идентификатор пользователя по-умолчанию.
         * </summary>
         */
        private const int DEFAULT_USER_ID = 1;
        /** Функция GetDefaultUser
         * <summary>
         *  Функция осуществляет получение объекта пользователя по-умолчанию.
         * </summary>
         * <returns>
         *  Возвращает полученный объект.
         * </returns>
         */
        private User? GetDefaultUser() {
            var defaultUser = db.Users.FirstOrDefault(
                x => x.Id == DEFAULT_USER_ID
            );
            return defaultUser;
        }
        #endregion
        #region Инкапсулированная логика аутентификации
        /** Функция GetTokenStr(string)
         * <summary>
         *  Функция осуществляет выведение строки токена из значения заголовка.
         * </summary>
         * <param name="header">Значение заголовка аутентификации.</param>
         * <returns>Возвращает строку токена.</returns>
         */
        private string GetTokenStr(string header) {
            int startingIndex = "Bearer ".Length;
            string tokenStr = header.Length > startingIndex ? 
                header[startingIndex..] : "";
            return tokenStr;
        }
        /** Функция GetToken(string)
         * <summary>
         *  Функция осуществляет выведение объекта токена из строки.
         * </summary>
         * <param name="tokenStr">Строка токена.</param>
         * <returns>Возвращает объект токена.</returns>
         */
        private Token GetToken(string tokenStr) {
            byte[] encryptedTokenBytes = Convert.FromBase64String(tokenStr);
            byte[] tokenBytes = AesProvider.Decrypt(encryptedTokenBytes, aesKey);
            string tokenJson = Encoding.UTF8.GetString(tokenBytes);
            var token = Token.FromJson(tokenJson);
            return token;
        }
        /** Функция Authenticate(string)
         * <summary>
         *  Функция осуществляет выведение объекта пользователя из строки токена.
         * </summary>
         * <param name="tokenStr">Строка токена.</param>
         * <returns>Возвращает объект пользователя, либо значение null.</returns>
         */
        public User? Authenticate(string tokenStr) {
            Token token;
            try {
                 token = GetToken(tokenStr);
            } catch(Exception exception) {
                return null;
            }
            if(token.Expires < DateTime.Now) {
                return null;
            }
            var user = db.Users.FirstOrDefault(
                x => x.Login    == token.UserLogin 
                &&   x.Password == token.UserPassword
            );
            return user;
        }
        #endregion
        #region Функция Authenticate(HttpRequest)
        /** Функция Authenticate(HttpRequest)
         * <summary>
         *  Функция осуществляет попытку получения записи пользователя исходя из<br/>
         *  данных авторизации в заголовках передаваемого объекта HttpRequest.
         * </summary>
         * <param name="request">Объект http-запроса.</param>
         * <returns>Возвращает объект пользователя, либо значение null.</returns>
         */
        public User? Authenticate(HttpRequest request) {
            var header = request.Headers["Authorization"];
            var headerStr = header.ToString();
            if (headerStr == String.Empty || headerStr == null) {
                var defaultUser = GetDefaultUser();
                return defaultUser;
            }
            var tokenStr = GetTokenStr(headerStr);
            var user = Authenticate(tokenStr);
            return user;
        }
        #endregion
    }
    #endregion
}
