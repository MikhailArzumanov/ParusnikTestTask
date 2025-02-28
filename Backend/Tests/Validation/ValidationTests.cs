using Backend.Constants;
using Backend.Cryptography;
using Backend.Models;
using Backend.Validation;

namespace Backend.Tests.Validation {
    #region Класс ValidationTests
    /** Класс ValidationTests
     * <summary>
     *  Класс предоставляет возможность тестирования модуля валидации.
     * </summary>
     */
    public class ValidationTests {
        #region Функция ThrowException
        /** Функция ThrowException
         * <summary>
         *  Осуществляет проброс исключения проваленного тестирования.
         * </summary>
         */
        private void ThrowException() {
            throw new Exception("Validation test have failed.");
        }
        #endregion
        #region Функция TestUsersValidation
        /** Функция TestUsersValidation
         * <summary>
         *  Осуществляет тестирование валидации пользователя.
         * </summary>
         */
        private void TestUsersValidation() {
            var user = new User {
                Name = "Имя",
                Surname = "Фамилия",
                Email = "anExample.address@mail.com",
                Login = "abcdefghijklmnopqrstuvwxyz1234567890",
                Password = "Example password",
            };
            if(Validator.Validate(user) != String.Empty) {
                ThrowException();
            }
            user.Login = "abcdefghijklmnopqrstuvwxyz1234567890\"";
            if (Validator.Validate(user) != RespMsgs.Users.LOGIN_IS_NONVALID) {
                ThrowException();
            }
            user.Login = "abcdefghijklmnopqrstuvwxyz1234567890";
            user.Name = "-+-+-+-+-";
            if (Validator.Validate(user) != RespMsgs.Users.NAME_IS_NONVALID) {
                ThrowException();
            }
            user.Name = "Имя";
            user.Email = "anExample@nonvalid-address@mail.com";
            if (Validator.Validate(user) != RespMsgs.Users.EMAIL_IS_NONVALID) {
                ThrowException();
            }
        }
        #endregion
        #region Функция TestCommentaryValidation
        /** Функция TestCommentaryValidation
         * <summary>
         *  Осуществляет тестирование валидации комментариев.
         * </summary>
         */
        private void TestCommentaryValidation() {
            var commentary = new Commentary {
                CommentText = "Valid text commentary."
            };
            if(Validator.Validate(commentary) != String.Empty) {
                ThrowException();
            }
            commentary.CommentText = "\0abc\0";
            if(Validator.Validate(commentary) != RespMsgs.Commentaries.TEXT_IS_NONVALID) {
                ThrowException();
            }
        }
        #endregion
        #region Функция TestTaskValidation
        /** Функция TestTaskValidation
         * <summary>
         *  Осуществляет тестирование валидации задачи.
         * </summary>
         */
        private void TestTaskValidation() {
            var task = new ProjectTask {
                Name = "Example name",
                Description = "Example description",
                StatusId = 1,
            };
            if (Validator.Validate(task) != String.Empty) {
                ThrowException();
            }
            task.Name = ",.}])!?";
            if (Validator.Validate(task) != RespMsgs.ProjectsTasks.NAME_IS_NONVALID) {
                ThrowException();
            }
            task.Name = "Example name";
            task.Description = "\0abc\0";
            if (Validator.Validate(task) != RespMsgs.ProjectsTasks.DESCRIPTION_IS_NONVALID) {
                ThrowException();
            }
            task.Description = "Example description";
            task.StatusId = 0;
            if (Validator.Validate(task) != RespMsgs.ProjectsTasks.TASK_STATUS_IS_NONVALID) {
                ThrowException();
            }
        }
        #endregion
        #region Функция TestTaskStatusValidation
        /** Функция TestTaskStatusValidation
         * <summary>
         *  Осуществляет тестирование валидации статуса задачи.
         * </summary>
         */
        private void TestTaskStatusValidation() {
            var taskStatus = new TasksStatus {
                Title = "Example name",
                Description = "Example description"
            };
            if (Validator.Validate(taskStatus) != String.Empty) {
                ThrowException();
            }
            taskStatus.Title = ",.}])!?";
            if (Validator.Validate(taskStatus) != RespMsgs.TaskStatuses.TITLE_IS_NONVALID) {
                ThrowException();
            }
            taskStatus.Title = "Example name";
            taskStatus.Description = "\0abc\0";
            if (Validator.Validate(taskStatus) != RespMsgs.TaskStatuses.DESCRIPTION_IS_NONVALID) {
                ThrowException();
            }
        }
        #endregion
        #region Функция TestProjectValidation
        /** Функция TestProjectValidation
         * <summary>
         *  Осуществляет тестирование валидации проекта.
         * </summary>
         */
        private void TestProjectValidation() {
            var project = new Project {
                Name = "Example name",
                Description = "Example description"
            };
            if (Validator.Validate(project) != String.Empty) {
                ThrowException();
            }
            project.Name = ",.}])!?";
            if (Validator.Validate(project) != RespMsgs.Projects.NAME_IS_NONVALID) {
                ThrowException();
            }
            project.Name = "Example name";
            project.Description = "\0abc\0";
            if (Validator.Validate(project) != RespMsgs.Projects.DESCRIPTION_IS_NONVALID) {
                ThrowException();
            }
        }
        #endregion
        #region Внешний интерфейс
        /** Функция Test
         * <summary>
         *  Функция осуществляет тестирование модуля валидации.
         * </summary>
         */
        public static void Test() {
            var instance = new ValidationTests();
            instance.TestUsersValidation();
            instance.TestCommentaryValidation();
            instance.TestTaskValidation();
            instance.TestTaskStatusValidation();
            instance.TestProjectValidation();
        }
        #endregion
    }
    #endregion
}
