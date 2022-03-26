/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2021.                                   *
 *                                                                            *
 * Use subject to the terms of the Apache License 2.0 available at            *
 * http://www.apache.org/licenses/LICENSE-2.0, or another agreement           *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using Leap.Unity;
using Leap.Unity.Attachments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Attachments
{

    public class AttachmentHandEnableDisable : MonoBehaviour
    {

        public AttachmentHand attachmentHand;
        public AudioSource startSound;
        public AudioSource stopSound;
        public AudioSource answerSound;

        void Update()
        {
            // Deactivation trigger
            if (!attachmentHand.isTracked && attachmentHand.gameObject.activeSelf)
            {
                attachmentHand.gameObject.SetActive(false);
                Debug.Log("deActivating hand");
                stopSound.Play();
                answerSound.Play();
            }

            // Reactivation trigger
            if (attachmentHand.isTracked && !attachmentHand.gameObject.activeSelf)
            {
                attachmentHand.gameObject.SetActive(true);
                Debug.Log("Activating hand");
                startSound.Play();
            }
        }

    }

}