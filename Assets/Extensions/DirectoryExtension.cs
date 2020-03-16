using System.IO;

namespace Extensions
{
    public static class DirectoryInfoExtension
    {
        public static void Copy(this DirectoryInfo sourceDirectory, string destinationDirectory)
        {
            if (sourceDirectory == null)
                return;

            if (!sourceDirectory.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found");

            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            var files = sourceDirectory.GetFiles();
            foreach (var file in files)
            {
                string destinationFileName = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(destinationFileName, true);
            }

            var subDirectoryInfos = sourceDirectory.GetDirectories();
            foreach (var subDirectoryInfo in subDirectoryInfos)
            {
                string subDestinationDirectory = Path.Combine(destinationDirectory, subDirectoryInfo.Name);
                Copy(subDirectoryInfo, subDestinationDirectory);
            }
        }
    }
}
