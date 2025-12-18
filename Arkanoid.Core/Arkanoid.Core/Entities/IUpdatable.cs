namespace Arkanoid.Core.Entities
{
    public interface IUpdatable
    {
        void Update(float deltaTime, float fieldWidth, float fieldHeight);
    }
}