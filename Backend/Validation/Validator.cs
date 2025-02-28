using Backend.Constants;
using Backend.Models;
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
        #region Функция Validate(Project)
        /** Функция Validate(Project)
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
        public static string Validate(Project subject) {
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
        #region Функция Validate(ProjectTask)
        /** Функция Validate(ProjectTask)
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
        public static string Validate(ProjectTask subject) {
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
        #region Функция Validate(TasksStatus)
        /** Функция Validate(TasksStatus)
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
        public static string Validate(TasksStatus subject) {
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
        #region Функция Validate(Commentary)
        /** Функция Validate(Commentary)
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
        public static string Validate(Commentary subject) {
            var textRegexp = RegularExpressions.TEXT_REGEXP;
            if (!textRegexp.IsMatch(subject.CommentText)) {
                return RespMsgs.Commentaries.TEXT_IS_NONVALID;
            }
            return String.Empty;
        }
        #endregion
        #region Функция Validate(User)
        /** Функция Validate(User)
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
        public static string Validate(User subject) {
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
