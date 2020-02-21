using System;
using Microsoft.Xna.Framework;

namespace MegamanX.GameStates
{
    public abstract class GameState : IUpdateable, IDrawable
    {
        private bool _enabled, _visible;
        private int _updateOrder, _drawOrder;

        public bool Enabled 
        { 
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    EnabledChanged?.Invoke(this, EventArgs.Empty);
                    _enabled = value;

                    if (_enabled)
                    {
                        OnResume();
                    }
                    else
                    {
                        OnPause();
                    }
                }
            }
        }
        public bool Visible 
        { 
            get => _visible;
            set
            {
                if (_visible != value)
                {
                    VisibleChanged?.Invoke(this, EventArgs.Empty);
                    _visible = value;

                    if (_visible)
                    {
                        OnShow();
                    }
                    else
                    {
                        OnHide();
                    }
                }
            }
        }
        public int UpdateOrder 
        { 
            get => _updateOrder;
            set
            {
                if (_updateOrder != value)
                {
                    UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
                    _updateOrder = value;
                }
            }
        }
        public int DrawOrder 
        { 
            get => _drawOrder;
            set
            {
                if (_drawOrder != value)
                {
                    DrawOrderChanged?.Invoke(this, EventArgs.Empty);
                    _drawOrder = value;
                }
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public virtual void Initialize() { }
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        public virtual void OnPause() { }
        public virtual void OnResume() { }
        public virtual void OnHide() { }
        public virtual void OnShow() { }
    }
}