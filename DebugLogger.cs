using System.Text;
using System.Text.RegularExpressions;

namespace Helper.Logging
{
  /// <summary>
  /// This Class is meant to be a simple logging tool, which offers static and instance oriented methods of logging.<br/><br/>
  /// You can either create one or multiple instances of this class, to write logs into either one single or multiple diffrent logfiles, scattered in diffrent directories.<br/>
  /// To keep track of your diffrent logger instances, you can name them.<br/>
  /// -> They will then mark every log entry with their name.<br/>
  /// This is useful when tracking multiple points of interest in a single application, like different internal workers or API's<br/><br/>
  /// 
  /// The simpler approach to this is the static <see cref="QuickLog(string, string)"/> method, which enables much simpler single line logpoints.
  /// </summary>
  public class DebugLogger
  {
    private const string _QuickLoggerName = "Quicklogger";

    private const string _StandardFilePath = @"C:\_DebugLogger\";

    private const string _StandardFileName = @"debug";

    private const string _StandardFileExtension = @".txt";

    public string FullFileName;

    public string LoggerName = "UnnamedLogger";

    public bool TimeStampInLogfileName = true;

    public DebugLogger(string fullFilePath, string loggerName = "", bool timeStampInLogfileName = true)
    {
      FullFileName = fullFilePath;
      LoggerName = loggerName != "" ? loggerName : LoggerName;
      TimeStampInLogfileName = timeStampInLogfileName;
    }

    /// <summary>
    /// Logs string using information of the <see cref="DebugLogger"/> instance<br/><br/>
    /// Throws <see cref="Exception"/> when necessary file-system-permissions are not in place<br/>
    /// Or an <see cref="ArgumentException"/> if the submitted filepath is invalid
    /// </summary>
    /// <param name="logContent"></param>
    public void Log(string logContent = "")
    {
      Log(fullFilePath: FullFileName, loggerName: LoggerName, timeStampInLogfileName: TimeStampInLogfileName, logContent: logContent);
    }

    /// <summary>
    /// Class internal method that implements the technicalities of logging<br/><br/>
    /// Throws <see cref="Exception"/> when necessary file-system-permissions are not in place<br/>
    /// Or an <see cref="ArgumentException"/> if the submitted filepath is invalid
    /// </summary>
    /// <param name="fullFilePath"></param>
    /// <param name="loggerName"></param>
    /// <param name="timeStampInLogfileName"></param>
    /// <param name="logContent"></param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentException"></exception>
    private void Log(string fullFilePath, string loggerName, bool timeStampInLogfileName, string logContent = "")
    {
      if (!Path.IsPathFullyQualified((ReadOnlySpan<char>)fullFilePath))
        throw new ArgumentException("Logging exception: Invalid filepath");

      if (!Path.Exists(Path.GetDirectoryName(fullFilePath)))
        Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));

      if (timeStampInLogfileName)
        fullFilePath = Path.GetDirectoryName(fullFilePath) + @"\" + Path.GetFileNameWithoutExtension(fullFilePath) + DateTime.Now.ToString("-yyyy-MM-dd_HH-mm-ss") + Path.GetExtension(fullFilePath);

      using (var file = new FileStream(fullFilePath, FileMode.Append, FileAccess.Write))
      {
        if (!file.CanWrite)
          throw new Exception("Logging exception: No append permission");

        string indentation = "  ";

        // Log headline
        string writeToFile = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "  Log generated by: \"" + loggerName + "\"\r\n";

        // Logbody
        writeToFile = Regex.Replace(writeToFile + logContent, "(\r\n|\n)", "\r\n" + indentation);

        // Empty line to make space for possible appends in the same file
        writeToFile += "\r\n\r\n";

        byte[] info = new UTF8Encoding(true).GetBytes(writeToFile);
        file.Write(info, 0, info.Length);
      }
    }

    #region Static functionallity

    /// <summary>
    /// Enables a quick simple logging functionality, with the Help of standard filepaths if none get supplied.<br/>
    /// </summary>
    /// <remarks>
    /// The default value for thre parameter <paramref name="fullFilePath"/> is currently faulty.<br/>
    /// Of course there is always only one "\" at a time.
    /// </remarks>
    public static void QuickLog(string logContent = "", string fullFilePath = _StandardFilePath + _StandardFileName + _StandardFileExtension)

    {
      var logger = new DebugLogger(fullFilePath: fullFilePath, loggerName: _QuickLoggerName, timeStampInLogfileName: true);

      Log(logger, logContent);
    }

    /// <summary>
    /// The only purpose of this function is increased usability.<br/>
    /// It is a plain forward to <see cref="QuickLog(string, string)"/>
    /// </summary>
    public static void QuickLog(string filePath, string fileName, string fileExtension, string logContent = "")
    {
      QuickLog(logContent: logContent, fullFilePath: filePath + fileName + fileExtension);
    }

    /// <summary>
    /// A class internal method, that is used to enable the static method <see cref="QuickLog(string, string)"/> to call the instance bound method <see cref="Log()"/>
    /// </summary>
    private static void Log(DebugLogger logger, string logContent = "")
    {
      logger.Log(fullFilePath: logger.FullFileName, loggerName: logger.LoggerName, timeStampInLogfileName: logger.TimeStampInLogfileName, logContent: logContent);
    }

    #endregion Static functionallity
  }
}
