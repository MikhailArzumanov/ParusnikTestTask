namespace Backend.Models {
    #region Перечисление UserRights
    /** Перечисление UserRights
     * <summary>
     *  Перечисление выражает уровень прав пользователя.
     *  <br/><br/>
     *  Роли:<br/>
     *  Гость -- минимальный права,<br/>
     *  Пользователь -- общие права,<br/>
     *  Администратор -- полные права.
     * </summary>
     */
    public enum UserRights {
        NONVALID = -1,
        GUEST    =  0,
        USER     =  1,
        ADMIN    =  2,
    }
    #endregion
    #region Класс User
    /** Класс User
     * <summary>
     *  Класс представляет модель данных, <br/>
     *  соответствующую сущности пользователя в БД.
     *  <br/><br/>
     *  Содержит следующие поля:<br/>
     *  Идентификатор <see cref="Id"/><br/>
     *  Логин <see cref="Login"/><br/>
     *  Пароль <see cref="Password"/><br/>
     *  Имя <see cref="Name"/><br/>
     *  Фамилия <see cref="Surname"/><br/>
     *  Электронная почта <see cref="Email"/><br/>
     *  Дата создания <see cref="CreationDate"/><br/>
     *  Дата модификации <see cref="LastModificationDate"/><br/>
     *  Права пользователя <see cref="Rights"/><br/>
     *  Проекты участия <see cref="ParticipatingProjects"/><br/>
     * </summary>
     */
    public class User {
        #region Данные авторизации и идентификатор
        /** Поле Id
         * <summary>
         *  Поле выражает идентификатор пользователя. <br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        public int    Id       { get; set; }
        /** Поле Login
         * <summary>
         *  Поле выражает логин пользователя. <br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        public string Login    { get; set; } = String.Empty;
        /** Поле Password
         * <summary>
         *  Поле соответствует паролю пользователя. <br/>
         *  См. <see cref="User"/>
         *  <br/><br/>
         *  В базе данных поле хранит контрольную сумму пароля.
         * </summary>
         */
        public string Password { get; set; } = String.Empty;
        /** Поле Name
         * <summary>
         *  Поле выражает личное имя пользователя. <br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        #endregion
        #region Личные данные
        public string Name     { get; set; } = String.Empty;
        /** Поле Surname
         * <summary>
         *  Поле выражает фамилию пользователя. <br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        public string Surname  { get; set; } = String.Empty;
        /** Поле Email
         * <summary>
         *  Поле выражает адрес электронной почты пользователя.<br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        public string Email    { get; set; } = String.Empty;
        #endregion
        #region Временные метки
        /** Поле CreationDate
         * <summary>
         *  Поле выражает момент создания записи в базе данных.<br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        public DateTime CreationDate         { get; set; } = DateTime.MinValue;
        /** Поле CreationDate
         * <summary>
         *  Поле выражает момент обновления записи в базе данных.<br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        public DateTime LastModificationDate { get; set; } = DateTime.MinValue;
        /** Поле CreationDate
         * <summary>
         *  Поле выражает степень прав пользователя.<br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        #endregion
        #region Права пользователя
        public UserRights Rights { get; set; } = UserRights.NONVALID;
        /** Поле CreationDate
         * <summary>
         *  При проведении присоединения,<br/> 
         *  поле содержит коллекцию объектов тех проектов,<br/>
         *  в которых пользователь принимает участие.<br/><br/>
         *  См. <see cref="User"/>
         * </summary>
         */
        #endregion
        #region Связи ко многим
        public ICollection<Project> ParticipatingProjects { get; set; }
            = new List<Project>();
        #endregion
    }
    #endregion
}
