using HelloMurder.Assets;
using Murder;

namespace HelloMurder.Services;

public static class LibraryServices
{
    public static LibraryAsset GetRoadLibrary()
    {
        return Game.Data.GetAsset<LibraryAsset>(HelloMurderGame.Profile.Library);
    }

    public static UiLocalizationResourcesAsset GetUiLocalizationResources()
    {
        return Game.Data.GetAsset<UiLocalizationResourcesAsset>(GetRoadLibrary().UiLocalizationResources);
    }
}
