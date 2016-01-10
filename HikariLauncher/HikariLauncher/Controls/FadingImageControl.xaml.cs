using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HikariLauncher.Controls
{
    public partial class FadingImageControl
    {
        #region Properties & Fields

        private Viewbox _currentVisible;
        private Viewbox _currentInvisible;

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource",
            typeof(ImageSource), typeof(FadingImageControl), new PropertyMetadata(ImageSourceChanged));

        public ImageSource ImageSource
        {
            get { return GetValue(ImageSourceProperty) as ImageSource; }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty FadeDurationProperty = DependencyProperty.Register("FadeDuration",
            typeof(TimeSpan), typeof(FadingImageControl), new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 500)));

        public TimeSpan FadeDuration
        {
            get { return GetValue(FadeDurationProperty) as TimeSpan? ?? default(TimeSpan); }
            set { SetValue(FadeDurationProperty, value); }
        }

        #endregion

        #region Constructors

        public FadingImageControl()
        {
            InitializeComponent();
            _currentVisible = img1;
            _currentInvisible = img2;
        }

        #endregion

        #region Methods

        private static void ImageSourceChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            FadingImageControl instance = dependencyObject as FadingImageControl;
            instance?.ImageSourceChanged(dependencyPropertyChangedEventArgs);
        }

        private void ImageSourceChanged(DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((Image)_currentInvisible.Child).Source = ImageSource;
            FadeImage(_currentInvisible, FadeDuration, 1.0);
            FadeImageInstant(_currentVisible, FadeDuration, 0.0);

            SetZIndex(_currentInvisible, FadeDuration, 1);
            SetZIndex(_currentVisible, FadeDuration, 2);

            Viewbox tmp = _currentInvisible;
            _currentInvisible = _currentVisible;
            _currentVisible = tmp;
        }

        private void FadeImage(IAnimatable image, TimeSpan duration, double toValue)
        {
            DoubleAnimation animation = new DoubleAnimation(toValue, duration);
            image.BeginAnimation(OpacityProperty, animation);
        }

        private void FadeImageInstant(IAnimatable image, TimeSpan beginTime, double toValue)
        {
            DoubleAnimation animation = new DoubleAnimation(toValue, new Duration(new TimeSpan(0)))
            {
                BeginTime = beginTime
            };
            image.BeginAnimation(OpacityProperty, animation);
        }

        private void SetZIndex(IAnimatable image, TimeSpan beginTime, int value)
        {
            Int32Animation animation = new Int32Animation(value, new Duration(new TimeSpan(0))) { BeginTime = beginTime };
            image.BeginAnimation(Panel.ZIndexProperty, animation);
        }

        #endregion
    }
}
