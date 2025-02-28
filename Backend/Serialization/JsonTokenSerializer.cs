using Backend.Models;
using System.Text.RegularExpressions;

namespace Backend.Serialization {
    #region Класс JsonTokenSerializer
    /** Класс JsonTokenSerializer
     * <summary>
     *  Класс инкапсулирует сериализацию одноуровнего json-токена.
     *  <br/><br/> 
     *  У данного класса отсутствует необходимость хранения состояния 
     *  и создания промежуточных объектов.
     *  <br/><br/>
     *  Данный класс предоставляет единичные статические функции <br/>
     *  <see cref="ToJson(Token)"/> и <br/>
     *  <see cref="FromJson(string)"/>, <br/>
     *  осуществляющие сериализацию.
     * </summary>
     */
    public class JsonTokenSerializer {
        #region Сериализация
        /** Функция ToJson
         * <summary>
         *  Функция осуществляет сериализацию объекта токена. <br/>
         *  См. <see cref="JsonTokenSerializer"/>
         * </summary>
         * <param name="token">
         *  Объект токена.
         * </param>
         */
        public static string ToJson(Token token) {
            var    loginPart = $"\"userLogin\":\"{token.UserLogin}\"";
            var passwordPart = $"\"userPassword\":\"{token.UserPassword}\"";
            var  expiresPart = $"\"expires\":\"{token.Expires}\"";
            var json = "{"+$"{loginPart},{passwordPart},{expiresPart}"+"}";
            return json;
        }
        #endregion
        #region Десериализация
        /** Функция ParseField
         * <summary>
         *  Функция осуществляет взятие значения из пары с ключем.
         * </summary>
         * <param name="json">
         *  Json-строка.
         * </param>
         * <param name="regexpStr">
         *  Регулярное выражение поля.<br/>
         *  Должно определять форматы пары ключ-значение и содержать группу "field". <br/>
         *  Прим. `"NAME":"(?field[^"])",`
         * </param>
         */
        private static string ParseField(string json, string regexpStr) {
            var regexp = new Regex(regexpStr);
            var fieldValue = regexp.Match(json).Groups["field"].Value;
            return fieldValue;
        }
        /** Функция FromJson
         * <summary>
         *  Функция осуществляет десериализацию json-строки токена. <br/>
         *  См. <see cref="JsonTokenSerializer"/>
         * </summary>
         * <param name="json">
         *  Json-строка.
         * </param>
         */
        public static Token FromJson(string json) {
            var login   = ParseField(json, regexpStr: "\"userLogin\":\"(?<field>[^\"]*)\"");
            var passwrd = ParseField(json, regexpStr: "\"userPassword\":\"(?<field>[^\"]*)\"");
            var expires = ParseField(json, regexpStr: "\"expires\":\"(?<field>[^\"]*)\"");
            var dtExpires = DateTime.Parse(expires);
            var token = new Token {
                UserLogin    = login     ,
                UserPassword = passwrd   ,
                Expires      = dtExpires ,
            };
            return token;
        }
        #endregion
    }
    #endregion
}
