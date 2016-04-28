namespace Assets.Scripts
{
    public interface IState
    {
        /// <summary>
        /// Запуск стейта
        /// </summary>
        void StartState();

        /// <summary>
        /// Отключение стейта
        /// </summary>
        void EndState();
    }
}

