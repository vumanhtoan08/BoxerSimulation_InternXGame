// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Spine.Unity;
// using UnityEngine;

// namespace KJam
// {
//     [RequireComponent(typeof(SkeletonGraphic))]
//     public class SequenceSkeletonGraphic : MonoBehaviour
//     {
//         private SkeletonGraphic skeletonGraphic;
//         public SequenceInfo[] sequenceInfos;
//         [SpineAnimation]
//         public string[] anims;
//         private bool isRunning;
//         // public bool isRunSquence;
//         private void Awake()
//         {
//             skeletonGraphic = GetComponent<SkeletonGraphic>();
//         }
//         private IEnumerator IERun()
//         {
//             isRunning = true;
//             float totalTime = 0;
//             for (int i = 0; i < sequenceInfos.Length; i++)
//             {
//                 var state = skeletonGraphic.SkeletonData.FindAnimation(sequenceInfos[i].anim);
//                 totalTime += state.Duration + sequenceInfos[i].delay;
//                 skeletonGraphic.AnimationState.AddAnimation(0, state, true, sequenceInfos[i].delay);
//             }
//             yield return new WaitForSeconds(totalTime);
//             StartCoroutine(IERun());
//         }
//         private void OnEnable()
//         {
//             if (isRunning) return;
//             StartCoroutine(IERun());
//         }
//         private void OnDisable()
//         {
//             StopAllCoroutines();
//             isRunning = false;
//         }
//         public float RunSequence(int index, float delay)
//         {
//             if (!gameObject.activeInHierarchy) return 0;
//             var state = skeletonGraphic.SkeletonData.FindAnimation(anims[index % anims.Length]);
//             skeletonGraphic.AnimationState.AddAnimation(0, state, false, delay);
//             float dur = state.Duration;
//             if (dur > 2f)
//             {
//                 dur = 2f;
//                 skeletonGraphic.AnimationState.AddEmptyAnimation(0, 0, delay + dur);
//             }
//             return dur;
//         }
//         [Serializable]
//         public struct SequenceInfo
//         {
//             [SpineAnimation] public string anim;
//             public float delay;
//         }
//     }
// }
