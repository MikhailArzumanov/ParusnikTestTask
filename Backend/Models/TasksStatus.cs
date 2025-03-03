using Backend.Models.Interfaces;

namespace Backend.Models {
    #region Класс TasksStatus
    /** Класс TasksStatus
     * <summary>
     *  Класс выражает модель сущности статуса задачи.<br/><br/>
     *  Содержит следующие поля:<br/>
     *  Идентификатор <see cref="Id"/><br/>
     *  Обозначение <see cref="Title"/><br/>
     *  Описание <see cref="Description"/><br/>
     *  Дата создания <see cref="CreationDate"/><br/>
     *  Дата модификации <see cref="LastModificationDate"/><br/>
     * </summary>
     */
    public class TasksStatus : IHasStatusData {
        #region Поле идентификатора
        /** Поле Id
         * <summary>
         *  Поле выражает идентификатор статуса задачи. <br/>
         *  См. <see cref="TasksStatus"/>
         * </summary>
         */
        public int    Id          { get; set; }
        #endregion
        #region Поля обозначения и описания
        /** Поле Title
         * <summary>
         *  Поле выражает обозначение статуса задачи. <br/>
         *  См. <see cref="TasksStatus"/>
         * </summary>
         */
        public string Title       { get; set; } = String.Empty;
        /** Поле Description
         * <summary>
         *  Поле выражает описание статуса задачи. <br/>
         *  См. <see cref="TasksStatus"/>
         * </summary>
         */
        public string Description { get; set; } = String.Empty;
        #endregion
        #region Поля дат создания и модификации
        /** Поле CreationDate
         * <summary>
         *  Поле выражает дату создания записи статуса задачи. <br/>
         *  См. <see cref="TasksStatus"/>
         * </summary>
         */
        public DateTime CreationDate         { get; set; } = DateTime.MinValue;
        /** Поле LastModificationDate
         * <summary>
         *  Поле выражает дату обновления записи статуса задачи.<br/>
         *  См. <see cref="TasksStatus"/>
         * </summary>
         */
        public DateTime LastModificationDate { get; set; } = DateTime.MinValue;
        #endregion
    }
    #endregion
}
