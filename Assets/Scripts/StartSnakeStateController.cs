using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class StartSnakeStateController : MonoBehaviour, IState
    {

        public Vector2 SnakePartSize;
        public Transform TSnakeParent;
        // Use this for initialization
        void Awake()
        {
            StateController.StartSnakeState = this;
        }

        // Update is called once per frame
        void Update()
        {

        }



        /// <summary>
        /// Сброс настроек 
        /// </summary>
        private void _setDefultOptions()
        {
            StateController.MoveSnakeState.MotionVector = new Vector2(0, 1);
            StateController.MoveSnakeState.SnakeCoods = new List<MatrixIdModel>();
            StateController.MoveSnakeState.FoodCoods = new List<MatrixIdModel>();
            StateController.MoveSnakeState.Speed = 1;
            StateController.MoveSnakeState.SpeedTimerMax = 0.25f;
            // Если первый запуск (матрица игрового поля пустая)
            if (StateController.MoveSnakeState.GameMatrix.Count == 0)
            {
                _createGameMatrix();
            }

        }


        /// <summary>
        /// Создание змейки
        /// </summary>
        private void _createSnake()
        {
            for (int i = 0; i < 4; i++)
            {
                var x = (StateController.MoveSnakeState.GameMatrix.Count / 2);
                var y = (StateController.MoveSnakeState.GameMatrix[x].Count / 2) - i;
                CreateSnakePoint(new MatrixIdModel() { x = x, y = y }, (i == 0));

            }
        }

        /// <summary>
        /// Создание части змейки
        /// </summary>
        /// <param name="coods"></param>
        /// <param name="head"></param>
        public void CreateSnakePoint(MatrixIdModel coods, bool head = false)
        {
            GameObject newGO = (head ? new GameObject("SnakeHead") : new GameObject("SnakeBody"));
            newGO.AddComponent<Image>().color = (head ? Color.yellow : Color.white);
            newGO.GetComponent<RectTransform>().sizeDelta = SnakePartSize;
            newGO.transform.SetParent(TSnakeParent);
            newGO.transform.position = StateController.MoveSnakeState.GameMatrix[(int)coods.x][(int)coods.y].Position;
            StateController.MoveSnakeState.GameMatrix[coods.x][coods.y].CellGO = newGO;
            StateController.MoveSnakeState.GameMatrix[coods.x][coods.y].CellState = Initialize.EnumСell.Block;
            StateController.MoveSnakeState.SnakeCoods.Add(new MatrixIdModel() { x = coods.x, y = coods.y });
        }

        /// <summary>
        /// Создание игрового поля
        /// </summary>
        private void _createGameMatrix()
        {
            var height = (int)(Screen.height / SnakePartSize.x);
            var width = (int)(Screen.width / SnakePartSize.y);

            for (int j = 0; j < height; j++)
            {
                List<PointModel> pointModelTemp = new List<PointModel>();
                for (int i = 0; i < width; i++)
                {
                    pointModelTemp.Add(new PointModel()
                    {
                        Position = new Vector2(i * SnakePartSize.x, j * SnakePartSize.y)
                    });
                }
                StateController.MoveSnakeState.GameMatrix.Add(pointModelTemp);
            }

            _setTeleportFields();


        }

        /// <summary>
        /// Находим и обозначаем края игрового поля
        /// </summary>
        private void _setTeleportFields()
        {
            var gameMatrix = StateController.MoveSnakeState.GameMatrix;
            for (int i = 0; i < gameMatrix.Count; i++)
            {
                gameMatrix[i][0].CellState = Initialize.EnumСell.Teleport;
                gameMatrix[i][0].TeleportPosition = new MatrixIdModel() { x = i, y = gameMatrix[i].Count - 2 };

                gameMatrix[i].Last().CellState = Initialize.EnumСell.Teleport;
                gameMatrix[i].Last().TeleportPosition = new MatrixIdModel() { x = i, y = 1 };

            }
            for (int i = 0; i < gameMatrix[0].Count; i++)
            {
                gameMatrix[0][i].CellState = Initialize.EnumСell.Teleport;
                gameMatrix[0][i].TeleportPosition = new MatrixIdModel() { x = gameMatrix.Count - 2, y = i };
            }
            for (int i = 0; i < gameMatrix.Last().Count; i++)
            {
                gameMatrix.Last()[i].CellState = Initialize.EnumСell.Teleport;
                gameMatrix.Last()[i].TeleportPosition = new MatrixIdModel() { x = 1, y = i };
            }
        }

        public void StartState()
        {
            gameObject.SetActive(true);
            _setDefultOptions();
            _createSnake();
            StateController.ChangeState(StateController.EnumStateType.MoveSnake);
        }

        public void EndState()
        {
            gameObject.SetActive(false);
        }
    }
}
