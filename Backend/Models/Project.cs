namespace Backend.Models {
    #region Класс Project
    /** Класс Project
     * <summary>
     *  Класс представляет модель данных, <br/>
     *  соответствующую сущности проекта в БД.<br/>
     *  Была проведена намеренная денормализация <br/>
     *  и установлена транзитивная зависимость с комментариями<br/>
     *  в соответсвии с пунктами задания.
     *  <br/><br/>
     *  Содержит следующие поля:<br/>
     *  Идентификатор <see cref="Id"/><br/>
     *  Наименование <see cref="Name"/><br/>
     *  Описание <see cref="Description"/><br/>
     *  Дата создания <see cref="CreationDate"/><br/>
     *  Дата модификации <see cref="LastModificationDate"/><br/>
     *  Задачи <see cref="Tasks"/><br/>
     *  Комментарии <see cref="Commentaries"/><br/>
     *  Пользователи-участия <see cref="Users"/><br/>
     * </summary>
     */
    public class Project {
        #region Поле идентификатора
        /** Поле Id
         * <summary>
         *  Поле выражает идентификатор проекта. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public int      Id                   { get; set; }
        #endregion
        #region Поля наименования и описания
        /** Поле Name
         * <summary>
         *  Поле выражает наименование проекта. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public string   Name                 { get; set; } = String.Empty;
        /** Поле Description
         * <summary>
         *  Поле выражает описание проекта. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public string   Description          { get; set; } = String.Empty;
        #endregion
        #region Временные метки
        /** Поле CreationDate
         * <summary>
         *  Поле выражает дату создания записи о проекте. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public DateTime CreationDate         { get; set; } = DateTime.MinValue;
        /** Поле LastModificationDate
         * <summary>
         *  Поле выражает дату последнего изменения записи о проекте. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public DateTime LastModificationDate { get; set; } = DateTime.MinValue;
        #endregion
        #region Связи ко многим
        /** Поле Tasks
         * <summary>
         *  Поле выражает задачи проекта. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public ICollection<ProjectTask> Tasks        { get; set; } 
            = new List<ProjectTask>();
        /** Поле Commentaries
         * <summary>
         *  Поле выражает комментарии в рамках проекта. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public ICollection<Commentary>  Commentaries { get; set; }
            = new List<Commentary>();
        /** Поле Users
         * <summary>
         *  Поле выражает пользователей-участников проекта. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public ICollection<User>        Users        { get; set; }
            = new List<User>();
        #endregion
    }
    #endregion
}
