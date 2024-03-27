namespace StationeersWebApi.Server
{
    /// <summary>
    /// HTTP Status Codes.
    /// </summary>
    public enum HttpStatusCode
    {
        /// <summary>
        /// The request was successful.
        /// </summary>
        OK = 200,

        /// <summary>
        /// The request was successful and a new resource was created.
        /// </summary>
        Created = 201,

        /// <summary>
        /// The request was successful and the response body contains no content.
        /// </summary>
        NoContent = 204,

        /// <summary>
        /// The request was invalid.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// The request was unauthorized.
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// The request was forbidden.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// The requested path was not found.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// Method not allowed.
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// Conflict.
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// Unprocessable Entity.
        /// </summary>
        UnprocessableEntity = 422,

        /// <summary>
        /// Internal Server Error.
        /// </summary>
        InternalServerError = 500,
    }
}
