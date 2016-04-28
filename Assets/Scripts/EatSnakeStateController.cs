using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EatSnakeStateController : MonoBehaviour, IState
    {
        public MatrixIdModel EatedFoodCood;

        // Use this for initialization
        void Awake()
        {
            StateController.EatSnakeState = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Наложение эффекта, от сьеденого "фрукта"
        /// </summary>
        private void _fruitBuff()
        {
            var tmpPointModel = StateController.MoveSnakeState.GameMatrix[EatedFoodCood.x][EatedFoodCood.y];

            // определение типа "фрукта"
            switch (tmpPointModel.Food)
            {
                case Initialize.EnumFood.NoFood:
                    break;
                case Initialize.EnumFood.NormalFood:
                    _normalFoodAte(tmpPointModel);
                    break;
                case Initialize.EnumFood.SpoiledFood:
                    _spoiledFoodAte(tmpPointModel);
                    break;
                case Initialize.EnumFood.FastFood:
                    _fastFoodAte();
                    break;
                case Initialize.EnumFood.SlowFood:
                    _slowFoodAte();
                    break;
                case Initialize.EnumFood.SwitchFood:
                    _switchFoodAte();
                    break;
            }
            StateController.ChangeState(StateController.EnumStateType.MoveSnake);
        }

        /// <summary>
        /// Сьел нормальную еду
        /// </summary>
        /// <param name="point"></param>
        private void _normalFoodAte(PointModel point)
        {
            // Изменить скорость + 10%
            StateController.MoveSnakeState.SpeedTimerMax /= 1.1f;

            // Создание хвоста (+1 к змейке)
            point.Ate = true;
        }

        /// <summary>
        /// Сьел испорченную еду
        /// </summary>
        /// <param name="point"></param>
        private void _spoiledFoodAte(PointModel point)
        {
            // Если змейка длиной 1  - то конец игры
            if (StateController.MoveSnakeState.SnakeCoods.Count != 1)
            {
                // Узнаем PointModel хвоста змейки
                point = StateController.MoveSnakeState.GameMatrix[StateController.MoveSnakeState.SnakeCoods.Last().x][StateController.MoveSnakeState.SnakeCoods.Last().y];
                // Удаляем хвост
                Destroy(point.CellGO);
                point.CellState = Initialize.EnumСell.Empty;
                StateController.MoveSnakeState.SnakeCoods.RemoveAt(StateController.MoveSnakeState.SnakeCoods.Count - 1);
            }
            else
            {
                StateController.ChangeState(StateController.EnumStateType.DieSnake);
                return;
            }
        }

        /// <summary>
        /// Сьел ускоряющую еду
        /// </summary>
        private void _fastFoodAte()
        {
            StateController.WaitController.ChangeSpeed(StateController.MoveSnakeState.Speed, 10);
            // Изменить текущую скорость  + 100%
            StateController.MoveSnakeState.Speed *= 2;
        }

        /// <summary>
        /// Сьел замедляющую еду
        /// </summary>
        private void _slowFoodAte()
        {
            StateController.WaitController.ChangeSpeed(StateController.MoveSnakeState.Speed, 10);
            // Изменить текущую скорость  - 50%
            StateController.MoveSnakeState.Speed /= 2;
        }

        /// <summary>
        /// Попа стала - головой (свитч между хвостом и головой)
        /// </summary>
        private void _switchFoodAte()
        {
            // Изменение вектора движение на противоположный
            StateController.MoveSnakeState.MotionVector *= -1;
            StateController.MoveSnakeState.SnakeCoods.Reverse();
            var snakeCoods = StateController.MoveSnakeState.SnakeCoods;
            // Хвост становится головой
            var head = StateController.MoveSnakeState.GameMatrix[snakeCoods[0].x][snakeCoods[0].y].CellGO;
            head.GetComponent<Image>().color = Color.yellow;
            head.name = "SnakeHead";
            // Голова становится хвостом
            var body = StateController.MoveSnakeState.GameMatrix[snakeCoods.Last().x][snakeCoods.Last().y].CellGO;
            body.GetComponent<Image>().color = Color.white;
            body.name = "SnakeBody";
        }


        /// <summary>
        /// Удаление всех "фруктов" (еды)
        /// </summary>
        private void _fruitDestroy()
        {
            for (int i = 0; i < StateController.MoveSnakeState.FoodCoods.Count; i++)
            {
                var tmpPointModel = StateController.MoveSnakeState.GameMatrix[(int)StateController.MoveSnakeState.FoodCoods[i].x][(int)StateController.MoveSnakeState.FoodCoods[i].y];
                tmpPointModel.CellState = Initialize.EnumСell.Empty;
                tmpPointModel.Food = Initialize.EnumFood.NoFood;
                Destroy(tmpPointModel.CellGO);
                tmpPointModel.CellGO = null;
            }
            StateController.MoveSnakeState.FoodCoods = new List<MatrixIdModel>();
        }

        public void StartState()
        {
            print("Star EatSnakeStateController");
            gameObject.SetActive(true);
            _fruitBuff();
        }

        public void EndState()
        {
            print("End EatSnakeStateController");
            _fruitDestroy();
            gameObject.SetActive(false);
        }




    }
}
