using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageDisplay : MonoBehaviour
{
    public Image image; // the image in which sprites are shown on display
    public Sprite[] sprites; // the array of possible sprites to be shown on display
    public int spriteIndex; // the index of the currently shown sprite on display
    public Sprite[] supplementarySprites; // the array of possible supplementary sprites that supplement the regular sprites
    public GameObject[] supplementarySpriteMouseoverRegions; // the array of mouse over regions that trigger the supplementary sprites for the regular sprites

    private void Start()
    {
        UpdateSupplementarySpriteMouseoverRegions(); // update the supplementary sprite mouse over regions
        UpdateSpriteOnDisplay(); // update the sprite on display
    }

    public void IncrementSpriteIndex()
    {
        if (spriteIndex < sprites.Length - 1) // check if the sprite index is not at the end of the array
        {
            spriteIndex++; // increment the sprite index
        } else if (spriteIndex == sprites.Length - 1) // check if the sprite index is at the end of the array
        {
            spriteIndex = 0; // cycle back to the beginning of the array
        }
        UpdateSupplementarySpriteMouseoverRegions(); // update the new supplementary sprite mouse over regions
        UpdateSpriteOnDisplay(); // update the new sprite on display

    }

    public void DecrementSpriteIndex()
    {
        if (spriteIndex > 0) // check if the sprite index is not at the start of the array
        {
            spriteIndex--; // decrement the sprite index
        } else if (spriteIndex == 0) // check if the sprite idex is at the start of the array
        {
            spriteIndex = sprites.Length - 1; // cycle back to the end of the array
        }
        UpdateSupplementarySpriteMouseoverRegions(); // update the new supplementary sprite mouser over regions
        UpdateSpriteOnDisplay(); // update the new sprite on display

    }

    public void UpdateSupplementarySpriteMouseoverRegions()
    {
        for (int i = 0; i < supplementarySpriteMouseoverRegions.Length; i++) // for each supplementary sprite mouser over region
        {
            supplementarySpriteMouseoverRegions[i].gameObject.SetActive(false); // disable the supplementary sprite mouse over region
        }
        switch (spriteIndex) // sprite index fork
        {
            case 0: // image0
                supplementarySpriteMouseoverRegions[9].gameObject.SetActive(true); // activate the corresponding mouser over regoins
                supplementarySpriteMouseoverRegions[10].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[11].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[12].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[13].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[14].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[15].gameObject.SetActive(true);
                break;
            case 1: // image1
                supplementarySpriteMouseoverRegions[0].gameObject.SetActive(true); // activate the corresponding mouse over regions
                supplementarySpriteMouseoverRegions[1].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[2].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[3].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[4].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[5].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[6].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[7].gameObject.SetActive(true);
                supplementarySpriteMouseoverRegions[8].gameObject.SetActive(true);
                break;
        }
    }

    public void UpdateSpriteOnDisplay()
    {
        image.sprite = sprites[spriteIndex]; // set the image sprite to appropiate sprite arrax index
    }

    public void setSupplementarySpriteIndex(int supplementarySpriteIndex)
    {
            image.sprite = supplementarySprites[supplementarySpriteIndex]; // set the image sprite to the appropiate supplementary sprite index
    }
}
