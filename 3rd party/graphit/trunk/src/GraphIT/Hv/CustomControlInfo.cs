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
using System;
using System.Windows;

namespace TechNewLogic.GraphIT.Hv
{
	class CustomControlInfo
	{
		public CustomControlInfo(
			UIElement control,
			Func<double> getTop,
			Func<double> getLeft)
		{
			_getTop = getTop;
			_getLeft = getLeft;
			Control = control;
		}

		private readonly Func<double> _getTop;
		private readonly Func<double> _getLeft;

		public Action RefreshRequest { get; set; }
		public UIElement Control { get; private set; }
		public double Top { get { return _getTop(); } }
		public double Left { get { return _getLeft(); } }

		public void RaiseRefreshRequest()
		{
			if (RefreshRequest != null)
				RefreshRequest();
		}
	}
}