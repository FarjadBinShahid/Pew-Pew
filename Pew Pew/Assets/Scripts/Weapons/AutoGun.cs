using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGun : Gun
{
   public override void Use()
    {
        Debug.Log("using Gun " + itemInfo.itemName);
    }
}
