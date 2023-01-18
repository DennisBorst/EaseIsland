using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public enum DialogueState
    {
        Dialogue,
        Options,
        None
    }

    [SerializeField] private Animator canvasAnim;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [Space]
    [SerializeField] private KeyCode nextTextButton;

    [Header("Options")]
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private List<OptionButton> optionButtons = new List<OptionButton>();

    private Queue<string> sentences;
    private NPC currentNPC;
    private Dialogue currentDialogue;
    private Coroutine converstationCor;
    private DialogueState dialogueState;
    private int optionSelected;
    private OptionButton currentOptionButtonSelected;

    public void CloseMenu()
    {
        StopAllCoroutines();
        canvasAnim.SetBool("OpenCanvas", false);
    }

    public void StartConversation(Dialogue dialogue, NPC npc)
    {
        currentNPC = npc;
        currentDialogue = dialogue;

        //Limit player
        CharacterMovement.Instance.FreezePlayer(true);
        CharacterMovement.Instance.OpenMenu();
        CharacterMovement.Instance.TalkCamActive(true);

        dialogueState = DialogueState.None;
        StartCoroutine(CheckForInput());

        canvasAnim.gameObject.SetActive(true);
        canvasAnim.SetBool("OpenCanvas", true);

        sentenceText.gameObject.SetActive(false);
        optionPanel.gameObject.SetActive(false);
    }

    public void StartOptions()
    {
        dialogueState = DialogueState.Options;
        nameText.text = "You";

        for (int i = 0; i < optionButtons.Count; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
        }

        optionPanel.gameObject.SetActive(true);

        for (int i = 0; i < currentDialogue.optionNames.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].ChangeName(currentDialogue.optionNames[i]);
        }
       
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionButtons[0].gameObject);
        currentOptionButtonSelected = optionButtons[0];
        currentOptionButtonSelected.Selected(true);
    }

    public void OptionSelected(OptionButton button)
    {
        if(currentOptionButtonSelected != null) { currentOptionButtonSelected.Selected(false); }
        currentOptionButtonSelected = button;
        optionSelected = optionButtons.IndexOf(button);
    }

    public void OptionClicked()
    {
        optionPanel.gameObject.SetActive(false);

        if (optionSelected == 0) 
        { 
            StartDialogue();
            return;
        }
        currentNPC.InvokeOptionEvent(optionSelected);
    }

    public void StartDialogue()
    {
        //Start dialogue
        dialogueState = DialogueState.Dialogue;
        nameText.text = currentDialogue.name;
        sentenceText.gameObject.SetActive(true);
        sentences.Clear();

        foreach (string sentence in currentDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        if(sentences.Count == 0) 
        { 
            EndConversation();
            return;
        }

        string sentence = sentences.Dequeue();
        if(converstationCor != null) StopCoroutine(converstationCor);
        converstationCor = StartCoroutine(TypeSentence(sentence));
    }

    public void EndConversation()
    {
        StopAllCoroutines();
        canvasAnim.SetBool("OpenCanvas", false);

        CharacterMovement.Instance.TalkCamActive(false);
        StartCoroutine(WaitCamMovement());
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(nextTextButton))
        {
            switch (dialogueState)
            {
                case DialogueState.Dialogue:
                    NextSentence();
                    break;
                case DialogueState.Options:
                    OptionClicked();
                    break;
                case DialogueState.None:
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        sentenceText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            yield return null;
        }
    }

    private IEnumerator CheckForInput()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            CheckInput();
        }
    }

    private IEnumerator WaitCamMovement()
    {
        yield return new WaitForSeconds(0.2f);

        if (currentNPC.idleNPC) { currentNPC.ChangeState(StateEnum.Idle); }
        else { currentNPC.ChangeState(StateEnum.Walk); }

        CharacterMovement.Instance.FreezePlayer(false);
        CharacterMovement.Instance.CloseMenu();

        currentNPC.inconversation = false;
        currentNPC = null;
    }

    #region Singleton
    private static DialogueManager instance;
    private void Awake()
    {
        instance = this;
        sentences = new Queue<string>();
    }
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DialogueManager();
            }

            return instance;
        }
    }
    #endregion
}
