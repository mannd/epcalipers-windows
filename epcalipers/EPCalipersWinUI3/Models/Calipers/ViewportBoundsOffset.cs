using Windows.Foundation;

namespace EPCalipersWinUI3.Models.Calipers
{
    public struct ViewportBoundsOffset
    {
        public ViewportBoundsOffset(Bounds bounds, Point offset)
        {
            Bounds = bounds;
            Offset = offset;
        }
        public Bounds Bounds { get; set; }
        public Point Offset { get; set; }
    }
}
