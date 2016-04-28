using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class StateController
    {

        /// <summary>
        /// Состояния игры
        /// </summary>
        public enum EnumStateType
        {
            MainMenu, // Главное меню
            StartSnake, // Старт игры 
            MoveSnake, // Движение змейки (ходы в игре)
            EatSnake, // Поедание еды
            DieSnake // Смерть змейки

        }

        // Текущее состояние
        public static EnumStateType CurrentState = EnumStateType.MainMenu;

        // Ссылки на классы - состояний игры
        public static StartSnakeStateController StartSnakeState;
        public static MoveSnakeStateController MoveSnakeState;
        public static MainMenuStateController MainMenuState;
        public static DieSnakeStateController DieSnakeState;
        public static EatSnakeStateController EatSnakeState;
        public static WaitController WaitController;

        // Связь стейтов с классами
        private static readonly Dictionary<EnumStateType, IState> StateDictionary = new Dictionary<EnumStateType, IState>();


        /// <summary>
        /// Инициализация
        /// </summary>
        public static void Start()
        {
            StateDictionary.Add(EnumStateType.MainMenu, MainMenuState);
            StateDictionary.Add(EnumStateType.StartSnake, StartSnakeState);
            StateDictionary.Add(EnumStateType.MoveSnake, MoveSnakeState);
            StateDictionary.Add(EnumStateType.EatSnake, EatSnakeState);
            StateDictionary.Add(EnumStateType.DieSnake, DieSnakeState);

            StartSnakeState.gameObject.SetActive(false);
            MoveSnakeState.gameObject.SetActive(false);
            EatSnakeState.gameObject.SetActive(false);
            DieSnakeState.gameObject.SetActive(false);
        }

        /// <summary>
        /// Смена состояния
        /// </summary>
        /// <param name="newState"></param>
        public static void ChangeState(EnumStateType newState)
        {
            StateDictionary[CurrentState].EndState();

            CurrentState = newState;

            StateDictionary[CurrentState].StartState();
        }
    }
}


