﻿using System;
using System.Linq;
using Mapsui.UI.iOS;
using UIKit;

namespace Mapsui.Samples.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var mapControl = new MapControl(View.Bounds)
            {
                Map = Common.AllSamples.CreateList().First(s => s.Key == "Various Layers").Value()
            };
            View.AddSubview(mapControl);
        }
    }
}