using System;
using MegamanX.GameObjects.Playable;
using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class StateChangeInfo
    {
        public PlayerState OldState;
        
        public PlayerState NewState;

        public StateChangeInfo(PlayerState oldState, PlayerState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }

    public abstract class PlayerState
    {
        public PlayerState(Player parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public Player Parent { get; private set; }

        public virtual void OnInputEnter(PlayerInput inputType) {}

        public virtual void OnInputLeave(PlayerInput inputType) {}

        public virtual void OnStateEnter(StateChangeInfo info) {}

        public virtual void OnStateLeave(StateChangeInfo info) {}

        public virtual void Update(GameTime gameTime) {}
    }
}