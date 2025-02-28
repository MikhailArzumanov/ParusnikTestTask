using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models {
    #region Класс Commentary
    /** Класс Commentary
     * <summary>
     *  Класс представляет модель данных, <br/>
     *  соответствующую сущности комментария в БД.<br/>
     *  Была проведена намеренная денормализация <br/>
     *  и установлена транзитивная зависимость с проектами<br/>
     *  в соответсвии с пунктами задания.
     *  Содержит следующие поля:<br/>
     *  Идентификатор <see cref="Id"/><br/>
     *  Текст комментария <see cref="CommentText"/><br/>
     *  Дата создания записи комментария <see cref="CreationDate"/><br/>
     *  Дата последней модификации записи комментария <see cref="LastModificationDate"/><br/>
     *  Ссылка на проект <see cref="Project"/><br/>
     *  Идентификатор проекта включения <see cref="ProjectId"/><br/>
     *  Ссылка на задачу <see cref="ProjectTask"/><br/>
     *  Идентификатор задачи комментария <see cref="ProjectTaskId"/><br/>
     * </summary>
     */
    public class Commentary {
        #region Поле идентификатора
        /** Поле Id
         * <summary>
         *  Поле выражает идентификатор комментария. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public int      Id                   { get; set; }
        #endregion
        #region Поле текста комментария
        /** Поле CommentText
         * <summary>
         *  Поле выражает текст комментария. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public string   CommentText          { get; set; } = String.Empty;
        #endregion
        #region Временные метки
        /** Поле CreationDate
         * <summary>
         *  Поле выражает дату создания записи комментария. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public DateTime CreationDate         { get; set; } = DateTime.MinValue;
        /** Поле LastModificationDate
         * <summary>
         *  Поле выражает дату последнего изменения записи комментария. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public DateTime LastModificationDate { get; set; } = DateTime.MinValue;
        #endregion
        #region Связь с таблицей проектов
        [ForeignKey("ProjectId")]
        /** Поле Project
         * <summary>
         *  Поле содержит ссылку на проект комментария, либо значение null. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public Project? Project   { get; set; }
        /** Поле ProjectId
         * <summary>
         *  Поле содержит идентификатор проекта комментария. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public int      ProjectId { get; set; }
        #endregion
        #region Связь с таблицей задач
        [ForeignKey("ProjectTaskId")]
        /** Поле ProjectTask
         * <summary>
         *  Поле содержит ссылку на задачу комментария, либо значение null. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public ProjectTask? ProjectTask   { get; set; }
        /** Поле ProjectTaskId
         * <summary>
         *  Поле содержит идентификатор задачи комментария. <br/>
         *  См. <see cref="Commentary"/>
         * </summary>
         */
        public int          ProjectTaskId { get; set; }
        #endregion
    }
    #endregion
}
