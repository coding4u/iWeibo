﻿using System;
using System.Runtime.Serialization;

namespace TencentWeiboSDK.Hammock.Caching
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public enum CacheMode
    {
#if !SILVERLIGHT && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch
        [EnumMember] NoExpiration,
        [EnumMember] AbsoluteExpiration,
        [EnumMember] SlidingExpiration
#else
        NoExpiration,
        AbsoluteExpiration,
        SlidingExpiration
#endif
    }
}