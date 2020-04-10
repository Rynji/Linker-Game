using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private List<Sprite> visualSprites;
    [Header("Animations")]
    [SerializeField] private AnimationCurve tileLinkedCurve;
    [SerializeField] private float tileLinkedAnimSpeed;
    [SerializeField] private AnimationCurve tileCompletedCurve;
    [SerializeField] private float tileCompletedAnimSpeed;
    [Header("Sound")]
    [SerializeField] private AudioClip linkCreatedClip;
    [SerializeField] private AudioClip linkFailedClip, linkCompletedClip;

    private int tileID;
    private bool isChecked; //Unused for now, can be used when checking tiles recursively for links paths if we ever need more than 3 minimum.
    private bool isCompleted;
    private Vector2 tileCoordinates;

    //Animations
    private Vector3 initialScale;
    private Vector3 finalScale;
    private float graphValue;

    public int TileID { get => tileID; set => tileID = value; }
    public bool IsChecked { get => isChecked; set => isChecked = value; }
    public Vector2 TileCoordinates { get => tileCoordinates; set => tileCoordinates = value; }
    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
    public AudioClip LinkCreatedClip { get => linkCreatedClip; }
    public AudioClip LinkCompletedClip { get => linkCompletedClip; }
    public AudioClip LinkFailedClip { get => linkFailedClip; }


    private void Awake()
    {
        initialScale = Vector3.zero;
        finalScale = transform.localScale; 
    }

    
    public void SetVisual()
    {
        this.tileID = Random.Range(0, visualSprites.Count);
        this.GetComponent<SpriteRenderer>().sprite = visualSprites[tileID];
    }

    public void SetVisualForced(int visualID)
    {
        this.tileID = visualID;
        this.GetComponent<SpriteRenderer>().sprite = visualSprites[tileID];
    }

    //Animations

    public IEnumerator DoTileCompletedVisuals()
    {
        //Animation
        float t = 0;
        float rate = 1f / tileCompletedAnimSpeed;
        while (t < 1)
        {
            t += rate * Time.deltaTime;
            transform.localScale = Vector3.Lerp(finalScale, Vector3.zero, tileCompletedCurve.Evaluate(t));
            yield return 0;
        }

        this.GetComponent<SpriteRenderer>().sprite = null;

        yield return 0;
    }

    public void DoTileLinkedVisuals(bool toggle, bool linkInvalid = false)
    {
        StartCoroutine(ToggleTileLinked(toggle, linkInvalid));
    }
    private IEnumerator ToggleTileLinked(bool toggle, bool linkInvalid = false)
    {
        this.transform.GetChild(0).gameObject.SetActive(toggle);

        if(linkInvalid)
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        
        //Animation
        float t = 0;
        float rate = 1f / tileLinkedAnimSpeed;
        while (t < 1)
        {
            t += rate * Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, tileLinkedCurve.Evaluate(t));
            yield return 0;
        }

        if(linkInvalid)
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        yield return 0;
    }
}
