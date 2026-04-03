using Bang;
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

namespace HelloMurder.StateMachines
{
    public class MainMenuStateMachine : StateMachine
    {
        [Serialize, GameAssetId(typeof(WorldAsset))]
        private readonly Guid _newGameWorld = Guid.Empty;

        private MenuInfo _menuInfo = new();

        private MenuInfo GetMainMenuOptions() =>
            new MenuInfo(new MenuOption[] { new(HelloMurderGame.Resources.Menu.Continue, selectable: Game.Data.CanLoadSaveData(0)), 
                new(HelloMurderGame.Resources.Menu.NewGame), new(HelloMurderGame.Resources.Menu.Options), new(HelloMurderGame.Resources.Menu.Exit) });

        private MenuInfo GetOptionOptions() =>
            new(new MenuOption[] {
                new(Game.Preferences.SoundVolume == 1 ? HelloMurderGame.Resources.Menu.SoundsOn : HelloMurderGame.Resources.Menu.SoundsOff),
                new(Game.Preferences.MusicVolume == 1 ? HelloMurderGame.Resources.Menu.MusicOn : HelloMurderGame.Resources.Menu.MusicOff),
                new(HelloMurderGame.Resources.Menu.CurrentLanguage),
                new(HelloMurderGame.Resources.Menu.Back) });

        public MainMenuStateMachine()
        {
            State(Main);
        }

        protected override void OnStart()
        {
            Entity.SetCustomDraw(DrawMainMenu);

            _menuInfo.Select(Game.Data.CanLoadSaveData(0) ? 0 : 1);
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
                            Guid? targetWorld = MurderSaveServices.LoadSaveAndFetchTargetWorld(0);
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
                            float volume = Game.Preferences.SetAllVolume(Game.Preferences.AllVolume == 1 ? 0 : 1);

                            _menuInfo.Options[0] = volume == 1 ? new(HelloMurderGame.Resources.Menu.SoundsOn) : new(HelloMurderGame.Resources.Menu.SoundsOff);
                            break;

                        case 1: // Tweak music
                            float sound = Game.Preferences.SetMusicVolume(Game.Preferences.MusicVolume == 1 ? 0 : 1);

                            _menuInfo.Options[1] = sound == 1 ? new(HelloMurderGame.Resources.Menu.MusicOn) : new(HelloMurderGame.Resources.Menu.MusicOff);
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
            // only english right now.
            // Game.Data.ChangeLanguage(Languages.Next(Game.Preferences.Language));
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
