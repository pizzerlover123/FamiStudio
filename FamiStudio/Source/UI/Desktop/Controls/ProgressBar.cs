using System.Diagnostics;

namespace FamiStudio
{
    public class ProgressBar : Control
    {
        private float progress;
        private float visibleProgress;

        public ProgressBar()
        {
            height = DpiScaling.ScaleForMainWindow(10);
        }

        public float Progress
        {
            get { return progress; }
            set
            {
                var clampedValue = Utils.Clamp(value, 0.0f, 1.0f);
                if (clampedValue != progress)
                {
                    // Dont do animation when going backwards.
                    if (clampedValue < progress)
                        visibleProgress = clampedValue;
                    progress = clampedValue;
                    MarkDirty();
                }
            }
        }

        public override void Tick(float delta)
        {
            var newVisibleProgress = Utils.Lerp(visibleProgress, progress, 0.1f);
            SetAndMarkDirty(ref visibleProgress, newVisibleProgress);
        }

        protected override void OnRender(Graphics g)
        {
            Debug.Assert(enabled); // TODO : Add support for disabled state.

            var c = parentDialog.CommandList;

            c.FillAndDrawRectangle(0, 0, width - 1, height - 1, ThemeResources.DarkGreyLineBrush1, ThemeResources.LightGreyFillBrush1);
            if (visibleProgress > 0.0f)
                c.FillAndDrawRectangle(0, 0, (visibleProgress * width) - 1, height - 1, ThemeResources.MediumGreyFillBrush1, ThemeResources.LightGreyFillBrush1);
        }
    }
}
