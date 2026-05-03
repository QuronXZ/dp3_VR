using System.Collections;
using TMPro;
using UnityEngine;

public class dailogue_appear : MonoBehaviour
{
    public TextMeshProUGUI Textcompo;
    public string[] lines;
    public float textspeed;
    private int index;


    [ExecuteInEditMode]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Textcompo.text = string.Empty;
        startdailogue();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void startdailogue()
    {
        index = 0;
        StartCoroutine(Typeline());
    }

    IEnumerator Typeline()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            Textcompo.text += c;
            yield return new WaitForSeconds(textspeed);

        }
    }

    void Nextline()
    {
        if (index < lines.Length - 1)
        {
            index++;
            Textcompo.text = string.Empty;
            StartCoroutine(Typeline());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void nextdailoguebtn()
    {

        {
            if (Textcompo.text == lines[index])
            {
                Nextline();
            }
            else
            {
                StopAllCoroutines();
                Textcompo.text = lines[index];
            }
        }
    }
}

/*
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class dailogue_appear : MonoBehaviour
{
    public TextMeshProUGUI Textcompo;
    public string[] lines;
    public float textspeed;
    private int index;
    private bool isTyping = false;
    private Coroutine typingCoroutine = null;

    void Start()
    {
        Textcompo.text = string.Empty;
        startdailogue();
    }

    void startdailogue()
    {
        index = 0;
        typingCoroutine = StartCoroutine(Typeline());
    }

    IEnumerator Typeline()
    {
        isTyping = true;
        Textcompo.text = string.Empty; // Clear before starting new line

        foreach (char c in lines[index].ToCharArray())
        {
            Textcompo.text += c;
            yield return new WaitForSeconds(textspeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    void Nextline()
    {
        if (index < lines.Length - 1)
        {
            index++;

            // Stop any existing coroutine before starting new one
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            // Start typing the next line
            typingCoroutine = StartCoroutine(Typeline());
        }
        else
        {
            // Dialogue finished
            gameObject.SetActive(false);
        }
    }

    public void nextdailoguebtn()
    {
        // If currently typing, finish the current line immediately
        if (isTyping)
        {
            // Stop the coroutine
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            // Display the full current line
            Textcompo.text = lines[index];
            isTyping = false;
        }
        else
        {
            // Move to next line if current line is complete
            if (Textcompo.text == lines[index])
            {
                Nextline();
            }
        }
    }

    void OnDisable()
    {
        // Clean up when dialogue panel is disabled
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        isTyping = false;
    }
}
*/