using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data {
    #region Класс ApplicationContext
    /** Класс ApplicationContext
     * <summary>
     *  Класс представляет контекст базы данных.<br/>
     *  Экземпляр класса инъектируется в контроллеры, позволяя доступаться до данных.<br/>
     *  См. <see cref="DbContext"/>
     * </summary>
     */
    public class ApplicationContext : DbContext {
        #region Конструктор
        /**
         * <summary>
         *  Конструктор способен создавать базу данных 
         *    при её отсутствии в случае, 
         *    если вызывается функция EnsureCreated.
         *  На данный момент вызов закомментирован.
         * </summary>
         * <param name="options">
         *  Опции контекста. 
         *  Передаются конструктору родительского класса.
         * </param>
         */
        public ApplicationContext(
            DbContextOptions<ApplicationContext> options
        ) : base(options) {
            //Database.EnsureCreated();
        }
        #endregion
        #region Обозначение связей.
        /** OnModelCreating
         * <summary>
         * Функция используется в обозначении связей для ORM там, 
         *  где функционал ORM не способен редуцировать их сам.
         *  
         * На данный момент определяет связь многие-ко-многим 
         *  для пользователей и проектах участия.
         * </summary>
         * <param name="modelBuilder">
         *  Параметр, предоставляющий интерфейс явного задания связей.
         * </param>
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>()
                .HasMany(u => u.ParticipatingProjects)
                .WithMany(r => r.Users);
        }
        #endregion
        #region Таблицы
        /** Таблица Users
         * <summary>
         *  Таблица соответствует сущности пользователя.
         * </summary>
         */
        public DbSet<User>        Users        { get; set; }
        /** Таблица Projects
         * <summary>
         *  Таблица соответствует сущности проекта.
         * </summary>
         */
        public DbSet<Project>     Projects     { get; set; }
        /** Таблица Tasks
         * <summary>
         *  Таблица соответствует сущности задачи.
         * </summary>
         */
        public DbSet<ProjectTask> Tasks        { get; set; }
        /** Таблица Commentaries
         * <summary>
         *  Таблица соответствует сущности комментария.
         * </summary>
         */
        public DbSet<Commentary>  Commentaries { get; set; }
        /** Таблица TaskStatuses
         * <summary>
         *  Таблица соответствует сущности статуса задачи.
         * </summary>
         */
        public DbSet<TasksStatus> TaskStatuses { get; set; }
        #endregion
    }
    #endregion
}
