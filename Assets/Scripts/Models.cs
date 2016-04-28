using UnityEngine;

namespace Assets.Scripts
{

    /// <summary>
    /// Модель точки - игрового поля
    /// </summary>
    public class PointModel
    {

        public Initialize.EnumСell CellState = Initialize.EnumСell.Empty;
        public Initialize.EnumFood Food;
        public GameObject CellGO = null;
        public Vector2 Position;
        public MatrixIdModel TeleportPosition;
        public bool Ate;

    }

    /// <summary>
    /// Модель координат
    /// </summary>
    public class MatrixIdModel
    {
        public int x;
        public int y;
    }
}
