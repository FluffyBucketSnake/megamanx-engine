using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace MegamanX.GameStates
{
    public static class GameStateManager
    {
        private static readonly List<GameState> _states = [];
        private static readonly List<GameState> _enabledStates = [];
        private static readonly List<GameState> _visibleStates = [];
        private static bool _enabledDirty;
        private static bool _visibleDirty;

        /// <summary>
        /// Contains all GameStates registered in the manager.
        /// </summary>
        public static IEnumerable<GameState> States { get; } = new ReadOnlyCollection<GameState>(_states);
        /// <summary>
        /// Contains all GameStates marked for updating in the manager. Sorted by order.
        /// </summary>
        public static IEnumerable<GameState> EnabledStates { get; } = new ReadOnlyCollection<GameState>(_enabledStates);
        /// <summary>
        /// Contains all GameStates marked for drawing in the manager. Sorted by order.
        /// </summary>
        public static IEnumerable<GameState> VisibleStates { get; } = new ReadOnlyCollection<GameState>(_visibleStates);

        /// <summary>
        /// Registers a state into the manager. Automatically marks it for updating/drawing if enabled/visible.
        /// </summary>
        /// <param name="state">The GameState to add.</param>
        public static void AddState(GameState state)
        {
            // Add listeners into state.
            state.EnabledChanged += OnGameStateEnabledChanged;
            state.VisibleChanged += OnGameStateVisibleChanged;
            state.UpdateOrderChanged += OnGameStateUpdateOrderChanged;
            state.DrawOrderChanged += OnGameStateDrawOrderChanged;

            // Add state into collections.
            _states.Add(state);

            if (state.Enabled)
            {
                _enabledStates.Add(state);
                _enabledDirty = true;
            }
            if (state.Visible)
            {
                _visibleStates.Add(state);
                _visibleDirty = true;
            }
        }

        /// <summary>
        /// Unregisters a state from this manager.
        /// </summary>
        /// <param name="state">The GameState to remove.</param>
        public static void RemoveState(GameState state)
        {
            // Remove listeners from state.
            state.EnabledChanged -= OnGameStateEnabledChanged;
            state.VisibleChanged -= OnGameStateVisibleChanged;
            state.UpdateOrderChanged -= OnGameStateUpdateOrderChanged;
            state.DrawOrderChanged -= OnGameStateDrawOrderChanged;

            // Remove state from collections.
            _ = _states.Remove(state);
            _ = _enabledStates.Remove(state);
            _ = _visibleStates.Remove(state);
        }

        /// <summary>
        /// Forces the manager to sort the currently enabled states.
        /// </summary>
        public static void SortEnabledStates()
        {
            _enabledStates.Sort(ComparisonUpdateOrder);
            _enabledDirty = false;
        }

        /// <summary>
        /// Forces the manager to sort the currently visible states.
        /// </summary>
        public static void SortVisibleStates()
        {
            _visibleStates.Sort(ComparisonDrawOrder);
            _visibleDirty = false;
        }

        /// <summary>
        /// Updates all enabled states.
        /// </summary>
        /// <param name="gameTime">The elapsed time since last call.</param>
        public static void UpdateStates(GameTime gameTime)
        {
            // Check if the enabled list is dirty. If yes, re-sort.
            if (_enabledDirty)
            {
                SortEnabledStates();
            }

            foreach (GameState state in _enabledStates)
            {
                state.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws all visible states.
        /// </summary>
        /// <param name="gameTime">The elapsed time since last call.</param>
        public static void DrawStates(GameTime gameTime)
        {
            // Check if the enabled list is dirty. If yes, re-sort.
            if (_visibleDirty)
            {
                SortVisibleStates();
            }

            foreach (GameState state in _visibleStates)
            {
                state.Draw(gameTime);
            }
        }

        private static void OnGameStateEnabledChanged(object? sender, EventArgs e)
        {
            GameState state = (sender as GameState)!;
            if (state.Enabled)
            {
                _enabledStates.Add(state);
                _enabledDirty = true;
            }
            else
            {
                _ = _enabledStates.Remove(state);
            }
        }

        private static void OnGameStateVisibleChanged(object? sender, EventArgs e)
        {
            GameState state = (sender as GameState)!;
            if (state.Visible)
            {
                _visibleStates.Add(state);
                _visibleDirty = true;
            }
            else
            {
                _ = _visibleStates.Remove(state);
            }
        }

        private static void OnGameStateUpdateOrderChanged(object? sender, EventArgs e)
        {
            GameState state = (sender as GameState)!;
            if (state.Enabled)
            {
                _enabledDirty = true;
            }
        }

        private static void OnGameStateDrawOrderChanged(object? sender, EventArgs e)
        {
            GameState state = (sender as GameState)!;
            if (state.Visible)
            {
                _visibleDirty = true;
            }
        }

        private static int ComparisonUpdateOrder(GameState a, GameState b)
        {
            return a.UpdateOrder < b.UpdateOrder ? -1 : a.UpdateOrder == b.UpdateOrder ? 0 : 1;
        }

        private static int ComparisonDrawOrder(GameState a, GameState b)
        {
            return a.DrawOrder < b.DrawOrder ? -1 : a.DrawOrder == b.DrawOrder ? 0 : 1;
        }
    }
}
