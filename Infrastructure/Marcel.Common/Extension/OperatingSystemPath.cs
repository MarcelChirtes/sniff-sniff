using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Marcel.Common.Extension
{
    public static class OperatingSystemPath
    {
        /// <summary>
        /// Make sure folder separator for unix: "/", windows: "\"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToRuntimeOSPath(this string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = path.Replace("/", @"\");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                path = path.Replace(@"\", "/");
            }
            else
            {
                throw new Exception("Operating System not supported");
            }
            return path;
        }

        /// <summary>
        /// Combine BaseDirectory with a path taking care of runtime OS
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>combined</returns>
        public static string GetOsRuntimeBaseDirectoryCombinedPath(this string path)
        {
            var runtimePath = AppContext.BaseDirectory.ToRuntimeOSPath();
            path = path.TrimStart('/').TrimStart('\\');
            return Path.Combine(runtimePath, path.ToRuntimeOSPath());
        }
    }
}