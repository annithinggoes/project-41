﻿using System.Collections;
using System.Collections.Generic;
 using UnityEngine;
 
 public class BillboardText : MonoBehaviour
 {
       void Update () {
         transform.rotation = Quaternion.LookRotation( transform.position - Camera.main.transform.position );
   }
 }