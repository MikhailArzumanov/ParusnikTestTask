namespace Backend.Models.Interfaces {
    /** Интерфейс IHasProjectData
     * <summary>
     *  Интерфейс служит для введения ряда обобщений. <br/>
     *  См. <see cref="Validation.Validator"/> <br/>
     *  <br/>
     *  Описание полей см. в <see cref="Project"/>
     * </summary>
     */
    public interface IHasProjectData {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}
