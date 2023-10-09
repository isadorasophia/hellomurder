using Bang.Contexts;
using Bang.Entities;
using Bang.Systems;
using Murder.Components;
using Murder;
using HelloMurder.Components;

namespace HelloMurder.Systems
{
    /// <summary>
    ///     System intended to capture and relay player inputs to entities.
    ///     System is called during frame updates and fixed updates thanks to interfaces.<br/>
    ///     Targets only enties with <b>both</b> PlayerComponent and AgentComponent
    ///     Example usage:<br/>
    ///     1. Poll input system with: <br/>
    ///         Game.Input <see cref="Game.Input"/><br/>
    ///     2. Send entity messages or call extension functions in FixedUpdate within the foreach:<br/>
    ///         entity.SendMessage <see cref="Entity.SendMessage{T}()"/><br/>
    ///         entity.SetImpulse <see cref="MurderEntityExtensions.SetAgentImpulse(Entity)"/><br/>
    /// </summary>
    [Filter(kind: ContextAccessorKind.Read, typeof(PlayerComponent), typeof(AgentComponent))]
    public class PlayerInputSystem : IUpdateSystem, IFixedUpdateSystem
    {

        /// <summary>
        ///     Called every fixed update.
        ///     We can apply input values to fixed updating components such as physics components.
        ///     For example the <see cref="AgentComponent"/>
        /// </summary>
        /// <param name="context"></param>
        public void FixedUpdate(Context context)
        {
            foreach (Entity entity in context.Entities)
            {
                // Send entity messages or use entity extensions to update relevant entities
            }
        }

        /// <summary>
        ///     Called every frame
        ///     This is where we should poll our input system
        ///     We can optionally cache these values and use them in the <see cref="FixedUpdate(Context)"/>
        /// </summary>
        /// <param name="context"></param>
        public void Update(Context context)
        {
            // Read from Game.Input
        }
    }
}
