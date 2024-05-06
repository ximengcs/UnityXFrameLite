
using System;

namespace Assets.Scripts.Entities
{
    public class EntityViewAttribute : Attribute
    {
        public Type Target { get; }

        public EntityViewAttribute(Type target)
        {
            Target = target;
        }
    }
}
