using Bang.Entities;
using Bang.StateMachines;
using HelloMurder.Core;
using Murder;
using Murder.Assets;
using Murder.Attributes;
using Murder.Core.Geometry;
using Murder.Core.Graphics;
using Murder.Core.Input;
using Murder.Services;
using Murder.Utilities;
using Newtonsoft.Json;
using System.Diagnostics;

namespace HelloMurder.StateMachines
{
    internal class MainMenuStateMachine : StateMachine
    {
        [JsonProperty, GameAssetId(typeof(WorldAsset))]
        private readonly Guid _newGameWorld = Guid.Empty;

        private MenuInfo _menuInfo = new();
        private OptionsInfo _optionsInfo = new();

        private OptionsInfo GetMainMenuOptions() =>
            new OptionsInfo(options: new MenuOption[] { new("Continue", selectable: MurderSaveServices.CanLoadSave()), new("New Game"), new("Options"), new("Exit") });

        private OptionsInfo GetOptionOptions() =>
            new OptionsInfo(options: new MenuOption[] {
                new(Game.Preferences.SoundVolume == 1 ? "Sounds on" : "Sounds off"),
                new(Game.Preferences.MusicVolume == 1 ? "Music on" : "Music off"),
                new("Back to menu") });

        public MainMenuStateMachine()
        {
            State(Main);
        }

        protected override void OnStart()
        {
            Entity.SetCustomDraw(DrawMainMenu);

            _menuInfo.Selection = MurderSaveServices.CanLoadSave() ? 0 : 1;
        }

        private IEnumerator<Wait> Main()
        {
            _optionsInfo = GetMainMenuOptions();
            _menuInfo.Selection = _optionsInfo.NextAvailableOption(-1, 1);

            while (true)
            {
                if (Game.Input.VerticalMenu(ref _menuInfo, _optionsInfo))
                {
                    switch (_menuInfo.Selection)
                    {
                        case 0: //  Continue Game
                            EffectsServices.FadeIn(World, .5f, Palette.Colors[1], false);
                            yield return Wait.ForSeconds(.5f);

                            Guid? targetWorld = MurderSaveServices.LoadSaveAndFetchTargetWorld();
                            Game.Instance.QueueWorldTransition(targetWorld ?? _newGameWorld);

                            break;

                        case 1: //  New Game
                            Game.Data.DeleteAllSaves();

                            EffectsServices.FadeIn(World, .5f, Palette.Colors[1], false);
                            yield return Wait.ForSeconds(.5f);

                            Game.Instance.QueueWorldTransition(_newGameWorld);
                            break;

                        case 2: // Options
                            yield return GoTo(Options);
                            break;

                        case 3: //  Exit
                            Game.Instance.ExitGame();
                            break;

                        default:
                            break;
                    }
                }

                yield return Wait.NextFrame;
            }
        }
        
        private IEnumerator<Wait> Options()
        {
            _optionsInfo = GetOptionOptions();
            _menuInfo.Selection = _optionsInfo.NextAvailableOption(-1, 1);

            Debug.Assert(_optionsInfo.Options is not null);

            while (true)
            {
                if (Game.Input.VerticalMenu(ref _menuInfo, _optionsInfo))
                {
                    switch (_menuInfo.Selection)
                    {
                        case 0: // Tweak sound
                            float volume = Game.Preferences.ToggleSoundVolumeAndSave();

                            _optionsInfo.Options[0] = volume == 1 ? new("Sounds on") : new("Sounds off");
                            break;

                        case 1: // Tweak music
                            float sound = Game.Preferences.ToggleMusicVolumeAndSave();

                            _optionsInfo.Options[1] = sound == 1 ? new("Music on") : new("Music off");
                            break;

                        case 2: // Go back
                            yield return GoTo(Main);
                            break;
                            
                        default:
                            break;
                    }
                }

                yield return Wait.NextFrame;
            }
        }

        private void DrawMainMenu(RenderContext render)
        {
            Debug.Assert(_optionsInfo.Options is not null);

            Point cameraHalfSize = render.Camera.Size / 2f - new Point(0, _optionsInfo.Length * 7);

            RenderServices.DrawVerticalMenu(render, cameraHalfSize, new Vector2(.5f, .5f), Game.Data.LargeFont, selectedColor: Palette.Colors[7], 
                color: Palette.Colors[5], shadow: Palette.Colors[1], _menuInfo.Selection, 
                out _, _optionsInfo.Options);
        }
    }
}
