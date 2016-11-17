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

namespace GameHub.Lists
{
    class Friends
    {
        private static Random RANDOM = new Random();

        public static int RandomNick
        {
            get
            {
                switch (RANDOM.Next(5))
                {
                    default:
                    case 0:
                        return Resource.Drawable.Icon;
                    case 1:
                        return Resource.Drawable.Icon;
                    case 2:
                        return Resource.Drawable.Icon;
                    case 3:
                        return Resource.Drawable.Icon;
                    case 4:
                        return Resource.Drawable.Icon;
                }
            }
        }
        public static List<string> NickStrings
        {
            get
            {
                return new List<string>()
                {
                    "Jarek", "Szymon", "Adam", "Kuba" , "S³awek", "Wanda", "Halina", "Bob", "Staszek","Jan","W³adek"
                };
            }
        }

        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {

                // Calculate ratios of height and width to requested height and
                // width
                int heightRatio = height / reqHeight;
                int widthRatio = width / reqWidth;

                // Choose the smallest ratio as inSampleSize value, this will
                // guarantee
                // a final image with both dimensions larger than or equal to the
                // requested height and width.
                inSampleSize = heightRatio < widthRatio ? heightRatio : widthRatio;
            }

            return inSampleSize;
        }
    }
}