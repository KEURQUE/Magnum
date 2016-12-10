using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Utils
{
  public static class StringUtils
  {
    public static bool SimpleMatch(string str, string filterText)
    {
      int index = str.IndexOf(filterText, 0, StringComparison.InvariantCultureIgnoreCase);

      return index > -1;
    }
    public static bool CaseMatch(string str, string filterText)
    {
      int index = str.IndexOf(filterText, 0, StringComparison.CurrentCulture);

      return index > -1;
    }
  }
}
