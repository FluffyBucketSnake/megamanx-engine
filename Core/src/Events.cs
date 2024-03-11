namespace MegamanX
{
    public delegate void ComponentEvent<TComponent>(TComponent source) where TComponent : IComponent;
    public delegate void ComponentEvent<TComponent, TEvent>(TComponent source, TEvent e) where TComponent : IComponent;

    public delegate void OwnedComponentEvent<TComponent>(TComponent source, Entity entity) where TComponent : IComponent;
    public delegate void OwnedComponentEvent<TComponent, TEvent>(TComponent source, Entity entity, TEvent e) where TComponent : IComponent;
}
