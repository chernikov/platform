﻿using Ninject;
using platformAthletic.Global.Config;
using platformAthletic.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace platformAthletic.Tools.GenerateTools
{
    public class Imaginarium : Filerarium
    {
        public static IConfig Config
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                return kernel.Get<IConfig>();
            }
        }

        public static string GetRandomSourceImage()
        {
            return GetRandomSourceFile("E:\\test\\sandbox\\images\\", "*.jpg");
        }

        public static string SaveRandomImage(string folder)
        {
            var imageUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var absFile = MakeAbsFolder(imageUrl);
            using (var fileSource = new FileStream(GetRandomSourceImage(), FileMode.Open))
            {
                using (var fs = new FileStream(absFile, FileMode.CreateNew))
                {
                    fileSource.CopyTo(fs);
                    fs.Flush();
                }
            }
            return imageUrl;
        }

        public static string MakeImage(Stream ms, string folder, string imageSize)
        {
            var outSize = new Size();
            return MakeImage(ms, folder, imageSize, out outSize);
        }

        public static string MakeImage(Stream ms, string folder, string imageSize, out Size outSize)
        {
            outSize = new Size();
            var imageUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == imageSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                var absFile = MakeAbsFolder(imageUrl);
                var brush = new SolidBrush(Color.White);
                PreviewCreator.CreateAndSaveImage(ms, previewSize, absFile, brush);
            }
            return imageUrl;
        }

        public string MakePreview(Stream ms, string folder, string imageSize, bool grayscale = false)
        {
            var imageUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var imageSizes = Config.IconSizes.FirstOrDefault(c => c.Name == imageSize);
            if (imageSizes != null)
            {
                var brush = new SolidBrush(Color.White);
                PreviewCreator.CreateAndSavePreview(ms, new Size(imageSizes.Width, imageSizes.Height), MakeAbsFolder(imageUrl), brush);
            }
            return imageUrl;
        }
    }
}
