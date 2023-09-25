using Bang.Contexts;
using Bang.Entities;
using Bang.Systems;
using Murder.Core.Geometry;
using Murder.Components;
using Murder;
using Bang.StateMachines;
using Bang.Components;
using Murder.Helpers;
using HelloMurder.Components;
using HelloMurder.Messages;
using HelloMurder.Core;
using HelloMurder.Core.Input;
using Bang.Interactions;
using System.Numerics;
using Murder.Utilities;

namespace HelloMurder.Systems
{
    [Filter(kind: ContextAccessorKind.Read, typeof(PlayerComponent), typeof(AgentComponent))]
    public class PlayerInputSystem : IUpdateSystem, IFixedUpdateSystem
    {
        private Vector2 _cachedInputAxis = Vector2.Zero;
        private int _cachedInputSkill = -1;
        
        private bool _previousCachedAttack = false;
        private bool _cachedAttack = false;

        private bool _interacted = false;
        
        /// <summary>
        /// Whether the player locked the movement and only wants to change the facing.
        /// </summary>
        private bool _lockMovement = false;

        public void FixedUpdate(Context context)
        {
            foreach (Entity entity in context.Entities)
            {
                PlayerComponent player = entity.GetComponent<PlayerComponent>();

                bool moved = _cachedInputAxis.HasValue();
                
                if (_interacted)
                {
                    entity.SendMessage<InteractorMessage>();
                }
                
                if (moved)
                {
                    Direction direction = DirectionHelper.FromVector(_cachedInputAxis);
                        
                    entity.SetAgentImpulse(
                        _lockMovement ? Vector2.Zero : _cachedInputAxis, direction);
                }

                if (_cachedInputSkill > 0)
                {
                    entity.SendMessage(new AgentInputMessage(_cachedInputSkill));
                }
                _cachedInputSkill = -1;
                
                if (!_previousCachedAttack && _cachedAttack)
                {
                    _previousCachedAttack = true;
                    entity.SendMessage(new AgentInputMessage(InputButtons.Attack));
                }
                else if (_previousCachedAttack && !_cachedAttack)
                {
                    _previousCachedAttack = false;
                    entity.SendMessage(new AgentReleaseInputMessage(InputButtons.Attack));
                }
                _interacted = false;
            }
        }

        public void Update(Context context)
        {
            _cachedInputAxis = Game.Input.GetAxis(InputAxis.Movement).Value;

            _lockMovement = Game.Input.Down(InputButtons.LockMovement);

            _cachedInputSkill =
                Game.Input.Down(InputButtons.Skill1) ? InputButtons.Skill1 :
                Game.Input.Down(InputButtons.Skill2) ? InputButtons.Skill2 :
                _cachedInputSkill;

            _cachedAttack = Game.Input.Down(InputButtons.Attack);
            
            if (Game.Input.Pressed(InputButtons.Interact))
            {
                _interacted = true;
                
                Game.Input.Consume(InputButtons.Interact);
            }
        }
    }
}
