namespace Game.Code.Services.EntityContainer
{
    public interface IEntityContainer
    {
        void RegisterEntity<TEntity>(TEntity entity) where TEntity : class, IFactoryEntity;
        TEntity GetEntity<TEntity>() where TEntity : class, IFactoryEntity;
    }
}