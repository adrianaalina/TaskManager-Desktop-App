using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TaskManagerWPF.Styles
{
    public static class WatermarkService
    {
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(string),
                typeof(WatermarkService),
                new PropertyMetadata(string.Empty, OnWatermarkChanged));

        public static string GetWatermark(DependencyObject obj)
            => (string)obj.GetValue(WatermarkProperty);

        public static void SetWatermark(DependencyObject obj, string value)
            => obj.SetValue(WatermarkProperty, value);

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                tb.Loaded += (s, _) => AddAdorner(tb);
                tb.TextChanged += (s, _) => UpdateAdorner(tb);
            }
        }

        private static void AddAdorner(TextBox tb)
        {
            var layer = AdornerLayer.GetAdornerLayer(tb);
            if (layer == null) return;

            layer.Add(new WatermarkAdorner(tb));
        }

        private static void UpdateAdorner(TextBox tb)
        {
            var layer = AdornerLayer.GetAdornerLayer(tb);
            if (layer == null) return;

            var adorners = layer.GetAdorners(tb);
            if (adorners == null) return;

            foreach (var adorner in adorners)
                adorner.Visibility = string.IsNullOrEmpty(tb.Text) ? Visibility.Visible : Visibility.Hidden;
        }
    }

    public class WatermarkAdorner : Adorner
    {
        private readonly TextBox _tb;

        public WatermarkAdorner(TextBox adornedElement) : base(adornedElement)
        {
            _tb = adornedElement;
            IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (!string.IsNullOrEmpty(_tb.Text)) return;

            var watermark = WatermarkService.GetWatermark(_tb);
            if (string.IsNullOrEmpty(watermark)) return;

            var formatted = new FormattedText(
                watermark,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                14,
                Brushes.Gray,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            dc.DrawText(formatted, new Point(6, 4));
        }
    }
}