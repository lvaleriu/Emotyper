﻿// Copyright 2013 Ronald Schlenker and Andre Krämer.
// 
// This file is part of GraphIT.
// 
// GraphIT is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// GraphIT is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GraphIT.  If not, see <http://www.gnu.org/licenses/>.
// 
// 

using System;
using System.Windows.Media;
using Helper;
using TechNewLogic.GraphIT.Hv;
using TechNewLogic.GraphIT.Hv.Legend;
using TechNewLogic.GraphIT.Hv.Vertical;

namespace Rulers
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			Loaded += (s, e) => new InfoControl(this).Show();
			SplashScreenHelper.Show();
			InitializeComponent();
			SplashScreenHelper.Close();

			// Set the bounds of the time axis
			CurveDisplay.TimeDoublePlottingSystem.TimeAxis.SetBounds(
				DateTime.UtcNow.AddMinutes(-50),
				DateTime.UtcNow.AddMinutes(10));

			// Initialize the table legend and add it to the page
			var tableLegend = new TableLegend(
				CurveDisplay,
				"Term",
				"Location",
				"Aggregate",
				"Medium",
				"Measurement");
			tableLegendHolder.Content = tableLegend;

			CreateCurveWithGap();

			// Now, we add some rulers
			CurveDisplay.TimeDoublePlottingSystem.AddStaticRuler(
				DateTime.UtcNow.AddMinutes(-30));
			CurveDisplay.TimeDoublePlottingSystem.AddStaticRuler(
				DateTime.UtcNow.AddMinutes(-15));
			CurveDisplay.TimeDoublePlottingSystem.AddStaticRuler(
				DateTime.UtcNow.AddMinutes(-5));
		}

		private void CreateCurveWithGap()
		{
			// Create a new curve
			var curve = CurveDisplay.TimeDoublePlottingSystem.AddCurve(
				"kg", -20, 20, Colors.Yellow, RedrawTime.Ms500, AxisMatchingMode.None, 
				CurveDrawingMode.SimpleLine(),
				new FloatingCommaValueFormater(),
				new InterpolationValueFetchStrategy(),
				AxisFormat.Double,
				int.MaxValue);
			// We have to set some description for the curve so that it can be displayed in the table legend!
			curve.Description.DescriptionText1 = "R2033";
			curve.Description.DescriptionText2 = "House 20";
			curve.Description.DescriptionText3 = "BHKW";
			curve.Description.DescriptionText4 = "Cur. Val.";
			curve.Description.DescriptionText5 = "Power Calc.";

			// Create some data and add it to the curve
			var startTime = DateTime.UtcNow.AddMinutes(-40);
			var timeSpan = TimeSpan.FromMinutes(20);
			var data = EntryGenerator.GenerateRandomData(
				startTime,
				timeSpan,
				5000,
				15,
				-15,
				0.2);
			curve.DataSeries.Append(data);

			// Not, we append a gap
			var latestTime = startTime.Add(timeSpan);
			var undefinedValue = curve.DataSeries.UndefinedValue;
			var gapEnd = latestTime.AddMinutes(10);
			curve.DataSeries.Append(
				new[]
					{
						new TimeDoubleDataEntry(latestTime.AddMilliseconds(1), undefinedValue),
						new TimeDoubleDataEntry(gapEnd, curve.DataSeries.UndefinedValue),
					});

			// Create some additional data after the gap
			var additionalData = EntryGenerator.GenerateRandomData(
				gapEnd.AddMilliseconds(1),
				TimeSpan.FromMinutes(10),
				5000,
				15,
				-15,
				0.2);
			curve.DataSeries.Append(additionalData);
		}
	}
}