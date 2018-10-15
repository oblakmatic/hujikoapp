using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot {

    Color c1 = new Color(147 / 255f, 197 / 255f, 4 / 255f, 1);
    Color c2 = new Color(255 / 255f, 100 / 255f, 7 / 255f, 1);

    public Transform tran;
    public MeshFilter mf;
    public MeshRenderer mr;
    public int type, place;
    public Vector3 startPos;

    public int count;

    public Dot(Transform t, int pos, int r) {
        tran = t;
        mr = tran.GetComponent<MeshRenderer>();
        mf = tran.GetComponent<MeshFilter>();
        place = pos;
        ColorIt( r);
        count = 0;
     
    }

    void ColorIt( int r) {
        Mesh mesh = mf.mesh;
        int l = mesh.vertices.Length;

        Color[] colors = new Color[l];
        
       
        type=r;
        for (int i = 0; i < l; i++) {
            if (r==0)
                colors[i] = c1;
            else
                colors[i] = c2;
        }
        
        mesh.colors = colors;
    }

    public void ColorMe(Color c1) {
        Mesh mesh = tran.GetComponent<MeshFilter>().mesh;
        int l = mesh.vertices.Length;
        
        Color[] colors = new Color[l];
        for (int i = 0; i < l; i++) {

                colors[i] = c1;
  
        }

        mesh.colors = colors;
    }

    public bool Ok() {
       
        return mr.material.name.Contains("dotGlow") ;
    }
}
