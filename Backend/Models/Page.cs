namespace Backend.Models {
    #region Класс Page
    /** Класс Page
     * <summary>
     *  Класс служит целям построения пагинированных коллекций.<br/>
     *  Включает статическую функцию построения страницы пагинированных данных.<br/><br/>
     *  Содержит следующие поля:<br/>
     *  Номер страницы <see cref="PageNumber"/><br/>
     *  Размер страницы <see cref="PageSize"/><br/>
     *  Число страницы <see cref="PageCount"/><br/>
     *  Элементы в рамках страницы <see cref="Items"/><br/>
     *  Содержит следующие функции:<br/>
     *  Функция формирования страницы данных <see cref="FormPage(ICollection{T}, int, int)"/>
     * </summary>
     */
    public class Page<T> {
        #region Параметры пагинации
        /** Поле PageNumber
         * <summary>
         *  Поле выражает номер страницы данных. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public int PageNumber { get; set; }
        /** Поле PageSize
         * <summary>
         *  Поле выражает размер страницы данных. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public int PageSize  { get; set; }
        /** Поле PageCount
         * <summary>
         *  Поле выражает число страниц пагинации. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public int PageCount { get; set; }
        #endregion
        #region Элементы
        /** Поле Items
         * <summary>
         *  Поле содержит коллекцию элементов страницы данных. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public ICollection<T> Items { get; set; } = new List<T>();
        #endregion
        #region Функционал
        /** Функция FormPage
         * <summary>
         *  Функция осуществляет формирование страницы данных. <br/>
         *  См. <see cref="Project"/>
         * </summary>
         */
        public static Page<T> FormPage(ICollection<T> src, int pageNumber, int pageSize) {
            int allPagesCount      = src.Count/pageSize;
            bool isTherePartialPage = src.Count%pageSize > 0;
            var pageCount =  allPagesCount + (isTherePartialPage ? 1 : 0);

            int pagesToSkip = pageSize * (pageNumber-1);
            var items = src.Skip(pagesToSkip).Take(pageSize).ToArray();

            var page = new Page<T>() {
                PageNumber = pageNumber , 
                PageCount = pageCount , 
                PageSize  = pageSize  ,
                Items     = items     ,
            };
            return page;
        }
        #endregion
    }
    #endregion
}
