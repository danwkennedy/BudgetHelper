using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace PieChartUserControl
{
    public sealed class PieChartControl : Control
    {
        public PieChartControl()
        {
            this.DefaultStyleKey = typeof(PieChartControl);

            Canvas canvas = this.FindName("Canvas") as Canvas;




        }
    }
}
