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
        AwardCurrency,
        Wager
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

    public enum SongRequestType
    {
        Queued,
        Playlist
    }

    public enum ServerStateType
    {
        Stopped,
        Playing
    }

    public enum PurchasableItemType
    {
        Playlist,
        PlyalistSongs,
        PurchaseCurrency
    }

    public enum PurchaseMethod
    {
        Paypal
    }

    public enum AuthToLockedRoomResponseType
    {
        Success,
        Fail
    }

    public enum ConnectionType
    {
        ListifyHub,
        RoomHub
    }

    public enum EndpointType
    {

    }

}
