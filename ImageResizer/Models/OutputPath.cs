namespace ImageResizer.Models;

public class OutputPath : AbstractOutputPath
{
   private string PathToPublicDir { get;  }
   private string PathFromPublicDir { get; }
   private string BaseFileName { get;  }
   private int Version { get; }

   public OutputPath(string pathToPublicDir, string pathFromPublicDir, string baseFileName, int version)
   {
      PathToPublicDir = pathToPublicDir;
      PathFromPublicDir = pathFromPublicDir;
      BaseFileName = baseFileName;
      Version = version;
   }

   public override string ToAbsoluteDirPathString(string ext)
   {
      var relativeDirPathString = ToRelativeDirPathString(ext);
      return Path.Join(PathToPublicDir, relativeDirPathString);
   }

   public override string ToRelativeDirPathString(string ext)
   {
      return Path.Join(PathFromPublicDir, BaseFileName, ext);
   }

   public override string ToAbsoluteFilePathString(int width, string ext)
   {
      var absoluteDirPathString = ToAbsoluteDirPathString(ext);
      var fileNameWithVersionAndFormat = GetFileNameWithVersionAndFormat(width, ext);
      return Path.Join(absoluteDirPathString, fileNameWithVersionAndFormat);
   }

   public override string ToRelativeFilePathString(int width, string ext)
   {
      var relativeDirPathString = ToRelativeDirPathString(ext);
      var fileNameWithVersionAndFormat = GetFileNameWithVersionAndFormat(width, ext);
      return Path.Join(relativeDirPathString, fileNameWithVersionAndFormat);
   }

   private string GetFileNameWithVersionAndFormat(int width, string ext)
   {
      return $"{BaseFileName}_{width}w_v{Version}{ext}";
   }
}