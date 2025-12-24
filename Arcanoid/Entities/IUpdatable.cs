using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.Entities
{
    public interface IUpdatable
    {
        void Update(float deltaTime, float fieldWidth, float fieldHeight);
    }
}