using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlantiT.Forms.GrundrezeptImport
{
  public class SimpleLogger : IDisposable
  {
    private static Regex newLine = new Regex(@"(\r\n|\n)");
    private FileStream file = null;

    public SimpleLogger(string prefix, string path = "")
    {
      if (path != "" && !path.EndsWith(@"\"))
      {
        path += @"\";
      }

      file = new FileStream(path + prefix + DateTime.Now.ToString("-yyyyMMdd-HHmmss") + ".txt", FileMode.Create, FileAccess.Write);

    }

    public void Log(string title, string text = "", int indentation = 0)
    {
      if (!file.CanWrite)
      {

        throw new Exception("Logging exception: No write permission");
      }
      var indent = new string(' ', 2 + indentation * 2);
      var line = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + indent + title;

      if (text != "")
      {
        indent = "\r\n" + new string(' ', line.Length + 5);
        line += "  -  " + newLine.Replace(text, indent);
      }

      if (!line.EndsWith("\n"))
      {
        line += "\r\n";
      }

      byte[] bytes = Encoding.UTF8.GetBytes(line);

      file.Write(bytes, 0, bytes.Length);
    }


    public void Dispose()
    {
      if (file != null)
      {
        file.Flush();

        file.Dispose();
        file = null;
      }
    }
  }
}
