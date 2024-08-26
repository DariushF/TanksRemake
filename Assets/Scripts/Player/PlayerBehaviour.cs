using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public MeshRenderer lowerPartRenderer;

        public Color ownPlayerColor;
        public Color enemyPlayerColor;


        void Start()
        {
            if (IsOwner)
                lowerPartRenderer.material.color = ownPlayerColor;
            else
                lowerPartRenderer.material.color = enemyPlayerColor;
        }
    }
}
