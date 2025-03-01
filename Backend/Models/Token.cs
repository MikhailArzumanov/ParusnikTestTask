using Backend.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Backend.Models {
    #region Класс Token
    /** Класс Token
     * <summary>
     *  Класс служит целям проверки прав пользователя на совершение ряда операций.<br/><br/>
     *  В отличие от стандарта JWT, применяется в совокупности с симметричным шифрованием.<br/><br/>
     *  Данный факт позволяет помещать в объект токена данные авторизации и полноценно получать запись пользователя.<br/><br/>
     *  Содержит следующие поля:<br/>
     *  Логин пользователя <see cref="UserLogin"/><br/>
     *  Пароль пользователя <see cref="UserPassword"/><br/>
     *  Дата истечения срока действия <see cref="Expires"/><br/>
     * </summary>
     */
    public class Token {
        #region Поля данных авторизации
        /** Поле UserLogin
         * <summary>
         *  Поле логина пользователя-владельца токена.<br/>
         *  См. <see cref="Token"/>
         * </summary>
         */
        public string   UserLogin    { get; set; } = String.Empty;
        /** Поле UserPassword
         * <summary>
         *  Поле пароля пользователя-владельца токена.<br/>
         *  См. <see cref="Token"/>
         * </summary>
         */
        public string   UserPassword { get; set; } = String.Empty;
        #endregion
        #region Дата истечения срока действия
        /** Поле Expires
         * <summary>
         *  Поле даты истечения срока действия токена.<br/>
         *  См. <see cref="Token"/>
         * </summary>
         */
        public DateTime Expires      { get; set; } = DateTime.MinValue;
        #endregion
        #region Сериализация
        /** Функция ToJson
         * <summary>
         *  Функция, осуществляющая конвертацию объекта в json-строку.<br/>
         *  См. <see cref="Token"/>
         * </summary>
         */
        public string ToJson() {
            var json = JsonTokenSerializer.ToJson(this);
            return json;
        }
        /** Функция FromJson
         * <summary>
         *  Функция, осуществляющая конвертацию json-строку в объект.<br/>
         *  См. <see cref="Token"/>
         * </summary>
         */
        public static Token? FromJson(string json) {
            var token = JsonTokenSerializer.FromJson(json);
            return token;
        }
        #endregion
    }
    #endregion
}
