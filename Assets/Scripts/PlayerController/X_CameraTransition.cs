using System.Collections;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] float speedMoveSit;
    [SerializeField] float speedRotateSit;

    [SerializeField] Transform tempPos;
    [SerializeField] Transform oldPos;
    [SerializeField] Transform birdViewCamera;
    [SerializeField] Transform _basicCamera;

    bool _playingCards = false;
    bool _isBirdView = false, _isBasicView = true;

    //Sits player down and makes lerp to look at pos(NPC), and sits ad sitPos
    public void UpdateLookAt(Transform pos, float durationOfLerp, GameObject sitPos)
    {
        _playingCards = true;
        StartCoroutine(LookAtLerp(pos, durationOfLerp, sitPos));
    }

    //Implements sitting with all the lerp functions
    private IEnumerator LookAtLerp(Transform pos, float durationOfLerp, GameObject sitPos)
    {
        /*
        transform.parent.transform.LookAt(pos);
        Vector3 eulerRotation = transform.parent.transform.rotation.eulerAngles;
        transform.parent.transform.rotation = Quaternion.Euler(0, eulerRotation.y,0 );
        transform.LookAt(pos);
        */
        Quaternion lookRotationParent = Quaternion.LookRotation(pos.position - transform.parent.transform.position);
        tempPos.position = pos.position + new Vector3(-0.3f, 0, 0.3f);         //WEIRD PIVOT HARDCODE
        oldPos.position = sitPos.transform.position + new Vector3(0, 0, 1);
        lookRotationParent.x = 0;
        lookRotationParent.z = 0;
        Debug.Log(pos.position);

        float time = 0;

        Vector3 startPosition = transform.parent.transform.position;
        Vector3 endPosition = sitPos.transform.position + new Vector3(0, 1f, 0) + new Vector3(0.3f, 0, -0.3f);         //WEIRD PIVOT HARDCODE

        while (time < durationOfLerp)
        {
            transform.parent.transform.position = Vector3.Lerp(startPosition, endPosition, time / durationOfLerp);
            transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation, lookRotationParent, time / 50);
            time += Time.deltaTime * speedMoveSit;

            lookRotationParent = Quaternion.LookRotation(tempPos.position - transform.parent.transform.position);
            lookRotationParent.x = 0;
            lookRotationParent.z = 0;
            yield return null;
        }

        //fix it on a correct position after Lerp
        transform.parent.transform.LookAt(tempPos);
        Vector3 eulerRotation = transform.parent.transform.rotation.eulerAngles;
        transform.parent.transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
        //

        time = 0;
        Quaternion lookRotation = Quaternion.LookRotation(tempPos.position - transform.position);
        Vector3 newStartPosition = transform.position;
        Vector3 newEndPosition = transform.position - new Vector3(0, 0.5f, 0);
        while (time < durationOfLerp)
        {
            transform.position = Vector3.Lerp(newStartPosition, newEndPosition, time / durationOfLerp);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * speedRotateSit;

            lookRotation = Quaternion.LookRotation(tempPos.position - transform.position);
            yield return null;
        }

        EnableCursor();

        _basicCamera.position = transform.position;
        Debug.Log("Camera:" + _basicCamera.position);
        yield return null;
    }

    //Stop card game
    public void CancelCardGame(float durationOfLerp)
    {
        _playingCards = false;
        StartCoroutine(SitUp(durationOfLerp));
        DisableCursor();
    }

    //Lerp function that makes player stand up after the card game
    public IEnumerator SitUp(float durationOfLerp)
    {
        float time = 0;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
        Vector3 newStartPosition = transform.position;
        Vector3 newEndPosition = transform.position + new Vector3(0, 0.5f, 0);

        while (time < durationOfLerp)
        {
            transform.position = Vector3.Lerp(newStartPosition, newEndPosition, time / durationOfLerp);
            transform.rotation = Quaternion.Slerp(lookRotation, transform.rotation, time / durationOfLerp);
            time += Time.deltaTime * speedRotateSit;

            lookRotation = Quaternion.LookRotation(tempPos.position - transform.position);
            yield return null;
        }

        Vector3 startPosition = transform.parent.transform.position;
        Vector3 endPosition = oldPos.transform.position + new Vector3(0, 1f, 0);

        Quaternion lookRotationParent = Quaternion.LookRotation(startPosition);
        lookRotationParent.x = 0;
        lookRotationParent.z = 0;

        time = 0;

        while (time < durationOfLerp * 4)
        {
            transform.parent.transform.position = Vector3.Lerp(startPosition, endPosition, time / durationOfLerp);
            transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation, lookRotationParent, time / durationOfLerp);

            lookRotationParent = Quaternion.LookRotation(tempPos.position - transform.parent.transform.position);
            lookRotationParent.x = 0;
            lookRotationParent.z = 0;

            time += Time.deltaTime * speedMoveSit;
            yield return null;
        }
    }

    public void SwitchToBirdCamera()
    {
        StartCoroutine(SwitchToBirdCameraCoroutine());
        _isBasicView = false;
        _isBirdView = true;
    }
    public IEnumerator SwitchToBirdCameraCoroutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = birdViewCamera.position;
        float time = 0;

        while (time < 2)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time / 2);

            time += Time.deltaTime * speedMoveSit;
            yield return null;
        }
        //
        //needs a lerp 
        //
        //Also hardcoded for current chair/table/npc combination
        //As pivot points of object are weird 

        transform.Rotate(56.11f, 0, 0);
    }

    public void SwitchToBasicCamera()
    {
        StartCoroutine(SwitchToBasicCameraCoroutine());
        _isBasicView = true;
        _isBirdView = false;
    }
    public IEnumerator SwitchToBasicCameraCoroutine()
    {

        Vector3 startPosition = transform.position;
        Vector3 endPosition = _basicCamera.position;


        float time = 0;

        while (time < 2)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time / 2);

            time += Time.deltaTime * speedMoveSit;
            yield return null;
        }
        //
        //needs a lerp 
        //
        //Also hardcoded for current chair/table/npc combination
        //As pivot points of object are weird 
        transform.Rotate(-56.11f, 0, 0);

    }

    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsPlayingCards()
    {
        return _playingCards;
    }

    public bool IsBasicView()
    {
        return _isBasicView;
    }
    public bool IsBirdView()
    {
        return _isBirdView;
    }
}
