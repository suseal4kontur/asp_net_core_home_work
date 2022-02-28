namespace API.Todo
{
    using System;

    /// <summary>
    /// Исключение, которое возникает при ошибки валидации модели
    /// </summary>
    public sealed class ValidationException : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения валидации
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public ValidationException(string message = "Validation failed") : base(message)
        {
        }
    }
}
