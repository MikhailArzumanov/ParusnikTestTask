using Backend.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models {
    #region Класс ProjectTask
    /** Класс ProjectTask
     * <summary>
     *  Класс выражает модель сущности задачи. <br/><br/>
     *  Содержит следующие поля:<br/>
     *  Идентификатор <see cref="Id"/><br/>
     *  Наименование <see cref="Name"/><br/>
     *  Описание <see cref="Description"/><br/>
     *  Дата создания <see cref="CreationDate"/><br/>
     *  Дата модификации <see cref="LastModificationDate"/><br/>
     *  Ссылка на статус <see cref="Status"/><br/>
     *  Идентификатор статуса <see cref="StatusId"/><br/>
     *  Ссылка на проект <see cref="Project"/><br/>
     *  Идентификатор проекта <see cref="ProjectId"/><br/>
     *  Коллекция комментариев <see cref="Commentaries"/><br/>
     * </summary>
     */
    public class ProjectTask : IHasTaskData {
        #region Поле идентификатора
        /** Поле Id
         * <summary>
         *  Поле выражает идентификатор задачи. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public int      Id                   { get; set; }
        #endregion
        #region Поля наименования и описания
        /** Поле Name
         * <summary>
         *  Поле выражает наименование задачи. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public string   Name                 { get; set; } = String.Empty;
        /** Поле Name
         * <summary>
         *  Поле выражает описание задачи. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public string   Description          { get; set; } = String.Empty;
        #endregion
        #region Поля дат создания и модификации
        /** Поле CreationDate
         * <summary>
         *  Поле выражает дату создания записи задачи. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public DateTime CreationDate         { get; set; } = DateTime.MinValue;
        /** Поле LastModificationDate
         * <summary>
         *  Поле выражает дату последнего изменения задачи. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public DateTime LastModificationDate { get; set; } = DateTime.MinValue;
        #endregion
        #region Связь с таблицей статусов задач
        [ForeignKey("StatusId")]
        /** Поле Status
         * <summary>
         *  Поле содержит ссылку на статус задачи, либо значение null. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public TasksStatus? Status   { get; set; }
        /** Поле StatusId
         * <summary>
         *  Поле содержит идентификатор связанного статуса. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public int          StatusId { get; set; }
        #endregion
        #region Связь с таблицей проектов
        [ForeignKey("ProjectId")]
        /** Поле Project
         * <summary>
         *  Поле содержит ссылку на проект задачи, либо значение null. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public Project? Project   { get; set; }
        /** Поле ProjectId
         * <summary>
         *  Поле содержит идентификатор связанного проекта. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public int      ProjectId { get; set; }
        #endregion
        #region Связи ко многим
        /** Поле Commentaries
         * <summary>
         *  Поле содержит коллекцию комментариев к задаче. <br/>
         *  См. <see cref="ProjectTask"/>
         * </summary>
         */
        public ICollection<Commentary> Commentaries { get; set; }
            = new List<Commentary>();
        #endregion
    }
    #endregion
}
