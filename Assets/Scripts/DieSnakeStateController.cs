using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DieSnakeStateController : MonoBehaviour, IState
    {

        private GameObject _dynamicResultGO;
        // Use this for initialization
        void Awake()
        {
            StateController.DieSnakeState = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Удаление змеи
        /// </summary>
        private void _delSnake()
        {
            var moveSnakeState = StateController.MoveSnakeState;

            // Очищаем игровое поле
            for (int i = 0; i < moveSnakeState.GameMatrix.Count; i++)
            {
                for (int j = 0; j < moveSnakeState.GameMatrix[i].Count; j++)
                {
                    if (moveSnakeState.GameMatrix[i][j].CellState == Initialize.EnumСell.Block || moveSnakeState.GameMatrix[i][j].CellState == Initialize.EnumСell.Food)
                    {
                        moveSnakeState.GameMatrix[i][j].CellState = Initialize.EnumСell.Empty;
                    }
                    if (moveSnakeState.GameMatrix[i][j].CellGO != null)
                    {
                        Destroy(moveSnakeState.GameMatrix[i][j].CellGO);
                    }

                }
            }
        }


        /// <summary>
        /// Создание кнопки-таблички, с указанием результатов
        /// </summary>
        private void _showResult()
        {

            // Создание главного обьекта таблицы (бекграунд и перент для остальных)
            _dynamicResultGO = new GameObject("Result");
            _dynamicResultGO.AddComponent<Image>();
            var dynamicResultRectTr = _dynamicResultGO.GetComponent<RectTransform>();
            dynamicResultRectTr.sizeDelta = new Vector2(Screen.width / 3f, Screen.height / 3f);
            _dynamicResultGO.transform.SetParent(StateController.StartSnakeState.TSnakeParent);
            _dynamicResultGO.transform.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

            // Создание обьекта - текста
            var textResultGO = new GameObject("Result Text");
            var txtResultComp = textResultGO.AddComponent<Text>();
            textResultGO.transform.SetParent(_dynamicResultGO.transform);
            textResultGO.GetComponent<RectTransform>().sizeDelta = dynamicResultRectTr.sizeDelta;
            textResultGO.transform.localPosition = Vector3.zero;
            // Создание компонента - текста
            txtResultComp.text = "LOOSE! \n You snake had " + (StateController.StartSnakeState.TSnakeParent.childCount - 4) + " blocks \n click to restart";
            txtResultComp.font = Font.CreateDynamicFontFromOSFont("font", 14);
            txtResultComp.alignment = TextAnchor.MiddleCenter;
            txtResultComp.resizeTextForBestFit = true;
            txtResultComp.color = Color.black;


            // Создание кнопки
            var buttnResultGO = new GameObject("Result Btn End");
            buttnResultGO.AddComponent<Image>().color = new Color(0, 0, 0, 0);
            var buttnResultBtnComp = buttnResultGO.AddComponent<Button>();
            buttnResultBtnComp.onClick.AddListener(_startNewGame);
            buttnResultGO.GetComponent<RectTransform>().sizeDelta = dynamicResultRectTr.sizeDelta;
            buttnResultGO.transform.SetParent(_dynamicResultGO.transform);
            buttnResultGO.transform.localPosition = Vector3.zero;

        }

        /// <summary>
        /// Начать игру заново
        /// </summary>
        private void _startNewGame()
        {
            StateController.ChangeState(StateController.EnumStateType.StartSnake);
        }

        public void StartState()
        {
            gameObject.SetActive(true);
            _showResult();
            _delSnake();
        }

        public void EndState()
        {
            Destroy(_dynamicResultGO);
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }
}
