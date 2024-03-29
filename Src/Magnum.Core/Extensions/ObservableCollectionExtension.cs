﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Extensions
{
  public static class ObservableCollectionExtension
  {
      public static void AddRange<T>(this ObservableCollection<T> oc, IEnumerable<T> collection)
      {
        if (collection == null)
        {
          throw new ArgumentNullException("collection");
        }
        foreach (var item in collection)
        {
          oc.Add(item);
        }
    }
  }
}
