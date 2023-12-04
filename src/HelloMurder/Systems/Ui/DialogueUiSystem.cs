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
        private readonly float _duration = 2;

        private float _timeAdded = 0;

        public void Draw(RenderContext render, Context context)
        {
            if (!context.HasAnyEntity)
            {
                return;
            }

            DialogueUiComponent dialogue = context.Entity.GetDialogueUi();
            string content = LocalizationServices.GetLocalizedString(dialogue.Content);

            float timeSinceAppeared = Game.NowUnscaled - _timeAdded;
            int currentLength = Calculator.CeilToInt(Calculator.ClampTime(timeSinceAppeared, _duration) * content.Length);

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
            _timeAdded = Game.NowUnscaled;
        }

        public void OnModified(World world, ImmutableArray<Entity> entities) { }

        public void OnRemoved(World world, ImmutableArray<Entity> entities) { }
    }
}
