using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rectangle  {

    public Transform tran;
    public MeshFilter mf;
    public Dot[] dots;
    float scale = 0.8f;
    public Vector3[] positions;
    public Rectangle[] neighbours;
    public bool canTouch = true;
    public int offset = 0;
    public int index;
   
    public Rectangle(Transform t, int i) {
        tran = t;
        mf = tran.GetComponent<MeshFilter>();
        index = i;
        positions = new Vector3[4];
       
        positions[0] = new Vector3(-tran.localScale.x / 2, -tran.localScale.x / 2, -1) * scale;
        positions[1] = new Vector3(-tran.localScale.x / 2, tran.localScale.x / 2, -1) * scale;
        positions[2] = new Vector3(tran.localScale.x / 2, tran.localScale.x / 2, -1) * scale;
        positions[3] = new Vector3(tran.localScale.x / 2, -tran.localScale.x / 2, -1) * scale;

        neighbours = new Rectangle[4];
    }


   


    public void AddDots(GameObject prefab, int index, int[] level) {

        dots = new Dot[4];
        for (int i=0; i < 4; i++) {
            GameObject go = GameObject.Instantiate(prefab, tran);
            go.transform.localScale = tran.localScale * 0.17f;
            Dot d = new Dot(go.transform, i, level[index*4+i]);
            dots[i] = d;
        }

        //spodaj
        dots[0].tran.localPosition = positions[0];
        dots[0].startPos = dots[0].tran.position;
        //levo
        dots[1].tran.localPosition = positions[1];
        dots[1].startPos = dots[1].tran.position;
        //zgoraj
        dots[2].tran.localPosition = positions[2];
        dots[2].startPos = dots[2].tran.position;
        //desno
        dots[3].tran.localPosition = positions[3];
        dots[3].startPos = dots[3].tran.position;
    }
    
    public void ResetDots() {
        //spodaj
        offset = 0;
        dots[0].tran.position = dots[0].startPos;
       // dots[0].tran.localPosition = positions[0];
        
        dots[0].place = 0;
        //levo
        dots[1].tran.position = dots[1].startPos;
       // dots[1].tran.localPosition = positions[1];
        dots[1].place = 1;
        //zgoraj
        dots[2].tran.position = dots[2].startPos;
        //dots[2].tran.localPosition = positions[2];
        dots[2].place = 2;
        //desno
        dots[3].tran.position = dots[3].startPos;
        //dots[3].tran.localPosition = positions[3];
        dots[3].place = 3;

        canTouch = true;
    }
}
