using Backend.Constants;
using Backend.Models;
using Backend.Models.Interfaces;
using System.Text.RegularExpressions;

namespace Backend.Validation {
    #region Класс Validator
    /** Класс Validator
     * <summary>
     *  Класс несёт своей целью 
     *  осуществлять проверку валидности поставляемых данных.
     * <br/><br/>
     *  В рамках входных данных выступают сущности базы данных.
     * <br/><br/>
     *  Проверка валидности осуществляется 
     *  с использованием регулярных выражений <see cref="Regex"/>.<br/>
     * </summary>
     */
    public class Validator {
        #region Функция Validate(IHasProjectData)
        /** Функция Validate(IHasProjectData)
         * <summary>
         *  Функция осуществляет проверку валидности наименования и описания проекта.
         * </summary>
         * <param name="subject">
         *  Объект проекта, подвергаемый валидации.
         * </param>
         * <returns>
         *  Возвращает текст ошибки, либо пустую строку при её остутствии.
         * </returns>
         */
        public static string Validate(IHasProjectData subject) {
            var nameRegexp = RegularExpressions.NAME_REGEXP;
            if (!nameRegexp.IsMatch(subject.Name)) {
                return RespMsgs.Projects.NAME_IS_NONVALID;
            }
            var descriptionRegexp = RegularExpressions.DESCRIPTION_REGEXP;
            if (!descriptionRegexp.IsMatch(subject.Description)) {
                return RespMsgs.Projects.DESCRIPTION_IS_NONVALID;
            }
            return String.Empty;
        }
        #endregion
        #region Функция Validate(IHasTaskData)
        /** Функция Validate(IHasTaskData)
         * <summary>
         *  Функция осуществляет проверку валидности наименования, описания и статуса задачи.
         * </summary>
         * <param name="subject">
         *  Объект задачи, подвергаемый валидации.
         * </param>
         * <returns>
         *  Возвращает текст ошибки, либо пустую строку при её остутствии.
         * </returns>
         */
        public static string Validate(IHasTaskData subject) {
            var nameRegexp = RegularExpressions.NAME_REGEXP;
            if (!nameRegexp.IsMatch(subject.Name)) {
                return RespMsgs.ProjectsTasks.NAME_IS_NONVALID;
            }
            var descriptionRegexp = RegularExpressions.DESCRIPTION_REGEXP;
            if (!descriptionRegexp.IsMatch(subject.Description)) {
                return RespMsgs.ProjectsTasks.DESCRIPTION_IS_NONVALID;
            }
            if (subject.StatusId == 0) {
                return RespMsgs.ProjectsTasks.TASK_STATUS_IS_NONVALID;
            }
            return String.Empty;
        }
        #endregion
        #region Функция Validate(IHasStatusData)
        /** Функция Validate(IHasStatusData)
         * <summary>
         *  Функция осуществляет проверку валидности наименования и описания статуса задачи.
         * </summary>
         * <param name="subject">
         *  Объект статуса, подвергаемый валидации.
         * </param>
         * <returns>
         *  Возвращает текст ошибки, либо пустую строку при её остутствии.
         * </returns>
         */
        public static string Validate(IHasStatusData subject) {
            var nameRegexp = RegularExpressions.NAME_REGEXP;
            if (!nameRegexp.IsMatch(subject.Title)) {
                return RespMsgs.TaskStatuses.TITLE_IS_NONVALID;
            }
            var descriptionRegexp = RegularExpressions.DESCRIPTION_REGEXP;
            if (!descriptionRegexp.IsMatch(subject.Description)) {
                return RespMsgs.TaskStatuses.DESCRIPTION_IS_NONVALID;
            }
            return String.Empty;
        }
        #endregion
        #region Функция Validate(IHasCommentaryData)
        /** Функция Validate(IHasCommentaryData)
         * <summary>
         *  Функция осуществляет проверку валидности текста комментария.
         * </summary>
         * <param name="subject">
         *  Объект комментария, подвергаемый валидации.
         * </param>
         * <returns>
         *  Возвращает текст ошибки, либо пустую строку при её остутствии.
         * </returns>
         */
        public static string Validate(IHasCommentaryData subject) {
            var textRegexp = RegularExpressions.TEXT_REGEXP;
            if (!textRegexp.IsMatch(subject.CommentText)) {
                return RespMsgs.Commentaries.TEXT_IS_NONVALID;
            }
            return String.Empty;
        }
        #endregion
        #region Функция Validate(IHasUserData)
        /** Функция Validate(IHasUserData)
         * <summary>
         *  Функция осуществляет проверку валидности<br/>
         *  фамилии, имени, адреса электронной почты,<br/>
         *  логина и пароля пользователя.
         * </summary>
         * <param name="subject">
         *  Объект пользователя, подвергаемый валидации.
         * </param>
         * <returns>
         *  Возвращает текст ошибки, либо пустую строку при её остутствии.
         * </returns>
         */
        public static string Validate(IHasUserData subject) {
            var nameRegexp = RegularExpressions.NAME_REGEXP;
            if (!nameRegexp.IsMatch(subject.Name)) {
                return RespMsgs.Users.NAME_IS_NONVALID;
            }
            if (!nameRegexp.IsMatch(subject.Surname)) {
                return RespMsgs.Users.SURNAME_IS_NONVALID;
            }
            var emailRegexp = RegularExpressions.EMAIL_REGEXP;
            if (!emailRegexp.IsMatch(subject.Email)) {
                return RespMsgs.Users.EMAIL_IS_NONVALID;
            }
            var loginRegexp = RegularExpressions.LOGIN_REGEXP;
            if (!loginRegexp.IsMatch(subject.Login)) {
                return RespMsgs.Users.LOGIN_IS_NONVALID;
            }
            var passwordRegexp = RegularExpressions.PASSWORD_REGEXP;
            if (!passwordRegexp.IsMatch(subject.Password)) {
                return RespMsgs.Users.PASSWORD_IS_NONVALID;
            }
            return String.Empty;
        }
        #endregion
    }
    #endregion
}
