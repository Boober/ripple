using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LightDisperser : MonoBehaviour {

    // Use this for initialization
    private List<GameObject> mobjects;
    private Collider2D[] prevhits;
    private Collider2D[] mhits;
    private List<RaycastHit2D> hitlocs;
    private List<Vector2[][]> coords;
    private MeshFilter mfilter;
    //private Vector3 originangle;
    //private float Angle = .00001f * (180 / Mathf.PI);
    private float anglex;
    private float angley;

    //For generating the mesh.

    public Mesh mvisibility;
    public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;
    public List<verts> allVertices = new List<verts>();
    private int lightSegments = 8;


    public Material lightMaterial;



    public int[] mTriangles;
    public int numOfRays = 50;
    public float radius = 15.0f;
    //public float lightRadius = 1000f;
    private List<Ray2D> rays;
    private int layerMask;



    public class verts
    {
        public float angle { get; set; }
        public int location { get; set; } // 1= left end point    0= middle     -1=right endpoint
        public Vector3 pos { get; set; }
        public bool endpoint { get; set; }

    }



    void Start ()
    {
        /*
        mobjects = new List<GameObject>();
        */
        layerMask = 1 << 10;
        coords = new List<Vector2[][]>();
        hitlocs = new List<RaycastHit2D>();
        mvisibility = new Mesh();
        MeshFilter meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));                // Add a Mesh Filter component to the light game object so it can take on a form
        MeshRenderer renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.sharedMaterial = lightMaterial;
        mvisibility.name = "Light Mesh";                                                          // Give it a name
        mvisibility.MarkDynamic();

        SinCosTable.initSenCos();


    //mfilter = GetComponent<MeshFilter>();
    //mfilter.mesh = mvisibility;
        meshFilter.mesh = mvisibility;
        mvisibility.vertices = newVertices;
        mvisibility.uv = newUV;
        mvisibility.triangles = newTriangles;
        //originangle = new Vector3(0.0f, 0.0f);
        //anglex = Mathf.Cos(Angle);
        //angley = Mathf.Sin(Angle);
        //Vector2 a = new Vector2(Mathf.Sin(Angle), Mathf.Cos(Angle));
    }
	
	// Update is called once per frame
	void Update () {
        /*
        Debug.Log(mobjects.Count);
        castRays();
        */
	}

    void FixedUpdate()
    {
        /*
        for (int i = 0; i < numOfRays; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, radius);
           
        }*/
        coords = new List<Vector2[][]>();
        bool check = DetectWalls(this.transform.position);
        if (!check)
        {
            Debug.Log("No Changes.");
        }
        else
        {
            Debug.Log(coords.Count);
            castRays();
            renderLightMesh();
            resetBounds();
        }

    }


    public bool DetectWalls(Vector2 center)
    {
        //Gathers all objects within the light's radius that we want to Raycast towards.
        mhits = Physics2D.OverlapCircleAll(center, radius, layerMask);
        
        if (prevhits != null && mhits.SequenceEqual(prevhits)) {
            return false;
        }

        prevhits = mhits;
        
        //Goes through those objects, calculating their corners to raycast to.
        for (int i = 0; i < mhits.Length; i++)
        {
            BoxCollider2D bob = (BoxCollider2D)mhits[i];
            Vector2 size = bob.bounds.size;
            Vector3 centerPoint = new Vector3(bob.offset.x, bob.offset.y, 0f);
            Vector3 worldPos = transform.TransformPoint(bob.offset);

            float top = bob.transform.position.y + (size.y / 2f);
            float btm = bob.transform.position.y - (size.y / 2f);
            float left = bob.transform.position.x - (size.x / 2f);
            float right = bob.transform.position.x + (size.x / 2f);

            Vector3 topLeft = new Vector3(left, top, 0f);
            Vector3 topRight = new Vector3(right, top, 0f);
            Vector3 btmLeft = new Vector3(left, btm, 0f);
            Vector3 btmRight = new Vector3(right, btm, 0f);


            //Testing This out

            //Vector3 tlheading = topLeft - this.transform.position;
            //Vector3 trheading = topRight - this.transform.position;
            //Vector3 brheading = btmRight - this.transform.position;
            //Vector3 blheading = btmLeft - this.transform.position;

            Vector2[] tlpos = PlusMinus(topLeft, this.transform.position);
            Vector2[] trpos = PlusMinus(topRight, this.transform.position);
            Vector2[] brpos = PlusMinus(btmRight, this.transform.position);
            Vector2[] blpos = PlusMinus(btmLeft,this.transform.position);

            //Vector2 tldir = tlheading / tlheading.magnitude;
            //Vector2 trdir = trheading / trheading.magnitude;
            //Vector2 brdir = brheading / brheading.magnitude;
            //Vector2 bldir = blheading / blheading.magnitude;
            
           Vector2[][] vectors = { tlpos, trpos, blpos, brpos };
            coords.Add(vectors);


            /*
           float tldis = Vector2.Distance(topLeft, this.gameObject.transform.position);
           float trdis = Vector2.Distance(topLeft, this.gameObject.transform.position);
           float bldis = Vector2.Distance(topLeft, this.gameObject.transform.position);
           float brdis = Vector2.Distance(topLeft, this.gameObject.transform.position);

           float[] dis = { tldis, trdis, bldis, brdis };
           int[] b = findTwoClosest(dis); */
            //Vector2[] newcoords = { vectors[b[0]], vectors[b[1]] };


            /* Debug.DrawLine(this.transform.position, topLeft, Color.black, 20);
              Debug.DrawLine(this.transform.position, topRight, Color.black, 20);
              Debug.DrawLine(this.transform.position, btmLeft, Color.black, 20);
              Debug.DrawLine(this.transform.position, btmRight, Color.black, 20); */


            //Debug.DrawRay(this.transform.position, topLeft - this.transform.position, Color.black, 20);
            //Debug.DrawRay(this.transform.position, topRight - this.transform.position, Color.black, 20);
            //Debug.DrawRay(this.transform.position, btmLeft - this.transform.position, Color.black, 20);
            //Debug.DrawRay(this.transform.position, btmRight - this.transform.position, Color.black, 20);

            /*
            Vector2 size = bob.size;

            float top = bob.offset.y + (bob.size.y / 2f);
            float btm = bob.offset.y - (bob.size.y / 2f);
            float left = bob.offset.x - (bob.size.x / 2f);
            float right = bob.offset.x + (bob.size.x / 2f);


            Vector3 topLeft = transform.TransformPoint(new Vector3(left, top, 0f));
            Vector3 topRight = transform.TransformPoint(new Vector3(right, top, 0f));
            Vector3 btmLeft = transform.TransformPoint(new Vector3(left, btm, 0f));
            Vector3 btmRight = transform.TransformPoint(new Vector3(right, btm, 0f));

            Vector3 tlheading = topLeft - this.transform.position;
            Vector3 trheading = topRight - this.transform.position;
            Vector3 brheading = btmRight - this.transform.position;
            Vector3 blheading = btmLeft - this.transform.position;

            Debug.DrawLine(this.transform.position, bob.transform.position,Color.black,20);
            Debug.DrawLine(this.transform.position, bob.transform.position, Color.black, 20); */

            //Vector2 tldir = tlheading / tlheading.magnitude;
            //Vector2 trdir = trheading / trheading.magnitude;
            //Vector2 brdir = brheading / brheading.magnitude;
            //Vector2 bldir = blheading / blheading.magnitude;
            /*
           Vector2[] vectors = { tlheading, trheading, blheading, brheading };

           float tldis = Vector2.Distance(topLeft, this.gameObject.transform.position);
           float trdis = Vector2.Distance(topLeft, this.gameObject.transform.position);
           float bldis = Vector2.Distance(topLeft, this.gameObject.transform.position);
           float brdis = Vector2.Distance(topLeft, this.gameObject.transform.position);

           float[] dis = { tldis, trdis, bldis, brdis };
           int[] b = findTwoClosest(dis);
           Vector2[] newcoords = { vectors[b[0]], vectors[b[1]] };
           coords.Add(newcoords); */

        }
        return true;
    }

    /*
    private void addVertices()
    {
        //Changes the vertices based on the new Raycast hit points.
        RaycastHit2D[] temp = hitlocs.ToArray();
        Vector3[] adVertices = new Vector3[hitlocs.Count];
        int[] adTriangles = new int[hitlocs.Count * 3];
        Vector2[] adUV = new Vector2[hitlocs.Count];


        for (int i = 0; i < hitlocs.Count; i++)
        {
            adVertices[i] = temp[i].point;
            adUV[i]  = new Vector2(0f, 0f);
        }
        mvisibility.vertices = adVertices;
        mvisibility.uv = adUV;
        adTriangles[0] = 0;
        adTriangles[1] = 1;
        adTriangles[2] = 2;
        mvisibility.triangles = adTriangles;
        mvisibility.RecalculateBounds();
        mvisibility.RecalculateNormals();
    } */

    /*

    public int[] findTwoClosest(float[] a)
    {
        //Finds the two closest corners to draw lines to.
        float min1 = a[0];
        int ind1 = 0;
        float min2 = a[1];
        int ind2 = 1;

        if (min2 < min1) {
            min1 = a[1];
            ind1 = 1;
            min2 = a[0];
            ind2 = 0;
        }

        for (int i = 2; i < a.Length; i++) {
            if (a[i] < min1)
            {
                min2 = min1;
                ind2 = ind1;
                min1 = a[i];
                ind1 = i;
            }
            else if (a[i] < min2)
            {
                min2 = a[i];
                ind2 = i;
            }
        }
        int[] b = { ind1, ind2 };
        return b;
    } */
    
    /*
    public void addObject(GameObject g)
    {
        Debug.Log("Added Object");
        mobjects.Add(g);
    }
    public void removeObject(GameObject g)
    {
        Debug.Log("Removed Object");
        mobjects.Remove(g);
    } */

    public void ObjectPositions()
    {
        /*
        for (int i = 0; i < mobjects.Count; i++)
        {
            BoxCollider2D b = mobjects[i].GetComponent<BoxCollider2D>();
            Vector3 center = b.transform.position;
            Vector2 size = b.size;
        } */
    }
    /*

        public List<GameObject> getObjects()
        {
            return mobjects;

        } */


    /*
     public void castRays()
     {

         for  (int i = 0; i < coords.Count; i++)
         {

             //Debug.DrawRay(this.gameObject.transform.position, mobjects[i].transform.position,Color.black);
             //Casting a ray to the corners that were found.
             Debug.Log("Drawing Line");
             Vector2[] curr = coords[i];
    //         Debug.DrawLine(this.transform.position, curr[0],Color.black,20);
    //         Debug.DrawLine(this.transform.position, curr[1],Color.black,20);
             for (int j = 0; j < curr.Length; j++)
             {
                 //Debug.DrawRay(this.transform.position, curr[j], Color.black, 20, false);
                 RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, curr[j], 20, layerMask);
                 int test = 0; //Makes sure we only select the first two points.
                 for (int k = 0; k < hits.Length; k++)
                 {
                     if (test > 1)
                     {
                         break;
                     }
                     hitlocs.Add(hits[k]);
                     test++;
                 }
             }

             //Debug.DrawRay(this.transform.position, curr[0], Color.black,20,false);
             //Debug.DrawRay(this.transform.position, curr[1], Color.black,20,false);
         }

         addVertices();
     }

     */

    void resetBounds()
    {
        Bounds b = mvisibility.bounds;
        b.center = Vector3.zero;
        mvisibility.bounds = b;     /*CHECK THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
    }


    private void castRays()
    {
        bool sortAngles = false;


        bool lows = false;
        bool his = false;

        float magRange = 0.15f;
        allVertices.Clear();
        List<verts> tempVerts = new List<verts>();

        
        //Obtain vertices for each mesh.

        for (int i = 0; i < coords.Count; i++)
        {
            tempVerts.Clear();
            verts v = new verts();
            for (int j = 0; j < coords[i].Length; j++)
            {
                for (int k = 0; k < coords[i][j].Length; k++)
                {
                    Vector2 worldPt = coords[i][j][k];
                    RaycastHit2D ray = Physics2D.Raycast(this.transform.position, worldPt - (Vector2)this.transform.position, 1000, layerMask);
                    
                    
                    if (ray)
                    {
                        v.pos = ray.point;
                        if (worldPt.sqrMagnitude >= (ray.point.sqrMagnitude - magRange) && worldPt.sqrMagnitude <= (ray.point.sqrMagnitude + magRange))
                            v.endpoint = true;
                    }
                    else
                    {
                        Debug.Log("Fuckaduck");
                        v.pos = coords[i][j][k];
                        v.endpoint = true;
                    }

                    Debug.DrawLine(transform.position, v.pos, Color.white,20);


                    v.pos = transform.InverseTransformPoint(v.pos);
                    //--Calculate angle
                    v.angle = getVectorAngle(true, v.pos.x, v.pos.y);


                    if (v.angle < 0f)
                        lows = true;

                    if (v.angle > 2f)
                        his = true;


                    //--Add verts to the main array
                    if ((v.pos).sqrMagnitude <= radius * radius)    //LIGHTRADIUS
                    {
                        tempVerts.Add(v);

                    }

                    if (sortAngles == false)
                        sortAngles = true;
                }
            }


            //Step 2

            // Indentify the endpoints (left and right)
            if (tempVerts.Count > 0)
            {

                sortList(tempVerts); // sort first

                int posLowAngle = 0; // save the indice of left ray
                int posHighAngle = 0; // same last in right side

                //Debug.Log(lows + " " + his);

                if (his == true && lows == true)
                {  //-- FIX BUG OF SORTING CUANDRANT 1-4 --//
                    float lowestAngle = -1f;//tempVerts[0].angle; // init with first data
                    float highestAngle = tempVerts[0].angle;


                    for (int d = 0; d < tempVerts.Count; d++)
                    {



                        if (tempVerts[d].angle < 1f && tempVerts[d].angle > lowestAngle)
                        {
                            lowestAngle = tempVerts[d].angle;
                            posLowAngle = d;
                        }

                        if (tempVerts[d].angle > 2f && tempVerts[d].angle < highestAngle)
                        {
                            highestAngle = tempVerts[d].angle;
                            posHighAngle = d;
                        }
                    }


                }
                else
                {
                    //-- convencional position of ray points
                    // save the indice of left ray
                    posLowAngle = 0;
                    posHighAngle = tempVerts.Count - 1;

                }


                tempVerts[posLowAngle].location = 1; // right
                tempVerts[posHighAngle].location = -1; // left



                //--Add vertices to the main meshes vertexes--//
                allVertices.AddRange(tempVerts);
                //allVertices.Add(tempVerts[0]);
                //allVertices.Add(tempVerts[tempVerts.Count - 1]);



                // -- r ==0 --> right ray
                // -- r ==1 --> left ray
                for (int r = 0; r < 2; r++)
                {

                    //-- Cast a ray in same direction continuos mode, start a last point of last ray --//
                    Vector3 fromCast = new Vector3();
                    bool isEndpoint = false;

                    if (r == 0)
                    {
                        fromCast = transform.TransformPoint(tempVerts[posLowAngle].pos);
                        isEndpoint = tempVerts[posLowAngle].endpoint;

                    }
                    else if (r == 1)
                    {
                        fromCast = transform.TransformPoint(tempVerts[posHighAngle].pos);
                        isEndpoint = tempVerts[posHighAngle].endpoint;
                    }





                    if (isEndpoint == true)
                    {
                        Vector2 from = (Vector2)fromCast;
                        Vector2 dir = (from - (Vector2)transform.position);



                        float mag = (radius);// - fromCast.magnitude;  //LIGHTRADIUS
                        const float checkPointLastRayOffset = 0.005f;

                        from += (dir * checkPointLastRayOffset);


                        RaycastHit2D rayCont = Physics2D.Raycast(from, dir, mag, layerMask);
                        Vector3 hitp;
                        if (rayCont)
                        {
                            hitp = rayCont.point;
                        }
                        else
                        {
                            Vector2 newDir = transform.InverseTransformDirection(dir);  //local p
                            hitp = (Vector2)transform.TransformPoint(newDir.normalized * mag); //world p
                        }

                        if (((Vector2)hitp - (Vector2)transform.position).sqrMagnitude > (radius * radius)) //LIGHTRADIUS
                        {
                            dir = (Vector2)transform.InverseTransformDirection(dir);    //local p
                            hitp = (Vector2)transform.TransformPoint(dir.normalized * mag);
                        }

                        Debug.DrawLine(fromCast, hitp, Color.green);

                        verts vL = new verts();
                        vL.pos = transform.InverseTransformPoint(hitp);

                        vL.angle = getVectorAngle(true, vL.pos.x, vL.pos.y);
                        allVertices.Add(vL);
                    }


                }


            }

        }


        //Step Three!

        //--Step 3: Generate vectors for light cast--//
        //---------------------------------------------------------------------//

        int theta = 0;
        //float amount = (Mathf.PI * 2) / lightSegments;
        int amount = 360 / lightSegments;



        for (int i = 0; i < lightSegments; i++)
        {

            theta = amount * (i);
            if (theta == 360) theta = 0;

            verts v = new verts();
            //v.pos = new Vector3((Mathf.Sin(theta)), (Mathf.Cos(theta)), 0); // in radians low performance
            v.pos = new Vector3((SinCosTable.SinArray[theta]), (SinCosTable.CosArray[theta]), 0); // in dregrees (previous calculate)

            v.angle = getVectorAngle(true, v.pos.x, v.pos.y);
            v.pos *= radius; //LIGHTRADIUS
            v.pos += transform.position;



            RaycastHit2D ray = Physics2D.Raycast(transform.position, v.pos - transform.position, radius, layerMask); //LIGHTRADIUS
            //Debug.DrawRay(transform.position, v.pos - transform.position, Color.white);

            if (!ray)
            {

                //Debug.DrawLine(transform.position, v.pos, Color.white);

                v.pos = transform.InverseTransformPoint(v.pos);
                allVertices.Add(v);

            }

        }


        //-- Step 4: Sort each vertice by angle (along sweep ray 0 - 2PI)--//
        //---------------------------------------------------------------------//
        //sortAngles = false;
        if (sortAngles == true)
        {
            sortList(allVertices);
        }
        //-----------------------------------------------------------------------------


        //--auxiliar step (change order vertices close to light first in position when has same direction) --//
        float rangeAngleComparision = 0.00001f;
        for (int i = 0; i < allVertices.Count - 1; i += 1)
        {

            verts uno = allVertices[i];
            verts dos = allVertices[i + 1];

            // -- Comparo el angulo local de cada vertex y decido si tengo que hacer un exchange-- //
            if (uno.angle >= dos.angle - rangeAngleComparision && uno.angle <= dos.angle + rangeAngleComparision)
            {

                if (dos.location == -1)
                { // Right Ray

                    if (uno.pos.sqrMagnitude > dos.pos.sqrMagnitude)
                    {
                        allVertices[i] = dos;
                        allVertices[i + 1] = uno;
                        //Debug.Log("changing left");
                    }
                }


                // ALREADY DONE!!
                if (uno.location == 1)
                { // Left Ray
                    if (uno.pos.sqrMagnitude < dos.pos.sqrMagnitude)
                    {

                        allVertices[i] = dos;
                        allVertices[i + 1] = uno;
                        //Debug.Log("changing");
                    }
                }


            }


        }






    }


    void renderLightMesh()
    {
        //-- Step 5: fill the mesh with vertices--//
        //---------------------------------------------------------------------//

        //interface_touch.vertexCount = allVertices.Count; // notify to UI

        Vector3[] initVerticesMeshLight = new Vector3[allVertices.Count + 1];

        initVerticesMeshLight[0] = Vector3.zero;


        for (int i = 0; i < allVertices.Count; i++)
        {
            //Debug.Log(allVertices[i].angle);
            initVerticesMeshLight[i + 1] = allVertices[i].pos;

            //if(allVertices[i].endpoint == true)
            //Debug.Log(allVertices[i].angle);

        }

        mvisibility.Clear();
        mvisibility.vertices = initVerticesMeshLight;

        Vector2[] uvs = new Vector2[initVerticesMeshLight.Length];
        for (int i = 0; i < initVerticesMeshLight.Length; i++)
        {
            uvs[i] = new Vector2(initVerticesMeshLight[i].x, initVerticesMeshLight[i].y);
        }
        mvisibility.uv = uvs;

        // triangles
        int idx = 0;
        int[] triangles = new int[(allVertices.Count * 3)];
        for (int i = 0; i < (allVertices.Count * 3); i += 3)
        {

            triangles[i] = 0;
            triangles[i + 1] = idx + 1;


            if (i == (allVertices.Count * 3) - 3)
            {
                //-- if is the last vertex (one loop)
                triangles[i + 2] = 1;
            }
            else
            {
                triangles[i + 2] = idx + 2; //next next vertex	
            }

            idx++;
        }


        mvisibility.triangles = triangles;
        //lightMesh.RecalculateNormals();
        GetComponent<Renderer>().sharedMaterial = lightMaterial;
    }





    /******************************************************
    ***Helper Functions based off DL asset package. *******
    ****************************************************/

    void sortList(List<verts> lista)
    {
        lista.Sort((item1, item2) => (item2.angle.CompareTo(item1.angle)));
    }

    float getVectorAngle(bool pseudo, float x, float y)
    {
        float ang = 0;
        if (pseudo == true)
        {
            ang = pseudoAngle(x, y);
        }
        else
        {
            ang = Mathf.Atan2(y, x);
        }
        return ang;
    }

    float pseudoAngle(float dx, float dy)
    {
        // Hight performance for calculate angle on a vector (only for sort)
        // APROXIMATE VALUES -- NOT EXACT!! //
        float ax = Mathf.Abs(dx);
        float ay = Mathf.Abs(dy);
        float p = dy / (ax + ay);
        if (dx < 0)
        {
            p = 2 - p;

        }
        return p;
    }


    private Vector2[] PlusMinus(Vector3 tgtPos, Vector3 originPos)
    {
        //Calculates a vector, and two additional vectors +/- .000001 radians.
        Vector3 v = tgtPos - originPos;
        //Vector3 vector1 = Quaternion.Euler(anglex,angley,0) * v;
        //Vector3 vector2 = Quaternion.Euler(-anglex,-angley,0) * v;
        //Quaternion.Euler
        Vector2 dir = v / v.magnitude;
        float x1 = dir.x + .1f;
        float x2 = dir.x - .1f;
        float y1 = dir.y + .1f;
        float y2 = dir.y - .1f;
        Vector2 v2 = new Vector2(x1, y1) * v.magnitude;
        Vector2 v3 = new Vector2(x2, y2) * v.magnitude;

        Vector2[] positions = new Vector2[3];
        positions[0] = tgtPos;
        positions[1] = v2; //originPos + vector1;
        positions[2] = v3; //originPos + vector2;
        return positions;
    } 

}
