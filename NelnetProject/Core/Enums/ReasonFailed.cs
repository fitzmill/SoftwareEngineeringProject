using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public enum ReasonFailed
    {
        CARD_EXPIRED=1,
        FRAUD_ALERT,
        INSUFFICIENT_FUNDS,
        CARD_DECLINED
    }
}
