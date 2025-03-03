namespace Backend.Models.Interfaces {
    /** Интерфейс IHasStatusData 
     * <summary>
     *  Интерфейс служит для введения ряда обобщений. <br/>
     *  См. <see cref="Validation.Validator"/> <br/>
     *  <br/>
     *  Описание полей см. в <see cref="TasksStatus"/>
     * </summary>
     */
    public interface IHasStatusData {
        public string Title       { get; set; }
        public string Description { get; set; }
    }
}
