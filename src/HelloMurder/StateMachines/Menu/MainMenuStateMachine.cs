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
using Newtonsoft.Json;

namespace HelloMurder.StateMachines
{
    internal class MainMenuStateMachine : StateMachine
    {
        [JsonProperty, GameAssetId(typeof(WorldAsset))]
        private readonly Guid _newGameWorld = Guid.Empty;

        private MenuInfo _menuInfo = new();

        private MenuInfo GetMainMenuOptions() =>
            new MenuInfo(new MenuOption[] { new("Continue", selectable: MurderSaveServices.CanLoadSave()), new("New Game"), new("Options"), new("Exit") });

        private MenuInfo GetOptionOptions() =>
            new MenuInfo(new MenuOption[] {
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

            _menuInfo.Select(MurderSaveServices.CanLoadSave() ? 0 : 1);
        }

        private IEnumerator<Wait> Main()
        {
            _menuInfo = GetMainMenuOptions();
            _menuInfo.Select(_menuInfo.NextAvailableOption(-1, 1));

            while (true)
            {
                if (Game.Input.VerticalMenu(ref _menuInfo))
                {
                    switch (_menuInfo.Selection)
                    {
                        case 0: //  Continue Game
                            Guid? targetWorld = MurderSaveServices.LoadSaveAndFetchTargetWorld();
                            Game.Instance.QueueWorldTransition(targetWorld ?? _newGameWorld);

                            break;

                        case 1: //  New Game
                            Game.Data.DeleteAllSaves();
                            Game.Instance.QueueWorldTransition(_newGameWorld);
                            break;

                        case 2: // Options
                            yield return GoTo(Options);
                            break;

                        case 3: //  Exit
                            Game.Instance.QueueExitGame();
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
            _menuInfo = GetOptionOptions();
            _menuInfo.Select(_menuInfo.NextAvailableOption(-1, 1));

            while (true)
            {
                if (Game.Input.VerticalMenu(ref _menuInfo))
                {
                    switch (_menuInfo.Selection)
                    {
                        case 0: // Tweak sound
                            float volume = Game.Preferences.ToggleSoundVolumeAndSave();

                            _menuInfo.Options[0] = volume == 1 ? new("Sounds on") : new("Sounds off");
                            break;

                        case 1: // Tweak music
                            float sound = Game.Preferences.ToggleMusicVolumeAndSave();

                            _menuInfo.Options[1] = sound == 1 ? new("Music on") : new("Music off");
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
            Point cameraHalfSize = render.Camera.Size / 2f - new Point(0, _menuInfo.Length * 7);

            _ = RenderServices.DrawVerticalMenu(
                render.UiBatch, 
                cameraHalfSize, 
                new DrawMenuStyle() 
                { 
                    Color = Palette.Colors[7], 
                    Shadow = Palette.Colors[1],
                    SelectedColor = Palette.Colors[9]
                },
                _menuInfo);
        }
    }
}
