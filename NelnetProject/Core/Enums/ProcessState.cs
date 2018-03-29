using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public enum ProcessState
    {
        NOT_YET_CHARGED=1,
        SUCCESSFUL,
        RETRYING,
        FAILED,
        DEFERRED //todo
    }
}
