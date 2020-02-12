using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Base16.Svg.BuildTasks.Examples.Controls
{
	public class SvgPresenter : Border
	{
		public static readonly DependencyProperty SvgVisualTypeProperty = DependencyProperty.Register(
			nameof(SvgVisualType),
			typeof(Type),
			typeof(SvgPresenter), new PropertyMetadata(
				default(String),
				(s, e) => ((SvgPresenter)s).SelectVisual(e.NewValue as Type)
			)
		);

		public Type SvgVisualType
		{
			get => (Type)GetValue(SvgVisualTypeProperty);
			set => SetValue(SvgVisualTypeProperty, value);
		}

		private readonly VisualBrush _visualBrush = new VisualBrush
		{
			Stretch = Stretch.Uniform
		};

		public override void BeginInit()
		{
			base.BeginInit();

			Child = new Grid
			{
				Margin = new Thickness(0),
				Background = _visualBrush,
			};
		}

		private void SelectVisual(Type visualType)
		{
			if(visualType == null)
			{
				_visualBrush.Visual = null;
				ToolTip = null;
				return;
			}

			if(!typeof(DrawingVisual).IsAssignableFrom(visualType))
				throw new NotSupportedException($"Type {visualType.Name} is not supported in {nameof(SvgPresenter)}");

			_visualBrush.Visual = (Visual)Activator.CreateInstance(visualType);
			ToolTip = visualType.Name;
		}
	}
}
