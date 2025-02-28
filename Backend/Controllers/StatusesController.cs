using Backend.Authentication;
using Backend.Constants;
using Backend.Data;
using Backend.Models;
using Backend.Validation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    #region Контроллер StatusesController
    /** Контроллер StatusesController
     * <summary>
     *  Данный контроллер отвечает за конечные точки, 
     *  связанные с взаимодействием с сущностью статуса задачи.
     * </summary>
     */
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class StatusesController :ControllerBase {
        #region Поля контроллера
        /** Поле db
         * <summary>
         *  Контекст приложения.
         * </summary>
         */
        private ApplicationContext db           ;
        /** Поле authenticator
         * <summary>
         *  Объект-аутентификатор.
         * </summary>
         */
        private Authenticator      authenticator;
        /** Поле config
         * <summary>
         *  Интерфейс конфигурации приложения.
         * </summary>
         */
        private IConfiguration     config       ;
        #endregion
        #region Конструктор контроллера
        /** Конструктор StatusesController(ApplicationContext, IConfiguration) 
         * <summary>
         *  Конструктор осуществляет инъекцию 
         *  контекста приложения 
         *  и интерфейса конфигурации
         * </summary>
         * <param name="context">Контекст приложения</param>
         * <param name="config">Интерфейс конфигурации</param>
         */
        public StatusesController(
            ApplicationContext context,
            IConfiguration config
        ) {
            db = context; this.config = config;
            authenticator = new Authenticator(context, config);
        }
        #endregion
        #region Общие процессы запросов
        /** Функция TryAuth(HttpRequest, UserRights)
         * <summary>
         *  Выполняет попытку аутентификации.
         * </summary>
         * <param name="request">Объект HTTP-запроса</param>
         * <param name="requiredRights">Требуемый уровень прав пользователя</param>
         * <returns>
         *  Возвращает объект ответа с ошибкой, <br/>
         *  либо значение null при успешной аутентификации.
         * </returns>
         */
        private IActionResult? TryAuth(HttpRequest request, UserRights requiredRights) {
            var user = authenticator.Authenticate(request);
            if (user == null) {
                return BadRequest(RespMsgs.TOKEN_INVALID);
            }
            if (user.Rights < UserRights.USER) {
                return Forbid(RespMsgs.NOT_ENOUGH_RIGHTS);
            }
            return null;
        }
        /** Функция ExecEntryPipeline(HttpRequest,UserRights,TasksStatus)
         * <summary>
         *  Функция осуществляет общий процесс валидации объекта, <br/>
         *  предлагаемого к записи в БД, а также попытку авторизации <br/>
         *  См. <see cref="TryAuth(HttpRequest, UserRights)"/>
         * </summary>
         * <param name="request">Объект HTTP-запроса</param>
         * <param name="requiredRights">Требуемый уровень прав.</param>
         * <param name="entry">Запись, подвергаемая валидации.</param>
         * <returns>
         *  Возвращает объект ответа с ошибкой, <br/>
         *  либо значение null при успешной аутентификации.
         * </returns>
         */
        private IActionResult? ExecEntryPipeline(
            HttpRequest request       ,
            UserRights  requiredRights,
            TasksStatus entry
        ) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if (authErrorResp != null) {
                return authErrorResp;
            }
            var validationMsg = Validator.Validate(entry);
            if (validationMsg != String.Empty) {
                return BadRequest(validationMsg);
            }
            return null;
        }
        #endregion
        #region Функция GetConcrete
        /** Функция GetConcrete(int)
         * <summary>
         *  Функция соответствует конечной точке получения единичной записи.
         * </summary>
         * <param name="statusId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Statuses.GET_CONCRETE)]
        public IActionResult GetConcrete([FromRoute] int statusId) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if(authErrorResponse != null) {
                return authErrorResponse;
            }

            var entry = db.TaskStatuses.FirstOrDefault(x => x.Id == statusId);
            if (entry == null) {
                return NotFound(RespMsgs.TaskStatuses.ID_NOT_FOUND);
            }

            return Ok(entry);
        }
        #endregion
        #region Функция GetList
        /** Функция GetList
         * <summary>
         *  Функция соответствует конечной точке получения списка записей.
         * </summary>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Statuses.GET_LIST)]
        public IActionResult GetList() {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if (authErrorResponse != null) {
                return authErrorResponse;
            }

            var entries = db.TaskStatuses;

            return Ok(entries);
        }
        #endregion
        #region Функция CreateEntry
        /** Функция CreateEntry(TasksStatus)
         * <summary>
         *  Функция соответствует конечной точке создания записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPost(Routes.Statuses.CREATE_ENTRY)]
        public IActionResult CreateEntry([FromBody] TasksStatus data) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            var timestamp = DateTime.UtcNow;
            data.CreationDate         = timestamp;
            data.LastModificationDate = timestamp;

            db.TaskStatuses.Add(data);
            db.SaveChanges();
            return Ok(data);
        }
        #endregion
        #region Функция RedactEntry
        /** Функция RedactEntry(TasksStatus, int)
         * <summary>
         *  Функция соответствует конечной точке редактирования записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <param name="statusId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPut(Routes.Statuses.REDACT_ENTRY)]
        public IActionResult RedactEntry([FromBody] TasksStatus data, [FromRoute] int statusId) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            var entry = db.TaskStatuses.FirstOrDefault(x => x.Id == statusId);
            if(entry == null) {
                return NotFound(RespMsgs.TaskStatuses.ID_NOT_FOUND);
            }

            entry.LastModificationDate = DateTime.UtcNow;
            entry.Title       = data.Title       ;
            entry.Description = data.Description ;

            db.TaskStatuses.Update(entry);
            db.SaveChanges();
            return Ok(entry);
        }
        #endregion
        #region  Функция RemoveEntry
        /** Функция RemoveEntry(int)
         * <summary>
         *  Функция соответствует конечной точке удаления записи.
         * </summary>
         * <param name="statusId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpDelete(Routes.Statuses.REMOVE_ENTRY)]
        public IActionResult RemoveEntry([FromRoute] int statusId) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if (authErrorResponse != null) {
                return authErrorResponse;
            }

            var entry = db.TaskStatuses.FirstOrDefault(x => x.Id == statusId);
            if(entry == null) {
                return NotFound(RespMsgs.TaskStatuses.ID_NOT_FOUND);
            }

            db.TaskStatuses.Remove(entry);
            db.SaveChanges();
            return Ok(entry);
        }
        #endregion
    }
    #endregion
}
