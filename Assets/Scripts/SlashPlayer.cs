using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlashPlayer : MonoBehaviour
{
    [Header("斬撃オブジェクト")]
    public GameObject slashObject;
    [Header("斬撃時間")]
    public float slashTime;

    private InputAction slashAction;
    private Coroutine slashStart;

    void Start()
    {
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        slashAction = actionMap.FindAction("Crouch");

        slashObject.SetActive(false);
    }

    void Update()
    {
        if(slashAction.WasPerformedThisFrame())
        {
            Slash();
        }
    }

    void Slash()
    {
        if (slashStart == null)
        {
            slashObject.SetActive(true);

            slashStart = StartCoroutine(SlashStart());
        }
    }

    IEnumerator SlashStart()
    {
        yield return new WaitForSeconds(slashTime);
        slashObject.SetActive(false);
        slashStart = null;
    }
}
