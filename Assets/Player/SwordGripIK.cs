using UnityEngine;

public class SwordGripIK : MonoBehaviour
{
    public Animator animator;
    public Transform rightHandTarget;
    public Transform leftHandTarget;

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);

            // 讓左腳維持在 Vector3(腳的正確世界座標)，你可以用 transform.position 或地面高度 + 偏移
            Vector3 leftFootPos = animator.transform.position + Vector3.forward * 0.1f;  // 或直接 transform.position
            leftFootPos.y = 0f; // 地板高度

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.identity);

            // 同理右腳
            Vector3 rightFootPos = animator.transform.position + Vector3.right * 0.1f;
            rightFootPos.y = 0f;

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.identity);


        }
    }
}
