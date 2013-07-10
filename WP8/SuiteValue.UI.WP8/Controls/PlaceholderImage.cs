using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SuiteValue.UI.WP8.Controls
{
    public class PlaceholderImage : ContentControl
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(PlaceholderImage), new PropertyMetadata(OnSourceChanged));

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(PlaceholderImage), null);



        public Thickness AnnotationMargin
        {
            get { return (Thickness)GetValue(AnnotationMarginProperty); }
            set { SetValue(AnnotationMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnnotationMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnnotationMarginProperty =
            DependencyProperty.Register("AnnotationMargin", typeof(Thickness), typeof(PlaceholderImage), new PropertyMetadata(new Thickness(0)));



        public ImageSource AnnotationSource
        {
            get { return (ImageSource)GetValue(AnnotationSourceProperty); }
            set { SetValue(AnnotationSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnnotationSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnnotationSourceProperty =
            DependencyProperty.Register("AnnotationSource", typeof(ImageSource), typeof(PlaceholderImage), null);



        public Visibility ShowAnnotation
        {
            get { return (Visibility)GetValue(ShowAnnotationProperty); }
            set { SetValue(ShowAnnotationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAnnotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAnnotationProperty =
            DependencyProperty.Register("ShowAnnotation", typeof(Visibility), typeof(PlaceholderImage), new PropertyMetadata(Visibility.Collapsed));



        public double AnnotationWidth
        {
            get { return (double)GetValue(AnnotationWidthProperty); }
            set { SetValue(AnnotationWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnnotationWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnnotationWidthProperty =
            DependencyProperty.Register("AnnotationWidth", typeof(double), typeof(PlaceholderImage), new PropertyMetadata(0.0));



        public HorizontalAlignment AnnotationHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(AnnotationHorizontalAlignmentProperty); }
            set { SetValue(AnnotationHorizontalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnnotationHorizontalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnnotationHorizontalAlignmentProperty =
            DependencyProperty.Register("AnnotationHorizontalAlignment", typeof(HorizontalAlignment), typeof(PlaceholderImage), new PropertyMetadata(HorizontalAlignment.Right));



        public VerticalAlignment AnnotationVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(AnnotationVerticalAlignmentProperty); }
            set { SetValue(AnnotationVerticalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnnotationVerticalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnnotationVerticalAlignmentProperty =
            DependencyProperty.Register("AnnotationVerticalAlignment", typeof(VerticalAlignment), typeof(PlaceholderImage), new PropertyMetadata(VerticalAlignment.Top));







        public PlaceholderImage()
        {
            DefaultStyleKey = typeof(PlaceholderImage);
        }

        public ImageSource Source
        {
            get
            {
                return (ImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public Stretch Stretch
        {
            get
            {
                return (Stretch)GetValue(StretchProperty);
            }
            set
            {
                SetValue(StretchProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            VisualStateManager.GoToState(this, "Content", false);
        }

        private void OnSourceChanged(ImageSource oldValue, ImageSource newValue)
        {
            
            VisualStateManager.GoToState(this, "Content", false);

            var oldBitmapSource = oldValue as BitmapImage;
            var newBitmapSource = newValue as BitmapImage;
            newBitmapSource.CreateOptions = BitmapCreateOptions.BackgroundCreation;

            if (oldBitmapSource != null)
            {
                oldBitmapSource.ImageOpened -= OnImageOpened;
            }

            if (newBitmapSource != null)
            {
                newBitmapSource.ImageOpened += OnImageOpened;
            }
        }

        private void OnImageOpened(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this, "Image", true);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var placeholderImage = (PlaceholderImage)d;
            placeholderImage.OnSourceChanged((ImageSource)e.OldValue, (ImageSource)e.NewValue);
        }
    }
}
