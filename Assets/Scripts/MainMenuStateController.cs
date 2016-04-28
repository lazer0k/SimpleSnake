using UnityEngine;

namespace Assets.Scripts
{
    public class MainMenuStateController : MonoBehaviour, IState
    {

        // Use this for initialization
        void Awake()
        {
            StateController.MainMenuState = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Start()
        {
           
        }

        /// <summary>
        /// Старт Игры
        /// </summary>
        public void StartGame()
        {
            StateController.ChangeState(StateController.EnumStateType.StartSnake);
        }

        public void StartState()
        {
            gameObject.SetActive(true);
        }

        public void EndState()
        {
            gameObject.SetActive(false);
        }
    }
}
