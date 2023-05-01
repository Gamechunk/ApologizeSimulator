using ProcessSystem;
using UnityEngine;

public class AnimatorProcessor : MonoBehaviour, IProcess
{
    [field:SerializeField] public bool Activated { get; set; } = true;

    private Animator _animator;
    public int WALK_BLEND_HASH { get; private set; }
    public int STUMBLED_BLEND_HASH { get; private set; }
    public int SLAP_IN_FACE_HASH { get; private set; }
    public int DRINK_BOOSTER_HASH { get; private set; }
    public int STUMBLED_HASH { get; private set; }


    public void AwakeMe()
    {
        _animator = GetComponent<Animator>();
        WALK_BLEND_HASH = Animator.StringToHash("WalkBlend");
        STUMBLED_BLEND_HASH = Animator.StringToHash("StumbledBlend");
        SLAP_IN_FACE_HASH = Animator.StringToHash("SlapInFace");
        DRINK_BOOSTER_HASH = Animator.StringToHash("DrinkBooster");
        STUMBLED_HASH = Animator.StringToHash("Stumbled");
    }

    public void StartMe()
    {

    }

    public void UpdateMe()
    {

    }

    public void DestroyMe()
    {

    }


    public void SetTrigger(int animationHashID)
    {
        _animator.SetTrigger(animationHashID);
    }

    public void SetBlend(int animationHashID, float value)
    {
        _animator.SetFloat(animationHashID, value);
    }
}
