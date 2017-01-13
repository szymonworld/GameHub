using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace GameHub.Helpers
{
    public static class ConnectAccount
    {
        private static Random RANDOM = new Random();

        public static int RandomAccount
        {
            get
            {
                switch (RANDOM.Next(5))
                {
                    default:
                    case 0:
                        return Resource.Drawable.Icon_battle;
                    case 1:
                        return Resource.Drawable.Icon_discord;
                    case 2:
                        return Resource.Drawable.Icon_origin;
                    case 3:
                        return Resource.Drawable.Icon_psn;
                    case 4:
                        return Resource.Drawable.Icon_xbox;
                }
            }
        }
        public static List<string> CheeseStrings
        {
            get
            {
                return new List<string>() {
                    "Seherim", "RavanBlade", "Kimchi", "Hasken2", "Destroyer",
            "Love Gothic", "Todd", "Dragon"};
            }
        }

        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {
                int heightRatio = height / reqHeight;
                int widthRatio = width / reqWidth;

                inSampleSize = heightRatio < widthRatio ? heightRatio : widthRatio;
            }

            return inSampleSize;
        }
    }
}