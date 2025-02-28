using Backend.Cryptography;
using Backend.Models;

namespace Backend.Data {
    #region Класс DbInitializer
    /** Класс DbInitializer
     * <summary>
     *  Включает в себя логику инициализации базы данных.
     * </summary>
     */
    public class DbInitializer {
        #region Поля.
        /** Поле db.
         * <summary>
         *  Поле ORM-интерфейса базы данных.
         * </summary>
         */
        private ApplicationContext db;
        #endregion
        #region Конструктор.
        /** Конструктор DbInitializer(ApplicationContext)
         * <summary>
         *  Данный конструктор инъектирует объект контекста в инциализатор.
         * </summary>
         * <param name="db">
         *  Объект контекста.
         * </param>
         */
        private DbInitializer(ApplicationContext db) {
            this.db = db;
        }
        #endregion
        #region Функционал частной инициализации.
        /** Функция PrefillUsersTable
         * <summary>
         *  Функция осуществляет заполнение таблицы пользователей.
         * </summary>
         */
        private void PrefillUsersTable() {
            var admin = new User { 
                Login    = "admin"              , 
                Password = Hasher.Hash("admin") ,
                Rights   = UserRights.ADMIN     ,
                Name    = "Администратор"       ,
                Surname = ""                    ,
                Email = "admin@parusnik.org"    ,
            };
            var user = new User { 
                Login    = "user"              , 
                Password = Hasher.Hash("user") ,
                Rights   = UserRights.USER     ,
                Name    = "Пользователь"       ,
                Surname = ""                   ,
                Email = "user@parusnik.org"    ,
            };
            var users = new List<User> { admin, user };
            db.Users.AddRange(users);
        }
        /** Функция PrefillStatusesTable
         * <summary>
         *  Функция осуществляет заполнение таблицы статусов задач.
         * </summary>
         */
        private void PrefillStatusesTable() {
            var timestamp = DateTime.UtcNow;
            var newTaskStatus = new TasksStatus {
                Title       = "Новая"               ,
                Description = "Статус новой задачи.",
                CreationDate         = timestamp,
                LastModificationDate = timestamp,
            };
            var inWorkTaskStatus = new TasksStatus {
                Title       = "В работе"                            ,
                Description = "Статус задачи, находящейся в работе.",
                CreationDate         = timestamp,
                LastModificationDate = timestamp,
            };
            var completedTaskStatus = new TasksStatus {
                Title       = "Выполнена"                ,
                Description = "Статус выполненой задачи.",
                CreationDate         = timestamp,
                LastModificationDate = timestamp,
            };
            var statuses = new List<TasksStatus>() { 
                newTaskStatus       , 
                inWorkTaskStatus    , 
                completedTaskStatus ,
            };
            db.AddRange(statuses);
        }
        /** Функция PrefillMainTables
         * <summary>
         *  Функция осуществляет заполнение основных таблиц БД.
         * </summary>
         */
        private void PrefillMainTables() {
            PrefillUsersTable();
            PrefillStatusesTable();
            db.SaveChanges();
        }
        #endregion
        #region Функционал общей инициализации.
        /** Функция DbIsNotInitialized
         * <summary>
         *  Функция осуществляет проверку заданности <br/>
         *   базе данных начальных значений.
         * </summary>
         * <returns>Возвращает флаг заданности.</returns>
         */
        private bool DbIsNotInitialized() {
            return !db.Users.Any();
        }
        /** Функция Initialize
         * <summary>
         *  Функция осуществляет проверку заданности <br/>
         *   базе данных начальных значений. <br/>
         *   <br/>
         *  Если значения не заданы, осуществляется заполнение базы.
         * </summary>
         */
        private void Initialize() {
            if (DbIsNotInitialized()) {
                PrefillMainTables();
            }
        }
        /** Функция Initialize(ApplicationContext)
         * <summary>
         *  Статическая функция инкапсулирует <br/>
         *   создание объекта класса-инициализатора <br/>
         *   и проведение связанных операций. <br/>
         *  <br/>
         *  После создания экземпляра передаёт управление функции <see cref="Initialize()"/>. <br/>
         * </summary>
         * <param name="context">
         *  Контекст приложения предоставляющий интерфейс к базе данных.
         * </param>
         */
        public static void Initialize(ApplicationContext context) {
            var instance = new DbInitializer(context);
            instance.Initialize();
        }
        #endregion
    }
    #endregion
}