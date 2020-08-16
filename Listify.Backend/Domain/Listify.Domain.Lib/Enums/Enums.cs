using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Enums
{
    public enum TransactionType
    {
        Request,
        Upvote,
        Downvote,
        PollingCurrency,
        AwardCurrency
    }

    public enum EndpointType
    {

    }

    public enum WagerType
    {
        Upvote,
        Downvote,
    }

    public enum PollingEventType
    {
        CurrencyPoll,
        PingPoll
    }
}
