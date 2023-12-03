using Bang.Entities;
using Bang.StateMachines;
using HelloMurder.Core;
using Murder;
using Murder.Assets;
using Murder.Assets.Localization;
using Murder.Attributes;
using Murder.Core.Geometry;
using Murder.Core.Graphics;
using Murder.Core.Input;
using Murder.Services;
using Newtonsoft.Json;
using System.Globalization;

namespace HelloMurder.StateMachines
{
    internal class MainMenuStateMachine : StateMachine
    {
        [JsonProperty, GameAssetId(typeof(WorldAsset))]
        private readonly Guid _newGameWorld = Guid.Empty;

        private MenuInfo _menuInfo = new();

        private MenuInfo GetMainMenuOptions() =>
            new MenuInfo(new MenuOption[] { new(LocalizedResources.Menu_Continue, selectable: MurderSaveServices.CanLoadSave()), 
                new(LocalizedResources.Menu_NewGame), new(LocalizedResources.Menu_Options), new(LocalizedResources.Menu_Exit) });

        private MenuInfo GetOptionOptions() =>
            new(new MenuOption[] {
                new(Game.Preferences.SoundVolume == 1 ? LocalizedResources.Menu_SoundsOn : LocalizedResources.Menu_SoundsOff),
                new(Game.Preferences.MusicVolume == 1 ? LocalizedResources.Menu_MusicOn : LocalizedResources.Menu_MusicOff),
                new(LocalizedResources.Menu_CurrentLanguage),
                new(LocalizedResources.Menu_BackToMenu) });

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

                            _menuInfo.Options[0] = volume == 1 ? new(LocalizedResources.Menu_SoundsOn) : new(LocalizedResources.Menu_SoundsOff);
                            break;

                        case 1: // Tweak music
                            float sound = Game.Preferences.ToggleMusicVolumeAndSave();

                            _menuInfo.Options[1] = sound == 1 ? new(LocalizedResources.Menu_MusicOn) : new(LocalizedResources.Menu_MusicOff);
                            break;

                        case 2: // Language
                            SwitchLanguage();

                            _menuInfo = GetOptionOptions();
                            _menuInfo.Select(2);

                            break;

                        case 3: // Go back
                            yield return GoTo(Main);
                            break;
                            
                        default:
                            break;
                    }
                }

                yield return Wait.NextFrame;
            }
        }

        private void SwitchLanguage()
        {
            Game.Preferences.Language = Languages.Next(Game.Preferences.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Game.Preferences.Language.Identifier);
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
