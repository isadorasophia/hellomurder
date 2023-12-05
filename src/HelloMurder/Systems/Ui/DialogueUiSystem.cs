using Bang;
using Bang.Contexts;
using Bang.Entities;
using Bang.Systems;
using HelloMurder.Components;
using HelloMurder.Core;
using Murder;
using Murder.Core.Graphics;
using Murder.Services;
using Murder.Utilities;
using System.Collections.Immutable;
using System.Numerics;

namespace HelloMurder.Systems
{
    [Filter(typeof(DialogueUiComponent))]
    [Watch(typeof(DialogueUiComponent))]
    internal class DialogueUiSystem : IReactiveSystem, IMurderRenderSystem
    {
        private readonly float _duration = .8f;
        private readonly float _screenTime = 2.4f;

        private float _timeUpdated = 0;
        private int _currentIndex = 0;

        public void Draw(RenderContext render, Context context)
        {
            if (!context.HasAnyEntity)
            {
                return;
            }

            DialogueUiComponent dialogue = context.Entity.GetDialogueUi();
            if (_currentIndex >= dialogue.Content.Length)
            {
                return;
            }

            string content = LocalizationServices.GetLocalizedString(dialogue.Content[_currentIndex]);

            float timeSinceAppeared = Game.NowUnscaled - _timeUpdated;

            int currentLength = Calculator.RoundToInt(Calculator.ClampTime(timeSinceAppeared, _duration) * content.Length);
            if (timeSinceAppeared > _screenTime)
            {
                if (_currentIndex + 1 >= dialogue.Content.Length)
                {
                    context.Entity.RemoveDialogueUi();
                }

                // Move on with the next line.
                _currentIndex++;
                _timeUpdated = Game.NowUnscaled;
            }

            int font = (int)MurderFonts.PixelFont;

            RenderServices.DrawText(
                render.UiBatch, 
                font, 
                content,
                new Vector2(x: render.Camera.Width / 2f, y: render.Camera.Height - 20), 
                maxWidth: 200, 
                currentLength,
                new DrawInfo(0.1f)
                {
                    Origin = new Vector2(.5f, 0),
                    Color = Palette.Colors[1],
                    Shadow = Palette.Colors[3],
                });
        }

        public void OnAdded(World world, ImmutableArray<Entity> entities)
        {
            _timeUpdated = Game.NowUnscaled;
            _currentIndex = 0;
        }

        public void OnModified(World world, ImmutableArray<Entity> entities) { }

        public void OnRemoved(World world, ImmutableArray<Entity> entities) { }
    }
}
