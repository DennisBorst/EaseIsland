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
    [SerializeField] private Rigidbody cubeRb;
    [SerializeField] private FirstTimeEvent firstTimeCounter;
    [SerializeField] private GameObject tutorialCloud;

    [Header("Preferences")]
    private float innerDisVisualCircle;
    private float maxDisVisualCircle;
    private float maxDisToTargetDot;
    private float closeToTargetRadius;
    private float minSizePlayerDot;
    private float maxSizePlayerDot;
    private float sizeInInnerCircle;
    [SerializeField] private float playerSpeedMultipler;

    private Vector3 targetPos;
    private float disToTarget;
    private float percentage;
    private bool hotSpot;
    private Vector3 moveDirection;

    private Vector3 playerPos;
    private Vector3 playerStartPos = new Vector3(0,0,0);

    public void LoadInFish(FishStats fishStats)
    {
        fish = fishStats.item;
        playerDotSpriteHotspot = fish.inventoryImg;
        colorPar = fishStats.color;

        innerDisVisualCircle = fishStats.innerDisVisualCircle;
        maxDisVisualCircle = fishStats.maxDisVisualCircle;
        maxDisToTargetDot = fishStats.maxDisToTargetDot;
        closeToTargetRadius = fishStats.closeToTargetRadius;
        minSizePlayerDot = fishStats.minSizePlayerDot;
        maxSizePlayerDot = fishStats.maxSizePlayerDot;
        sizeInInnerCircle = fishStats.sizeInInnerCircle;

        playerPos = new Vector3(0, 0, 0);
        PlayerAnimation.Instance.PlayAnimCount(8);
        StartCoroutine(waitForFishString());
        CreateDotOnCircle();

        FMODUnity.RuntimeManager.PlayOneShot("event:/Activities/Fishing Dopper Hits Water", transform.position);

        if (!firstTimeCounter.fishing && tutorialCloud != null) { tutorialCloud.SetActive(true); }
    }

    public void TryToCatch()
    {
        PlayerAnimation.Instance.PlayAnimCount(10);

        if (hotSpot)
        {
            InventoryManager.Instance.AddToInvWithAnim(fish);
            Instantiate(catchPar, this.transform.position, Quaternion.identity);
            firstTimeCounter.fishing = true;
        }
        else
        {
            Debug.Log("Missed");
        }
        
        Destroy(this.gameObject);
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

        playerPos += new Vector3(horizontal, vertical, 0f) * playerSpeedMultipler * Time.deltaTime;

        if (Vector3.Distance(playerPos, playerStartPos) > maxDisVisualCircle)
        {
            Vector3 offset = playerPos - playerStartPos;
            playerPos = Vector3.ClampMagnitude(offset, maxDisVisualCircle);
        }

        playerDotVisual.transform.localPosition = playerPos;
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
    }

    private Vector3 GetTargetDotLocation()
    {
        float randomX = Random.Range(-maxDisCircleVisual, maxDisCircleVisual);
        float randomY = Random.Range(-maxDisCircleVisual, maxDisCircleVisual);
        Vector3 v3 = new Vector3(randomX, randomY, 0.004f);
        return v3;
    }

    private IEnumerator waitForFishString()
    {
        ItemInHand.Instance.itemObj.GetComponent<FishingRod>().StartFishing(playerDotImg.gameObject);
        yield return new WaitForSeconds(0.8f);
        ItemInHand.Instance.itemObj.GetComponent<FishingRod>().StartUpdateRod();
    }
}
