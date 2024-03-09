namespace MegamanX.GameObjects.Components
{
    public delegate void ComponentEvent<TComponent, TEvent>(TComponent source, TEvent e) where TComponent : IComponent;

    public delegate void OwnedComponentEvent<TComponent, TEvent>(TComponent source, GameObject parent, TEvent e) where TComponent : IComponent;
}
