using System;

namespace MegamanX.GameObjects.Playable
{
    public enum PlayerInput
    {
        Left,
        Right,
        Jump,
        Dash,
        Fire
    }

    public class PlayerInputData : ICloneable
    {
        public bool Left;
        public bool Right;
        public bool Jump;
        public bool Dash;
        public bool Fire;

        public bool IsMovingLeft => Left && !Right;

        public bool IsMovingRight => !Left && Right;

        public bool IsMoving => IsMovingLeft || IsMovingRight;

        public object Clone()
        {
            var clone = new PlayerInputData();
            clone.Left = this.Left;
            clone.Right = this.Right;
            clone.Jump = this.Jump;
            clone.Dash = this.Dash;
            clone.Fire = this.Fire;
            return clone;
        }
    }
}