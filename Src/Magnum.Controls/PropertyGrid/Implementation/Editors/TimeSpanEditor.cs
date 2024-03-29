﻿/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/

using Magnum.Controls.PropertyGrid.Converters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Magnum.Controls.PropertyGrid.Editors
{
  public class TimeSpanEditor : DateTimeUpDownEditor
  {
    protected override void SetControlProperties()
    {
      base.SetControlProperties();
      Editor.Format = DateTimeFormat.LongTime;
    }

    protected override IValueConverter CreateValueConverter()
    {
      return new EditorTimeSpanConverter();
    }
  }
}
