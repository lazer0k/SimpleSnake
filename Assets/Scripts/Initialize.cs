using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Initialize : MonoBehaviour
    {
        public enum EnumСell
        {
            Empty, // Пустая клетка
            Food, // Клекта с едой "фруктом"
            Block, // Клетка с препятствием в т.ч. змейкой
            Teleport // Крайняя клетка
        }

        public enum EnumFood
        {
            NoFood, // default
            NormalFood,//а) объект, при съедении которого змейка удлиняется
            SpoiledFood,//б) объект, при съедении которого змейка укорачивается
            FastFood,//в) объект, который увеличивает или замедляет скорость на некоторое время
            SlowFood,
            SwitchFood//г) объект, который разворачивает змейку задом наперед (змея начинает двигаться в обратную сторону, хвост заменяется на голову)
        }




        void Start()
        {
            StateController.Start();
        }



    }
}
