namespace Backend.Constants {
    public class RespMsgs {
        public const string TOKEN_INVALID =
            "Срок токена аутентификации истёк или токен невалиден.";
        public const string NOT_ENOUGH_RIGHTS =
            "У пользователя недостаточно прав для совершения данной операции.";
        public const string ID_NOT_FOUND =
            "Запись с заданным идентификатором не найдена.";

        public const string NONVALID_NAME =
                "Имя начинается с буквы, либо подчеркивания. " +
                "Остальные символы аналогичны с допущением цифр. " +
                "Число символов в имени не должно превышать 71 знак.";
        public const string NONVALID_DESCRIPTION =
            "Описание может содержать только стандартный набор символов. " +
            "Число символов в описании не должно превышать 313 знаков.";

        public const string NONVALID_TEXT =
            "Текст может содержать только стандартный набор символов. " +
            "Число символов в тексте не должно превышать 1380 знаков.";
        public class Projects {
            public const string NAME_IS_NONVALID =
                "Имя проекта невалидно. " + NONVALID_NAME;
            public const string DESCRIPTION_IS_NONVALID =
                "Описание проекта невалидно. " + NONVALID_DESCRIPTION;

            public const string ID_NOT_FOUND =
                "Запись проекта с заданным идентификатором не найдена.";

            public const string USER_NOT_FOUND =
                "Пользователь с заданным идентификатором не принимает участие в проекте.";
        }

        public class ProjectsTasks {
            public const string NAME_IS_NONVALID =
                "Имя задачи невалидно. " + NONVALID_NAME;
            public const string DESCRIPTION_IS_NONVALID =
                "Описание задачи невалидно. " + NONVALID_DESCRIPTION;
            public const string TASK_STATUS_IS_NONVALID =
                "Статус задачи невалиден.";

            public const string ID_NOT_FOUND =
                "Запись задачи с заданным идентификатором не найдена.";
            public const string ANOTHER_PROJECT_TASK =
                "Задача относится к другому проекту.";
        }

        public class TaskStatuses {
            public const string TITLE_IS_NONVALID =
                "Заглавие статуса невалидно. " + NONVALID_NAME;
            public const string DESCRIPTION_IS_NONVALID =
                "Описание статуса невалидно. " + NONVALID_DESCRIPTION;

            public const string ID_NOT_FOUND =
                "Запись статуса с заданным идентификатором не найдена.";
        }

        public class Commentaries {
            public const string TEXT_IS_NONVALID =
                "Текст комментария невалиден. " + NONVALID_TEXT;

            public const string ID_NOT_FOUND =
                "Запись комментария с заданным идентификатором не найдена.";
            public const string ANOTHER_TASK_COMMENTARY =
                "Комментарий относится к иной задаче.";
        }

        public class Users {
            public const string NAME_IS_NONVALID =
                "Имя пользователя невалидно. " +
                "Имя начинается с заглавной буквы, содержит не более 40 символов.";
            public const string SURNAME_IS_NONVALID =
                "Фамилия пользователя невалидна. " +
                "Фамилия начинается с заглавной буквы, содержит не более 40 символов.";
            public const string EMAIL_IS_NONVALID =
                "Адрес электронной почты невалиден.";
            public const string LOGIN_IS_NONVALID =
                "Логин пользователя невалиден. " +
                "Логин начинается с буквы латиницы, либо подчеркивания. " +
                "Остальными символами могут также выступать цифры. " +
                "Логин содержит не более 40 символов.";
            public const string PASSWORD_IS_NONVALID =
                "Пароль пользователя невалиден. " +
                "Символами пароля могут выступать цифры, буквы латиницы, " +
                "а также следующие спецсимволы: `~!?@#$%^&*()[]{}<>+-*/=:;'\\_.| . " +
                "Пароль содержит не более 40 символов.";

            public const string ID_NOT_FOUND =
                "Запись пользователя с заданным идентификатором не найдена.";
            public const string USER_NOT_FOUND =
                "Запись пользователя с заданным параметрами не найдена.";
            public const string ALREADY_IN_PROJECT = 
                "Пользователь уже добавлен в проект.";
        }

    }
}
