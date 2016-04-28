using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class MoveSnakeStateController : MonoBehaviour, IState
    {
        //Вектор направления - движения
        public Vector2 MotionVector = new Vector2(0, 1);
        //Игровое поле
        public List<List<PointModel>> GameMatrix = new List<List<PointModel>>();
        //Координаты змейки в игровом поле
        public List<MatrixIdModel> SnakeCoods = new List<MatrixIdModel>();
        //Координаты еды в игровом поле
        public List<MatrixIdModel> FoodCoods = new List<MatrixIdModel>();
        //Скорость змейки
        public float Speed = 1;
        //Таймеры для скорости змейки
        public float SpeedTimerMax = 0.25f;
        private float _speedTimerCurrent = 0.25f;

        //Проверка совершённости хода
        private bool _wasMove = true;


        // Use this for initialization
        void Awake()
        {
            StateController.MoveSnakeState = this;
        }

        // Update is called once per frame
        void Update()
        {

            _inputCheck();

            //Проверка таймера
            if (_speedTimerCurrent < Time.deltaTime)
            {
                _speedTimerCurrent = SpeedTimerMax / Speed;
                _moveSnake();
            }
            else
            {
                _speedTimerCurrent -= Time.deltaTime;
            }






        }

        /// <summary>
        /// Проверка изменения вектора движения
        /// </summary>
        private void _inputCheck()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow) && MotionVector != new Vector2(0, 1)&& _wasMove)
            {
                MotionVector = new Vector2(0, -1);
                _wasMove = false;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) && MotionVector != new Vector2(0, -1) && _wasMove)
            {
                MotionVector = new Vector2(0, 1);
                _wasMove = false;
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) && MotionVector != new Vector2(-1, 0) && _wasMove)
            {
                MotionVector = new Vector2(1, 0);
                _wasMove = false;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) && MotionVector != new Vector2(1, 0) && _wasMove)
            {
                MotionVector = new Vector2(-1, 0);
                _wasMove = false;
            }
        }


        /// <summary>
        /// Передвижение змейки
        /// </summary>
        private void _moveSnake()
        {
            _wasMove = true;

            //Потенциальная точка в игровом поле (где окажется голова змейки)
            MatrixIdModel newCoods = new MatrixIdModel() { x = (SnakeCoods[0].x + (int)MotionVector.x), y = (SnakeCoods[0].y + (int)MotionVector.y) };

            //Проверка потенциальной точки
            switch (GameMatrix[newCoods.x][newCoods.y].CellState)
            {
                case Initialize.EnumСell.Food:
                    newCoods = _willEatFood(newCoods);
                    break;
                case Initialize.EnumСell.Block:
                    // Змейка врезалась и погибла
                    StateController.ChangeState(StateController.EnumStateType.DieSnake);
                    return;
                case Initialize.EnumСell.Teleport:
                    // Змейка телепортировалась (край карты)
                    newCoods = GameMatrix[newCoods.x][newCoods.y].TeleportPosition;
                    break;
            }

            // Передвижение
            for (int i = 0; i < SnakeCoods.Count; i++)
            {
                var snakePartGO = GameMatrix[SnakeCoods[i].x][SnakeCoods[i].y].CellGO;
                // Перемещаем часть змейки
                snakePartGO.transform.position = GameMatrix[newCoods.x][newCoods.y].Position;
                GameMatrix[newCoods.x][newCoods.y].CellGO = snakePartGO;
                GameMatrix[newCoods.x][newCoods.y].CellState = Initialize.EnumСell.Block;
                // Очищаем освободившеюся часть
                GameMatrix[SnakeCoods[i].x][SnakeCoods[i].y].CellGO = null;
                GameMatrix[SnakeCoods[i].x][SnakeCoods[i].y].CellState = Initialize.EnumСell.Empty;
                var currentCood = newCoods;
                // Изменяем потенциальную точку
                newCoods = SnakeCoods[i];
                // Изменяем координаты змейки
                SnakeCoods[i] = currentCood;
            }

            // Змейка сьела фрукт и должна вырости
            if (GameMatrix[newCoods.x][newCoods.y].Ate)
            {
                GameMatrix[newCoods.x][newCoods.y].Ate = false;
                StateController.StartSnakeState.CreateSnakePoint(newCoods);
            }
        }

        /// <summary>
        /// Поедание "фрукта"
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns> возвращаем возможно изменённый newCoods </returns>
        private MatrixIdModel _willEatFood(MatrixIdModel coordinates)
        {
            bool willReturn = (GameMatrix[coordinates.x][coordinates.y].Food == Initialize.EnumFood.SwitchFood);
            StateController.EatSnakeState.EatedFoodCood = coordinates;
            StateController.ChangeState(StateController.EnumStateType.EatSnake);
            //Если фрукт был - switch, изменяем потенциальную точку 
            if (willReturn)
            {
                coordinates = new MatrixIdModel() { x = (SnakeCoods[0].x + (int)MotionVector.x), y = (SnakeCoods[0].y + (int)MotionVector.y) };
            }
            return coordinates;
        }


        /// <summary>
        /// Создание еды
        /// </summary>
        private void _createFood()
        {

            var r = new System.Random();
            for (int i = 0; i < 3; i++)
            {
                // Создание еды в свободной точке
                PointModel currentPoint;
                var x = 0;
                var y = 0;
                do
                {
                    x = Random.Range(0, GameMatrix.Count);
                    y = Random.Range(0, GameMatrix[x].Count);
                    currentPoint = GameMatrix[x][y];
                } while (currentPoint.CellState != Initialize.EnumСell.Empty);

                // Выбор типа еды
                currentPoint.Food = ((Initialize.EnumFood)r.Next(1, Enum.GetValues(typeof(Initialize.EnumFood)).Length));
                // Создание ГеймОбьекта еды
                GameObject newGO = new GameObject("Food");
                // Цвет обьекта еды
                switch (currentPoint.Food)
                {
                    case Initialize.EnumFood.NormalFood:
                        newGO.AddComponent<Image>().color = Color.green;
                        break;
                    case Initialize.EnumFood.SpoiledFood:
                        newGO.AddComponent<Image>().color = Color.red;
                        break;
                    case Initialize.EnumFood.FastFood:
                        newGO.AddComponent<Image>().color = Color.blue;
                        break;
                    case Initialize.EnumFood.SlowFood:
                        newGO.AddComponent<Image>().color = Color.grey;
                        break;
                    case Initialize.EnumFood.SwitchFood:
                        newGO.AddComponent<Image>().color = Color.magenta;
                        break;
                }
                // Размер обьекта еды
                newGO.GetComponent<RectTransform>().sizeDelta = StateController.StartSnakeState.SnakePartSize;
                // Перент обьекта еды
                newGO.transform.SetParent(StateController.StartSnakeState.TSnakeParent);
                // Позиция  обьекта еды
                newGO.transform.position = currentPoint.Position;
                // Добавление в PointModel  обьекта еды
                currentPoint.CellGO = newGO;
                currentPoint.CellState = Initialize.EnumСell.Food;
                // Добавление в list с координатами "еды" (обьекта еды)
                FoodCoods.Add(new MatrixIdModel() { x = x, y = y });
            }
        }

        public void StartState()
        {
            gameObject.SetActive(true);
            _createFood();
        }

        public void EndState()
        {
            gameObject.SetActive(false);
        }
    }
}
