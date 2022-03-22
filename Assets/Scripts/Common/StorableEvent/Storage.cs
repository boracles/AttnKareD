using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG_Cus_AddOn
{
    public class Storage : MonoBehaviour
    {
        [Header("Hand Side")]
        public RemoteController HandSide = RemoteController.Right;

        [Header("Grab Settings")]
        public HoldType DefaultHoldType = HoldType.HoldDown;

        [Header("Hold / Release")]

        /// <summary>
        /// 0-1 �׸��� �󸶳� ������� ����
        /// ex) �׸��� 3/4�� ������ 0.75
        /// </summary>
        [Tooltip("0-1 determine how much to consider a grip. Example : 0.75 is holding the grip down 3/4 of the way.")]
        [Range(0.0f, 1f)]
        public float R_GripAmount = 0.9f;

        /// <summary>
        /// Release �����Ǵ� �׸��� ����(0-1)
        /// </summary>
        [Tooltip("How much grip considered to release an object (0-1). Example : 0.75 is holding the grip down 3/4 of the way")]
        [Range(0.0f, 1f)]
        public float Release_R_GripAmount = 0.5f;

        /// <summary>
        /// �׸��� ������ �ִ� ���� �׷� �Է��� Ȯ���ϴ� �� �ɸ��� �ð�(��)
        /// </summary>
        [Tooltip("How many seconds to check for grab input while Grip is held down. After grip is held down for this long, grip will need to be repressed in order to pick up an object.")]
        public float GrabCheckSeconds = 0.5f;
        float current_R_GrabTime;
    }
}


