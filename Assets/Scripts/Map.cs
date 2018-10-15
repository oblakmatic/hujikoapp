using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Map : MonoBehaviour {

    int[] level1 = new int[] { 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 0, 3};
    int[] level2 = new int[] {0,1,0,1,0,1,1,0,0,1,1,0,1,0,1,0,0,1,1,1,1,0,0,1,1,1,1,0,1,0,1,0,0,0,0,0, 4 };
    int[] level3 = new int[] {1,0,1,0,1,1,1,0,1,1,0,0,0,1,0,1,0,1,0,0,1,1,0,1,1,1,0,1,0,1,0,0,1,1,0,1, 9 };
    const int MENU = 1, TRANSITION=2, PLAY=3, GAMEOVER=4;
    int state, lvl, steps;
    public Button play;
    public Image bg;
    public Text title, subTitle, buttonText, level, stepsText;

    //0->down
    //1->left
    //2->up
    //3->right
    public AudioClip win, swap, gameover;
    AudioSource source;
    public Material dotNormal, dotGlow;
    public LayerMask mask;
    const float lineWidth = 0.3f;
    const float lineLength = 40f;
   


    public int size = 2;
    public float width = 1;
    public Vector2[] map;
    public GameObject linePrefab, rectPrefab;
    public GameObject dotPrefab;

    Transform lineParent, rectParent;


    public List<Rectangle> rectangles;

    void Start() {

        state = MENU;
        lvl = 1;
        steps = level1[level1.Length-1];
        lineParent = transform.GetChild(0);
        rectParent = transform.GetChild(1);
        map = new Vector2[size * size];
        rectangles = new List<Rectangle>(size * size);
        source = GetComponent<AudioSource>();
        //set positions of rectangles
        SetMap();
        //add rectangles to positions, add dots to rectangles
        AddRectangles();
        SetNeighbours();
        stepsText.text = "Steps: " + steps;
        for (int i=0; i < rectangles.Count; i++) {
            Check(rectangles[i]);
        }

    }


    void Update() {
        switch (state){

            case MENU:

            break;

            case TRANSITION:

            break;

            case GAMEOVER:

            break;

            case PLAY:
            if (Input.GetMouseButtonDown(0)) {
                Clicked();

                bool isWin = true;
                for (int i = 0; i < rectangles.Count; i++) {
                    Check(rectangles[i]);
                    for (int h = 0; h < 4; h++) {

                        if (!rectangles[i].dots[h].Ok()) {
                            
                            isWin = false;
                        }
                    }

                }

                if (isWin) {
                    state = TRANSITION;
                    lvl++;
                    source.PlayOneShot(win, 1f);
                    level.text = "#" + lvl;
                    StartCoroutine(Fade2(true));
                }
                else if (steps <= 0) {
              
                    source.PlayOneShot(gameover, 1f);
                    ResetDots();
                    state = GAMEOVER;
                }
            }
            break;


        }
      
    }


    void Check(Rectangle r) {
        //check if equal
        Dot[] dots1 = r.dots;
        for (int i = 0; i < 4; i++) {
            dots1[i].count = 2;
        }
        for (int i = 0; i < 4; i++) {
            if (r.neighbours[i] != null) {
                Rectangle nei = r.neighbours[i];
                Dot[] dots2 = nei.dots;
                int ind1 = -1, ind2 = -1, ind11 = -1, ind22 = -1;
                //down rect
                if (i == 0) {
                    //0->1 and 3->2
                    ind1 = (0 - r.offset + 4) % 4;
                    ind2 = (3 - r.offset + 4) % 4;
                    ind11 = (1 - nei.offset + 4) % 4;
                    ind22 = (2 - nei.offset + 4) % 4;
                 
                }
                //up rect
                else if (i == 1) {
                    //0->3 and 1->2
                    ind1 = (0 - r.offset + 4) % 4;
                    ind2 = (1 - r.offset + 4) % 4;
                    ind11 = (3 - nei.offset + 4) % 4;
                    ind22 = (2 - nei.offset + 4) % 4;

                }
                //up rect
                else if (i == 2) {
                    //1->0 and 2->3
                    ind1 = (1 - r.offset + 4) % 4;
                    ind2 = (2 - r.offset + 4) % 4;
                    ind11 = (0 - nei.offset + 4) % 4;
                    ind22 = (3 - nei.offset + 4) % 4;

                }

                else if (i == 3) {
                    //2->1 and 3->0
                    ind1 = (2 - r.offset + 4) % 4;
                    ind2 = (3 - r.offset + 4) % 4;
                    ind11 = (1 - nei.offset + 4) % 4;
                    ind22 = (0 - nei.offset + 4) % 4;

                }

                if (dots1[ind1].type != dots2[ind11].type) {
                    dots1[ind1].count--;
                }
              

                if (dots1[ind2].type != dots2[ind22].type) {
                    dots1[ind2].count--;
                }
               
            }
            else {
                int ind1 = -1, ind2 =-1;
                if (i == 0) {
                    //0->1 and 3->2
                    ind1 = (0 - r.offset + 4) % 4;
                    ind2 = (3 - r.offset + 4) % 4;


                }
                //up rect
                else if (i == 1) {
                    //0->3 and 1->2
                    ind1 = (0 - r.offset + 4) % 4;
                    ind2 = (1 - r.offset + 4) % 4;


                }
                //up rect
                else if (i == 2) {
                    //1->0 and 2->3
                    ind1 = (1 - r.offset + 4) % 4;
                    ind2 = (2 - r.offset + 4) % 4;
   

                }

                else if (i == 3) {
                    //2->1 and 3->0
                    ind1 = (2 - r.offset + 4) % 4;
                    ind2 = (3 - r.offset + 4) % 4;

                }

                
                dots1[ind1].count--;
                dots1[ind2].count--;
            }
            
        }

        for (int i = 0; i < 4; i++) {
            if (dots1[i].count > 0) {
                dots1[i].mr.material = dotGlow;
                
            }
            else {
                dots1[i].mr.material = dotNormal;

            }
        }
        if (r.index == 0) {
            int edge0 = (0 - r.offset + 4) % 4;
            r.dots[edge0].mr.material = dotGlow;
        }
        else if (r.index == size - 1) {
            int edge1 = (3 - r.offset + 4) % 4;
            r.dots[edge1].mr.material = dotGlow;
        }
        else if (r.index == rectangles.Count - size) {
            int edge2 = (1 - r.offset + 4) % 4;
            r.dots[edge2].mr.material = dotGlow;
        }
        else if (r.index == rectangles.Count - 1) {
            int edge3 = (2 - r.offset + 4) % 4;
            r.dots[edge3].mr.material = dotGlow;
        }
    }



    //dots switch places
    void Switch(Rectangle r) {
        Dot[] dots = r.dots;
        r.canTouch = false;
        r.offset++;
        r.offset = r.offset % 4;
        for (int i = 0; i < 4; i++) {
            dots[i].place++;
            dots[i].place %= 4;
            StartCoroutine(Rotate(dots[i], r));
        }
    }


    void Clicked() {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 50, mask);
        if (hit.collider != null) {
            for (int i = 0; i < rectangles.Count; i++) {
                if (rectangles[i].tran == hit.transform && rectangles[i].canTouch) {
                    source.PlayOneShot(swap, 1f);
                    steps--;
                    stepsText.text = "Steps: " + steps;
                    Switch(rectangles[i]);
                    Check(rectangles[i]);
                    break;
                }
            }
        }

    }

    void ResetDots() {
       
        StartCoroutine(Fade2(false));
       
    }

    void AddRectangles() {

        for (int i = 0; i < map.Length; i++) {
            GameObject go = GameObject.Instantiate(rectPrefab, rectParent);
            go.transform.localPosition = map[i];
            go.transform.localScale = Vector3.one * width * 0.9f;
            Rectangle r = new Rectangle(go.transform, i);
            go.name = "Rectangle " + i;
            if (lvl == 1) {
                steps = level1[level1.Length - 1];
                r.AddDots(dotPrefab, i, level1);

            }
            else if (lvl == 2) {
                steps = level2[level2.Length - 1];
                r.AddDots(dotPrefab, i, level2);
            }
            else {
                steps = level3[level3.Length - 1];
                r.AddDots(dotPrefab, i, level3);
            }
            rectangles.Add(r);

        }
            stepsText.text = "Steps: " + steps;




    }


    IEnumerator Rotate(Dot d, Rectangle r) {
        int index = d.place;
        float t = 0;
        Vector3 start = d.tran.localPosition;
        while (t < 1) {
            t += Time.deltaTime * 3;
            d.tran.localPosition = Vector3.Lerp(start, r.positions[index], t);
            yield return null;
        }
        r.canTouch = true;
    }

    void SetMap() {
        int count = 0;
        float start = -size / 2f * width + width / 2;

        GameObject gov = GameObject.Instantiate(linePrefab, lineParent);
        gov.transform.localPosition = new Vector2(start - width / 2, 0);
        gov.transform.localScale = new Vector3(lineWidth, lineLength, 1);

        GameObject goh = GameObject.Instantiate(linePrefab, lineParent);
        goh.transform.localPosition = new Vector2(0, start - width / 2);
        goh.transform.localScale = new Vector3(lineLength, lineWidth, 1);

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                Vector2 pos = new Vector2();
                pos.x = start + width * j;
                pos.y = start + width * i;
                map[count] = pos;
                count++;

                if (i == 0) {
                    //vertical line
                    GameObject go1 = GameObject.Instantiate(linePrefab, lineParent);
                    go1.transform.localPosition = new Vector2(pos.x + width / 2, 0);
                    go1.transform.localScale = new Vector3(lineWidth, lineLength, 1);

                }
            }
            //horizontal line
            GameObject go2 = GameObject.Instantiate(linePrefab, lineParent);
            go2.transform.localPosition = new Vector2(0, start + width / 2 + width * i);
            go2.transform.localScale = new Vector3(lineLength, lineWidth, 1);
        }
    }


    void SetNeighbours() {
        for (int j = 0; j < rectangles.Count; j++) {
            string c = j + ": ";


            Rectangle r = rectangles[j];


            //down
            if (j - size >= 0) {
                r.neighbours[0] = (rectangles[j - size]);

            }
            //left
            if (j / size == (j - 1) / size && j - 1 >= 0) {
                r.neighbours[1] = (rectangles[j - 1]);

            }

            //up
            if (j + size < size * size) {
                r.neighbours[2] = (rectangles[j + size]);

            }
            //right
            if (j / size == (j + 1) / size && j + 1 < size * size) {
                r.neighbours[3] = (rectangles[j + 1]);

            }
            for (int h = 0; h < 4; h++) {
                c += (r.neighbours[h] == null) + " ";
            }
          //  print(c);

        }

    }


    public void Play() {
 
        
        StartCoroutine(Fade());
        title.CrossFadeAlpha(0,1, false);
        subTitle.CrossFadeAlpha(0, 1, false);
        
        play.GetComponent<Image>().CrossFadeAlpha(0, 1, false);
        buttonText.CrossFadeAlpha(0, 1, false);
    }

    IEnumerator Fade() {
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime;
            yield return null;
        }
        play.gameObject.SetActive(false);
        state = TRANSITION;
        t = 1.7f;
        level.gameObject.SetActive(true);
        while (t > 0) {
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, t);
            t -= Time.deltaTime / 2;
            if (t < 0.4f)
                state = PLAY;
            yield return null;
        }
        level.gameObject.SetActive(false);
        

    }

    IEnumerator Fade2(bool todo) {
       
        float t = -1.6f;
        while (t < 1) {
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, t);
            t += Time.deltaTime*2;
            yield return null;
        }
        stepsText.text = "Steps: " + steps;

        if (todo) {


            foreach (Transform child in lineParent) {
                Destroy(child.gameObject);

            }

            size = 3;
            map = new Vector2[size * size];
            SetMap();
        }
            foreach (Transform child in rectParent)
                Destroy(child.gameObject);
            rectangles = new List<Rectangle>(size * size);
    
           
            //add rectangles to positions, add dots to rectangles
            AddRectangles();
            SetNeighbours();
            yield return null;
      

        for (int i = 0; i < rectangles.Count; i++) {
            Check(rectangles[i]);
        }
        t = 1.7f;
        level.gameObject.SetActive(true);
      
        while (t > 0) {
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, t);
            t -= Time.deltaTime / 2;
            if (t<0.4f)
                state = PLAY;

            yield return null;
        }
        level.gameObject.SetActive(false);
     

    }
    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < map.Length; i++) {
    //        Gizmos.DrawSphere(map[i], 0.1f);
    //    }
    //}
}
