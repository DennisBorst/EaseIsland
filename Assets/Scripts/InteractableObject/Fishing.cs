using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] private float maxDisCircleVisual;
    [SerializeField] private GameObject playerDotVisual;
    [SerializeField] private GameObject targetDot;
    [SerializeField] private Image playerDotImg;
    [SerializeField] private GameObject catchPar;
    [SerializeField] private GameObject itemAnim;
    [SerializeField] private Color32 colorPar;
    [Space]
    [SerializeField] private Item fish;
    [Space]
    [SerializeField] private Color playerDotColorNormal;
    [SerializeField] private Color playerDotColorHotspot;
    [SerializeField] private Sprite playerDotSpriteNormal;
    [SerializeField] private Sprite playerDotSpriteHotspot;

    [Header("Preferences")]
    [SerializeField] private float innerDisVisualCircle;
    [SerializeField] private float maxDisVisualCircle;
    [SerializeField] private float maxDisToTargetDot;
    [SerializeField] private float closeToTargetRadius;
    [SerializeField] private float minSizePlayerDot;
    [SerializeField] private float maxSizePlayerDot;
    [SerializeField] private float sizeInInnerCircle;
    [SerializeField] private float playerSpeedMultipler;

    private Vector3 targetPos;
    private float disToTarget;
    private float percentage;
    private bool hotSpot;
    private Vector3 moveDirection;

    private Vector3 playerPos;
    private Vector3 playerStartPos = new Vector3(0,0,0);

    public void TryToCatch()
    {
        if (hotSpot)
        {
            Debug.Log("Catched Fish");
            Instantiate(itemAnim, transform.position, Quaternion.identity, CharacterMovement.Instance.transform).GetComponent<GainItem>().ChangeItem(fish, colorPar);
            Instantiate(catchPar, this.transform.position, Quaternion.identity);
            //InventoryManager.Instance.AddToInv(fish);
        }
        else
        {
            Debug.Log("Missed");
        }

        Destroy(this.gameObject);
    }

    private void Start()
    {
        playerPos = new Vector3(0, 0, 0);
        CreateDotOnCircle();
    }

    private void Update()
    {
        if (Vector3.Distance(playerStartPos, targetPos) < innerDisVisualCircle)
        {
            CreateDotOnCircle();
        }

        PlayerPosition();
        CheckDisToTargetDots();
    }

    private void PlayerPosition()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraRot = new Vector3(0f, 0f, 0f);
        cameraRot.y = Camera.main.transform.eulerAngles.y;
        this.transform.eulerAngles = cameraRot;
        /*
        Vector3 playerInput = Camera.main.transform.TransformDirection(new Vector3(horizontal, vertical, 0f));
        float translateHor = (horizontal * Mathf.Cos(Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad)) - (vertical * Mathf.Sin(Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad));
        Debug.Log("hor: " + translateHor);
        float translateVer = (horizontal * Mathf.Sin(Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad)) + (vertical * Mathf.Cos(Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad));
        Debug.Log("ver: " + translateVer);
        Vector3 currentVector = new Vector3(horizontal, vertical, 0f);
        Debug.DrawLine(transform.position, transform.position + (cameraRot * 5f), Color.red);
        Vector3 forward = Vector3.Cross(transform.position, Camera.main.transform.position);
        Vector3 cameraRot = transform.position + (Camera.main.transform.forward);
        cameraRot.z = 0f;
        Vector3 playerInput = new Vector3(horizontal, vertical, 0f) * playerSpeedMultipler;
        cameraRot += playerInput;
        playerDotVisual.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        */
        playerPos += new Vector3(horizontal, vertical, 0f) * playerSpeedMultipler;

        if (Vector3.Distance(playerPos, playerStartPos) > maxDisVisualCircle)
        {
            Vector3 offset = playerPos - playerStartPos;
            playerPos = Vector3.ClampMagnitude(offset, maxDisVisualCircle);
        }

        playerDotVisual.transform.localPosition = playerPos;

        //playerDotVisual.Move(moveDirection * playerSpeedMultipler * Time.deltaTime);
        //playerDotVisual.transform.position += (cameraRot - playerDotVisual.transform.position).normalized * playerSpeedMultipler * Time.deltaTime;
    }

    private void CheckDisToTargetDots()
    {
        disToTarget = Vector3.Distance(targetPos, playerDotVisual.transform.localPosition);
        if (disToTarget < closeToTargetRadius)
        {
            //Player dot is in inner circle
            if (disToTarget < maxDisToTargetDot)
            {
                //InputBridge.Instance.VibrateController(m_vibrateInnerCircle, m_vibrateInnerCircle, 0.1f, m_grabbable.heldByGrabbers[0].HandSide);
                playerDotVisual.transform.localScale = new Vector3(sizeInInnerCircle, sizeInInnerCircle, 0);
                playerDotImg.sprite = playerDotSpriteHotspot;

                if (!hotSpot)
                {
                    hotSpot = true;
                    //if (m_hoverHotspot != null) { VRUtils.Instance.PlaySpatialClipAt(m_hoverHotspot, transform.position, 0.75f); }
                }
                return;
            }
            else
            {
                hotSpot = false;
                playerDotImg.sprite = playerDotSpriteNormal;
            }


            percentage = (disToTarget / closeToTargetRadius) * -1 + 1;

            //Changes the size of the dot
            float sizeDif = minSizePlayerDot + ((maxSizePlayerDot - minSizePlayerDot) * percentage);
            playerDotVisual.transform.localScale = new Vector3(sizeDif, sizeDif, 0);

            //Impacts how much the controller vibrates
            //float vibrate = (Mathf.Pow(2, m_percentage) - 1) * 0.15f;
            //InputBridge.Instance.VibrateController(vibrate, vibrate, 0.1f, m_grabbable.heldByGrabbers[0].HandSide);
        }
    }


    private void CreateDotOnCircle()
    {
        targetPos = GetTargetDotLocation();
        targetDot.transform.localPosition = targetPos;
        Debug.Log(targetPos);
    }

    private Vector3 GetTargetDotLocation()
    {
        float randomX = Random.Range(-maxDisCircleVisual, maxDisCircleVisual);
        float randomY = Random.Range(-maxDisCircleVisual, maxDisCircleVisual);
        Vector3 v3 = new Vector3(randomX, randomY, 0.004f);
        return v3;
    }
}
