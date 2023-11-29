using Microsoft.Windows.ApplicationModel.Resources;

namespace EPCalipersWinUI3.Helpers
{
    // Note this is brazenly stolen from the Template Studio helper.
    public static class ResourceExtensions
    {
        private static readonly ResourceLoader _resourceLoader = new();

        public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);
    }
}
