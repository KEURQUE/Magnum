using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Services
{
  public interface IProgressBarService
  {
    bool Progress(bool On, uint current, uint total);
  }
}
