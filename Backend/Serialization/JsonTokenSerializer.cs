using Backend.Models;
using System.Text.RegularExpressions;
using System.Text.Json;

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
            var json = JsonSerializer.Serialize<Token>(token); ;
            return json;
        }
        #endregion
        #region Десериализация
        /** Функция FromJson
         * <summary>
         *  Функция осуществляет десериализацию json-строки токена. <br/>
         *  См. <see cref="JsonTokenSerializer"/>
         * </summary>
         * <param name="json">
         *  Json-строка.
         * </param>
         */
        public static Token? FromJson(string json) {
            var token = JsonSerializer.Deserialize<Token>(json);
            return token;
        }
        #endregion
    }
    #endregion
}
