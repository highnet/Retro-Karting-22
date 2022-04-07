using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Steam : MonoBehaviour
{
    public bool GetSteamInitialized()
    {
        return SteamManager.Initialized; // check if steam is initialized
    }

    public string GetSteamName()
    {
        return SteamFriends.GetPersonaName(); // get the user's steam persona neme
    }

    public Sprite GetSteamAvatar()
    {
        int friendAvatar = SteamFriends.GetMediumFriendAvatar(SteamUser.GetSteamID()); // gets the id of the user'S medium steam avatar
        uint imageWidth; // variable to store the image's height
        uint imageHeight; // variable to store the image's width
        bool success = SteamUtils.GetImageSize(friendAvatar, out imageWidth, out imageHeight); // get the image size and store success = true if it worked

        if (success && imageWidth > 0 && imageHeight > 0) // if we have the image size and the image width is greater than zero and the image height is greater than zero
        {
            byte[] image = new byte[imageWidth * imageHeight * 4]; // allocate a new byte array for the appropiate image size
            Texture2D returnTexture = new Texture2D((int)imageWidth, (int)imageHeight, TextureFormat.RGBA32, false, true); // generate the 2d texture for the avatar
            success = SteamUtils.GetImageRGBA(friendAvatar, image, (int)(imageWidth * imageHeight * 4)); // check if the image is properly bufferable and buffer it
            if (success) // check if hte image is properly bufferable
            {
                returnTexture.LoadRawTextureData(image); // load the raw texture data from the buffered image
                returnTexture.Apply(); // apply the raw texture data
            }
            return Sprite.Create(returnTexture, new Rect(new Vector2(0, 0), new Vector2(64, 64)), new Vector2(64, 64)); // genereate a sprite based on the raw texture data and return it
        }

        return null; // return null if the image has not been properly downloaded
    }

    public string GetSteamBuild()
    {
        return SteamApps.GetAppBuildId().ToString(); // get the current steam build id of the game
    }
}
