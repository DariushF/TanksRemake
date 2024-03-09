using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    /// <summary>
    /// used to make sync of transform work without going trough server
    /// </summary>
    public class ClientNetworkTransform : NetworkTransform
    {
        /// <summary>
        /// this trusts client and makes transform not server authoritive
        /// only use if it does not impose security threats
        /// </summary>
        /// <returns></returns>
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
