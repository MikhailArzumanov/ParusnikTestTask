using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Configuring {
    #region Класс AppConfigurator
    /** Класс AppConfigurator
     * <summary>
     *  Класс нацелен на осуществление конфигурации приложения, 
     *  связанной с интерфейсом <see cref="IApplicationBuilder"/>.
     * </summary>
     */
    public class AppConfigurator {
        #region Поля
        /** Поле Configuration
         * <summary>
         *  Поле интерфейса конфигурации.
         * </summary>
         */
        private IConfiguration Configuration { get; set; }
        #endregion
        #region Конструктор
        /** Конструктор AppConfigurator(IConfiguration)
         * <summary>
         *  Осуществляет задание контекста интерфейса конфигурации.
         * </summary>
         * <param name="configuration">
         *  Интерфейс конфигурации.
         * </param>
         */
        private AppConfigurator(IConfiguration configuration) {
            Configuration = configuration;
        }
        #endregion
        #region Функционал контекста
        /** Функция Configure(IApplicationBuilder, bool)
         * <summary>
         *  Функция осуществляет настройку приложения.
         * </summary>
         */
        private void Configure(
            IApplicationBuilder app,
            bool isDevelopmentEnv
        ) {
            app.UseRouting();
            app.UseCors();

            if (isDevelopmentEnv) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
        #endregion
        #region Статический функционал
        /** Функция Configure(IApplicationBuilder, IConfiguration, bool)
         * <summary>
         *  Функция осуществляет настройку приложения.
         * </summary>
         */
        public static void Configure(
            IApplicationBuilder app,
            IConfiguration configuration,
            bool isDevelopmentEnv
        ) {
            var instance = new AppConfigurator(configuration);
            instance.Configure(app, isDevelopmentEnv);
        }
        #endregion
    }
    #endregion
}
