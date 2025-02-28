using Backend.Cryptography;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Backend.Auth {
    #region Класс Authorization
    /** Класс Authorization
     * <summary>
     *  Класс инкапсулирует функционал выдачи временных регалий авторизационных сессий.
     * </summary>
     */
    public class Authorization {
        #region Функционал авторизации
        /** Функция GetToken
         * <summary>
         *  Функция осуществляет генерацию токена к записи пользователя.
         * </summary>
         */
        public static string GetToken(User entry, IConfiguration configuration) {
            var aesKeyStr = configuration["Tokens:Key"] ?? String.Empty;
            var aesKey = Encoding.UTF8.GetBytes(aesKeyStr);

            var lifetimeStr = configuration["Tokens:LifeTime"] ?? "14400";
            var lifetime = Convert.ToUInt32(lifetimeStr);
            var expirationStamp = DateTime.UtcNow.AddMinutes(lifetime);
            var token = new Token { 
                UserLogin    = entry.Login    , 
                UserPassword = entry.Password ,
                Expires      = expirationStamp,
            };

            var tokenJson  = token.ToJson();
            var tokenBytes = Encoding.UTF8.GetBytes(tokenJson);
            var encrypted  = AesProvider.Encrypt(tokenBytes, aesKey);
            var tokenStr   = Convert.ToBase64String(encrypted);

            return tokenStr;
        }
        #endregion
    }
    #endregion
}
