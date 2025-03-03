using Backend.Authentication;
using Backend.Constants;
using Backend.Data;
using Backend.Models;
using Backend.Models.Interfaces;
using Backend.Validation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Backend.Constants.RespMsgs;
using static Backend.Constants.Routes;

namespace Backend.Controllers {
    #region Контроллер CommentariesController
    /** Контроллер CommentariesController
     * <summary>
     *  Данный контроллер отвечает за конечные точки, 
     *  связанные с взаимодействием с сущностью комментариев.
     * </summary>
     */
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class CommentariesController : ControllerBase {
        #region Поля контроллера
        /** Поле db
         * <summary>
         *  Контекст приложения.
         * </summary>
         */
        private ApplicationContext db            ;
        /** Поле authenticator
         * <summary>
         *  Объект-аутентификатор.
         * </summary>
         */
        private Authenticator      authenticator ;
        /** Поле config
         * <summary>
         *  Интерфейс конфигурации приложения.
         * </summary>
         */
        private IConfiguration     config        ;
        #endregion
        #region Конструктор контроллера
        /** Конструктор CommentariesController(ApplicationContext, IConfiguration) 
         * <summary>
         *  Конструктор осуществляет инъекцию 
         *  контекста приложения 
         *  и интерфейса конфигурации
         * </summary>
         * <param name="context">Контекст приложения</param>
         * <param name="config">Интерфейс конфигурации</param>
         */
        public CommentariesController(
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
        /** Функция ExecEntryPipeline(HttpRequest,UserRights,Commentary)
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
            HttpRequest        request        ,
            UserRights         requiredRights ,
            IHasCommentaryData entry
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
        /** Функция SetNulls(ProjectTask, Commentary)
         * <summary>
         *  Функция задаёт пустые объекты выходным переменным.<br/>
         *  <br/>
         *  Требуется для того, чтобы выходные переменные имели значения, отличные от null.
         * </summary>
         * <param name="task">Объект задачи</param>
         * <param name="commentary">Объект комментария</param>
         */
        private void SetNulls(out ProjectTask task, out Commentary commentary) {
            task = new ProjectTask(); commentary = new Commentary();
        }
        /** Функция ExecTaskEntryPipeline(ProjectTask, Commentary, int, int)
         * <summary>
         *  Функция осуществляет общий процесс получение записи задачи, <br/>
         *  а также комментария, предлагаемого к записи в БД. 
         * </summary>
         * <param name="task">Объект задачи</param>
         * <param name="entry">Объект комментария</param>
         * <param name="taskId">Идентификатор  задачи</param>
         * <param name="commentaryId">Идентификатор  комментария</param>
         */
        private IActionResult? ExecTaskEntryPipeline(
            out ProjectTask task, out Commentary entry,
            int taskId, int commentaryId
        ) {
            var tsk = db.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (tsk == null) {
                SetNulls(out task, out entry);
                return NotFound(RespMsgs.ProjectsTasks.ID_NOT_FOUND);
            }

            var entr = db.Commentaries.FirstOrDefault(x => x.Id == commentaryId);
            if(entr == null) {
                SetNulls(out task, out entry);
                return NotFound(RespMsgs.Commentaries.ID_NOT_FOUND);
            }
            if (entr.ProjectTaskId != taskId) {
                SetNulls(out task, out entry);
                return BadRequest(RespMsgs.Commentaries.ANOTHER_TASK_COMMENTARY);
            }

            task  = tsk ;
            entry = entr;
            return null;
        }
        #endregion
        #region Функция CreateEntry
        /** Функция CreateEntry(data, int)
         * <summary>
         *  Функция соответствует конечной точке создания записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <param name="taskId">Идентификатор задачи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPost(Routes.Commentaries.CREATE_ENTRY)]
        public IActionResult CreateEntry(
            [FromBody]  CommentaryRequest data  , 
            [FromRoute] int               taskId
        ) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            var task = db.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (task == null) {
                return NotFound(RespMsgs.ProjectsTasks.ID_NOT_FOUND);
            }

            var timestamp = DateTime.UtcNow;
            var entry = new Commentary {
                CommentText = data.CommentText,
                CreationDate         = timestamp,
                LastModificationDate = timestamp,
                ProjectId     = task.ProjectId,
                ProjectTaskId = taskId        ,      
            };

            db.Commentaries.Add(entry);
            db.SaveChanges();

            var response = new CommentaryResponse(entry);
            return Ok(response);
        }
        #endregion
        #region Функция RedactEntry
        /** Функция RedactEntry(CommentaryRequest, int, int)
         * <summary>
         *  Функция соответствует конечной точке редактирования записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <param name="taskId">Идентификатор задачи</param>
         * <param name="commentaryId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPut(Routes.Commentaries.REDACT_ENTRY)]
        public IActionResult RedactEntry(
            [FromBody]  CommentaryRequest data        ,
            [FromRoute] int               taskId      ,
            [FromRoute] int               commentaryId
        ) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            ProjectTask task ;
            Commentary  entry;
            var errorResp = ExecTaskEntryPipeline(
                out task, out entry, taskId, commentaryId
            );
            if (errorResp != null) {
                return errorResp;
            }

            var timestamp = DateTime.UtcNow;
            entry.LastModificationDate = timestamp;
            entry.CommentText = data.CommentText;

            db.Commentaries.Update(entry);
            db.SaveChanges();

            var response = new CommentaryResponse(entry);
            return Ok(response);
        }
        #endregion
        #region Функция RemoveEntry
        /** Функция RemoveEntry(int, int)
         * <summary>
         *  Функция соответствует конечной точке удаления записи.
         * </summary
         * <param name="taskId">Идентификатор задачи</param>
         * <param name="commentaryId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpDelete(Routes.Commentaries.REMOVE_ENTRY)]
        public IActionResult RemoveEntry(
            [FromRoute] int taskId      , 
            [FromRoute] int commentaryId
        ) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if (authErrorResp != null) {
                return authErrorResp;
            }
            ProjectTask task;
            Commentary entry;
            var errorResp = ExecTaskEntryPipeline(
                out task, out entry, taskId, commentaryId
            );
            if (errorResp != null) {
                return errorResp;
            }

            db.Commentaries.Remove(entry);
            db.SaveChanges();

            var response = new CommentaryResponse(entry);
            return Ok(response);
        }
        #endregion
        #region Функция GetList
        /** Функция GetList(int, int, int, string, string)
         * <summary>
         *  Функция соответствует конечной точке получения пагинированного списка записей.
         * </summary>
         * <param name="pageNumber">Номер страницы</param>
         * <param name="pageSize">Размер страницы</param>
         * <param name="commentaryTextFilter">Фильтр текста комментария</param>
         * <param name="commentaryTextSortionDirection">Направление сортировки</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Commentaries.GET_LIST)]
        public IActionResult GetList(
            [FromRoute] int taskId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string commentaryTextFilter = "",
            [FromQuery] string commentaryTextSortionDirection = Sortion.ASC_KEY
        ) {
            var entries = db.Commentaries
                .Where(x => x.ProjectTaskId == taskId)
                .Where(x => x.CommentText.Contains(commentaryTextFilter));
            
            IOrderedQueryable<Commentary> sortedEntries;
            if (commentaryTextSortionDirection == Sortion.ASC_KEY) {
                sortedEntries = entries.OrderBy(x => x.CommentText);
            } else {
                sortedEntries = entries.OrderByDescending(x => x.CommentText);
            }

            var response = new CommentariesResponse(sortedEntries, pageNumber, pageSize);
            return Ok(response);
        }
        #endregion
        #region Структуры запросов
        /** Класс CommentaryRequest
         * <summary>
         *  Класс представляет структуру запроса с передачей данных комментария.
         *  Описание полей см. в <see cref="Commentary"/>
         * </summary>
         */
        public class CommentaryRequest : IHasCommentaryData {
            public string CommentText   { get; set; } = String.Empty;
        }
        #endregion
        #region Структуры ответов
        /** Класс CommentaryResponse
         * <summary>
         *  Класс представляет структуру ответа при запросе с возвратом записи комментария.
         *  Описание полей см. в <see cref="Commentary"/>
         * </summary>
         */
        public class CommentaryResponse {
            public int    Id            { get; set; }
            public string CommentText   { get; set; } = String.Empty;
            public int    ProjectTaskId { get; set; }
            public int    ProjectId     { get; set; }
            public CommentaryResponse(Commentary entry) {
                Id            = entry.Id            ;
                CommentText   = entry.CommentText   ;
                ProjectTaskId = entry.ProjectTaskId ;
                ProjectId     = entry.ProjectId     ;
            }
        }
        /** Класс CommentariesResponse
         * <summary>
         *  Класс представляет структуру ответа при запросе с возвратом набора записей.
         * </summary>
         */
        public class CommentariesResponse {
            public int PageNumber { get; set; }
            public int PageSize   { get; set; }
            public int PagesCount { get; set; }
            public ICollection<CommentaryResponse> Entries { get; set; }
            public CommentariesResponse(IQueryable<Commentary> entries, int pageNumber, int pageSize) {
                PageNumber = pageNumber; PageSize = pageSize;
                PagesCount = (int) Math.Ceiling(entries.Count()/1.0/PageSize);

                var skipCount = PageSize*(PageNumber - 1);
                this.Entries = entries
                    .OrderBy(x => x.Id).Skip(skipCount).Take(PageSize)
                    .Select(x => new CommentaryResponse(x)).ToList();
            }
        }
        #endregion
    }
    #endregion
}
