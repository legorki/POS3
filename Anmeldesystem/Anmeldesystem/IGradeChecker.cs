using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anmeldesystem
{
    internal interface IGradeChecker
    {
        public bool CanBeAccepted(IReadOnlyDictionary<string, int> grades);
    }
}
